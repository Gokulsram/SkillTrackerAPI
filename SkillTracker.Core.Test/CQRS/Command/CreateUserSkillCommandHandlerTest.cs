using FakeItEasy;
using FluentAssertions;
using SkillTracker.Domain;
using SkillTracker.RabbitMQ;
using Xunit;

namespace SkillTracker.Core.Test
{
    public class CreateUserSkillCommandHandlerTest
    {
        private readonly CreateUserSkillCommandHandler _createUserSkillCommandHandler;
        private readonly IUserProfileRepository _userSkillRepository;
        private readonly IUserSkillUpdateSender _userSkillUpdateSender;
        public CreateUserSkillCommandHandlerTest()
        {
            _userSkillRepository = A.Fake<IUserProfileRepository>();
            _userSkillUpdateSender = A.Fake<IUserSkillUpdateSender>();
           // _createUserSkillCommandHandler = new CreateUserSkillCommandHandler(_userSkillRepository, _userSkillUpdateSender);
        }
        [Fact]
        public async void Handle_ShouldCallAddAsync()
        {
            await _createUserSkillCommandHandler.Handle(new CreateUserSkillCommand(), default);

            A.CallTo(() => _userSkillRepository.AddUserProfile(A<UserSkill>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void Handle_ShouldReturnCreatedUserSkill()
        {
            A.CallTo(() => _userSkillRepository.AddUserProfile(A<UserSkill>._)).Returns(new BaseResponse
            {
                StatusDescription = "Success",
                ID = 1
            });

            var result = await _createUserSkillCommandHandler.Handle(new CreateUserSkillCommand(), default);

            result.Should().BeOfType<BaseResponse>();
            result.ID.Should().Be(1);
        }
    }
}
