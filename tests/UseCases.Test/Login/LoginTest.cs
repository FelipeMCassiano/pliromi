using System.Security.Authentication;
using CommonTestUtilities.AccessToken;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Validators;
using Exceptions;
using Exceptions.ExceptionsBase;
using Pliromi.Application.UseCase.Login;
using Pliromi.Domain.Repositories;
using Shouldly;

namespace UseCases.Test.Login;

public class LoginTest
{
	[Fact]
	public async Task Success()
	{
		var (user, password) = UserBuilder.BuildPerson();
		var request = RequestLoginBuilder.Build(user.Email, password);

		var useCase = CreateUseCase(user);
		
		Func<Task> act = async () => await useCase.Execute(request);
		
		await act.ShouldNotThrowAsync();

	}

	[Fact]
	public async Task ErrorInvalidCredentials()
	{
		var (user, _) = UserBuilder.BuildPerson();
		var request = RequestLoginBuilder.Build(user.Email, "fakePassword");
		var useCase = CreateUseCase(user);
		Func<Task> act = async () => await useCase.Execute(request);

		var ex = await act.ShouldThrowAsync<InvalidLoginException>();
		ex.GetErrorMessages().ShouldBe(new[] { PliromiLoginMessagesErrors.InvalidCredentials });
	}

	[Fact]
	public async Task ErrorInvalidEmail()
	{
			var (user, password) = UserBuilder.BuildPerson();
		var request = RequestLoginBuilder.Build(user.Email,password);
		request.Email = "TEsttsa.com";
		
		var useCase = CreateUseCase(user);
		Func<Task> act = async () => await useCase.Execute(request);

		var ex = await act.ShouldThrowAsync<ErrorOnValidationException>();
		ex.GetErrorMessages().ShouldBe(new[] { PliromiUserMessagesErrors.InvalidEmail  });
	}

	[Fact]
	public async Task ErrorInvalidPassword()
	{
		var (user, password) = UserBuilder.BuildPerson();
		var request = RequestLoginBuilder.Build(user.Email,password);
		request.Password = string.Empty;
		
		var useCase = CreateUseCase(user);
		Func<Task> act = async () => await useCase.Execute(request);

		var ex = await act.ShouldThrowAsync<ErrorOnValidationException>();
		ex.GetErrorMessages().ShouldBe(new[] { PliromiUserMessagesErrors.PasswordLengthInvalid  });
	}
	

	private static LoginUseCase CreateUseCase(Pliromi.Domain.Entities.User? user = null)
	{
		var repository = new UserReadOnlyRepositoryBuilder();
		var accessTokenGenerator = AccessTokenGeneratorBuilder.Build();
		var passwordEncrypter = PasswordEncrypterBuilder.Build();
		var validator = LoginValidatorBuilder.Build();

		repository.GetUserByEmail(user);
		
		return new LoginUseCase(repository.Build(), accessTokenGenerator, passwordEncrypter, validator);
	}
	
}