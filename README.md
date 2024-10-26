1. To Run the Project 
Need to first restore the nugets:
or Install the following Packages:
2.
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Newtonsoft.Json

3. Change the DB Instance in Connection String to your DB

4. 
and run the DB Migrations

Add-Migration InitialCreate
Update-Database

5. add the auth token and base url of your original whatsapp 

run the Project and Enjoy!
