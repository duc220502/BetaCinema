using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Common
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class
    {
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehavior(IValidator<TRequest>? validator)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validator != null)
            {
                var result = await _validator.ValidateAsync(request, cancellationToken);
                if (!result.IsValid)
                {
                    var message = string.Join(" | ", result.Errors.Select(x => x.ErrorMessage));


                    if (!typeof(TResponse).IsGenericType || typeof(TResponse).GetGenericTypeDefinition() != typeof(ResponseObject<>))
                        throw new InvalidOperationException("TResponse must be of type ResponseObject<T>");

                    var method = typeof(ResponseObject<>)
                        .MakeGenericType(typeof(TResponse).GetGenericArguments()[0])
                        .GetMethod("ResponseError", BindingFlags.Public | BindingFlags.Static);

                    if (method == null)
                        throw new InvalidOperationException("ResponseError method not found");


                    return (TResponse)method.Invoke(null, new object[] { 400, message, default! })!;
                }
            }

            return await next();
        }
    }
}
