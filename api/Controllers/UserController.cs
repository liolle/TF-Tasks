using apiExo.bll.entity;
using apiExo.bll.services;
using Microsoft.AspNetCore.Mvc;

public class UserController(IUserService userService,IConfiguration configuration) : ControllerBase
{
    public IActionResult Register([FromBody] RegisterModel model){

        try
        {
            userService.Register(model);
            return Ok(new{message=""});
        }
        catch (Exception e)
        {
            return BadRequest(new{message=e.Message});
        }
    }

    public IActionResult Login([FromBody] CredentialModel model){
        try
        {
            string? token_name = configuration["AUTH_TOKEN_NAME"] ?? throw new Exception("Missing AUTH_TOKEN_NAME Configuration");
            string jwtToken = userService.Login(model);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // 🔒 Prevents JavaScript access (XSS protection)
                Secure = true,   // 🔒 Send only over HTTPS (set to false in development)
                SameSite = SameSiteMode.Strict, // Prevent CSRF attacks
                Expires = DateTime.UtcNow.AddHours(1) // Set cookie expiration same as JWT expiration
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
        return Ok();
    }
}