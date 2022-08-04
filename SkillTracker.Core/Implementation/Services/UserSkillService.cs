using MediatR;
using Serilog;
using SkillTracker.Domain;
using System;

namespace SkillTracker.Core
{
    public class UserSkillService : IUserSkillService
    {
        private readonly IMediator _mediator;
        public UserSkillService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async void AddUserSkill(UserSkill userSkill)
        {
            try
            {
                await _mediator.Send(new CreateUserSkillCommand
                {
                    UserSkill = userSkill,
                    IsRabbitMqPublish = false
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, "Error Occured in userskillservice - AddUserSkill()");
            }
        }
    }
}
