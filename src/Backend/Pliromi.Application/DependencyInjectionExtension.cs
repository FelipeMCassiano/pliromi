using AutoMapper;
using Communication.Requests;
using Microsoft.Extensions.DependencyInjection;
using Pliromi.Application.Services.AutoMapper;
using Pliromi.Application.UseCase.Login;
using Pliromi.Application.UseCase.Transaction;
using Pliromi.Application.UseCase.User.Register;
using Validot;

namespace Pliromi.Application;

public static class DependencyInjectionExtension
{
	public static void AddApplication(this IServiceCollection services)
	{
		AddAutoMapper(services);
		AddUseCases(services);
		AddValidators(services);
	}

	private static void AddAutoMapper(IServiceCollection services)
	{
		services.AddScoped(_ => new MapperConfiguration(config =>
		{
			config.AddProfile(new AutoMapping());
		}).CreateMapper());
	}

	private static void AddUseCases(IServiceCollection services)
	{
		services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
		services.AddScoped<IRegisterTransactionUseCase, RegisterTransactionUseCase>();
		services.AddScoped<ILoginUseCase, LoginUseCase>();
	}

	private static void AddValidators(IServiceCollection services)
	{
		var userRegisterSpecification = RegisterUserSpecification.CreateSpecification();
		var userRegisterValidator = Validator.Factory.Create(userRegisterSpecification); 
		
		var transactionRegisterSpecification = RegisterTransactionSpecification.CreateSpecification();
		var transactionRegisterValidator = Validator.Factory.Create(transactionRegisterSpecification);
		
		var loginSpecification = LoginSpecification.CreateSpecification();
		var loginValidator = Validator.Factory.Create(loginSpecification);
		
		services.AddSingleton<IValidator<RequestRegisterTransaction>>(transactionRegisterValidator);
		services.AddSingleton<IValidator<RequestRegisterUser>>(userRegisterValidator);
		services.AddSingleton<IValidator<RequestLogin>>(loginValidator);

	}
	
}