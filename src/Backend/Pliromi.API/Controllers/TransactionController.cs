using System.Transactions;
using Communication.Requests;
using Microsoft.AspNetCore.Mvc;
using Pliromi.API.Attributes;
using Pliromi.Application.UseCase.Transaction;

namespace Pliromi.API.Controllers;

[Route("[controller]")]
[ApiController]
[AuthenticatedUser]
public class TransactionController: ControllerBase
{
	

	// add unprocessable entity status code
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<ActionResult> Post([FromServices] IRegisterTransactionUseCase useCase,[FromBody] RequestRegisterTransaction transaction)
	{
		await useCase.Execute(transaction );
		return Ok();
	}
	
}