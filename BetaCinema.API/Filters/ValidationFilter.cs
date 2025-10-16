using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BetaCinema.API.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var parameter = context.ActionDescriptor.Parameters
                .FirstOrDefault(p => p.BindingInfo?.BindingSource == BindingSource.Body);

            if (parameter is null)
            {
                await next();
                return;
            }

            if (!context.ActionArguments.TryGetValue(parameter.Name, out var dto))
            {
                await next();
                return;
            }

            var validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);
            var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

            if (validator is null)
            {
                await next();
                return;
            }
            ValidationResult validationResult;

            if (dto is null)
            {

                validationResult = new ValidationResult(new[] {
                    new ValidationFailure("", "Request body không được để trống.")
                });

                context.Result = new BadRequestObjectResult(CreateErrorResponse(validationResult));
                return;
            }

            var validationContext = new ValidationContext<object>(dto);
            validationResult = await validator.ValidateAsync(validationContext);

            if (!validationResult.IsValid)
            {
                context.Result = new BadRequestObjectResult(CreateErrorResponse(validationResult));
                return;
            }

            await next();

        }

            

        private static object CreateErrorResponse(ValidationResult validationResult)
        {
            var errors = validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

            return new { errors };
        }
    }
}
