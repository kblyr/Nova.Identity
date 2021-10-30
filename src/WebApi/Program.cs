using CodeCompanion.EntityFrameworkCore;
using CodeCompanion.Exceptions;
using CodeCompanion.Extensions.AspNetCore;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Nova.Common;
using Nova.Common.Validators;
using Nova.Identity;
using Nova.Identity.Data;
using Nova.Identity.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNovaIdentity()
    .AddData(builder.Configuration.GetConnectionString("NovaIdentity"));

builder.Services
    .AddRequestValidationProcessor()
    .AddRequestAccessValidationProcessor();

builder.Services.AddScoped<ICurrentFootprintProvider, CurrentFootprintProvider>();

builder.Services
    .Configure<BoundaryOptions>(builder.Configuration.GetSection(BoundaryOptions.ConfigKey))
    .Configure<ClientAppOptions>(builder.Configuration.GetSection(ClientAppOptions.ConfigKey))
    .Configure<RoleOptions>(builder.Configuration.GetSection(RoleOptions.ConfigKey))
    .Configure<PermissionOptions>(builder.Configuration.GetSection(PermissionOptions.ConfigKey));

builder.Services.AddProblemDetails(options => {
    options.IncludeExceptionDetails = (context, exception) => false;
    options.Map<CodeCompanionException>(exception => new ProblemDetails()
    {
        Title = "Invalid Request",
        Detail = exception.ClientMessage,
        Status = StatusCodes.Status400BadRequest
    }.AddExtension("errorData", exception.ErrorData));

    options.Map<ValidationException>(exception => new ProblemDetails()
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
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Nova.Identity.WebApi", Version = "v1" });
    c.UseNovaSchemaIds();
});

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
