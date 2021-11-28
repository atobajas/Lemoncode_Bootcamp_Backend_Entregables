using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.Books.FunctionalTests.Utils
{
    public static class AuthorizationUtil
    {
        public static AuthenticationHeaderValue GetBasicAuthorizationHeaderValue(IConfiguration configuration)
        {
            var testUsername = configuration.GetValue<string>("BasicAuthentication:Username");
            var testPassword = configuration.GetValue<string>("BasicAuthentication:Password");
            var credentials = $"{testUsername}:{testPassword}";
            var credentialsBytes = Encoding.UTF8.GetBytes(credentials);
            var credentialsBase64 = Convert.ToBase64String(credentialsBytes);
            var result = new AuthenticationHeaderValue("Basic", credentialsBase64);
            return result;
        }
    }
}
