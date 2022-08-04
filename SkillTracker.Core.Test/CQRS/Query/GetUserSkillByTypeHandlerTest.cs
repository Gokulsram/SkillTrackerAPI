using FakeItEasy;
using FluentAssertions;
using SkillTracker.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SkillTracker.Core.Test
{
    public class GetUserSkillByTypeHandlerTest
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly GetUserSkillByTypeHandler _getUserSkillByTypeHandler;
        private readonly List<UserProfileDetail> _userProfileDetails;

        public GetUserSkillByTypeHandlerTest()
        {
            _userProfileRepository = A.Fake<IUserProfileRepository>();
            _getUserSkillByTypeHandler = new GetUserSkillByTypeHandler(_userProfileRepository);

            _userProfileDetails = new List<UserProfileDetail> { new UserProfileDetail { Name = "Gokul" }, new UserProfileDetail { Name = "Suresh" } };
        }
        [Fact]
        public async Task Handle_WithValidId_ShouldReturnUserSkill()
        {
            A.CallTo(() => _userProfileRepository.GetAllUserProfile(default)).Returns(_userProfileDetails);

            var result = await _getUserSkillByTypeHandler.Handle(new GetUserSkillByTypeQuery(), default);

            A.CallTo(() => _userProfileRepository.GetAllUserProfile(default)).MustHaveHappenedOnceExactly();
            result.Should().BeOfType<List<UserProfileDetail>>();
            result.Count.Should().Be(2);
        }
    }
}
