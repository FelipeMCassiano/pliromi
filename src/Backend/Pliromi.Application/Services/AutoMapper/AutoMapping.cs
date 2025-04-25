using AutoMapper;
using Communication.Requests;
using Communication.Responses;
using Pliromi.Domain.Entities;

namespace Pliromi.Application.Services.AutoMapper;

public class AutoMapping:Profile
{
	public AutoMapping()
	{
		RequestToDomain();
		EntityToResponse();
	}

	private void RequestToDomain()
	{
		CreateMap<RequestRegisterUser, User>()
			.ForMember(dest => dest.Password, opt => opt.Ignore());
		CreateMap<RequestRegisterTransaction, ReceiverDataForTransaction>();
	}

	private void EntityToResponse()
	{
		CreateMap<Transaction, ResponseDashboardTransaction>()
			.ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.CreatedOn))
			.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));
		CreateMap<User, ResponseDashboardUser>()
			.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Fullname))
			.ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.Cpf))
			.ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.Cnpj));
	}
}