using FakeItEasy;
using FluentAssertions;
using SkillTracker.Domain;
using System.Collections.Generic;
using Xunit;

namespace SkillTracker.Core.Test
{
    public class UpdateUserSkillCommandHandlerTest
    {
        private readonly UpdateUserSkillCommandHandler _updateUserSkillCommandHandler;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly int _userId = 1;

        public UpdateUserSkillCommandHandlerTest()
        {
            _userProfileRepository = A.Fake<IUserProfileRepository>();
            //_updateUserSkillCommandHandler = new UpdateUserSkillCommandHandler(_userProfileRepository);
        }

        [Fact]
        public async void Handle_ShouldReturnUpdatedUserSkill()
        {
            _ = A.CallTo(() => _userProfileRepository.EditUserProfile(default, default)).Returns(new BaseResponse
            {
                StatusDescription = "Success",
                ID = 1
            });

            var result = await _updateUserSkillCommandHandler.Handle(new UpdateUserSkillCommand(), default);

            result.Should().BeOfType<BaseResponse>();
            result.ID.Should().Be(_userId);
        }

        [Fact]
        public async void Handle_ShouldUpdateAsync()
        {
            await _updateUserSkillCommandHandler.Handle(new UpdateUserSkillCommand(), default);
            A.CallTo(() => _userProfileRepository.EditUserProfile(default, A<List<Skills>>._)).MustHaveHappenedOnceExactly();
        }
    }
}

