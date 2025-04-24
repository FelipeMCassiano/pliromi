using Communication.Requests;
using Exceptions;
using Validot;

namespace Pliromi.Application.UseCase.User.Register;

public  static class RegisterUserSpecification
{


	public static Specification<RequestRegisterUser> CreateSpecification()
	{
	Specification<RequestRegisterUser> specification = s => s
    .Member(m => m.Email, m => m.Email().WithMessage(PliromiUserMessagesErrors.InvalidEmail).And().NotEmpty().WithMessage(PliromiUserMessagesErrors.EmailCannotBeEmpty).And().LengthBetween(1, 255)).WithMessage(PliromiUserMessagesErrors.EmailInvalidLength)
    .And()
    .Member(m => m.FullName, m => m.NotEmpty().WithMessage(PliromiUserMessagesErrors.FullnameCannotBeEmpty).And().LengthBetween(1, 255)).WithMessage(PliromiUserMessagesErrors.FullnameInvalidLength)
    .And()
    .Member(m => m.Password, m => m.NotEmpty().WithMessage(PliromiUserMessagesErrors.PasswordCannotBeEmpty).And().LengthBetween(1, 255)).WithMessage(PliromiUserMessagesErrors.PasswordInvalidLength)
    .And()
    .Rule(m => !m.IsStore || (string.IsNullOrEmpty(m.Cpf) && !string.IsNullOrEmpty(m.Cnpj)))
    .WithMessage(PliromiUserMessagesErrors.StoreAccountsMustHaveOnlyACpnj)
    .And()
    .Rule(m => m.IsStore || (!string.IsNullOrEmpty(m.Cpf) && string.IsNullOrEmpty(m.Cnpj)))
    .WithMessage(PliromiUserMessagesErrors.PersonalAccountMustHaveOnlyACpf)
    .And()
    .Member(m => m.Balance, m => m.GreaterThan(0)).WithMessage(PliromiUserMessagesErrors.BalanceCannotBeLessThanZero);
		
	return specification;
		
	}

}