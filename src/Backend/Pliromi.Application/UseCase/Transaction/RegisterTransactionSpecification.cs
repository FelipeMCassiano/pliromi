using AutoMapper.Execution;
using Communication.Requests;
using Exceptions;
using Exceptions.ExceptionsBase;
using Validot;

namespace Pliromi.Application.UseCase.Transaction;

public  static class RegisterTransactionSpecification
{
	public static Specification<RequestRegisterTransaction> CreateSpecification()
	{
		Specification<RequestRegisterTransaction> specification = s => s
		                                                               .Member(m => m.Value, m => m.GreaterThan(0))
		                                                               .WithMessage(PliromiTransactionMessagesErrors.ValueMustBeGreaterThanZero)
		                                                               .And()
		                                                               .Member(m => m.PliromiKey, m => m.NotEmpty())
		                                                               .WithMessage(PliromiTransactionMessagesErrors.PliromiMustBeProvided);
			
		return specification;
	}
	
}