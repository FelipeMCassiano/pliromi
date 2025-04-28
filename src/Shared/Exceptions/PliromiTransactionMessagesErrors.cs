namespace Exceptions;

public static class PliromiTransactionMessagesErrors
{
	public const string ValueMustBeGreaterThanZero = "Value must be greater than zero";
	public const string PliromiMustBeProvided = "You must provide the PliromiKey";
	public const string ReceiverNotFound = "Receiver account not found";
	public const string InsufficientBalance = "Insufficient account balance";
}