using Amazon.Lambda.Core;

namespace Amazon.Lambda.Core
{
    public static class LambdaContextExtensions
    {
        public static object GetMainProperties(this ILambdaContext context) => new
        {
            context.AwsRequestId,
            context.FunctionName,
            context.FunctionVersion
        };
    }
}