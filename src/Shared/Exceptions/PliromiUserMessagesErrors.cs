namespace Exceptions;

public static class PliromiUserMessagesErrors
{
	public const string CpfAlreadyRegistered = "This CPF is already registered.";
	public const string CnpjAlreadyRegistered = "This CNPJ is already registered.";
	public const string EmailAlreadyRegistered = "This email is already registered.";
	public const string InvalidEmail ="The email is invalid.";
	public const string FullNameLengthInvalid = "Fullname should be between 1 and 255 characters ";
	public const string PasswordLengthInvalid = "Password should be between 1 and 255 characters ";
	public const string NegativeBalanceNotAllowed = "Account balance cannot be negative.";
	public const string CannotRegisterAEmptyCpfAsPliromiKey = "Cannot register an empty cpf as a pliromi";
	public const string CannotRegisterAEmptyCnpjAsPliromiKey = "Cannot register an empty cnpj as a pliromi";
	public const string PersonalAccountRequiresCpfNoCnpj = "Personal accounts require CPF and no CNPJ.";
	public const string StoreAccountRequiresCnpjNoCpf = "Store accounts require CNPJ and no CPF.";
	public const string InvalidPliromiKeyType = "Invalid Pliromi key type.";
		
}