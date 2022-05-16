using AngularAdo.Web.App.Employee;
using AngularAdo.Web.App.Extensions;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var Nonce = Guid.NewGuid().ToString("n");

builder.Services.AddCors();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging(options =>
{
    options.AddConsole();
    options.AddDebug();
});

builder.Services.AddMvc()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddControllersWithViews();
builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureApiDocumentation();
builder.Services.ConfigureAuthentication(builder.Configuration);

// Add services to the container.
builder.Services.AddScoped<EmpleadoAccessData>();

var app = builder.Build();

app.ConfigureCors(app.Configuration);
app.ConfigureHeaders(Nonce);

app.UseRouting();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();

app.UseStaticFiles();
app.UseSwagger(Nonce);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapFallbackToFile("index.html"); ;

app.Run();