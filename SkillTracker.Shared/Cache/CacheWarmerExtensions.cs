using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SkillTracker.Core;
using System;

namespace SkillTracker.Shared
{
    public static class CacheWarmerExtensions
    {
        public static void UseCacheWarmer(this IServiceCollection services)
        {
            try
            {
                Log.Information("Starting Cache Warmer");
                var serviceProvider = services.BuildServiceProvider();
                var cacheHelper = serviceProvider.GetService<ICacheHelper>();
                var skillDetail = serviceProvider.GetService<ISkillDetailService>();
                cacheHelper.SkillDetail = skillDetail.GetSkillDetail().Result;
                services.Add(new ServiceDescriptor(typeof(ICacheHelper), provider => cacheHelper, ServiceLifetime.Singleton));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occured in in Cache Warmer {ex}");
            }
        }
    }
}
