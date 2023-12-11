using CityRide.ClientService.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CityRide.ClientService.API.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilterAttribute()
        {
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>()
            {
                { typeof(ClientNotFoundException), HandleClientNotFoundException},
            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
            base.OnException(context);
        }
        private void HandleException(ExceptionContext context)
        {
            Type type = context.Exception.GetType();
            if (_exceptionHandlers.TryGetValue(type, out Action<ExceptionContext?> action)) {

                action.Invoke(context);
                return;
            }
        }
        private void HandleClientNotFoundException(ExceptionContext context)
        {
            var exception = (ClientNotFoundException)context.Exception;
            context.Result = new ObjectResult(exception.Message)
            {
                StatusCode = exception.ErrorCode
            };
            context.ExceptionHandled = true;
        }
    }
}
