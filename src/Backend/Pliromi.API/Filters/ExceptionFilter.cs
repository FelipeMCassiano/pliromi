using Communication.Responses;
using Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Pliromi.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
	public void OnException(ExceptionContext context)
	{
		if (context.Exception is PliromiException pliromiException)
		{
			HandleProjectException(pliromiException, context);
			return;
		}
		ThrowUnknownException(context);
	}

	private static void HandleProjectException(PliromiException ex, ExceptionContext context)
	{
		switch (ex)
		{
			case ErrorOnValidationException errorOnValidationException:
				context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
				context.Result = new BadRequestObjectResult(new ResponseError(errorOnValidationException.GetErrorMessages()));
				break;
			case UnauthorizedException unauthorizedException:
				context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
				context.Result = new UnauthorizedObjectResult(new ResponseError(unauthorizedException.GetErrorMessages()));
				break;
			case InsufficientBalanceException insufficientBalanceException:
				context.HttpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
				context.Result = new UnprocessableEntityObjectResult(new ResponseError(insufficientBalanceException.GetErrorMessages()));
				break;
			case NotFoundUserException notFoundUserException:
				context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
				context.Result = new NotFoundObjectResult(new ResponseError(notFoundUserException.GetErrorMessages()));
				break;
			case InvalidLoginException invalidLoginException:
				context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
				context.Result = new UnauthorizedObjectResult(new ResponseError(invalidLoginException.GetErrorMessages()));
				break;
			case AlreadyRegisteredException alreadyRegisteredException:
				context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
				context.Result = new BadRequestObjectResult(new ResponseError(alreadyRegisteredException.GetErrorMessages()));
				break;
			case StoreCannotDoTransactionException storeCannotDoTransactionException:
				context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
				context.Result = new BadRequestObjectResult(new ResponseError(storeCannotDoTransactionException.GetErrorMessages()));
				break;
		}
		
	}

	private static void ThrowUnknownException(ExceptionContext context)
	{
		
		context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
		context.Result = new ObjectResult(new ResponseError(context.Exception.ToString()));
	}
}