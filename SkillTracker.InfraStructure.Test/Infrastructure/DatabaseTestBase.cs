using Microsoft.EntityFrameworkCore;
using System;

namespace SkillTracker.InfraStructure.Test
{
    public class DatabaseTestBase : IDisposable
    {
       protected readonly SkillTrackerDBContext Context;

       public DatabaseTestBase()
       {
            var options = new DbContextOptionsBuilder<SkillTrackerDBContext>().UseSqlServer("data source=CTSDOTNET318;initial catalog=SkillTrackerTest;uid=sa;pwd=pass@word1;packet size=4096;pooling=true;max pool size=500;Persist Security Info=True;Connect Timeout=60;").Options;

            Context = new SkillTrackerDBContext(options);

           Context.Database.EnsureCreated();

            DatabaseInitializer.Initialize(Context);
       }

        public void Dispose()
       {
            Context.Database.EnsureDeleted();

           Context.Dispose();
        }
   }
}
