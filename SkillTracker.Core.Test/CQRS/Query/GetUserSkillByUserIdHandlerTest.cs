using FakeItEasy;
using FluentAssertions;
using SkillTracker.Domain;
using System.Threading.Tasks;
using Xunit;

namespace SkillTracker.Core.Test
{
    public class GetUserSkillByUserIdHandlerTest
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly GetUserSkillByUserIdHandler _getUserSkillByUserIdHandler;
        private readonly int _id = 1;
        private readonly User _user;
        public GetUserSkillByUserIdHandlerTest()
        {
            _userProfileRepository = A.Fake<IUserProfileRepository>();
            _getUserSkillByUserIdHandler = new GetUserSkillByUserIdHandler(_userProfileRepository);
            _user = new User { UserID = 1 };
        }
        [Fact]
        public async Task Handle_WithValidId_ShouldReturnUserSkill()
        {
            A.CallTo(() => _userProfileRepository.GetUserProfileByUserId(_id)).Returns(_user);

            var result = await _getUserSkillByUserIdHandler.Handle(new GetUserSkillByUserIdQuery { UserId = _id }, default);

            A.CallTo(() => _userProfileRepository.GetUserProfileByUserId(_id)).MustHaveHappenedOnceExactly();
            result.UserID.Should().Be(1);
        }
    }
}
