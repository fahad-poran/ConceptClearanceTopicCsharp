using InterviewPrep.Shared.Domain;

namespace InterviewPrep.Shared.Services;

// Delegate + event pattern:
// the processor emits a completion signal, and the caller decides how to react.
public delegate void PaymentCompletedHandler(string orderId, decimal finalAmount);

public sealed class PaymentProcessor
{
    public event PaymentCompletedHandler? PaymentCompleted;

    public string Process(PaymentMethod paymentMethod)
    {
        // Polymorphism here means the caller passes the base type,
        // and the processor branches on the concrete runtime record.
        return paymentMethod switch
        {
            CardPayment(var cardNumber, _) when cardNumber.Length >= 4
                => "Card payment accepted",
            MobileWalletPayment { WalletNumber.Length: >= 6 }
                => "Wallet payment accepted",
            CashOnDeliveryPayment
                => "Cash on delivery selected",
            _ => "Invalid payment method"
        };
    }

    // The service raises an event after processing is complete.
    // This keeps notification logic decoupled from the payment decision code.
    public void Complete(string orderId, decimal amount) => PaymentCompleted?.Invoke(orderId, amount);
}
