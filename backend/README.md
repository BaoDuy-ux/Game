# MyGarden Backend Server (C#)

Backend server đơn giản cho game MyGarden multiplayer sử dụng C# ASP.NET Core.

## Yêu cầu

- .NET 8.0 SDK
- MySQL Server
- Visual Studio 2022 hoặc VS Code

## Cài đặt

### 1. Tạo database MySQL

Chạy file `database.sql` trong MySQL để tạo database và các bảng:

```bash
mysql -u root -p < database.sql
```

### 2. Cấu hình connection string

Mở file `DatabaseHelper.cs` và sửa connection string nếu cần:

```csharp
private static string connectionString = "Server=localhost;Database=mygarden_db;User=root;Password=;";
```

### 3. Cài đặt packages

```bash
dotnet restore
```

### 4. Chạy server

```bash
dotnet run
```

Server sẽ chạy tại: `http://localhost:5000`

## API Endpoints

### Authentication
- `POST /api/auth/register` - Đăng ký
  ```json
  {
    "email": "user@example.com",
    "password": "password123"
  }
  ```

- `POST /api/auth/login` - Đăng nhập
  ```json
  {
    "email": "user@example.com",
    "password": "password123"
  }
  ```

### Farm
- `GET /api/farm/{userId}` - Lấy thông tin farm

## SignalR Hub

Server có SignalR hub tại: `http://localhost:5000/gamehub`

### Events từ Client:
- `SendChatMessage(username, message)` - Gửi tin nhắn chat
- `UpdatePlayerPosition(userId, x, y)` - Cập nhật vị trí player
- `JoinRoom(roomName)` - Tham gia room (scene)

### Events từ Server:
- `ReceiveChatMessage(username, message)` - Nhận tin nhắn chat
- `PlayerMoved(userId, x, y)` - Player khác di chuyển

## Kết nối từ Unity

Trong Unity, cần cài package **SignalR Client** và kết nối đến:
```
ws://localhost:5000/gamehub
```

## Lưu ý

- Đây là version đơn giản, chưa có JWT authentication
- Password được hash bằng BCrypt
- Mỗi user tự động có 1 farm khi đăng ký


