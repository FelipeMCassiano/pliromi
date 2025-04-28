using Communication.Enums;
using Communication.Requests;
using Exceptions;
using Pliromi.Domain.Entities;
using Validot;

namespace Pliromi.Application.UseCase.User.Register;

public  static class RegisterUserSpecification
{


	public static Specification<RequestRegisterUser> CreateSpecification()
	{
Specification<RequestRegisterUser> specification = s => s
    .Member(m => m.Email, m => m
        .NotEmpty().WithMessage(PliromiUserMessagesErrors.InvalidEmail)
        .Email().WithMessage(PliromiUserMessagesErrors.InvalidEmail)
        .LengthBetween(1, 255).WithMessage(PliromiUserMessagesErrors.InvalidEmail)
    )
    
    .And()
    .Member(m => m.FullName, m => m
        .LengthBetween(1, 255).WithMessage(PliromiUserMessagesErrors.FullNameLengthInvalid)
    )
    
    .And()
    .Member(m => m.Password, m => m
        .LengthBetween(1, 255).WithMessage(PliromiUserMessagesErrors.PasswordLengthInvalid)
    )
    
    .And()
    .Rule(m => !m.IsStore || (string.IsNullOrEmpty(m.Cpf) && !string.IsNullOrEmpty(m.Cnpj)))
        .WithMessage(PliromiUserMessagesErrors.StoreAccountRequiresCnpjNoCpf)
    .And()
    .Rule(m => m.IsStore || (!string.IsNullOrEmpty(m.Cpf) && string.IsNullOrEmpty(m.Cnpj)))
        .WithMessage(PliromiUserMessagesErrors.PersonalAccountRequiresCpfNoCnpj)
    
    .And()
    .Member(m => m.Balance, m => m
        .GreaterThanOrEqualTo(
            0).WithMessage(PliromiUserMessagesErrors.NegativeBalanceNotAllowed)
    )
    .And()
    .Rule(m => Enum.IsDefined(typeof(PliromiKeyType), m.PliromiKeyType)).WithExtraMessage(PliromiUserMessagesErrors.InvalidPliromiKeyType);

	return specification;
	}

}