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
    }
}