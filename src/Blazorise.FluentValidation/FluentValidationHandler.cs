using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blazorise.FluentValidation
{
    public class FluentValidationHandler : IValidationHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public FluentValidationHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Validate(IValidation validation, object newValidationValue)
        {
            try
            {
                var validator = CreateValidator(validation);

                validation.NotifyValidationStarted();
                var context = CreateContext(validation);
                var result = validator.Validate(context);

                NotifyValidationChanged(result,validation);
            }
            catch (Exception ex)
            {
                HandleException(ex, validation);
            }
        }

        public async Task ValidateAsync(IValidation validation, object newValidationValue, CancellationToken cancellationToken = default)
        {
            try
            {
                var validator = CreateValidator(validation);

                validation.NotifyValidationStarted();
                var context = CreateContext(validation);
                var result = await validator.ValidateAsync(context, cancellationToken);

                NotifyValidationChanged(result, validation);
            }
            catch (Exception ex)
            {
                HandleException(ex,validation);
            }
        }

        private static void HandleException(Exception exception, IValidation validation)
        {
            var msg = $"An unhandled exception occurred when validating field name: '{validation.FieldIdentifier.FieldName}'";

            if (validation.EditContext.Model != validation.FieldIdentifier.Model)
            {
                msg += $" of a child object of type: {validation.FieldIdentifier.Model.GetType()}";
            }

            msg += $" of <EditForm> model type: '{validation.EditContext.Model.GetType()}'";

            throw new InvalidOperationException(msg, exception);
        }

        private IValidator CreateValidator(IValidation validation)
        {
            var model = validation.FieldIdentifier.Model;
            return TryGetValidatorForObjectType(model.GetType());
        }

        private static ValidationContext<object> CreateContext(IValidation validation)
        {
            var selector = new MemberNameValidatorSelector(new[] { validation.FieldIdentifier.FieldName });

            return new ValidationContext<object>(validation.FieldIdentifier.Model, new PropertyChain(), selector);
        }

        private static void NotifyValidationChanged(ValidationResult result, IValidation validation)
        {
            if (result.IsValid)
                validation.NotifyValidationStatusChanged(ValidationStatus.Success);
            else
            {
                var messages = result.Errors.Select(e => e.ErrorMessage);
                validation.NotifyValidationStatusChanged(ValidationStatus.Error, messages);
            }
        }

        private IValidator TryGetValidatorForObjectType(Type modelType)
        {
            var validatorType = typeof(IValidator<>);
            var formValidatorType = validatorType.MakeGenericType(modelType);
            return _serviceProvider.GetRequiredService(formValidatorType) as IValidator;
        }
    }
}
