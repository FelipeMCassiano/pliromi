using AutoMapper;
using Pliromi.Application.Services.AutoMapper;

namespace CommonTestUtilities.Mapper;

public static class MapperBuilder
{
	public static IMapper Build()
	{
		return new MapperConfiguration(opt => opt.AddProfile(new AutoMapping())).CreateMapper();
	}
	
}