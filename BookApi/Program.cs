using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                options.JsonSerializerOptions.WriteIndented = true;
            });


// Add DbContext
builder.Services.AddDbContext<BookApiDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqLiteConnectionString")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
       {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookApi", Version = "v1" });
       } 

);




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
