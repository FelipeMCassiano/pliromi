using Pliromi.Domain.Enums;

namespace Pliromi.Domain.Entities;

public class PliromiKey
{
	public Guid Id { get; set; }
	public string Key { get;  set; } = string.Empty;
	public  PliromiKeyType  Type { get;  set; }

	public Guid UserId { get;  set; }
	public User User { get; set; } = null!;
}