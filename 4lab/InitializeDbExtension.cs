using lab4.DbModels;
using Microsoft.AspNetCore.Builder;

namespace lab4
{
    public static class InitializeDbExtension
    {
        public static IApplicationBuilder UseInitializer(this IApplicationBuilder builder, SewingCompanyContext context)
        {
            return builder.UseMiddleware<InitializeDbMiddleware>(context);
        }
    }
}

