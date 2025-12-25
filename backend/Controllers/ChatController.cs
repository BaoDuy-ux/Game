using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    [HttpPost("send")]
    public IActionResult SendChat([FromBody] ChatRequest? request)
    {
        if (request == null)
        {
            return BadRequest(new { status = "error", message = "Request body không hợp lệ" });
        }

        if (request.userId <= 0 || string.IsNullOrEmpty(request.message))
        {
            return BadRequest(new { status = "error", message = "UserId và message không hợp lệ" });
        }

        Console.WriteLine($"Chat send: userId={request.userId}, username={request.username}, message={request.message}");

        // Lưu tin nhắn vào database
        bool success = DatabaseHelper.SaveChatMessage(request.userId, request.username, request.message);
        
        if (success)
        {
            return Ok(new { status = "success", message = "Tin nhắn đã được gửi" });
        }
        else
        {
            return StatusCode(500, new { status = "error", message = "Lỗi lưu tin nhắn" });
        }
    }

    [HttpGet("history")]
    public IActionResult GetChatHistory([FromQuery] int limit = 20)
    {
        try
        {
            var messages = DatabaseHelper.GetChatHistory(limit);
            return Ok(new { status = "success", messages = messages });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi lấy chat history: {ex.Message}");
            return StatusCode(500, new { status = "error", message = "Lỗi lấy lịch sử chat" });
        }
    }

    [HttpGet("online-users")]
    public IActionResult GetOnlineUsers()
    {
        try
        {
            var users = DatabaseHelper.GetOnlineUsers();
            return Ok(new { status = "success", users = users });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi lấy online users: {ex.Message}");
            return StatusCode(500, new { status = "error", message = "Lỗi lấy danh sách online users" });
        }
    }
}

public class ChatRequest
{
    public int userId { get; set; }
    public string? username { get; set; }
    public string? message { get; set; }
}

