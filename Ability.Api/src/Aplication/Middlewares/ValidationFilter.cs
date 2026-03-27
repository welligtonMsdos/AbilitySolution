using Ability.Api.src.Aplication.Common;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ability.Api.src.Aplication.Middlewares;

public class ValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {        
        var dto = context.ActionArguments.Values
            .FirstOrDefault(v => v != null &&
                                 v.GetType() != typeof(string) && 
                                 !v.GetType().IsValueType);

        if (dto != null)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(dto.GetType());

            var validator = _serviceProvider.GetService(validatorType) as IValidator;

            if (validator != null)
            {
                var validationContext = new ValidationContext<object>(dto);

                var validationResult = await validator.ValidateAsync(validationContext);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );

                    context.Result = new BadRequestObjectResult(Result<object>.Failure(errors));

                    return;
                }
            }
        }
        else if (context.ActionArguments.Any(x => x.Value == null))
        {
            context.Result = new BadRequestObjectResult(Result<object>.Failure("O corpo da requisição não pode ser vazio."));

            return;
        }

        await next();
    }
}

