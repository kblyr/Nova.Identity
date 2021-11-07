using System.Text.Json;
using CodeCompanion.EntityFrameworkCore;
using CodeCompanion.Exceptions;
using CodeCompanion.Extensions.AspNetCore;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Nova.Common;
using Nova.Common.Security.AccessValidation;
using Nova.Common.Security.AccessValidation.Exceptions;
using Nova.Common.Validators;
using Nova.Identity;
using Nova.Identity.Configuration;
using Nova.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNovaCommon()
    .WithDefaults()
    .WithDefaultsForWebApi();

builder.Services.AddNovaIdentity()
    .AddData(builder.Configuration.GetConnectionString("NovaIdentity"));

builder.Services
    .AddScoped<ICurrentFootprintProvider, CurrentFootprintProvider>()
    .AddHttpContextAccessor();

builder.Services.WithPipelineBehaviors()
    .AddRequestValidation()
    .AddRequestAccessValidation();

builder.Services
    .Configure<BoundaryOptions>(builder.Configuration.GetSection(BoundaryOptions.ConfigKey))
    .Configure<ClientAppOptions>(builder.Configuration.GetSection(ClientAppOptions.ConfigKey))
    .Configure<RoleOptions>(builder.Configuration.GetSection(RoleOptions.ConfigKey))
    .Configure<PermissionOptions>(builder.Configuration.GetSection(PermissionOptions.ConfigKey));

builder.Services.AddProblemDetails(options => {
    options.IncludeExceptionDetails = (context, exception) => false;
    options.Map<CodeCompanionException>(exception => new ProblemDetails
    {
        Title = "Invalid Request",
        Detail = exception.ClientMessage,
        Status = StatusCodes.Status400BadRequest
    }.AddExtension("errorData", exception.ErrorData));

    options.Map<ValidationException>(exception => new ProblemDetails
    {
        Title = "Validation Failed",
        Detail = exception.Message,
        Status = StatusCodes.Status400BadRequest
    }.AddExtension("validationErrors",
        exception.Errors.Select(error => new
        {
            propertyName = error.PropertyName,
            errorCode = error.ErrorCode,
            errorMessage = error.ErrorMessage
        })));

    options.Map<AccessValidationFailedException>(exception => new ProblemDetails()
    {
        Title = "Access Denied",
        Detail = "Accessing protected resource",
        Status = StatusCodes.Status403Forbidden
    }.AddExtension("validationErrors", 
        exception.FailedRules.Select(rule => new
        {
            ruleName = rule.RuleName,
            payload = rule.GetPayload()
        })));
});

builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Nova.Identity.WebApi", Version = "v1" });
    c.UseNovaSchemaIds();
});

builder.Services.AddAuthentication()
    .AddJwtBearerWithNovaDefaults(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nova.Identity.WebApi v1"));
}

app.UseCors(policy => {
    policy.AllowAnyHeader();
    policy.AllowAnyOrigin();
    policy.AllowAnyMethod();
});
app.UseHttpsRedirection();
app.UseProblemDetails();
app.UseAuthorization();
app.MapControllers();
app.Run();
