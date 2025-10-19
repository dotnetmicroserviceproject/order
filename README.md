# Order Microservice

## Create and publish package

```powershell
$version="1.0.2"
$owner="dotnetmicroserviceproject"
$gh_pat="[PAT HERE]"

dotnet pack src\Order.Contracts\ --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/order -o ..\packages

dotnet nuget push ..\packages\Order.Contracts.$version.nupkg --api-key $gh_pat --source "github"

```
