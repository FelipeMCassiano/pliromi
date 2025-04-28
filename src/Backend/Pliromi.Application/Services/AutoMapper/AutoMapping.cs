using AutoMapper;
using Communication.Requests;
using Communication.Responses;
using Pliromi.Domain.Entities;
using Pliromi.Domain.Entities.Events;

namespace Pliromi.Application.Services.AutoMapper;

public class AutoMapping:Profile
{
	public AutoMapping()
	{
		RequestToDomain();
		EntityToResponse();
		DomainToEvent();
	}

	private void RequestToDomain()
	{
		CreateMap<RequestRegisterUser, User>()
			.ForMember(dest => dest.Password, opt => opt.Ignore())
			.ForMember(dest => dest.PliromiKey, opt => opt.Ignore());
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

	private void DomainToEvent()
	{
		CreateMap<Transaction, TransactedEventConsumer>()
			.ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.CreatedOn))
			.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
			.ForMember(dest => dest.SenderEmail, opt => opt.MapFrom(src => src.Sender.Email))
			.ForMember(dest => dest.ReceiverName, opt => opt.MapFrom(src => src.Receiver.Fullname))
			.ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.Key, opt => opt.Ignore());
		
	}
}