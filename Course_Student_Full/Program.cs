using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

//register DB context 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))

);

//adding controller IgnoreCycles 
//----------------------------------

builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;

});

var app = builder.Build();

// SCOPE populate database from json and Auto-Migrate 
//-----------------------------------------
using (var scope=app.Services.CreateScope())
{   
    //create dbContext    
    var dbContext=scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //auto migrate 
    dbContext.Database.Migrate();
    await SeedDatabase(dbContext);
}

app.MapControllers();


app.MapGet("/", () => "Hello World!");

app.Run();



//static method to populate database
async Task SeedDatabase(ApplicationDbContext dbContext)
{
    if (!dbContext.Students.Any() && !dbContext.Courses.Any()) //  Check if data already exists
    {
        var courseJson = await File.ReadAllTextAsync("wwwroot/data/courses.json");
        var studentJson = await File.ReadAllTextAsync("wwwroot/data/students.json");

        var courses = JsonSerializer.Deserialize<List<Course>>(courseJson);
        var students = JsonSerializer.Deserialize<List<Student>>(studentJson);

        if (courses != null && students != null)
        {
            await dbContext.Courses.AddRangeAsync(courses);
            await dbContext.Students.AddRangeAsync(students);
            await dbContext.SaveChangesAsync();
        }
    }
}


