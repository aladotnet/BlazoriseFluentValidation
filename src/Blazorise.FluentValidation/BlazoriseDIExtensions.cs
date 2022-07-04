using Blazorise.FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blazorise
{
    public static class BlazoriseDIExtensions
    {
        /// <summary>
        /// Adds an implementation of the FluentValidationHandler implementation of the IValidationHandler interface
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddFluentValidationHandler(this IServiceCollection services)
        {
            services.TryAddScoped(HandlerTypes.FluentValidation);
            return services;
        }

        /// <summary>
        /// Adds Balsorise services calling AddBlazorise and replaces the default Dataanotation based validation
        /// with the FluentValidation
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        [Obsolete("this extension is not compatible with the current Blazorise version")]
        public static IServiceCollection AddBlazoriseWithFluentValidation(this IServiceCollection services, Action<BlazoriseOptions> configureOptions = null)
        {
            services.AddBlazorise(configureOptions);

            services.Remove(ServiceDescriptor.Scoped(sp => sp.GetService<IEditContextValidator>()));
            services.AddScoped<IEditContextValidator, EditContextFluentValidator>();

            return services;
        }
    }
}
