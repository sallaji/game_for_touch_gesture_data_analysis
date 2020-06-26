using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shapematch_Restful_Service.Contracts;
using System;

namespace Shapematch_Restful_Service.Controllers
{
    [Route("api/[controller]")]
    public class GameLogsController : Controller
    {
        private static List<GameLog> gameLogs = new List<GameLog>()
        {
            /*
             * TODO: The data to be logged belongs to paths that corresponds to 
             * the matched figures (data transfer optimization)
             */
                new GameLog{
                Id = 1,
                xTouch = 347.32f
                },
                new GameLog{
                Id = 2,
                xTouch = 657.32f
                },
        };

        // GET api/game-logs
        [HttpGet]
        public JsonResult Get()
        {
            return Json(gameLogs);
        }

        // GET api/game-logs/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            GameLog gameLog = gameLogs.Single(
            gameLogObject => gameLogObject.Id == id);

            return Json(gameLog);
        }

        // POST api/game-logs
        [HttpPost]
        public JsonResult Post([FromBody] GameLog newGameLog)
        {
            gameLogs.Add(newGameLog);
            Console.WriteLine("el amigo fue creado"+newGameLog.ToString());
            return Json(gameLogs);
        }

        // PUT api/game-logs/5
        [HttpPut("{id}")]
        public JsonResult Put(int id, [FromBody] float newXtouch)
        {
            GameLog gameLog = gameLogs.Single(
            gameLogObject => gameLogObject.Id == id);
            gameLog.xTouch = newXtouch;
            return Json(gameLog);
        }

        // DELETE api/game-logs/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            GameLog gameLog = gameLogs.Single
            (gameLogObject => gameLogObject.Id == id);
            gameLogs.Remove(gameLog);
            Console.WriteLine("me veo");
        }
    }
}
