using AdministrationSystem.Eamv.Models.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

new DbContextServices(builder).Invoke();
new AuthorAuthenServices(builder).Invoke();
new ScopedServices(builder).Invoke();
var app = builder.Build();


app.Use(async (context, next) => // Used to redirect any HTTP Error 404 to the 404 view
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/Error/Index";
        await next();
    }
});

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Activity}/{action=CreateActivity}/{id?}");

app.Run();
