using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using API.Helpers;
using API.Middleware;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using API.Errors;
using API.Extensions;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddAutoMapper(typeof(MappingProfiles));
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StoreContext>(x => x.UseSqlite(connectionString));
builder.Services.AddApplicationServices();
builder.Services.AddCors(opt=>{
    opt.AddPolicy("CorsPolicy",policy=>{
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:7265");
    });
});
var app = builder.Build();

//Migration code -- Start
using (var scope=app.Services.CreateScope()){
    var services=scope.ServiceProvider;
    var loggerFactory=services.GetRequiredService<ILoggerFactory>();
    try{
        var context=services.GetRequiredService<StoreContext>();
        await context.Database.MigrateAsync();
        await StoreContextSeed.SeedAsync(context,loggerFactory);
    }
    catch(Exception ex){
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex,"An error occurred during migration");
    }
}

//Migration code -- End

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseSwaggerDocumentation();
app.UseCors("CorsPolicy");
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
