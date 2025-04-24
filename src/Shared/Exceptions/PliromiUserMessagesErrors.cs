namespace Exceptions;

public static class PliromiUserMessagesErrors
{
	public const string CpfAlreadyRegistered = "This CPF is already registered.";
	public const string CnpjAlreadyRegistered = "This CNPJ is already registered.";
	public const string EmailAlreadyRegistered = "This email is already registered.";
	public const string InvalidEmailFormat ="The email format is invalid.";
	public const string EmailRequired = "Email address is required.";
	public const string EmailLengthInvalid = "Email should be between 1 and 255 characters ";
	public const string FullNameRequired = "Full name is required.";
	public const string FullNameLengthInvalid = "Fullname should be between 1 and 255 characters ";
	public const string PasswordRequired = "Password cannot be empty";
	public const string PasswordLengthInvalid = "Password should be between 1 and 255 characters ";
	public const string StoreAccountsMustHaveOnlyACpnj = "Store must have only a CPNJ";
	public const string PersonalAccountMustHaveOnlyACpf = "PersonalAccount must have only a Cpf";
	public const string NegativeBalanceNotAllowed = "Account balance cannot be negative.";
}