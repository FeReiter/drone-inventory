namespace DroneInventory.Api.Errors;

public static class ApiProblemDetails
{
    public static IServiceCollection AddApiProblemDetails(this IServiceCollection services)
    {
        return services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Detail ??= context.ProblemDetails.Status >= 500
                    ? "Die Anfrage konnte wegen eines technischen Fehlers nicht verarbeitet werden."
                    : "Die Anfrage konnte nicht verarbeitet werden.";
            };
        });
    }

    public static void UseApiProblemDetails(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseStatusCodePages();
    }
}
