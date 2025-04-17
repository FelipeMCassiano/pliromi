using Communication.Requests;
using Communication.Responses;
using Microsoft.AspNetCore.Mvc;
using Pliromi.Application.UseCase.User.Register;

namespace Pliromi.API.Controllers;


[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
	[HttpPost]
	[ProducesResponseType(typeof(ResponseRegister), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Register([FromServices] IRegisterUserUseCase useCase,
		[FromBody] RequestRegisterUser request)
	{
		var response = await useCase.Execute(request);
		return Created(string.Empty, response);
	}
}