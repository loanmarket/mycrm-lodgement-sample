using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.  
builder.Services.AddControllers(options => { options.Conventions.Add(new ApiExplorerConvention()); })
    .AddXmlSerializerFormatters()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });


//builder.Services.AddServicesSample(Configuration);

// builder.Services.AddProblemDetails(options =>
//             {
//                 options.IncludeExceptionDetails = (context, ex) => false;
//             });

builder.Services.AddHealthChecks().AddCheck("default", _ => HealthCheckResult.Healthy("The API is responding"));

builder.Services.AddSwaggerGen(c =>
{
    c.CustomOperationIds(e =>
        e.ActionDescriptor.AttributeRouteInfo?.Name
        ?? $"{e.ActionDescriptor.RouteValues["controller"]}_{e.ActionDescriptor.RouteValues["action"]}");
    c.SwaggerDoc(ApiExplorerConvention.LodgementApi,
        new OpenApiInfo { Title = "MyCRM Lodgement API (v1)", Version = "v1" });
    c.SwaggerDoc(ApiExplorerConvention.BackchannelApi,
        new OpenApiInfo { Title = "MyCRM Lodgement Backchannel API (v1)", Version = "v1" });

    c.CustomSchemaIds(x => x.FullName);
   
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.  
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"{ApiExplorerConvention.LodgementApi}/swagger.json", "MyCRM Lodgement API (v1)");
    c.SwaggerEndpoint($"{ApiExplorerConvention.BackchannelApi}/swagger.json", "MyCRM LodgementApi Backchannel API (v1)");
});

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();