using Microsoft.Extensions.DependencyInjection;
using SkillTracker.Core;
using SkillTracker.InfraStructure;

namespace SkillTracker.API
{
    public static class ServiceConfiguration
    {
        public static void AddEfCoreRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericEfRepository<>), typeof(GenericEfRepository<>));
            services.AddScoped(typeof(ISkillDetailRepository), typeof(SkillDetailRepository));
            services.AddScoped(typeof(IUserProfileRepository), typeof(UserProfileRepository));
            services.AddScoped(typeof(IUserSkillMappingRepository), typeof(UserSkillMappingRepository));
        }
        public static void AddEntityServices(this IServiceCollection services)
        {
            services.AddScoped<ISkillDetailService, SkillDetailService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IUserSkillMappingService, UserSkillMappingService>();
        }
    }
}
