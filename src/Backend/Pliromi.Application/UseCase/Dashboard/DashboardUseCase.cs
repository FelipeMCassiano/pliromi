using AutoMapper;
using Communication.Responses;
using Pliromi.Domain.Repositories;
using Pliromi.Domain.Services.LoggedUser;

namespace Pliromi.Application.UseCase.Dashboard;

public class DashboardUseCase : IDashboardUseCase
{
	private readonly ILoggedUser _loggedUser;
	private readonly ITransactionReadOnlyRepository _readOnlyRepository;
	private readonly IMapper _mapper;
	private readonly IUserReadOnlyRepository _userReadOnlyRepository;

 
	public DashboardUseCase(ILoggedUser loggedUser, ITransactionReadOnlyRepository readOnlyRepository, IMapper mapper, IUserReadOnlyRepository userReadOnlyRepository)
	{
		_loggedUser = loggedUser;
		_readOnlyRepository = readOnlyRepository;
		_mapper = mapper;
		_userReadOnlyRepository = userReadOnlyRepository;
	}

	public async Task<ResponseDashboard> Execute()
	{
		var loggedUser =await _loggedUser.User();
		var transactions = await _readOnlyRepository.GetLastTransactions(loggedUser.Id);

		var income = 0;
		var outcome = 0;
		
		transactions.ForEach(t =>
		{
			if (t.ReceiverId == loggedUser.Id)
			{
				income += t.Value;
			}else if (t.SenderId == loggedUser.Id)
			{
				outcome += t.Value;
			}
		});

		var mappedTransactions = transactions.Select(t => _mapper.Map<ResponseDashboardTransaction>(t)).ToList();

		return new ResponseDashboard()
		{
			PliromiKey = await _userReadOnlyRepository.GetPliromiKeyByUserId(loggedUser.Id),
			Balance = loggedUser.Balance,
			Income = income,
			Outcome = outcome,
			LastTransactions = mappedTransactions
		};
	}
}