using Communication.Requests;
using Communication.Responses;

namespace Pliromi.Application.UseCase.Login;

public interface ILoginUseCase
{
	Task<ResponseRegister> Execute(RequestLogin request);
}