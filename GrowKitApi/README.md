# Web API

## Basic knowledge
This web applicatio has been written in C# and functions as API endpoint for devices to readand write information to the underlying database. We have chosen to make use of Postgres as our database provider due to the scalability factor and continues support from it's community. In case you want to change the database provider, you can change it (for example to a microsoft sql database) in the startup class by changing:
```
services.AddDbContext<AuthenticationContext>(options =>
{
    options.UseNpgsql(Configuration.GetConnectionString("AuthenticationDbString"));
});
services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(Configuration.GetConnectionString("ApplicationDbString"));
});
```
to:
```
services.AddDbContext<AuthenticationContext>(options =>
{
    options.UseSqlServer(Configuration.GetConnectionString("AuthenticationDbString"));
});
services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(Configuration.GetConnectionString("ApplicationDbString"));
});
```

## Database diagram
![Diagram]/GitResources/DatabaseDiagram.PNG?token=AFNCXVGHONJJNY7OZQVXENC5CYUC4?raw=true)

## Setting up
To make sure the application the application runs on your machine, please install the [.NET core runtime](https://dotnet.microsoft.com/download)

You can then change the database string in `appsettings.json` to point to your database, it can be run on the same machine. For settings used while debugging, change `appsettings.Development.json`

## Advice
The current endpoints are not production ready due to time constraints, it would be wise to have a second look atthem and to make sure unwanted acces is avoided.
The authorization service is not ready, as the middleware for authorization has to be set up. The autorization JWT sent tothe client should be added to the request header under as `Bearer <token>`
