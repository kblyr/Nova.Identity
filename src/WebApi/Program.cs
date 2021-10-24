using CodeCompanion.EntityFrameworkCore;
using Nova.Identity;
using Nova.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNovaIdentity()
    .AddData(builder.Configuration.GetConnectionString("NovaIdentity"));

builder.Services.AddScoped<ICurrentFootprintProvider, CurrentFootprintProvider>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Nova.Identity.WebApi", Version = "v1" });
    c.CustomSchemaIds(type => type.FullName?.Replace($"{type.Namespace}.", ""));
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
app.UseAuthorization();
app.MapControllers();
app.Run();
