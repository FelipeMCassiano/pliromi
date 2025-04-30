using Communication.Requests;
using Pliromi.Application.UseCase.Transaction;
using Validot;

namespace CommonTestUtilities.Validators;

public static class TransactionRegisterValidatorBuilder
{

	public static IValidator<RequestRegisterTransaction> Build()
	{
		var specification = RegisterTransactionSpecification.CreateSpecification();
		
		return Validator.Factory.Create(specification);
	}
	
	
}