using AdministrationSystem.Eamv.Models.Services.Abstract;

namespace AdministrationSystem.Eamv.Models.Services
{
    public class AuthorAuthenServices : AWebBuilder
    {
        public AuthorAuthenServices(WebApplicationBuilder builder) : base(builder) { }

        public override void Invoke()
        {
            Builder.Services.AddAuthentication("EamvCookie").AddCookie("EamvCookie", options =>
            {
                options.Cookie.Name = "EamvCookie";
                options.LoginPath = "/Login/Index";
                options.AccessDeniedPath = "/Home/Accessdenied";
            });

            Builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("StandardUser", policy => policy.RequireClaim("StandardUser"));
                options.AddPolicy("AdminUser", policy => policy.RequireClaim("AdminUser"));
            });
        }
    }
}
