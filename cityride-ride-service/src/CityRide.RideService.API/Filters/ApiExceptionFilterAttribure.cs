using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CityRide.RideService.Application.Exceptions;

namespace CityRide.RideService.API.Filters;

public class ApiExceptionFilterAttribure : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandler;

    public ApiExceptionFilterAttribure()
    {
        _exceptionHandler = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(RideNotFoundException), HandleRideNotFoundException }
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
        if (_exceptionHandler.TryGetValue(type, out var value))
        {
            value.Invoke(context);

            return;
        }
    }

    private void HandleRideNotFoundException(ExceptionContext context)
    {
        var ex = (RideNotFoundException)context.Exception;
        context.Result = new ObjectResult(ex.Message)
        {
            StatusCode = ex.StatusCode
        };
        context.ExceptionHandled = true;
    }
}