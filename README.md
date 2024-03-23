# API setup

Before any thing make sure you have :<br>
- [dotnetCore 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) 
- microsoftSQL sever express you can downloadit from [here](https://go.microsoft.com/fwlink/p/?linkid=2216019&clcid=0x409&culture=en-us&country=us)<br>
- [microsoft entity framwork](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/7.0.7)<br>
- texteditor like [vscode](https://code.visualstudio.com/download) or an IDE like [visual studio](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&channel=Release&version=VS2022&source=VSLandingPage&cid=2030&passive=false)<br><br>


rename `.env.example` to `.env` and change the settings that sutes you environments<br>

make sure the databae you specified in the `CONNECTION` variable of your `.env` exists<br>
open the terminal (windows command prumpt or powershell) and run the migrations to create the tables of the database<br>
```
dotnet ef database update 
```
open the project in visual studio or vscode and run it<br>
API URL : https://localhost:7179/<br>
SWAGGER URL : https://localhost:7179/swagger/index.html
