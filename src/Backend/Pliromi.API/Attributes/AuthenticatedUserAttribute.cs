using Microsoft.AspNetCore.Mvc;
using Pliromi.API.Filters;

namespace Pliromi.API.Attributes;

public class AuthenticatedUserAttribute : TypeFilterAttribute
{
	public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter))
	{
	}
}