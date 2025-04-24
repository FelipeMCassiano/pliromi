namespace Exceptions;

public static class PliromiTransactionMessagesErrors
{
	public const string ValueMustBeGreaterThanZero = "Value must be greater than zero";
	public const string MustProvideOneIdentifier = "You must provide exactly one of: CPF, CNPJ, or Email";
	public const string ReceiverNotFound = "Receiver account not found";
	public const string InsufficientBalance = "Insufficient account balance";
}