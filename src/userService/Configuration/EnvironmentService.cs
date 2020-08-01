using System;

namespace UserService.Configuration
{
    public class EnvironmentService : IEnvironmentService
    {
        public EnvironmentService() =>
            EnvironmentName = Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.Stage);

        public string EnvironmentName { get; set; }
    }
}