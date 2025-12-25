using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest? request)
    {
        // Kiểm tra request null
        if (request == null)
        {
            return BadRequest(new { status = "error", message = "Request body không hợp lệ" });
        }

        if (string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.password))
        {
            return BadRequest(new { status = "error", message = "Email và password không được để trống" });
        }

        Console.WriteLine($"Register request: email={request.email}");

        bool success = DatabaseHelper.RegisterUser(request.email, request.password);
        
        if (success)
        {
            return Ok(new { status = "success", message = "Đăng ký thành công!" });
        }
        else
        {
            return BadRequest(new { status = "error", message = "Email đã tồn tại hoặc có lỗi xảy ra" });
        }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest? request)
    {
        // Kiểm tra request null
        if (request == null)
        {
            return BadRequest(new { status = "error", message = "Request body không hợp lệ" });
        }

        if (string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.password))
        {
            return BadRequest(new { status = "error", message = "Email và password không được để trống" });
        }

        Console.WriteLine($"Login request: email={request.email}");

        int? userId = DatabaseHelper.LoginUser(request.email, request.password);
        
        if (userId.HasValue)
        {
            return Ok(new { status = "success", userId = userId.Value, message = "Đăng nhập thành công!" });
        }
        else
        {
            return Unauthorized(new { status = "error", message = "Sai email hoặc password" });
        }
    }
}

public class RegisterRequest
{
    public string email { get; set; } = "";
    public string password { get; set; } = "";
}

public class LoginRequest
{
    public string email { get; set; } = "";
    public string password { get; set; } = "";
}

