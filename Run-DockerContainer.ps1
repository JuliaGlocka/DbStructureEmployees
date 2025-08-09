param(
    [string]$containerName = "mydbstructureemployees",
    [string]$imageName = "glockajulia/dbstructureemployees:latest",
    [int]$portHost = 5000,
    [int]$portContainer = 80
)

# Sprawdź, czy kontener istnieje
$existing = docker ps -a --filter "name=$containerName" --format "{{.ID}}"

if ($existing) {
    Write-Host "Stopping container $containerName ..."
    docker stop $containerName

    Write-Host "Removing container $containerName ..."
    docker rm $containerName
} else {
    Write-Host "Container $containerName does not exist. Proceeding..."
}

Write-Host "Running new container $containerName ..."
docker run -d -p ${portHost}:${portContainer} --name $containerName $imageName
