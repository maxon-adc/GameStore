﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.DAL.Abstract;
using GameStore.Domain.Entities;
using GameStore.Services.Abstract;
using GameStore.Services.DTOs;
using AutoMapper;

namespace GameStore.Services.Concrete
{
    public class UOWGameService : IGameService
    {
        private IUnitOfWork _unitOfWork;

        public UOWGameService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Create(GameDTO gameDTO)
        {
            Game game = Mapper.Map<GameDTO, Game>(gameDTO);
            if (_unitOfWork.GameRepository.Get().All(g => g.Name != game.Name))
            {
                _unitOfWork.GameRepository.Insert(game);
                _unitOfWork.Save();
            }
            else
            {
                throw new ArgumentException("There is a game with such key already");
            }
        }

        public void Edit(GameDTO newGame)
        {
            Game oldGame = _unitOfWork.GameRepository.GetById(newGame.Id);
            if (oldGame != null)
            {
                oldGame = Mapper.Map(newGame, oldGame);
                _unitOfWork.GameRepository.Update(oldGame);
                _unitOfWork.Save();
            }
            else
            {
                throw new ArgumentException("There is no such game");
            }
        }

        public void Delete(int id)
        {
            Game gameToRemove = _unitOfWork.GameRepository.Get().FirstOrDefault(g => g.Id == id);
            if (gameToRemove != null)
            {
                _unitOfWork.GameRepository.Delete(id);
                _unitOfWork.Save();
            }
            else
            {
                throw new ArgumentException("There is no such game");
            }
        }

        public GameDTO Get(int id)
        {
            Game game = _unitOfWork.GameRepository.Get().First(g => g.Id == id);
            if (game != null)
            {
                GameDTO gameDTO = Mapper.Map<Game, GameDTO>(game);
                return gameDTO;
            }

            throw new ArgumentException("There is no game with such id");
        }

        public GameDTO GetGameByKey(string key)
        {
            Game game = _unitOfWork.GameRepository.Get().First(g => g.Key.ToLower() == key.ToLower());
            if (game != null)
            {
                GameDTO gameDTO = Mapper.Map<Game, GameDTO>(game);
                return gameDTO;
            }

            throw new ArgumentException("There is no game with such key");
        }

        public IEnumerable<GameDTO> GetAll()
        {
            IEnumerable<Game> games = _unitOfWork.GameRepository.Get();
            var gameDTOs = Mapper.Map<IEnumerable<Game>, List<GameDTO>>(games);
            return gameDTOs;
        }

        

        public IEnumerable<GameDTO> GetGamesByGenre(string genreName)
        {
            IEnumerable<Game> games =  _unitOfWork.GameRepository.Get().Where(game => game.Genres.Any(genre => genre.Name == genreName));
            IEnumerable<GameDTO> gameDTOs = Mapper.Map<IEnumerable<Game>, List<GameDTO>>(games);
            return gameDTOs;
        }

        public IEnumerable<GameDTO> GetGamesByPlatformTypes(IEnumerable<string> platformTypeNames)
        {
            IEnumerable<Game> allGames = _unitOfWork.GameRepository.Get();
            List<Game> matchedGames = new List<Game>();
            foreach (Game game in allGames)
            {
                foreach (PlatformType type in game.PlatformTypes)
                {
                    if (platformTypeNames.Contains(type.Type))
                    {
                        matchedGames.Add(game);
                    }
                }
            }

            IEnumerable<GameDTO> gameDTOs = Mapper.Map<IEnumerable<Game>, List<GameDTO>>(matchedGames);
            return gameDTOs;
        }
    }
}
