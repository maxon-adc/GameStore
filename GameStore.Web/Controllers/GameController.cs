﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameStore.Services.DTOs;
using GameStore.Services.Abstract;

namespace GameStore.Web.Controllers
{
    [OutputCache(Duration = 60, VaryByHeader = "get;post")]
    public class GameController : Controller
    {
        private IGameService _gameService;

        public GameController(IGameService service)
        {
            _gameService = service;
        }

        [HttpPost]
        public ActionResult NewGame(GameDTO newGame)
        {
            _gameService.Create(newGame);
            return Redirect("https://youtube.com");
        }

        [HttpPost]
        public ActionResult UpdateGame(GameDTO game)
        {
            _gameService.Edit(game);
            return Redirect("https://youtube.com");
        }

        [HttpGet]
        public JsonResult ShowGame(string gameKey)
        {
            GameDTO game = _gameService.GetGameByKey(gameKey);
            return Json(game, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListAllGames()
        {
            IEnumerable<GameDTO> games = _gameService.GetAll();
            return Json(games, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteGame(GameDTO game)
        {
            _gameService.Delete(game);
            return null;
        }

        [HttpGet]
        public FileResult DownloadGame()
        {
            return null;
        }
    }
}