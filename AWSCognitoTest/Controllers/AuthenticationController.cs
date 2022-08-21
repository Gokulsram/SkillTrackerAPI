using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AWSCognitoTest.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private const string _clientId = "4ii01vcbrgq2piqs8mehc43p0v";
        private readonly RegionEndpoint _region = RegionEndpoint.APSouth1;

        public AuthenticationController()
        {

        }
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<string>> Register(User user)
        {
            try
            {
                AmazonCognitoIdentityProviderClient cognito = new AmazonCognitoIdentityProviderClient(
                  new AnonymousAWSCredentials());

                //  var cognito = new AmazonCognitoIdentityProviderClient(
                //new BasicAWSCredentials("AKIA4ZH6BSWG6SD3S3S4", "5tCM2UQJZD492pY0u2aO0KfueTLDi4IsBz9A8bzL"), _region);

                //var request = new SignUpRequest
                //{
                //    ClientId = _clientId,
                //    Password = user.Password,
                //    Username = user.Username
                //};

                //var emailAttribute = new AttributeType
                //{
                //    Name = "email",
                //    Value = user.Email
                //};
                //request.UserAttributes.Add(emailAttribute);

                var signUpRequest = new SignUpRequest
                {
                    ClientId = _clientId,
                    SecretHash = GetUserPoolSecretHash(user.Username, _clientId, "de84rqcgf5q1nmj79gg9eko6pj34qdoiul4pbjd70is70bkqc3k"),
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

                //await cognito.SignUpAsync(signUpRequest);

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
            //private AmazonCognitoIdentityProviderClient provider;
            var cognito = new AmazonCognitoIdentityProviderClient(
                 new BasicAWSCredentials("AKIA4ZH6BSWG7PRMRUPH", "q4wjWnngs5ITe/p/nKFFiErgjzThDiKbkJR/1X2/"), _region);

            //     var request = new AdminInitiateAuthRequest
            //{
            //    UserPoolId = "ap-south-1_IcIWkqxRU",
            //    ClientId = _clientId,
            //    AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH
            //};

            //request.AuthParameters.Add("USERNAME", user.Username);
            //request.AuthParameters.Add("PASSWORD", user.Password);

            //var response = await cognito.AdminInitiateAuthAsync(request);

            var authReq = new AdminInitiateAuthRequest
            {
                UserPoolId = "ap-south-1_IcIWkqxRU",
                ClientId = _clientId,
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
            };
            authReq.AuthParameters.Add("USERNAME", user.Username);
            authReq.AuthParameters.Add("PASSWORD", user.Password);
            authReq.AuthParameters.Add("SECRET_HASH", GetUserPoolSecretHash(user.Username, _clientId, "de84rqcgf5q1nmj79gg9eko6pj34qdoiul4pbjd70is70bkqc3k"));

            var authResp = await cognito.AdminInitiateAuthAsync(authReq);

            return Ok(authResp.AuthenticationResult.IdToken);
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
