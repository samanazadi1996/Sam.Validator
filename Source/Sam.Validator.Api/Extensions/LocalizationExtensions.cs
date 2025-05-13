using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace Sam.Validator.Api.Extensions;


public static class LocalizationExtensions
{
    public static IServiceCollection AddCustomLocalization(this IServiceCollection services)
    {
        var defaultRequestCulture = new CultureInfo("en");

        List<CultureInfo> supportedCultures = new List<CultureInfo>()
        {
            defaultRequestCulture,
            new CultureInfo("en"),
            new CultureInfo("fa"),
            new CultureInfo("ru"),
            new CultureInfo("de"),
            new CultureInfo("fr"),
            new CultureInfo("es"),
            new CultureInfo("zh"),
            new CultureInfo("ja"),
            new CultureInfo("it"),
            new CultureInfo("tr"),
            new CultureInfo("ko"),
            new CultureInfo("hi")
        };

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(defaultRequestCulture);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });

        return services;
    }
    public static IApplicationBuilder UseCustomLocalization(this IApplicationBuilder app)
    {
        app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

        return app;
    }

}