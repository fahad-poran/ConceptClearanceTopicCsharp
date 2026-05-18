namespace InterviewPrep.Console.Domain;

public abstract record PaymentMethod;

public sealed record CardPayment(string CardNumber, string CardHolder) : PaymentMethod;
public sealed record MobileWalletPayment(string WalletNumber) : PaymentMethod;
public sealed record CashOnDeliveryPayment() : PaymentMethod;
