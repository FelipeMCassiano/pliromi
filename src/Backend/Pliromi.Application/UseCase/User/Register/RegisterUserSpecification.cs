using Communication.Requests;
using Validot;

namespace Pliromi.Application.UseCase.User.Register;

public  static class RegisterUserSpecification
{


	public static Specification<RequestRegisterUser> CreateSpecification()
	{
	Specification<RequestRegisterUser> specification = s => s
    .Member(m => m.Email, m => m.Email().NotEmpty().LengthBetween(1, 255))
    .And()
    .Member(m => m.FullName, m => m.NotEmpty().LengthBetween(1, 255))
    .And()
    .Member(m => m.Password, m => m.NotEmpty().LengthBetween(1, 255))
    .And()
    .Rule(m => !m.IsStore || (string.IsNullOrEmpty(m.Cpf) && !string.IsNullOrEmpty(m.Cnpj)))
    .And()
    .Rule(m => m.IsStore || (!string.IsNullOrEmpty(m.Cpf) && string.IsNullOrEmpty(m.Cnpj)))
    .And()
    .Member(m => m.Balance, m => m.GreaterThan(0));
	return specification;
		
	}

}