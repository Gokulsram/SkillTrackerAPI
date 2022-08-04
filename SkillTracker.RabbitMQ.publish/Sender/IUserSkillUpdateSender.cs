using SkillTracker.Domain;

namespace SkillTracker.RabbitMQ
{
    public interface IUserSkillUpdateSender
    {
        void SendUserSkill(UserSkill userSkill);
    }
}
