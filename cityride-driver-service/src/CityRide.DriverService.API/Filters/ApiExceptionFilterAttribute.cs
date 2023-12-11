using CityRide.DriverService.Application.Exceptions.DriverExceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CityRide.DriverService.API.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(DriverNotFoundException), HandleDriverNotFoundException }
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        if (_exceptionHandlers.TryGetValue(type, out Action<ExceptionContext>? value)) value.Invoke(context);
    }

    private void HandleDriverNotFoundException(ExceptionContext context)
    {
        var ex = (DriverServiceException)context.Exception;

        context.Result = new ObjectResult(ex.Message)
        {
            StatusCode = ex.StatusCode
        };

        context.ExceptionHandled = true;
    }
}