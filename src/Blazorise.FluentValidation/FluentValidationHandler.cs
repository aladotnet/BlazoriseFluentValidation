using FluentValidation;
using FluentValidation.Internal;
using System;
using System.Linq;

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
                var selector = new MemberNameValidatorSelector(new[] { validation.FieldIdentifier.FieldName });

                var validatorEventArgs = new ValidatorEventArgs(newValidationValue);

                var model = validation.FieldIdentifier.Model;

                var context = new ValidationContext<object>(model, new PropertyChain(), selector);

                var validator = TryGetValidatorForObjectType(model.GetType());
                validation.NotifyValidationStarted();
                var result = validator.Validate(context);

                if (result.IsValid)
                    validation.NotifyValidationStatusChanged(ValidationStatus.Success);
                else
                {
                    var messages = result.Errors.Select(e => e.ErrorMessage);
                    validation.NotifyValidationStatusChanged(ValidationStatus.Error, messages);
                }
            }
            catch (Exception ex)
            {

                var msg = $"An unhandled exception occurred when validating field name: '{validation.FieldIdentifier.FieldName}'";

                if (validation.EditContext.Model != validation.FieldIdentifier.Model)
                {
                    msg += $" of a child object of type: {validation.FieldIdentifier.Model.GetType()}";
                }

                msg += $" of <EditForm> model type: '{validation.EditContext.Model.GetType()}'";

                throw new InvalidOperationException(msg, ex);
            }

        }

        private IValidator TryGetValidatorForObjectType(Type modelType)
        {
            var validatorType = typeof(IValidator<>);
            var formValidatorType = validatorType.MakeGenericType(modelType);
            return _serviceProvider.GetService(formValidatorType) as IValidator;
        }
    }
}
