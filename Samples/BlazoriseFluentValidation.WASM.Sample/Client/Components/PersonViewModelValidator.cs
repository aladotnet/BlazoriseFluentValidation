using FluentValidation;

namespace BlazoriseFluentValidation.WASM.Sample.Client.Components
{
    public class PersonViewModelValidator : AbstractValidator<PersonViewModel>
    {
        public PersonViewModelValidator()
        {
            RuleFor(vm => vm.FirstName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(vm => vm.LastName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(vm => vm.Age)                
                .GreaterThanOrEqualTo(18);
        }
    }
}
