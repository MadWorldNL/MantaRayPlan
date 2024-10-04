using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace MadWorldNL.MantaRayPlan.Base;

public class MessageBusFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string DbName = "MantaRayPlan";
    private const string DbUser = "postgres";
    private const string DbPassword = "Password1234!";
    
    private PostgreSqlContainer? _postgreSqlContainer;
    
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

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var newSettings = new Dictionary<string, string>
        {
            ["Database:Host"] = _postgreSqlContainer!.Hostname,
            ["Database:Port"] = _postgreSqlContainer.GetMappedPublicPort(5432).ToString(),
            ["Database:DbName"] = DbName,
            ["Database:User"] = DbUser,
            ["Database:Password"] = DbPassword
        };
        
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(newSettings!);
        });
        
        return base.CreateHost(builder);
    }
    
    public new async Task DisposeAsync()
    {
        if (_postgreSqlContainer is not null)
        {
            await _postgreSqlContainer.DisposeAsync();
        }
        
        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}