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
		                                                               .Rule(m =>
		                                                               {
			                                                               List<string?> values =
			                                                               [
				                                                               m.ReceiverCnpj, m.ReceiverCpf,
				                                                               m.ReceiverEmail
			                                                               ];

			                                                               return values.Count(x =>
				                                                                      !string.IsNullOrEmpty(x)) ==
			                                                                      1;
		                                                               }).WithMessage(PliromiTransactionMessagesErrors.MustProvideOneIdentifier);
			
		return specification;
	}
	
}