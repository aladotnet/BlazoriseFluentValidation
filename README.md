# BlazoriseFluentValidation 0.9.3
Blazorise has a couple of breacking changes since 0.9.3 and  [Validation](https://blazorise.com/news/release-notes/093/#validation) is one of the Areas that has been refactored. because of that a new implementation of the FluentValidation support was required.

### Usage
First we have to register the required services

```markdown
services.AddFluentValidationHandler();
```

#### Blazor markup
```markdown
<Validation HandlerType="HandlerTypes.FluentValidation">
```
> **Note:** you can also use typeof(FluentValidationHandler) instead of HandlerTypes.FluentValidation
> 
so know you just have to implement your Model validatores and register them and you are good to go. I also added a sample Blazor Application.

# BlazoriseFluentValidation 0.9.2
> **Note:** this is compatible with Blazorise 0.9.2.x (there are breacking changes in 0.9.3 concerning the validation topic - i will provide some help as soon as i can)
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



