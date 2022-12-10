using Common.Models;
using GameAPI.Repos;
using GameAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace GameAPI.Controllers
{
    [Route("api/[controller]")]
     [ApiController]
     public class GameController : ControllerBase
     {
          private readonly IMongoRepo<Game> _gameRepo;
          private readonly ISyncService<Game> _syncService;
          public GameController(IMongoRepo<Game> gameRepo, ISyncService<Game> service)
          {
               _gameRepo = gameRepo;
               _syncService = service;
          }

          [HttpGet]
          public List<Game> GetAllGames()
          {
               var games = _gameRepo.GetAllGames();
               return games;
          }

          [HttpGet("{id}")]
          public Game GetGameById(Guid id)
          {
               var result = _gameRepo.GetGameById(id);
               return result;
          }

          [HttpPost]
          public IActionResult Create(Game game)
          {
               game.LastChange = DateTime.UtcNow;
               var result = _gameRepo.InsertGame(game);
               _syncService.Upsert(game);
               return Ok(result);
          }

          [HttpPut]
          public IActionResult Upsert(Game game)
          {
               if (game.ID == Guid.Empty)
               {
                    return BadRequest("Empty ID");
               }
               game.LastChange = DateTime.UtcNow;
               _gameRepo.UpsertGame(game);
               _syncService.Upsert(game);
               return Ok(game);
          }

          [HttpPut("sync")]
          public IActionResult SyncUpsert(Game game)
          {
               var existingGame = _gameRepo.GetGameById(game.ID);
               if (existingGame == null || (game.LastChange > existingGame.LastChange))
               {
                    _gameRepo.UpsertGame(game);
               }
               return Ok(game);
          }

        [HttpDelete("sync")]
        public IActionResult DeleteSync(Game game)
        {
            var existingGame = _gameRepo.GetGameById(game.ID);
            if (existingGame != null || (game.LastChange > existingGame.LastChange))
            {
                _gameRepo.DeleteGame(game.ID);
            }
            return Ok("Game " + game.ID + " sync deleted");
        }

        [HttpDelete("{id}")]
          public IActionResult Delete(Guid id)
          {
               var result = _gameRepo.GetGameById(id);
               if (result == null)
               {
                    return BadRequest("Game not found");
               }
               _gameRepo.DeleteGame(id);
               result.LastChange = DateTime.UtcNow;
               _syncService.Delete(result);
               return Ok("Game " + id + " deleted");
          }
     }
}
