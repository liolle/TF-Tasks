using apiExo.CQS;
using apiExo.domain.Commands;
using apiExo.domain.Queries;
using apiExo.domain.services;
using Microsoft.AspNetCore.Mvc;

public class UserController(IUserService userService,IConfiguration configuration) : ControllerBase
{
  [HttpPost]
  [Route("/register")]
  public IActionResult Register([FromBody] RegisterCommand command){
    return Ok(userService.Execute(command));
  }

  [HttpPost]
  [Route("/login")]
  public IActionResult Login([FromBody] LoginQuery query){

    if(!ModelState.IsValid){
      return BadRequest("Invalid body content");
    }
    string? token_name = configuration["AUTH_TOKEN_NAME"] ?? throw new Exception("Missing AUTH_TOKEN_NAME Configuration");
    QueryResult<string> result = userService.Execute(query);
    if (result.IsFailure){return BadRequest(result.ErrorMessage);}

    var cookieOptions = new CookieOptions
    {
      HttpOnly = true, // 🔒 Prevents JavaScript access (XSS protection)
      Secure = true,   // 🔒 Send only over HTTPS
      SameSite = SameSiteMode.Strict, // Prevent CSRF attacks
      Expires = DateTime.UtcNow.AddHours(1) 
    };

    Response.Cookies.Append(token_name, result.Result, cookieOptions);
    return Ok();
  }

  public IActionResult Logout(){
    string? token_name = configuration["AUTH_TOKEN_NAME"] ?? throw new Exception("Missing AUTH_TOKEN_NAME Configuration");
    Response.Cookies.Delete(token_name);
    return Ok(ICommandResult.Success());
  }
}
