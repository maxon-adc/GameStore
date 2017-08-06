﻿using AutoMapper;
using GameStore.Services.Abstract;
using GameStore.Services.DTOs;
using GameStore.Web.Controllers;
using GameStore.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GameStore.Web.Tests
{
    [TestClass]
    public class PublisherControllerTests
    {
        private Mock<IPublisherService> _mockOfPublisherService;
        private PublishersController _target;
        private List<PublisherDto> _publishers;
        private const string ValidString = "test";
        private const string InvalidString = "testtest";
        private readonly IMapper _mapper = new Mapper(
            new MapperConfiguration(cfg => cfg.AddProfile(new WebProfile())));

        [TestInitialize]
        public void Initialize()
        {
            _publishers = new List<PublisherDto>();
            _mockOfPublisherService = new Mock<IPublisherService>();
            _mockOfPublisherService.Setup(m => m.Create(It.IsAny<PublisherDto>())).Callback<PublisherDto>(p => _publishers.Add(p));
            _mockOfPublisherService.Setup(m => m.GetSingleBy(It.IsAny<string>())).Returns(new PublisherDto());
            _target = new PublishersController(_mockOfPublisherService.Object, _mapper);
        }

        [TestMethod]
        public void New_SendsPublisherToView()
        {
            var result = ((ViewResult)_target.New()).Model;

            Assert.IsInstanceOfType(result, typeof(PublisherViewModel));
        }

        [TestMethod]
        public void New_ReturnsHttpStatusCode_WhenModelStateIsValid()
        {
            var result = _target.New(new PublisherViewModel());

            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
        }

        [TestMethod]
        public void New_SendsPublisherToView_WhenModelStateIsInvalid()
        {
            _target.ModelState.AddModelError(InvalidString, InvalidString);

            var result = ((ViewResult)_target.New(new PublisherViewModel())).Model;

            Assert.IsInstanceOfType(result, typeof(PublisherViewModel));
        }

        [TestMethod]
        public void New_CreatesPublisher_WhenModelStateIsValid()
        {
            _target.New(new PublisherViewModel());

            Assert.IsTrue(_publishers.Count == 1);
        }

        [TestMethod]
        public void Show_SendsPublisherToView_WhenAnyCompanyNameIsPassed()
        {
            var result = ((ViewResult)_target.Show(InvalidString)).Model;

            Assert.IsInstanceOfType(result, typeof(PublisherViewModel));
        }

        [TestMethod]
        public void Update_SendsPublisherToView_WhenValidCompanyNameIsPassed()
        {
            _publishers = new List<PublisherDto> { new PublisherDto { CompanyName = ValidString } };
            var result = ((ViewResult)_target.Update(ValidString)).Model;

            Assert.IsInstanceOfType(result, typeof(PublisherViewModel));
        }

        [TestMethod]
        public void Update_ReturnsViewResult_IfModelStateIsInvalid()
        {
            _target.ModelState.AddModelError(InvalidString, InvalidString);

            var result = _target.Update(ValidString);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Update_UpdatesPublisher_IfModelStateIsValid()
        {
            _publishers = new List<PublisherDto> { new PublisherDto { CompanyName = InvalidString } };
            var publisher = new PublisherViewModel { CompanyName = ValidString };
            _mockOfPublisherService.Setup(m => m.Update(_mapper.Map<PublisherViewModel, PublisherDto>(publisher))).Callback<PublisherDto>(p => _publishers[0] = p);

            _target.Update(publisher);

            Assert.AreEqual(_publishers[0].CompanyName, ValidString);
        }

        [TestMethod]
        public void Update_ReturnsRedirectResult_IfModelStateIsValid()
        {
            var result = _target.Update(new PublisherViewModel());

            Assert.IsInstanceOfType(result, typeof(RedirectResult));
        }
    }
}