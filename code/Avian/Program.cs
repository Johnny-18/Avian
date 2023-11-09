using Avian.Dal;
using Avian.Extensions;
using Avian.Infrastructure;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseDefaultServiceProvider((context, options) =>
    {
        options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
        options.ValidateOnBuild = true;
    });

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
        options.SuppressModelStateInvalidFilter = true;
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    });


builder.Services.AddInfrastructure();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DisplayRequestDuration();
        options.EnableValidator();
        options.DocumentTitle = "Avian service";
        options.SwaggerEndpoint(url: "v1", name: "v1");
    });
}

app.UseRouting();

app.UseCors(x =>
{
    x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var migrationRunner = MigrationRunner.Create(app);
var migrationsResult = await migrationRunner.Apply<AvianContext>();

if (migrationsResult == MigrationRunner.Result.TerminateService)
{
    await app.DisposeAsync();
    return 0;
}

app.Run();

return 0;