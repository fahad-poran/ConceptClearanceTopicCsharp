namespace InterviewPrep.Shared.Domain;

// The domain layer defines the shape of the payment model.
// PaymentProcessor chooses behavior based on the concrete runtime type,
// so the base type stays focused on polymorphic structure instead of logic.
public abstract record PaymentMethod;

// Each derived record represents one payment option with its own data shape.
public sealed record CardPayment(string CardNumber, string CardHolder) : PaymentMethod;
public sealed record MobileWalletPayment(string WalletNumber) : PaymentMethod;
public sealed record CashOnDeliveryPayment() : PaymentMethod;
