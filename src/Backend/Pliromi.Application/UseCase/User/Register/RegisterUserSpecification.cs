using Communication.Requests;
using Exceptions;
using Validot;

namespace Pliromi.Application.UseCase.User.Register;

public  static class RegisterUserSpecification
{


	public static Specification<RequestRegisterUser> CreateSpecification()
	{
Specification<RequestRegisterUser> specification = s => s
    // Email (separate messages for format, empty, and length)
    .Member(m => m.Email, m => m
        .NotEmpty().WithMessage(PliromiUserMessagesErrors.EmailRequired)
        .Email().WithMessage(PliromiUserMessagesErrors.InvalidEmailFormat)
        .LengthBetween(1, 255).WithMessage(PliromiUserMessagesErrors.EmailLengthInvalid)
    )
    
    // Full Name (separate messages for empty and length)
    .And()
    .Member(m => m.FullName, m => m
        .NotEmpty().WithMessage(PliromiUserMessagesErrors.FullNameRequired)
        .LengthBetween(1, 255).WithMessage(PliromiUserMessagesErrors.FullNameLengthInvalid)
    )
    
    // Password (separate messages for empty and length)
    .And()
    .Member(m => m.Password, m => m
        .NotEmpty().WithMessage(PliromiUserMessagesErrors.PasswordRequired)
        .LengthBetween(1, 255).WithMessage(PliromiUserMessagesErrors.PasswordLengthInvalid)
    )
    
    // Store Account Rules (CPF/CNPJ logic)
    .And()
    .Rule(m => !m.IsStore || (string.IsNullOrEmpty(m.Cpf) && !string.IsNullOrEmpty(m.Cnpj)))
        .WithMessage(PliromiUserMessagesErrors.StoreAccountsMustHaveOnlyACpnj)
    .And()
    .Rule(m => m.IsStore || (!string.IsNullOrEmpty(m.Cpf) && string.IsNullOrEmpty(m.Cnpj)))
        .WithMessage(PliromiUserMessagesErrors.PersonalAccountMustHaveOnlyACpf)
    
    // Balance
    .And()
    .Member(m => m.Balance, m => m
        .GreaterThan(0).WithMessage(PliromiUserMessagesErrors.NegativeBalanceNotAllowed)
    );

	return specification;
	}

}