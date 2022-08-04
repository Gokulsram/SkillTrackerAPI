using FakeItEasy;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillTracker.Core;
using SkillTracker.Domain;
using SkillTracker.WebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace SkillTracker.API.Test
{
    public class AdminControllerTest
    {
        private readonly IMediator _mediator;
        private readonly AdminController _adminController;
        public AdminControllerTest()
        {
            _mediator = A.Fake<IMediator>();
            _adminController = new AdminController(_mediator);

            List<UserProfileDetail> userProfileDetails = new List<UserProfileDetail> {
                                                                                        new UserProfileDetail {
                                                                                            Name = "XUnitTest10001",
                                                                                        AssociateID = "CTSX10001",
                                                                                        Email = "xunit@email.com",
                                                                                        Mobile = 1234567890,UserID=1,
                                                                                        UserTechnicalSkillDetails=new List<UserTechnicalSkill>{
                                                                                            new UserTechnicalSkill {SkillDetailID=1,SkillName="",ExpertiseLevel=20,IsTechnical=true  } }
                                                                                        },
                                                                                             new UserProfileDetail {
                                                                                            Name = "XUnitTest10001",
                                                                                        AssociateID = "CTSX10001",
                                                                                        Email = "xunit@email.com",
                                                                                        Mobile = 1234567890,UserID=1
                                                                                             }};

            A.CallTo(() => _mediator.Send(A<GetUserSkillByTypeQuery>._, default)).Returns(userProfileDetails);
        }

        [Fact]
        public async void Get_ShouldReturnUserSkill()
        {
            var result = await _adminController.GetAllUserProfile("name", "XUnitTest10001");

            (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<List<UserProfileDetail>>();
            result.Value.Count.Should().Be(2);
        }

        [Theory]
        [InlineData("User Skill could not be loaded")]
        public async void Get_WhenAnExceptionOccurs_ShouldReturnBadRequest(string exceptionMessage)
        {
            A.CallTo(() => _mediator.Send(A<GetUserSkillByTypeQuery>._, default)).Throws(new Exception(exceptionMessage));

            var result = await _adminController.GetAllUserProfile("name", "XUnitTest10001");

            (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result.Result as BadRequestObjectResult)?.Value.Should().Be(exceptionMessage);
        }

    }
}
