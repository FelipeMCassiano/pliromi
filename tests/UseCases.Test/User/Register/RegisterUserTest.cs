using CommonTestUtilities;
using CommonTestUtilities.AccessToken;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Validators;
using Communication.Enums;
using Exceptions;
using Exceptions.ExceptionsBase;
using Microsoft.IdentityModel.Tokens;
using Pliromi.Application.UseCase.User.Register;
using Shouldly;

namespace UseCases.Test.User.Register;

public class RegisterUserTest
{
	[Fact]
	public async Task SuccessPerson()
	{
		var request = RequestRegisterUserPersonBuilder.Build();
		
		var useCase = CreateUseCase();
		Func<Task> act = async ()  => await useCase.Execute(request);

		await act.ShouldNotThrowAsync();
	}
	[Fact]
	public async Task SuccessStore()
	{
		var request = RequestRegisterUserStoreBuilder.Build();
		
		var useCase = CreateUseCase();
		Func<Task> act = async ()  => await useCase.Execute(request);

		await act.ShouldNotThrowAsync();
	}

	
	[Fact]
	public async Task ErrorInvalidEmail()
	{
		var request = RequestRegisterUserPersonBuilder.Build();
		request.Email = "Testtest.test.com";
		var useCase = CreateUseCase();
		
		Func<Task> act = async () => await useCase.Execute(request);
		
		var ex = await act.ShouldThrowAsync<ErrorOnValidationException>();

		ex.GetErrorMessages().ShouldBe(new []{PliromiUserMessagesErrors.InvalidEmail});
	}

	[Fact]
	public async Task ErrorInvalidEmptyFullName()
	{
		var request = RequestRegisterUserPersonBuilder.Build();
		request.FullName = string.Empty;
		var useCase = CreateUseCase();
		Func<Task> act = async () => await useCase.Execute(request);
		
		var ex = await act.ShouldThrowAsync<ErrorOnValidationException>();
		ex.GetErrorMessages().ShouldBe(new []{PliromiUserMessagesErrors.FullNameLengthInvalid});
	}

	[Fact]
	public async Task ErrorInvalidPassword()
	{
		var request = RequestRegisterUserPersonBuilder.Build();
		request.Password = string.Empty;
		var useCase = CreateUseCase();
		Func<Task> act = async () => await useCase.Execute(request);
		
		var ex = await act.ShouldThrowAsync<ErrorOnValidationException>();
		ex.GetErrorMessages().ShouldBe(new []{PliromiUserMessagesErrors.PasswordLengthInvalid});
	}

	[Fact]
	public async Task ErrorStoreWithACpj()
	{
		var request = RequestRegisterUserStoreBuilder.Build();
		request.Cpf = CpfAndCnpjBuilder.Cpf();
		
		var useCase = CreateUseCase();
		Func<Task> act = async () => await useCase.Execute(request);
		var ex = await act.ShouldThrowAsync<ErrorOnValidationException>();
		ex.GetErrorMessages().ShouldBe(new []{PliromiUserMessagesErrors.StoreAccountRequiresCnpjNoCpf});
	}
	[Fact]
	public async Task ErrorPersonWithCnpj()
	{
		var request = RequestRegisterUserStoreBuilder.Build();
		request.Cnpj = CpfAndCnpjBuilder.Cnpj();
		
		var useCase = CreateUseCase();
		Func<Task> act = async () => await useCase.Execute(request);
		var ex = await act.ShouldThrowAsync<ErrorOnValidationException>();
		ex.GetErrorMessages().ShouldBe(new []{PliromiUserMessagesErrors.PersonalAccountRequiresCpfNoCnpj});
	}

	[Fact]
	public async Task ErrorNegativeBalance()
	{
		var request = RequestRegisterUserPersonBuilder.Build();
		request.Balance = -1;
		var useCase = CreateUseCase();
		Func<Task> act = async () => await useCase.Execute(request);
		
		var ex = await act.ShouldThrowAsync<ErrorOnValidationException>();
		ex.GetErrorMessages().ShouldBe(new []{PliromiUserMessagesErrors.NegativeBalanceNotAllowed});
	}

	[Fact]
	public async Task ErrorInvalidPliromiKeyType()
	{
		var request = RequestRegisterUserPersonBuilder.Build();
		request.PliromiKeyType = (PliromiKeyType)10000;
		var useCase = CreateUseCase();
		Func<Task> act = async () => await useCase.Execute(request);
		var ex = await act.ShouldThrowAsync<ErrorOnValidationException>();
		ex.GetErrorMessages().ShouldBe(new []{PliromiUserMessagesErrors.InvalidPliromiKeyType});
	}

	private static RegisterUserUseCase CreateUseCase(Pliromi.Domain.Entities.User? user = null)
	{
		var mapper = MapperBuilder.Build();
		var unitOfWork = IUnitOfWorkBuilder.Build();
		var repositoryReadOnlyBuilder = new UserReadOnlyRepositoryBuilder();
		var repositoryWriteOnly = IUserWriteOnlyRepositoryBuilder.Build();
		var passwordEncrypter = PasswordEncrypterBuilder.Build();
		var validator = UserRegisterValidatorBuilder.Build();
		var accessGenerator = IAccessTokenGeneratorBuilder.Build();
		if (user is not null)
		{
			repositoryReadOnlyBuilder.ActiveUserWithCpfOrEmailOrCpnj(user);
		}
		
		return new RegisterUserUseCase(repositoryWriteOnly, unitOfWork, mapper, validator, accessGenerator,passwordEncrypter, repositoryReadOnlyBuilder.Build());
	}
	
}