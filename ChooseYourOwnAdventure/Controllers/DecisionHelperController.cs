using ChooseYourOwnAdventure.Models;
using ChooseYourOwnAdventure.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ChooseYourOwnAdventure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DecisionHelperController : ControllerBase
    {
        private readonly ChooseYourOwnAdventureService _service;

        public DecisionHelperController(ChooseYourOwnAdventureService service)
        {
            _service = service;
        }

        // GET: api/<DecisionHelperController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                Decision decisionTree = _service.GetDecisions();

                return Ok(decisionTree);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
