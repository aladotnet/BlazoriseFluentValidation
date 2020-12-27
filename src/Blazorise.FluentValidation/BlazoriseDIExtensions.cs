using Blazorise.FluentValidation;
using Blazorise.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blazorise
{
    public static class BlazoriseDIExtensions
    {
        /// <summary>
        /// Adds Balsorise services calling AddBlazorise and replaces the default Dataanotation based validation
        /// with the FluentValidation
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddBlazoriseWithFluentValidation(this IServiceCollection services, Action<BlazoriseOptions> configureOptions = null)
        {
            services.AddBlazorise(configureOptions);

            services.Remove(ServiceDescriptor.Scoped(sp => sp.GetService<IEditContextValidator>()));
            services.AddScoped<IEditContextValidator, EditContextFluentValidator>();

            return services;
        }
    }
}
