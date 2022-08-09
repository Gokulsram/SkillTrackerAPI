using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SkillTracker.Core;
using SkillTracker.Domain;
using System.Collections.Generic;

namespace SkillTracker.WebAPI
{
    public static class ServiceConfiguration
    {
        public static void AddEfCoreRepository(this IServiceCollection services)
        {
            //services.AddScoped(typeof(IGenericEfRepository<>), typeof(GenericEfRepository<>));
            //services.AddScoped(typeof(ISkillDetailRepository), typeof(SkillDetailRepository));
            //services.AddScoped(typeof(IUserProfileRepository), typeof(UserProfileRepository));
            //services.AddScoped(typeof(IUserSkillMappingRepository), typeof(UserSkillMappingRepository));
            //services.AddScoped(typeof(IMemCacheHelper), typeof(MemCacheRepository));
        }
        public static void AddEntityServices(this IServiceCollection services)
        {
            //services.AddScoped<ISkillDetailService, SkillDetailService>();
            //services.AddScoped<IUserProfileService, UserProfileService>();
            //services.AddScoped<IUserSkillMappingService, UserSkillMappingService>();
        }
        public static void AddCQRSServices(this IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<CreateUserSkillCommand, BaseResponse>, CreateUserSkillCommandHandler>();
            //services.AddTransient<IRequestHandler<UpdateUserSkillCommand, BaseResponse>, UpdateUserSkillCommandHandler>();
            // services.AddTransient<IRequestHandler<GetUserSkillByUserIdQuery, User>, GetUserSkillByUserIdHandler>();
            services.AddTransient<IRequestHandler<GetUserSkillByTypeQuery, List<UserSkill>>, GetUserSkillByTypeHandler>();
        }
    }
}
