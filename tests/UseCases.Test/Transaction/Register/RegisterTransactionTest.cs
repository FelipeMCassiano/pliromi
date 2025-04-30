using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.ServiceBus;
using CommonTestUtilities.Validators;
using Exceptions;
using Exceptions.ExceptionsBase;
using Pliromi.Application.UseCase.Transaction;
using Shouldly;

namespace UseCases.Test.Transaction.Register;

public class RegisterTransactionTest
{
	[Fact]
	public async Task Success()
	{
		var (sender, _) = UserBuilder.BuildPerson();
		var (receiver, _) = UserBuilder.BuildStore();
		var useCase = CreateUseCase(sender, receiver);
		
		var request = RequestRegisterTransactionBuilder.Build(receiver.PliromiKey.Key);
		
		Func<Task> act = async () => await useCase.Execute(request);

		await act.ShouldNotThrowAsync();
	}

	[Fact]
	public async Task ErrorInsufficientBalance()
	{
		var (sender, _) = UserBuilder.BuildPerson();
		var (receiver, _) = UserBuilder.BuildStore();
		sender.Balance = 10;
		
		var useCase = CreateUseCase(sender, receiver);
		
		var request = RequestRegisterTransactionBuilder.Build(receiver.PliromiKey.Key);
		request.Value = 11;
		Func<Task> act = async () => await useCase.Execute(request);
		
		var ex = 	await act.ShouldThrowAsync<InsufficientBalanceException>();
		ex.GetErrorMessages().ShouldBe(new []{PliromiTransactionMessagesErrors.InsufficientBalance});
	}

	[Fact]
	public async Task ErrorSenderIsAStore()
	{
		var (sender, _) = UserBuilder.BuildStore();
		var (receiver, _) = UserBuilder.BuildStore();
		var useCase = CreateUseCase(sender, receiver);
		
		var request = RequestRegisterTransactionBuilder.Build(receiver.PliromiKey.Key);
		
		Func<Task> act = async () => await useCase.Execute(request);
	
		var ex = 	await act.ShouldThrowAsync<StoreCannotDoTransactionException>();
		ex.GetErrorMessages().ShouldBe(new []{PliromiTransactionMessagesErrors.StoreCannotDoTransactions});
	}

	[Fact]
	public async Task ErrorTransactionValueMustBeGreaterThanZero()
	{
		var (receiver, _) = UserBuilder.BuildPerson();
		var request = RequestRegisterTransactionBuilder.Build(receiver.PliromiKey.Key);
		request.Value = 0;
		var useCase = CreateUseCase(receiver, receiver);
		
		Func<Task> act = async () => await useCase.Execute(request);
		var ex = 	await act.ShouldThrowAsync<ErrorOnValidationException>();
		ex.GetErrorMessages().ShouldBe(new []{PliromiTransactionMessagesErrors.ValueMustBeGreaterThanZero});
	}

	[Fact]
	public async Task ErrorPliromiKeyNotProvided()
	{
		var (receiver, _) = UserBuilder.BuildPerson();
		var request = RequestRegisterTransactionBuilder.Build(receiver.PliromiKey.Key);
		request.PliromiKey = string.Empty;
		var useCase = CreateUseCase(receiver, receiver);
		Func<Task> act = async () => await useCase.Execute(request);
		var ex = 	await act.ShouldThrowAsync<ErrorOnValidationException>();
		ex.GetErrorMessages().ShouldBe(new []{PliromiTransactionMessagesErrors.PliromiMustBeProvided});
	}

	[Fact]
	public async Task ErrorNotFoundReceiver()
	{
		var (sender, _) = UserBuilder.BuildPerson();
		var (receiver, _) = UserBuilder.BuildStore();
		
		var useCase = CreateUseCase(sender);
		var request = RequestRegisterTransactionBuilder.Build(receiver.PliromiKey.Key);
		
		Func<Task> act = async () => await useCase.Execute(request);
		var ex =await act.ShouldThrowAsync<NotFoundUserException>();
		ex.GetErrorMessages().ShouldBe(new []{PliromiTransactionMessagesErrors.ReceiverNotFound});
	}
	

	private static RegisterTransactionUseCase CreateUseCase(Pliromi.Domain.Entities.User user, Pliromi.Domain
		.Entities.User? receiverUser = null)
	{
		var unitOfWork = IUnitOfWorkBuilder.Build();
		var mapper = MapperBuilder.Build();
		var repository = TransactionWriteOnlyRepositoryBuilder.Build();
		var loggedUser = LoggedUserBuilder.Build(user);
		var validator = TransactionRegisterValidatorBuilder.Build();
		var updateRepository = new UserUpdateOnlyRepositoryBuilder();
		var queue = SendEmailQueueBuilder.Build(); 
		
		return new RegisterTransactionUseCase(repository, unitOfWork, validator, mapper, loggedUser, updateRepository
			.Build(user, receiverUser), queue);
		
	}
	
}