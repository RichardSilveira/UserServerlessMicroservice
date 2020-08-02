using System;

namespace UserService.Configuration
{
    /// <summary>
    /// Tiny class that generates some hosting environment info for your Lambda before your Lambda handler method being executed.
    /// There is no need to work with IHostingEnvironment because the environment info should be collected from ILambdaContext -
    /// when your handler method is invoked.
    /// Plus there is no ASP.Core runtime - to work with you need to use other approach with Amazon.Lambda.AspNetCoreServer.
    ///  https://aws.amazon.com/pt/blogs/developer/running-serverless-asp-net-core-web-apis-with-amazon-lambda/
    /// </summary>
    public class EnvironmentService : IEnvironmentService
    {
        public EnvironmentService() =>
            EnvironmentName = Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.Stage);

        public string EnvironmentName { get; set; }
    }
}