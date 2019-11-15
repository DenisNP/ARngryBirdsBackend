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
        
        [HttpPost]
        public string Post()
        {
            return JsonConvert.SerializeObject(State, Utils.ConverterSettings);
        }
    }
}