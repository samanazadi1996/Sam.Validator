using Microsoft.AspNetCore.Mvc;
using Sam.Validator.Api.Models;

namespace Sam.Validator.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public IActionResult Create([FromBody] UserDto data)
        {
            return Ok("Valid!");
        }
    }

}
