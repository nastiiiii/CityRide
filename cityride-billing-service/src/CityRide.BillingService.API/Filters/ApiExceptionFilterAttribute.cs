using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CityRide.BillingService.Application.Exceptions;

namespace CityRide.BillingService.API.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(RidePriceNotFoundException), HandleRidePriceNotFoundException }
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

    private void HandleRidePriceNotFoundException(ExceptionContext context)
    {
        var ex = (RidePriceNotFoundException)context.Exception;

        context.Result = new ObjectResult(ex.Message)
        {
            StatusCode = ex.StatusCode
        };

        context.ExceptionHandled = true;
    }
}