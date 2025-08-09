param(
    [string]$containerName = "mydbstructureemployees",
    [string]$imageName = "glockajulia/dbstructureemployees:latest",
    [int]$portHost = 5000,
    [int]$portContainer = 80,
    [string]$environment = "Development",   # or "Production"
    [string]$connectionString = ""          # optional: if empty, will use appsettings/default
)

function Test-PortInUse {
    param([int]$port)
    # Sprawdzamy, czy port jest używany (nasłuchuje proces)
    $tcpListeners = Get-NetTCPConnection -State Listen -LocalPort $port -ErrorAction SilentlyContinue
    return $tcpListeners -ne $null
}

# Sprawdź, czy port jest wolny
if (Test-PortInUse -port $portHost) {
    Write-Host ""
    Write-Host "ERROR: The port $portHost is already in use." -ForegroundColor Red
    Write-Host "Please stop the process using this port or choose a different port using the -portHost parameter." -ForegroundColor Yellow
    Write-Host "You can check which process is using the port by running:" -ForegroundColor Yellow
    Write-Host "    netstat -ano | findstr :$portHost" -ForegroundColor Cyan
    Write-Host "    tasklist /FI `"PID eq <PID>`"" -ForegroundColor Cyan
    exit 1
}

# Stop and remove existing container
$existing = docker ps -a --filter "name=$containerName" --format "{{.ID}}"
if ($existing) {
    Write-Host "Stopping container $containerName ..."
    docker stop $containerName | Out-Null
    Write-Host "Removing container $containerName ..."
    docker rm $containerName | Out-Null
} else {
    Write-Host "Container $containerName does not exist. Proceeding..."
}

# Build env parameters
$envParams = @(
    "-e", "ASPNETCORE_ENVIRONMENT=$environment"
)
if ($connectionString -ne "") {
    $envParams += @("-e", "ConnectionStrings__DefaultConnection=$connectionString")
}

# Run new container
Write-Host "Running new container $containerName on port $portHost..."
docker run -d `
    -p "${portHost}:${portContainer}" `
    --name $containerName `
    $envParams `
    $imageName
