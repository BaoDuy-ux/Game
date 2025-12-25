# Script test nhiều người chơi cùng lúc

Write-Host "=== TẠO 5 USER ĐỂ TEST MULTIPLAYER ===" -ForegroundColor Cyan

$users = @()
$baseUrl = "http://localhost:5000"

# Tạo 5 user
for ($i = 1; $i -le 5; $i++) {
    Write-Host "`n--- Đăng ký User $i ---" -ForegroundColor Yellow
    
    $registerBody = @{
        email = "user$i@test.com"
        password = "123456"
    } | ConvertTo-Json

    try {
        $registerResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/register" `
            -Method POST `
            -ContentType "application/json" `
            -Body $registerBody

        if ($registerResponse.status -eq "success") {
            Write-Host "✓ User $i đăng ký thành công" -ForegroundColor Green
            
            # Đăng nhập để lấy userId
            $loginBody = @{
                email = "user$i@test.com"
                password = "123456"
            } | ConvertTo-Json

            $loginResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/login" `
                -Method POST `
                -ContentType "application/json" `
                -Body $loginBody

            if ($loginResponse.status -eq "success") {
                $userId = $loginResponse.userId
                Write-Host "✓ User $i đăng nhập thành công - UserID: $userId" -ForegroundColor Green
                
                # Lấy thông tin farm
                $farmResponse = Invoke-RestMethod -Uri "$baseUrl/api/farm/$userId" `
                    -Method GET
                
                Write-Host "  Farm: $($farmResponse.farm.farm_name) - Coins: $($farmResponse.farm.coins)" -ForegroundColor Gray
                
                $users += @{
                    UserNumber = $i
                    Email = "user$i@test.com"
                    UserId = $userId
                    Farm = $farmResponse.farm
                }
            }
        }
    }
    catch {
        Write-Host "✗ Lỗi với User $i : $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "`n=== TỔNG KẾT ===" -ForegroundColor Cyan
Write-Host "Đã tạo thành công $($users.Count) users:" -ForegroundColor Green
foreach ($user in $users) {
    Write-Host "  User $($user.UserNumber): Email=$($user.Email), ID=$($user.UserId), Farm=$($user.Farm.farm_name)" -ForegroundColor White
}

Write-Host "`n=== HƯỚNG DẪN TEST MULTIPLAYER ===" -ForegroundColor Cyan
Write-Host "1. Mỗi user có farm riêng (kiểm tra bằng cách lấy farm của từng userId)" -ForegroundColor Yellow
Write-Host "2. Để test chat và multiplayer, cần kết nối SignalR hub từ Unity hoặc browser" -ForegroundColor Yellow
Write-Host "3. SignalR hub URL: ws://localhost:5000/gamehub" -ForegroundColor Yellow
Write-Host "`nDanh sách UserID để test:" -ForegroundColor Yellow
foreach ($user in $users) {
    Write-Host "  UserID $($user.UserId) - Email: $($user.Email)" -ForegroundColor White
}


