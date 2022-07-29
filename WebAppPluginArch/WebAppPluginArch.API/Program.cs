using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Reflection;
using System.Runtime.Loader;
using WebAppPluginArch.API;
using WebAppPluginArch.Shared;

var builder = WebApplication.CreateBuilder(args);

var baseDir = AppContext.BaseDirectory;

var pluginLocation = @"D:\work\develops\AngularProjects\bootset-workspace\NetPluginArchitecture\WebAppPluginArch\WebAppPluginArch.PluginOne\bin\Debug\net6.0\WebAppPluginArch.PluginOne.dll";

//Assembly assembly = Assembly.LoadFrom(pluginLocation);
//AssemblyPart part = new(assembly);
//builder.Services.AddControllers().PartManager.ApplicationParts.Add(part);

AssemblyLoadContext loadContext = new(pluginLocation);
// Assembly assembly = loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
Assembly assembly = loadContext.LoadFromAssemblyPath(pluginLocation);
AssemblyPart part = new(assembly);
builder.Services.AddControllers().PartManager.ApplicationParts.Add(part);
// builder.Services.AddControllersWithViews().AddApplicationPart(assembly);

var atypes = assembly.GetTypes();
var pluginClass = atypes.SingleOrDefault(t => t.GetInterface(nameof(IPlugin)) != null);

if (pluginClass != null)
{
    MethodInfo? initMethod = pluginClass.GetMethod(nameof(IPlugin.Initialize), BindingFlags.Public | BindingFlags.Instance);
    var obj = Activator.CreateInstance(pluginClass);
    Microsoft.Extensions.DependencyInjection.IServiceCollection services = builder.Services;
    var invokeRes = initMethod?.Invoke(obj, new object[] { services });
}

// Add services to the container.

// builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
