# BlazoriseFluentValidation

Is an implementation of the [Balsorise](https://github.com/stsrki/Blazorise) IEditContextValidator to support [FluentValidation](https://github.com/FluentValidation/FluentValidation).

this implementation is based on the [EditoContextValidatior](https://github.com/stsrki/Blazorise/blob/master/Source/Blazorise/EditContextValidator.cs) and the [FluentValidator](https://github.com/ryanelian/FluentValidation.Blazor/blob/a12029fdf72a80a12cf9167a5c395fd797ea53e5/FluentValidation.Blazor/FluentValidator.cs#L100).

### Usage
First we have to register the required services

```markdown
services.AddBlazoriseWithFluentValidation();
```

> **Note:** the AddBlazoriseWithFluentValidation calls AddBlazorise method internally

so know you just have to implement your Model validatores and register them and you are good to go. I also added a sample Blazor Application.


> #### @icon-info-circle To learn more about Blazorise take a look at the [docs](https://blazorise.com/docs/)

> #### @icon-info-circle To learn more about FluentValidation take a look at the  [docs](https://docs.fluentvalidation.net/en/latest/index.html)




