using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Reflection;
using WebAppPluginArch.Shared;

var builder = WebApplication.CreateBuilder(args);

Assembly assembly = Assembly
    .LoadFrom(@"D:\work\develops\AngularProjects\bootset-workspace\NetPluginArchitecture\WebAppPluginArch\WebAppPluginArch.PluginOne\bin\Debug\net6.0\WebAppPluginArch.PluginOne.dll");
var part = new AssemblyPart(assembly);
builder.Services.AddControllers().PartManager.ApplicationParts.Add(part);

var atypes = assembly.GetTypes();
var pluginClass = atypes.SingleOrDefault(t => t.GetInterface(nameof(IPlugin)) != null);

if (pluginClass != null)
{
    MethodInfo? initMethod = pluginClass.GetMethod(nameof(IPlugin.Initialize), BindingFlags.Public | BindingFlags.Instance);
    var obj = Activator.CreateInstance(pluginClass);
    var invokeRes = initMethod?.Invoke(obj, new object[] { builder.Services });
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
