using Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestLoginBuilder
{
	public static RequestLogin Build(string email, string password)
	{
		return new RequestLogin()
		{
			Email = email,
			Password = password,
		};
	}
	
}