namespace Exceptions;

public static class PliromiTransactionMessagesErrors
{
	public const string ValueMustBeGreaterThanZero = "Value must be greater than zero";
	public const string MustProvideOneOf = "You must provide exactly one of: CPF, CNPJ, or Email";
	public const string NotFoundReceiver = "Receiver could not be found";
	public const string InsufficientBalance = "Insufficient balance";
}