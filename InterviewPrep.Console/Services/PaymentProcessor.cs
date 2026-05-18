using InterviewPrep.Console.Domain;

namespace InterviewPrep.Console.Services;

// Delegate commonly appears in interviews (callback pipeline).
public delegate void PaymentCompletedHandler(string orderId, decimal finalAmount);

public sealed class PaymentProcessor
{
    public event PaymentCompletedHandler? PaymentCompleted;

    public string Process(PaymentMethod paymentMethod)
    {
        // Pattern matching over type hierarchy (very common interview topic).
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

    public void Complete(string orderId, decimal amount) => PaymentCompleted?.Invoke(orderId, amount);
}
