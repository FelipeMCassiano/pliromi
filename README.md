# Payment platform
This project was built using C#, .NET Core and Amazon SES

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![AWS](https://img.shields.io/badge/AWS-%23FF9900.svg?style=for-the-badge&logo=amazon-aws&logoColor=white)

## Instalation
1. Clone the repository
```bash
git clone https://github.com/FelipeMCassiano/pliromi
```
2. Run ```dotnet restore```
3. Update `appsettings.Development.json` putting your AWS credentials
```json
{
  "Settings" : {
    "SES": {
      "AccessKey" : "your_access_key",
      "SecretKey" : "your_secret_key",
      "EmailSource" : "your_email_source"
    }
  }
}
```
## Usage 
1. Start the app with ```bash dotnet run dotnet run --project /Pliromi/src/Backend/Pliromi.API
2. The api will be accessible and documented at [http://localhost:5085/swagger/index.html](http://localhost:5244/swagger/index.html)
```
