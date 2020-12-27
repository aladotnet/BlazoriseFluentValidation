using FluentValidation;
using FluentValidation.Internal;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Blazorise.FluentValidation
{
    public class EditContextFluentValidator : IEditContextValidator
    {
        private readonly IServiceProvider _serviceProvider;

        public EditContextFluentValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

     

        /// <inheritdoc/>
        public virtual void ValidateField(EditContext editContext, ValidationMessageStore messages, in FieldIdentifier fieldIdentifier, Func<string, IEnumerable<string>, string> messageLocalizer)
        {
            messages.Clear(fieldIdentifier);

            var fluentValidator = TryGetValidatorForObjectType(fieldIdentifier.Model.GetType());

            var formatedResults = TryValidateField(fluentValidator, editContext, fieldIdentifier);

            var errors = formatedResults.Select(x => x.ErrorMessage).ToArray();

            messages.Add(fieldIdentifier, errors);

            // We have to notify even if there were no messages before and are still no messages now,
            // because the "state" that changed might be the completion of some async validation task
            editContext.NotifyValidationStateChanged();
        }

        /// <summary>
        /// Try acquiring the typed validator implementation from the DI.
        /// </summary>
        /// <param name="modelType"></param>
        /// <returns></returns>
        private IValidator TryGetValidatorForObjectType(Type modelType)
        {
            var validatorType = typeof(IValidator<>);
            var formValidatorType = validatorType.MakeGenericType(modelType);
            return _serviceProvider.GetService(formValidatorType) as IValidator;
        }

        /// <summary>
        /// Creates an instance of a ValidationContext for an object model.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="validatorSelector"></param>
        /// <returns></returns>
        private IValidationContext CreateValidationContext(object model, IValidatorSelector validatorSelector = null)
        {
            // This method is required due to breaking changes in FluentValidation 9!
            // https://docs.fluentvalidation.net/en/latest/upgrading-to-9.html#removal-of-non-generic-validate-overload

            if (validatorSelector == null)
            {
                // No selector specified - use the default.
                validatorSelector = ValidatorOptions.Global.ValidatorSelectors.DefaultValidatorSelectorFactory();
            }

            // Don't need to use reflection to construct the context. 
            // If you create it as a ValidationContext<object> instead of a ValidationContext<T> then FluentValidation will perform the conversion internally, assuming the types are compatible. 
            var context = new ValidationContext<object>(model, new PropertyChain(), validatorSelector);

            // InjectValidator looks for a service provider inside the ValidationContext with this key. 
            context.RootContextData["_FV_ServiceProvider"] = _serviceProvider;
            return context;
        }

        /// <summary>
        /// Attempts to validate a single field or property of a form model or child object model.
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="editContext"></param>
        /// <param name="fieldIdentifier"></param>
        /// <returns></returns>
        private IEnumerable<ValidationResult> TryValidateField(IValidator validator, EditContext editContext, FieldIdentifier fieldIdentifier)
        {
            try
            {
                var vselector = new MemberNameValidatorSelector(new[] { fieldIdentifier.FieldName });
                var vctx = CreateValidationContext(fieldIdentifier.Model, validatorSelector: vselector);
                var result = validator.Validate(vctx);

                return
                result.Errors
                .Select(e => new ValidationResult(e.ErrorMessage, new[] { e.PropertyName }));
            }
            catch (Exception ex)
            {
                var msg = $"An unhandled exception occurred when validating field name: '{fieldIdentifier.FieldName}'";

                if (editContext.Model != fieldIdentifier.Model)
                {
                    msg += $" of a child object of type: {fieldIdentifier.Model.GetType()}";
                }

                msg += $" of <EditForm> model type: '{editContext.Model.GetType()}'";
                throw new InvalidOperationException(msg, ex);
            }
        }

    }
}
