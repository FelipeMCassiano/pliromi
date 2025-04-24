namespace Exceptions;

public static class PliromiUserMessagesErrors
{
	public const string AlreadyRegisteredCpf = "Already Registered Cpf";
	public const string AlreadyRegisteredCnpj = "Already Registered Cpnj";
	public const string AlreadyRegisteredEmail = "Already Registered Email";
	public const string InvalidEmail = "Invalid Email";
	public const string EmailCannotBeEmpty = "Email cannot be empty";
	public const string EmailInvalidLength = "Email should be between 1 and 255 characters ";
	public const string FullnameCannotBeEmpty = "Fullname cannot be empty";
	public const string FullnameInvalidLength = "Fullname should be between 1 and 255 characters ";
	public const string PasswordCannotBeEmpty = "Password cannot be empty";
	public const string PasswordInvalidLength = "Password should be between 1 and 255 characters ";
	public const string StoreAccountsMustHaveOnlyACpnj = "Store must have only a CPNJ";
	public const string PersonalAccountMustHaveOnlyACpf = "PersonalAccount must have only a Cpf";
	public const string BalanceCannotBeLessThanZero = "Balance cannot be less than zero";
}