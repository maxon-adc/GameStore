﻿using AutoMapper;
using GameStore.Common.Entities;
using GameStore.DAL.Abstract.Common;
using GameStore.Services.Abstract;
using GameStore.Services.Dtos;
using System.Collections.Generic;

namespace GameStore.Services.Concrete
{
	public class PublisherService : IPublisherService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
        private readonly IInputLocalizer<Publisher> _localizer;
		private readonly IPublisherRepository _publisherRepository;
		private readonly IGameRepository _gameRepository;

		public PublisherService(IUnitOfWork unitOfWork,
			IMapper mapper,
            IInputLocalizer<Publisher> localizer,
			IPublisherRepository publisherRepository,
			IGameRepository gameRepository)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
            _localizer = localizer;
			_publisherRepository = publisherRepository;
			_gameRepository = gameRepository;
		}

		public void Create(string language, PublisherDto publisherDto)
		{
			var publisher = _mapper.Map<PublisherDto, Publisher>(publisherDto);
            publisher = _localizer.Localize(language, publisher);
			_publisherRepository.Insert(publisher);
			_unitOfWork.Save();
		}

		public PublisherDto GetSingle(string language, string companyName)
		{
			var publisher = _publisherRepository.GetSingle(language, p => p.CompanyName.ToLower() == companyName.ToLower());
			var publisherDto = _mapper.Map<Publisher, PublisherDto>(publisher);

			return publisherDto;
		}

		public IEnumerable<PublisherDto> GetAll(string language)
		{
			var publishers = _publisherRepository.GetAll(language);
			var publisherDtos = _mapper.Map<IEnumerable<Publisher>, IEnumerable<PublisherDto>>(publishers);

			return publisherDtos;
		}

		public void Update(string language, PublisherDto publisherDto)
		{
			var publisher = _publisherRepository.GetSingle(language, p => p.Id == publisherDto.Id);
			_mapper.Map(publisherDto, publisher);
            publisher = _localizer.Localize(language, publisher);
            _publisherRepository.Update(publisher);
			_unitOfWork.Save();
		}

		public void Delete(string companyName)
		{
			_publisherRepository.Delete(companyName);
			_unitOfWork.Save();
		}

		public bool Contains(string companyName)
		{
			return _publisherRepository.Contains(p => p.CompanyName == companyName);
		}
	}
}