瀏覽器測試網址
https://localhost:7122/CBoxUI/Index
https://localhost:7122/CBoxUI/Create
https://localhost:7122/CBoxUI/Edit/{id}
https://localhost:7122/CBoxUI/Delete/{id}

https://localhost:7122/CPackUI/Index
https://localhost:7122/CPackUI/Create
https://localhost:7122/CPackUI/Edit/{id}
https://localhost:7122/CPackUI/Delete/{id}


API 測試方法, powershell
Invoke-WebRequest -Uri "https://localhost:7122/api/CBox" -Method GET
Invoke-RestMethod -Uri "https://localhost:7122/api/CBox" -Method POST -Headers @{ "Content-Type"="application/json" } -Body '{ "_Key": 1, "_Name": "Sample", "_Level": 3, "_CreateTime": "2025-05-18T19:30:00" }'
Invoke-RestMethod -Uri "https://localhost:7122/api/CBox/1" -Method PUT -Headers @{ "Content-Type"="application/json" } -Body '{ "_Key": 1, "_Name": "Updated", "_Level": 5, "_CreateTime": "2025-05-18T19:30:00" }'
Invoke-RestMethod -Uri "https://localhost:7122/api/CBox/1" -Method DELETE


Invoke-WebRequest -Uri "https://localhost:7122/api/CPack" -Method GET
Invoke-RestMethod -Uri "https://localhost:7122/api/CPack" -Method POST -Headers @{ "Content-Type"="application/json" } -Body '{ "_SeqNo": 1, "_Msg": "Hello", "_Code": 42, "_UpdateTime": "2025-05-18T20:00:00" }'
Invoke-RestMethod -Uri "https://localhost:7122/api/CPack/1" -Method PUT -Headers @{ "Content-Type"="application/json" } -Body '{ "_SeqNo": 1, "_Msg": "Updated Message", "_Code": 99, "_UpdateTime": "2025-05-18T20:30:00" }'
Invoke-RestMethod -Uri "https://localhost:7122/api/CPack/1" -Method DELETE

