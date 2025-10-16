using BetaCinema.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BetaCinema.API.Filters
{
    public class TransactionFilter (IUnitOfWork unitOfWork) : IAsyncActionFilter
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async  Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                await next();
                return;
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await next();

                if (result.Exception != null && !result.ExceptionHandled)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                }
                else 
                {
                    await _unitOfWork.CommitTransactionAsync();
                }
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
