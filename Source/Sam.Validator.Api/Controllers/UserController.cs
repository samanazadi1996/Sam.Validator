using Microsoft.AspNetCore.Mvc;

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
    public class UserDto : Validator<UserDto>
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public int Age { get; set; }
        public string? Password { get; set; }
        public DateTime BirthDate { get; set; }

        public override void Validate()
        {
            RuleFor(x => x.Username)
                .NotNull()
                .NotEmpty()
                .Length(3, 20)
                .Matches(@"^[a-zA-Z0-9_]+$"); // Only letters, digits, underscores

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .Email();

            RuleFor(x => x.Role)
                .NotNull()
                .In("Admin", "User", "Guest");

            RuleFor(x => x.Age)
                .GreaterThan(17)
                .LessThan(100)
                .Min(18)
                .Max(99);

            RuleFor(x => x.Password)
                .NotNull()
                .Length(8, 100)
                .Matches(@"[A-Z]").Must(x => x.Password!.Any(char.IsUpper), "Password must contain at least one uppercase letter.")
                .Must(x => x.Password!.Any(char.IsDigit), "Password must contain at least one digit.");

            RuleFor(x => x.BirthDate)
                .Must(x => x.BirthDate < DateTime.Now, "BirthDate must be in the past.");
        }
    }

}
