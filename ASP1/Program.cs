using Microsoft.EntityFrameworkCore;
using ASP1.Data;
using ASP1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Seed Initial Data into Database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Ensure Database Created
    context.Database.EnsureCreated();

    if (!context.Departments.Any())
    {
        var dept = new Department { Name = "Computer Science", MgrName = "Dr. Ahmed" };
        context.Departments.Add(dept);
        context.SaveChanges();

        var course = new Course { Name = "ASP.NET Core", Degree = 100, MinDegree = 50, DepartmentId = dept.Id };
        context.Courses.Add(course);
        context.SaveChanges();

        var student = new Student { Name = "Mostafa Mahmoud", Age = 22, DepartmentId = dept.Id };
        context.Students.Add(student);
        context.SaveChanges();

        context.StuCrsRes.Add(new StuCrsRes { StudentId = student.Id, CourseId = course.Id, Grade = 95 });
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();