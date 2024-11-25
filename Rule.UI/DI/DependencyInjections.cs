using Rule.DAL.Repositories.Interfaces;
using Rule.DAL.UnitOfWork.Interfaces;
using Rule.DAL.UnitOfWork;
using Rule.DAL.Repositories;
using Rule.BL.Services;

namespace Rule.UI.DI
{
    public static class DependencyInjections
    {
        public static void AddDependencyInjections(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<UsersService>();
            services.AddTransient<PostsService>();
            services.AddTransient<FoundationsService>();
        }
    }
}
