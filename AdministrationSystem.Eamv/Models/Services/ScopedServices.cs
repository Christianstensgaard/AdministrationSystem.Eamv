using AdministrationSystem.Eamv.Models.EntityFramework;
using AdministrationSystem.Eamv.Models.Interfaces;
using AdministrationSystem.Eamv.Models.Services.Abstract;

namespace AdministrationSystem.Eamv.Models.Services
{
    public class ScopedServices: AWebBuilder
    {
        public ScopedServices(WebApplicationBuilder builder) : base(builder) { }

        public override void Invoke()
        {
            Builder.Services.AddScoped<IUserRepository, EFUserRepository>();
            Builder.Services.AddScoped<IRepositoryCrud<Department>, EFDepartmentRepository>();
            Builder.Services.AddScoped<IRepositoryCrud<Room>, EFRoomRepository>();
            Builder.Services.AddScoped<IFeedbackRepository, EFFeedbackRepository>();
            Builder.Services.AddScoped<IActiveRepository, EFActivityRepository>();
            Builder.Services.AddScoped<IBannerRepository, EFBannerRepository>();
        }
    }
}
