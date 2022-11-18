namespace WebAppiTest.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UserCorsDefault(this IApplicationBuilder app)
        {
            app.UseCors(config => config.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod().WithMethods("PUT", "DELETE", "GET", "POST"));
            return app;
        }
    }
}
