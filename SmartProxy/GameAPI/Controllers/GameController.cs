using Common.Models;
using GameAPI.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameAPI.Controllers
{
     [Route("api/[controller]")]
     [ApiController]
     public class GameController : ControllerBase
     {
          private readonly IMongoRepo<Game> _gameRepo;
          public GameController(IMongoRepo<Game> gameRepo)
          {
               _gameRepo = gameRepo;
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
               _gameRepo.InsertGame(game);
               return Ok(game);
          }

          [HttpPut("{id}")]
          public IActionResult Upsert(Game game)
          {
               game.LastChange = DateTime.UtcNow;
               _gameRepo.UpsertGame(game);
               return Ok(game);
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
               return Ok("Game " + id + " deleted");
          }
     }
}
