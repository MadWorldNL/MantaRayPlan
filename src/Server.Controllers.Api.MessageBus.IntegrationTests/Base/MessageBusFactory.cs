using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace MadWorldNL.MantaRayPlan.Base;

public class MessageBusFactory : WebApplicationFactory<Program>
{
    private const string DbName = "MantaRayPlan";
    private const string DbUser = "postgres";
    private const string DbPassword = "Password";
    
    private PostgreSqlContainer? _postgreSqlContainer;

    public MessageBusFactory()
    {
        InitializeAsync().GetAwaiter().GetResult();
    }
    
    public async Task InitializeAsync()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithDatabase(DbName)
            .WithUsername(DbUser)
            .WithPassword(DbPassword)
            .Build();
        
        await _postgreSqlContainer.StartAsync();

    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("Database__Host", _postgreSqlContainer!.Hostname);
        builder.UseSetting("Database__Port", _postgreSqlContainer.GetMappedPublicPort(5432).ToString());
        builder.UseSetting("Database__DbName", DbName);
        builder.UseSetting("Database__User", DbUser);
        builder.UseSetting("Database__Password", DbPassword);
        
        base.ConfigureWebHost(builder);
    }
    
    public override async ValueTask DisposeAsync()
    {
        if (_postgreSqlContainer is not null)
        {
            await _postgreSqlContainer.DisposeAsync();
        }
        
        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}