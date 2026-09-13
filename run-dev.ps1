# Levanta backend (dotnet run) y frontend (npm run dev) cada uno en su propia ventana de PowerShell.
# Uso: desde la carpeta raiz del proyecto -> .\run-dev.ps1
# Cerrar cada ventana (o Ctrl+C dentro de ellas) detiene ese proceso de forma independiente.

$root = $PSScriptRoot

Start-Process powershell -ArgumentList @(
    "-NoExit",
    "-Command",
    "Set-Location '$root\Productos'; dotnet run"
)

Start-Process powershell -ArgumentList @(
    "-NoExit",
    "-Command",
    "Set-Location '$root\Productos.Web'; npm run dev"
)

Write-Host "Backend  -> http://localhost:5080"
Write-Host "Frontend -> http://localhost:5173"
