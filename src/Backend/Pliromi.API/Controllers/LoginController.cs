using Communication.Requests;
using Communication.Responses;
using Microsoft.AspNetCore.Mvc;
using Pliromi.Application.UseCase.Login;

namespace Pliromi.API.Controllers;


[Route("[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
	[HttpPost]
	[ProducesResponseType(typeof(ResponseRegister), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
	public async Task<ActionResult> Login([FromBody] RequestLogin request, [FromServices] ILoginUseCase useCase)
	{
		var response = await useCase.Execute(request);
		return Ok(response);
	}
	
}