using Communication.Responses;

namespace Pliromi.Application.UseCase.Dashboard;

public interface IDashboardUseCase
{
	Task<ResponseDashboard> Execute();
}