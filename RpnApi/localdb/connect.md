Here the command we have to type on the terminal to make connection to ms qql server localdb :
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=HamdiBEJI#200584' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest

dotnet add package Microsoft.Data.SqlClient;

https://www.connectionstrings.com/microsoft-data-sqlclient/

dotnet add package Microsoft.EntityFrameworkCore.SqlServer

https://www.c-sharpcorner.com/article/entity-framework-7-code-first-using-cli/