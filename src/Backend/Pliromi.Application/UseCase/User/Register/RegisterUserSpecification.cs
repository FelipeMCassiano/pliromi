using Communication.Requests;
using Exceptions;
using Validot;

namespace Pliromi.Application.UseCase.User.Register;

public  static class RegisterUserSpecification
{


	public static Specification<RequestRegisterUser> CreateSpecification()
	{
	Specification<RequestRegisterUser> specification = s => s
    .Member(m => m.Email, m => m.Email().WithMessage(PliromiUserMessagesErrors.InvalidEmailFormat).And().NotEmpty().WithMessage(PliromiUserMessagesErrors.EmailRequired).And().LengthBetween(1, 255)).WithMessage(PliromiUserMessagesErrors.EmailLengthInvalid)
    .And()
    .Member(m => m.FullName, m => m.NotEmpty().WithMessage(PliromiUserMessagesErrors.FullNameRequired).And().LengthBetween(1, 255)).WithMessage(PliromiUserMessagesErrors.FullNameLengthInvalid)
    .And()
    .Member(m => m.Password, m => m.NotEmpty().WithMessage(PliromiUserMessagesErrors.PasswordRequired).And().LengthBetween(1, 255)).WithMessage(PliromiUserMessagesErrors.PasswordLengthInvalid)
    .And()
    .Rule(m => !m.IsStore || (string.IsNullOrEmpty(m.Cpf) && !string.IsNullOrEmpty(m.Cnpj)))
    .WithMessage(PliromiUserMessagesErrors.StoreAccountsMustHaveOnlyACpnj)
    .And()
    .Rule(m => m.IsStore || (!string.IsNullOrEmpty(m.Cpf) && string.IsNullOrEmpty(m.Cnpj)))
    .WithMessage(PliromiUserMessagesErrors.PersonalAccountMustHaveOnlyACpf)
    .And()
    .Member(m => m.Balance, m => m.GreaterThan(0)).WithMessage(PliromiUserMessagesErrors.NegativeBalanceNotAllowed);
		
	return specification;
		
	}

}