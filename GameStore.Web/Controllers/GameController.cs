﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameStore.Services.DTOs;
using GameStore.Services.Abstract;
using System.IO;
using GameStore.Web.Infrastructure.Attributes;

namespace GameStore.Web.Controllers
{
    //[OutputCache(Duration = 60, VaryByHeader = "get;post")]
    public class GameController : Controller
    {
        private IGameService _gameService;

        public GameController(IGameService service)
        {
            _gameService = service;
        }

        [HttpPost]
        public ActionResult NewGame(GameDto game)
        {
            _gameService.Create(game);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult UpdateGame(GameDto game)
        {
            _gameService.Edit(game);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public JsonResult ShowGame(string gameKey)
        {
            GameDto game = _gameService.GetGameByKey(gameKey);
            return Json(game, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListAllGames()
        {
            IEnumerable<GameDto> games = _gameService.GetAll();
            return Json(games, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteGame(int id)
        {
            _gameService.Delete(id);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public FileResult DownloadGame()
        {
            return null;
        }

    }
}