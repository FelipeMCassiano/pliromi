using Communication.Responses;
using Microsoft.AspNetCore.Mvc;
using Pliromi.API.Attributes;
using Pliromi.Application.UseCase.Dashboard;

namespace Pliromi.API.Controllers;

[ApiController]
[Route("[controller]")]
[AuthenticatedUser]
public class DashboardController: ControllerBase
{
	[HttpGet]
	[ProducesResponseType(typeof(ResponseDashboard),StatusCodes.Status200OK)]
	public async Task<IActionResult> Get([FromServices] IDashboardUseCase useCase)
	{
		var response = await useCase.Execute();
		return Ok(response);
	}
	
}