using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using MySql.Data.MySqlClient;

class TcpServer
{
    static string connectionString = "Server=localhost;Database=mygarden;User=root;Password=duywudang;";

    static void Main()
    {
        int port = 5000;
        TcpListener server = new TcpListener(IPAddress.Any, port);
        server.Start();
        Console.WriteLine($"MySQL Server started on port {port}");

        // Tạo bảng nếu chưa có
        InitDatabase();

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Client connected");

            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytes = stream.Read(buffer, 0, buffer.Length);
            string json = Encoding.UTF8.GetString(buffer, 0, bytes);

            Console.WriteLine($"Received: {json}");

            var data = JsonDocument.Parse(json).RootElement;
            string type = data.GetProperty("type").GetString();

            string response = "";

            if (type == "register")
            {
                string email = data.GetProperty("email").GetString();
                string password = data.GetProperty("password").GetString();

                response = RegisterUser(email, password);
            }
            else if (type == "login")
            {
                string email = data.GetProperty("email").GetString();
                string password = data.GetProperty("password").GetString();

                response = LoginUser(email, password);
            }

            byte[] resp = Encoding.UTF8.GetBytes(response);
            stream.Write(resp, 0, resp.Length);

            client.Close();
        }
    }

    // Tạo bảng MySQL nếu chưa có
    static void InitDatabase()
    {
        using (var conn = new MySqlConnection(connectionString))
        {
            conn.Open();
            string sql =
                @"CREATE TABLE IF NOT EXISTS users (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    email VARCHAR(255) UNIQUE,
                    password VARCHAR(255)
                )";

            new MySqlCommand(sql, conn).ExecuteNonQuery();
        }
        Console.WriteLine("Database checked/created.");
    }

    static string RegisterUser(string email, string password)
    {
        using (var conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            // Kiểm tra email tồn tại
            var checkCmd = new MySqlCommand("SELECT COUNT(*) FROM users WHERE email=@e", conn);
            checkCmd.Parameters.AddWithValue("@e", email);

            long count = (long)checkCmd.ExecuteScalar();

            if (count > 0)
                return "Email đã tồn tại!";

            // Thêm user
            var insert = new MySqlCommand("INSERT INTO users (email, password) VALUES (@e, @p)", conn);
            insert.Parameters.AddWithValue("@e", email);
            insert.Parameters.AddWithValue("@p", password);
            insert.ExecuteNonQuery();

            return "Đăng ký thành công!";
        }
    }

    static string LoginUser(string email, string password)
    {
        using (var conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            var cmd = new MySqlCommand("SELECT COUNT(*) FROM users WHERE email=@e AND password=@p", conn);
            cmd.Parameters.AddWithValue("@e", email);
            cmd.Parameters.AddWithValue("@p", password);

            long count = (long)cmd.ExecuteScalar();

            if (count > 0)
                return "Đăng nhập thành công!";
            else
                return "Sai email hoặc mật khẩu!";
        }
    }
}
