# Chạy từng lệnh này trong PowerShell (copy/paste)

# Test đăng ký
$body = '{"email":"test999@test.com","password":"123456"}'
Invoke-RestMethod -Uri "http://localhost:5000/api/auth/register" -Method POST -ContentType "application/json" -Body $body

# Test đăng nhập
$body = '{"email":"test999@test.com","password":"123456"}'
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/auth/login" -Method POST -ContentType "application/json" -Body $body
$response

# Test lấy farm (thay 1 bằng userId từ đăng nhập)
Invoke-RestMethod -Uri "http://localhost:5000/api/farm/1" -Method GET


