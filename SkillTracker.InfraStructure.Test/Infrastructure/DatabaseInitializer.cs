using SkillTracker.Domain;
using System.Linq;

namespace SkillTracker.InfraStructure.Test
{
    public class DatabaseInitializer
    {
        public static void Initialize(SkillTrackerDBContext context)
        {
            if (context.User.Any())
            {
                return;
            }

            Seed(context);
        }

        private static void Seed(SkillTrackerDBContext context)
        {
            var user = new[]
            {
                new User
                {

                    Name ="Gokul",
                    AssociateID = "CTS10001",
                    Email = "gokul@cts.com",
                    Mobile = 1234567890
                },
                new User
                {

                    Name ="Selva",
                    AssociateID = "CTS10002",
                    Email = "selva@cts.com",
                    Mobile = 1234567890
                },
                new User
                {

                    Name ="Sankara",
                    AssociateID = "CTS10002",
                    Email = "sankara@cts.com",
                    Mobile = 1234567890
                }
            };

            context.User.AddRange(user);
            context.SaveChanges();
        }
    }
}
