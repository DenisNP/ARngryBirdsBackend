using System;
using ARngryBirdsBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ARngryBirdsBackend.Controllers
{
    [ApiController]
    [Route("/api")]
    public class StateController : ControllerBase
    {
        private static readonly State State = new State();
        private static readonly Engine Engine = new Engine(State);
        
        [HttpGet]
        public string Get()
        {
            Engine.Tick(DateTime.Now);
            return JsonConvert.SerializeObject(State, Utils.ConverterSettings);
        }

        [HttpGet("/reset")]
        public string Reset()
        {
            Engine.Reset();
            return JsonConvert.SerializeObject(State, Utils.ConverterSettings);
        }

        [HttpGet("/coords")]
        public string SetXy()
        {
            var x = int.Parse(Request.Query["x"]);
            var y = int.Parse(Request.Query["y"]);
            Engine.SetCoords(x, y);
            return "ok";
        }

        [HttpGet("/hit")]
        public string Hit()
        {
            var strength = int.Parse(Request.Query["strength"]);
            Engine.Hit(strength / 100.0);
            return "ok";
        }

        [HttpGet("/mode")]
        public string Mode()
        {
            var mode = int.Parse(Request.Query["v"]);
            Engine.SetMode(mode);
            return "ok";
        }
    }
}