using AdministrationSystem.Eamv.Models.EntityFramework;
using AdministrationSystem.Eamv.Models.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AdministrationSystem.Eamv.Models.Services
{
    public class DbContextServices : AWebBuilder
    {
        public DbContextServices(WebApplicationBuilder builder) : base(builder) {}

        public override void Invoke()
        {
            Builder.Services.AddDbContext<UserDbContext>(opts =>
            {
                opts.UseSqlServer(Builder.Configuration[$"ConnectionStrings:{DatabaseLocation}"]);
            });

            Builder.Services.AddDbContext<MainDbContext>(opts =>
            {
                opts.UseSqlServer(Builder.Configuration[$"ConnectionStrings:{MainDBLocation}"]);
            });

            Builder.Services.AddDbContext<FeedbackDbContext>(opts =>
            {
                opts.UseSqlServer(Builder.Configuration[$"ConnectionStrings:{FeedbackDBLocation}"]);
            });
        }

        const string DatabaseLocation = "UserDB";
        const string MainDBLocation = "MainDB";
        const string FeedbackDBLocation = "FeedbackDB";
    }
}
