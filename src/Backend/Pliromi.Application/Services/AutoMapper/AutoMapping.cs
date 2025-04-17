using AutoMapper;
using Communication.Requests;
using Pliromi.Domain.Entities;

namespace Pliromi.Application.Services.AutoMapper;

public class AutoMapping:Profile
{
	public AutoMapping()
	{
		RequestToDomain();
	}

	private void RequestToDomain()
	{
		CreateMap<RequestRegisterUser, User>()
			.ForMember(dest => dest.Email, opt => opt.Ignore());
		CreateMap<RequestRegisterTransaction, ReceiverDataForTransaction>();
	}
}