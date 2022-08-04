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
    public class EngineerControllerTest
    {
        private readonly IMediator _mediator;
        private readonly EngineerController _engineerController;
        private readonly UserSkill _userSkill;
        private readonly List<Skills> _skillList;
        private readonly int _id = 1;

        public EngineerControllerTest()
        {
            _mediator = A.Fake<IMediator>();
            _engineerController = new EngineerController(_mediator);

            _userSkill = new UserSkill
            {
                Name = "XUnitTest10001",
                AssociateID = "CTSX10001",
                Email = "xunit@email.com",
                Mobile = 1234567890,
                SkillList = new List<Skills>
                                                                        {
                                                                            new Skills { SkillName = "ANGULAR",ExpertiseLevel=10 },
                                                                            new Skills { SkillName = "REACT" ,ExpertiseLevel=20}
                                                                        }
            };

            _skillList = new List<Skills>
            {
                new Skills { SkillName = "ANGULAR",ExpertiseLevel=10 },
                new Skills { SkillName = "REACT" ,ExpertiseLevel=20}
            };

            BaseResponse baseResponse = new BaseResponse { ID = 1, StatusCode = 200, StatusDescription = "Success" };


            A.CallTo(() => _mediator.Send(A<CreateUserSkillCommand>._, default)).Returns(baseResponse);
            A.CallTo(() => _mediator.Send(A<UpdateUserSkillCommand>._, default)).Returns(baseResponse);
        }
        [Theory]
        [InlineData("AddUserProfile: user skill is null")]
        public async void Post_WhenAnExceptionOccurs_ShouldReturnBadRequest(string exceptionMessage)
        {
            A.CallTo(() => _mediator.Send(A<CreateUserSkillCommand>._, default)).Throws(new ArgumentException(exceptionMessage));

            var result = await _engineerController.AddUserProfile(_userSkill);

            (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result.Result as BadRequestObjectResult)?.Value.Should().Be(exceptionMessage);
        }

        [Theory]
        [InlineData("EditUserProfile: user skill is null")]
        [InlineData("No user with this userid found")]
        public async void Put_WhenAnExceptionOccurs_ShouldReturnBadRequest(string exceptionMessage)
        {
            A.CallTo(() => _mediator.Send(A<UpdateUserSkillCommand>._, default)).Throws(new Exception(exceptionMessage));

            var result = await _engineerController.EditUserProfile(1, _skillList);

            (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result.Result as BadRequestObjectResult)?.Value.Should().Be(exceptionMessage);
        }
        [Fact]
        public async void Post_ShouldReturnBaseResponse()
        {
            var result = await _engineerController.AddUserProfile(_userSkill);

            (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<BaseResponse>();
            result.Value.ID.Should().Be(_id);
        }

        [Fact]
        public async void Put_ShouldReturnBaseResponse()
        {
            var result = await _engineerController.EditUserProfile(1, _skillList);

            (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<BaseResponse>();
            result.Value.ID.Should().Be(_id);
        }
    }
}
