using FakeItEasy;
using FluentAssertions;
using SkillTracker.Core;
using SkillTracker.Domain;
using System;
using System.Linq;
using Xunit;

namespace SkillTracker.InfraStructure.Test.Implementation.Repository
{
    public class UserProfileRepositoryTest : DatabaseTestBase
    {
        private readonly SkillTrackerDBContext _userContext;
        private readonly UserProfileRepository _testee;
        private readonly UserProfileRepository _testeeFake;
        private readonly ICacheHelper _cacheHelper;
        private readonly UserSkill _newUser;
        public UserProfileRepositoryTest()
        {
            _userContext = A.Fake<SkillTrackerDBContext>();
            _testeeFake = new UserProfileRepository(_userContext, _cacheHelper);
            _testee = new UserProfileRepository(Context, _cacheHelper);
            _newUser = new UserSkill
            {

                Name = "Gokul",
                AssociateID = "CTS1001",
                Mobile = 1282123456,
                Email = "gokul@cts.com"
            };
        }
        [Theory]
        [InlineData("Changed")]
        public async void UpdateUserAsync_WhenUserIsNotNull_ShouldReturnUser(string associteName)
        {
            var user = Context.User.First();
            user.AssociateID = associteName;

            var result = await _testee.UpdateUserProfile(user);
            var userResult = await _testee.GetUserProfileByUserId(user.UserID);

            userResult.Should().BeOfType<User>();
            userResult.AssociateID.Should().Be(associteName);
        }
        [Fact]
        public void AddAsync_WhenEntityIsNull_ThrowsException()
        {
            _testee.Invoking(x => x.AddUserProfile(null)).Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async void CreateUserAsync_WhenUserIsNotNull_ShouldReturnUser()
        {
            var result = await _testee.AddUserProfile(_newUser);

            result.Should().BeOfType<BaseResponse>();
        }
        [Fact]
        public async void CreateUserAsync_WhenUserIsNotNull_ShouldShouldAddUser()
        {
            var UserCount = Context.User.Count();

            await _testee.AddUserProfile(_newUser);

            Context.User.Count().Should().Be(UserCount);
        }
        [Fact]
        public void UpdateAsync_WhenEntityIsNull_ThrowsException()
        {
            _testee.Invoking(x => x.UpdateUserProfile(null)).Should().ThrowAsync<ArgumentNullException>();
        }


    }

}

