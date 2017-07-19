﻿using System;
using AutoMapper;
using GameStore.Services.Abstract;
using GameStore.Services.DTOs;
using GameStore.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GameStore.Web.Controllers
{
	public class GameController : Controller
	{
		private readonly IGameService _gameService;
		private readonly IGenreService _genreService;
		private readonly IPlatformTypeService _platformTypeService;
		private readonly IPublisherService _publisherService;
		private readonly IMapper _mapper;
		private static FilterViewModel _filterState;  // TODO Required: Move static after constants
		private const int DefaultPageSize = 10; //TODO Required: Move constants to top
		private const int DefaultPage = 1;

		public GameController(IGameService gameService,
			IGenreService genreService,
			IPlatformTypeService platformTypeService,
			IPublisherService publisherService,
			IMapper mapper)
		{
			_gameService = gameService;
			_genreService = genreService;
			_platformTypeService = platformTypeService;
			_publisherService = publisherService;
			_mapper = mapper;
		}

		[HttpGet]
		public ActionResult New()
		{
			var gameViewModel = new GameViewModel
			{
				GenresData = MapGenres(),
				PlatformTypesData = MapPlatformTypes(),
				PublishersData = MapPublishers()
			};

			return View(gameViewModel);
		}

		[HttpPost]
		public ActionResult New(GameViewModel gameViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(gameViewModel);
			}

			var gameDto = _mapper.Map<GameViewModel, GameDto>(gameViewModel);
			_gameService.Create(gameDto);

			return RedirectToAction("ListAll");
		}

		[HttpPost]
		public ActionResult Update(GameDto game)
		{
			_gameService.Edit(game);

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}

		public ActionResult Show(string gameKey)
		{
			var gameDto = _gameService.GetSingleBy(gameKey);
			_gameService.SaveView(gameDto.Id);
			var gameViewModel = _mapper.Map<GameDto, GameViewModel>(gameDto);

			return View(gameViewModel);
		}

		[HttpGet]
		public ActionResult ListAll(AllGamesViewModel model)
		{
			int itemsToSkip, itemsToTake;

			if (!ModelState.IsValidField("Filter.GameName") && model.FilterIsChanged) //TODO Required: Don't validate fields separately. You should validate all model
			{
				model.FilterIsChanged = false;
				model.Filter.PlatformTypesData = MapPlatformTypes();
				model.Filter.GenresData = MapGenres();
				model.Filter.PublishersData = MapPublishers();
				itemsToSkip = (model.PageSize * (model.CurrentPage - 1)); //TODO Required: Remove useless '()'
				itemsToTake = model.PageSize;
				model.Games = MapGames(_filterState, itemsToSkip, itemsToTake);

				return View(model);
			}

			if (!ModelState.IsValidField("Filter.GameName") && !model.FilterIsChanged) //TODO Required: Don't validate fields separately. You should validate all model
			{
				ModelState.Clear(); //TODO: Why do you need clear ModelState if you don't use it below?
			}

			if (model.Filter == null)
			{
				model.Filter = new FilterViewModel();
				model.CurrentPage = DefaultPage;
				model.PageSize = DefaultPageSize;
				model.TotalItems = MapGames().Count;
				_filterState = model.Filter;
			}
			else
			{
				if (model.FilterIsChanged)
				{
					_filterState = model.Filter;
					model.CurrentPage = DefaultPage;
					model.TotalItems = MapGames(_filterState).Count; // TODO Required: GetAll->Map->CalculateCount Really? Do it simply.
					model.FilterIsChanged = false;
				}
			}

			model.TotalPages = (int)Math.Ceiling((decimal)model.TotalItems / model.PageSize);
			itemsToSkip = (model.PageSize * (model.CurrentPage - 1));
			itemsToTake = model.PageSize;
			model.Games = MapGames(_filterState, itemsToSkip, itemsToTake);
			model.Filter.PlatformTypesData = MapPlatformTypes();
			model.Filter.GenresData = MapGenres();
			model.Filter.PublishersData = MapPublishers();

			return View(model);
		}

		[HttpPost]
		public ActionResult Delete(int gameId)
		{
			_gameService.Delete(gameId);

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}

		[OutputCache(Duration = 60)]
		public ActionResult ShowCount()
		{
			var games = _gameService.GetAll(); // TODO Required: GetAll->Map->CalculateCount Really? Do it simply.
			var count = games.Count();

			return PartialView(count);
		}

		public FileResult Download(string gameKey)
		{
			var path = Server.MapPath("~/file.pdf");
			var fileBytes = new byte[0];

			if (System.IO.File.Exists(path))
			{
				fileBytes = System.IO.File.ReadAllBytes(path);
			}

			return new FileContentResult(fileBytes, "application/pdf");
		}
		//TODO Consider: Move private methods to separate Mapper class.
		//TODO: Required: You can't give a method name 'Map....' and invoke service method inside. (stick to this signatures: Type1 Map(Type2), Type1 Get(params/no params))
		private List<GameViewModel> MapGames(FilterViewModel filter = null, int? itemsToSkip = null, int? itemsToTake = null)
		{
			if (filter != null)
			{
				var filterDto = _mapper.Map<FilterViewModel, FilterDto>(filter);

				if (itemsToSkip != null && itemsToTake != null)
				{
					return _mapper.Map<IEnumerable<GameDto>, List<GameViewModel>>(_gameService.GetAll(filterDto, itemsToSkip, itemsToTake));
				}

				return _mapper.Map<IEnumerable<GameDto>, List<GameViewModel>>(_gameService.GetAll(filterDto));
			}

			return _mapper.Map<IEnumerable<GameDto>, List<GameViewModel>>(_gameService.GetAll());
		}

		private List<PlatformTypeViewModel> MapPlatformTypes()
		{
			var platformTypes = Mapper.Map<IEnumerable<PlatformTypeDto>, List<PlatformTypeViewModel>>(_platformTypeService.GetAll());

			return platformTypes;
		}

		private List<GenreViewModel> MapGenres()
		{
			var genres = Mapper.Map<IEnumerable<GenreDto>, List<GenreViewModel>>(_genreService.GetAll());

			return genres;
		}

		private List<PublisherViewModel> MapPublishers()
		{
			var publishers = Mapper.Map<IEnumerable<PublisherDto>, List<PublisherViewModel>>(_publisherService.GetAll());

			return publishers;
		}
	}
}