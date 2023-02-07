namespace AdministrationSystem.Eamv.Models.Services.Abstract
{
    public abstract class AWebBuilder
    {
        protected AWebBuilder(WebApplicationBuilder builder)
        {
            Builder = builder;
        }
        public abstract void Invoke();
        protected WebApplicationBuilder Builder { get; private set; }
    }
}
