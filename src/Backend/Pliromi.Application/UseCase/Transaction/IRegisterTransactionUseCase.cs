using Communication.Requests;

namespace Pliromi.Application.UseCase.Transaction;

public interface IRegisterTransactionUseCase
{
	Task Execute(RequestRegisterTransaction request);

}