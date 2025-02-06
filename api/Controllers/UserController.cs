using apiExo.domain.Commands;
using apiExo.domain.Queries;
using apiExo.domain.services;
using Microsoft.AspNetCore.Mvc;

public class UserController(IUserService userService,IConfiguration configuration) : ControllerBase
{
    public IActionResult Register([FromBody] RegisterCommand command){

        try
        {
            userService.Execute(command);
            return Ok(new{message=""});
        }
        catch (Exception e)
        {
            return BadRequest(new{message=e.Message});
        }
    }

    public IActionResult Login([FromBody] LoginQuery query){
        try
        {
            string? token_name = configuration["AUTH_TOKEN_NAME"] ?? throw new Exception("Missing AUTH_TOKEN_NAME Configuration");
            string jwtToken = userService.Execute(query);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // 🔒 Prevents JavaScript access (XSS protection)
                Secure = true,   // 🔒 Send only over HTTPS
                SameSite = SameSiteMode.Strict, // Prevent CSRF attacks
                Expires = DateTime.UtcNow.AddHours(1) 
            };

            Response.Cookies.Append(token_name, jwtToken, cookieOptions);
            return Ok(new{message="Logged in successfully!"});
        }
        catch (Exception e)
        {
            return BadRequest(new{message=e.Message});
        }
  
    }

    public IActionResult Logout(){
        try
        {
            string? token_name = configuration["AUTH_TOKEN_NAME"] ?? throw new Exception("Missing AUTH_TOKEN_NAME Configuration");
            Response.Cookies.Delete(token_name);
        }
        catch (Exception )
        {
        }
        return Ok(new{message="Logged in successfully!"});
    }
}