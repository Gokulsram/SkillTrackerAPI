using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SkillTracker.Domain;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SkillTracker.WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IOptions<AWSCofiguration> _awsConfiguration;
        public AuthenticationController(IOptions<AWSCofiguration> awsConfiguration)
        {
            _awsConfiguration = awsConfiguration;
        }
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<string>> Register(User user)
        {
            try
            {
                var cognito = new AmazonCognitoIdentityProviderClient(
                         new BasicAWSCredentials(_awsConfiguration.Value.AccessKey, _awsConfiguration.Value.SecretKey), RegionEndpoint.APSouth1);

                var signUpRequest = new SignUpRequest
                {
                    ClientId = _awsConfiguration.Value.CognitoClientID,
                    SecretHash = GetUserPoolSecretHash(user.Username, _awsConfiguration.Value.CognitoClientID, _awsConfiguration.Value.CognitoSecretID),
                    Username = user.Username,
                    Password = user.Password,
                    UserAttributes = new System.Collections.Generic.List<AttributeType>
                    {
                       new AttributeType
                            {
                                Name = "email",
                                Value = user.Email
                            }
                    }
                };

                var response = await cognito.SignUpAsync(signUpRequest);

                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        [HttpPost]
        [Route("signin")]
        public async Task<ActionResult<string>> SignIn(User user)
        {
            try
            {
                var cognito = new AmazonCognitoIdentityProviderClient(
                 new BasicAWSCredentials(_awsConfiguration.Value.AccessKey, _awsConfiguration.Value.SecretKey), RegionEndpoint.APSouth1);

                var authReq = new AdminInitiateAuthRequest
                {
                    UserPoolId = _awsConfiguration.Value.UserPoolId,
                    ClientId = _awsConfiguration.Value.CognitoClientID,
                    AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
                };
                authReq.AuthParameters.Add("USERNAME", user.Username);
                authReq.AuthParameters.Add("PASSWORD", user.Password);
                authReq.AuthParameters.Add("SECRET_HASH",
                    GetUserPoolSecretHash(user.Username, _awsConfiguration.Value.CognitoClientID, _awsConfiguration.Value.CognitoSecretID));

                var authResp = await cognito.AdminInitiateAuthAsync(authReq);

                return Ok(authResp.AuthenticationResult.IdToken);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }
        private static string GetUserPoolSecretHash(string userID, string clientID, string clientSecret)
        {
            string message = userID + clientID;
            byte[] keyBytes = Encoding.UTF8.GetBytes(clientSecret);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            HMACSHA256 hmacSha256 = new HMACSHA256(keyBytes);
            byte[] hashMessage = hmacSha256.ComputeHash(messageBytes);

            return Convert.ToBase64String(hashMessage);
        }
    }
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
