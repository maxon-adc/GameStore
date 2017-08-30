﻿using AutoMapper;
using GameStore.Authentification.Abstract;
using GameStore.Common.App_LocalResources;
using GameStore.Common.Enums;
using GameStore.Services.Abstract;
using GameStore.Services.Dtos;
using GameStore.Web.Infrastructure.Attributes;
using GameStore.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GameStore.Web.Controllers
{
	[AuthorizeUser(AuthorizationMode.Allow, AccessLevel.Manager)]
	public class PublishersController : BaseController
	{
		private readonly IPublisherService _publisherService;
		private readonly IMapper _mapper;

		public PublishersController(IPublisherService publisherService,
			IMapper mapper,
			IAuthentication authentication)
			: base(authentication)
		{
			_publisherService = publisherService;
			_mapper = mapper;
		}

		[HttpGet]
		public ActionResult New()
		{
			return View(new PublisherViewModel());
		}

		[HttpPost]
		public ActionResult New(PublisherViewModel model)
		{
			CheckIfCompanyNameIsUnique(model);
			CheckIfDescriptionIsNull(model);

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var publisherDto = _mapper.Map<PublisherViewModel, PublisherDto>(model);
			_publisherService.Create(CurrentLanguage, publisherDto);

			return RedirectToAction("ShowAll", "Publishers");
		}

		[HttpGet]
		public ActionResult Update(string key)
		{
			var publisherDto = _publisherService.GetSingle(CurrentLanguage, key);
			var publisherViewModel = _mapper.Map<PublisherDto, PublisherViewModel>(publisherDto);

			return View(publisherViewModel);
		}

		[HttpPost]
		public ActionResult Update(PublisherViewModel model)
		{
			CheckIfCompanyNameIsUnique(model);
			CheckIfDescriptionIsNull(model);

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var publisherDto = _mapper.Map<PublisherViewModel, PublisherDto>(model);
			_publisherService.Update(CurrentLanguage, publisherDto);

			return RedirectToAction("ShowAll", "Publishers");
		}

		public ActionResult Show(string key)
		{
			var model = _mapper.Map<PublisherDto, PublisherViewModel>(_publisherService.GetSingle(CurrentLanguage, key));

			return View(model);
		}

		[HttpPost]
		public ActionResult Delete(string key)
		{
			_publisherService.Delete(key);

			return RedirectToAction("ShowAll", "Publishers");
		}

		public ActionResult ShowAll()
		{
			var publisherDtos = _publisherService.GetAll(CurrentLanguage);
			var model = _mapper.Map<IEnumerable<PublisherDto>, List<PublisherViewModel>>(publisherDtos);

			return View(model);
		}

		private void CheckIfCompanyNameIsUnique(PublisherViewModel model)
		{
			if (!_publisherService.Contains(model.CompanyName))
			{
				return;
			}

			var existingPublisher = _publisherService.GetSingle(CurrentLanguage, model.CompanyName);

			if (existingPublisher.Id != model.Id)
			{
				ModelState.AddModelError("CompanyName", GlobalResource.PublisherWithSuchCompanyNameAlreadyExists);
			}
		}

		private void CheckIfDescriptionIsNull(PublisherViewModel model)
		{
			if (model.Description == null)
			{
				ModelState.AddModelError("Description", GlobalResource.DescriptionIsRequired);
			}
		}
	}
}