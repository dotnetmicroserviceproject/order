# Order Microservice

## Create and publish package

```powershell
$version="1.0.7"
$owner="dotnetmicroserviceproject"
$gh_pat="[PAT HERE]"

dotnet pack src\Order.Contracts\ --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/order -o ..\packages

dotnet nuget push ..\packages\Order.Contracts.$version.nupkg --api-key $gh_pat --source "github"

```
## Build the docker image 
```powershell 
$env:GH_OWNER="dotnetmicroserviceproject" 
$env:GH_PAT="[PAT HERE]" 

docker build --secret id=GH_OWNER --secret id=GH_PAT -t order.service:latest . 
```
## Run the docker image 
```powershell 

docker run -it --rm -p 5175:5175 --name order-service --network infra_backend -e MongoDbSettings__Host=mongo -e RabbitMQSettings__Host=rabbitmq order.service:latest
``` 