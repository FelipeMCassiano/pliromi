using Communication.Requests;
using Communication.Responses;

namespace Pliromi.Application.UseCase.User.Register;

public interface IRegisterUserUseCase
{
	Task<ResponseRegister> Execute(RequestRegisterUser request);

}