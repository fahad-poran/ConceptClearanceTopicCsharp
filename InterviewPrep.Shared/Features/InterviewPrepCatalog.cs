namespace InterviewPrep.Shared.Features;

public sealed record CodeWalkthrough(
    string Title,
    string Code,
    string Explanation,
    string WhyUsed);

public sealed record CourseTopic(
    string Id,
    string Title,
    string Definition,
    string Description,
    IReadOnlyList<string> Steps,
    IReadOnlyList<CodeWalkthrough> CodeWalkthroughs,
    string Recap);

public static class InterviewPrepCatalog
{
    public static string ProjectOverview =>
        "Scenario: an e-commerce order app that teaches core .NET concepts through one guided beginner course.";

    public static IReadOnlyList<CourseTopic> CourseTopics { get; } =
    [
        new CourseTopic(
            "oop",
            "OOP and polymorphism",
            "OOP lets you model real-world choices as types, then let shared code work with the base type.",
            "This lesson uses payment processing to show how a base abstraction stays stable while concrete payment records capture the details of each choice.",
            [
                "Start with `PaymentMethod` as the base abstraction that describes the idea of payment without locking into one option.",
                "Use `CardPayment`, `MobileWalletPayment`, and `CashOnDeliveryPayment` as concrete records so each option carries only the data it needs.",
                "Pass the base type into `PaymentProcessor` so the service can apply one workflow to all payment choices.",
                "Keep event handling separate so completion notifications do not become part of the payment decision itself."
            ],
            [
                new CodeWalkthrough(
                    "Common parent payment type",
                    """
                    public abstract record PaymentMethod;

                    public sealed record CardPayment(string CardNumber, string CardHolder) : PaymentMethod;
                    public sealed record MobileWalletPayment(string WalletNumber) : PaymentMethod;
                    public sealed record CashOnDeliveryPayment() : PaymentMethod;
                    """,
                    "`PaymentMethod` is the common parent idea: it means \"some kind of payment\". `abstract` means we do not create a plain payment by itself. `CardPayment`, `MobileWalletPayment`, and `CashOnDeliveryPayment` are the real choices a user can make. The colon means each one belongs to the `PaymentMethod` family.",
                    "This lets the project keep one payment family instead of writing separate unrelated code paths for cards, wallets, and cash."
                ),
                new CodeWalkthrough(
                    "Processor accepts the parent type",
                    """
                    public string Process(PaymentMethod paymentMethod)
                    {
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
                    """,
                    "`Process` takes `PaymentMethod`, the parent type, so callers can pass any payment choice. The `switch` checks the real kind of payment at runtime. The card branch also checks that the card number is long enough for this demo. `_` is the fallback branch when nothing else matches.",
                    "The processor has one method for all payment choices. That is easier to maintain than one separate processor method for every payment type."
                ),
                new CodeWalkthrough(
                    "Completion event is only a notification",
                    """
                    public delegate void PaymentCompletedHandler(string orderId, decimal finalAmount);

                    public event PaymentCompletedHandler? PaymentCompleted;

                    public void Complete(string orderId, decimal amount) =>
                        PaymentCompleted?.Invoke(orderId, amount);
                    """,
                    "The delegate describes the shape of a method that can receive an order id and amount. The event lets other code listen for payment completion. `?.Invoke` means \"call the listeners only if someone subscribed\".",
                    "The event is not choosing the payment method. It only announces that payment work finished, so notification code stays separate from decision code."
                )
            ],
            "Base types define the contract, derived records hold the specific data, and the processor applies behavior without owning notification logic."
        ),
        new CourseTopic(
            "cts",
            "CTS and type behavior",
            "The Common Type System explains how .NET represents value types, reference types, and conversions consistently.",
            "The app uses simple domain records and service types to make it easier to explain why some data is copied and some objects are shared.",
            [
                "Identify which types are records, classes, or structs in the project.",
                "Compare value semantics with reference semantics using `OrderItem` and service objects.",
                "Notice where immutability reduces accidental changes in the learning flow.",
                "Use the project examples to connect the abstract CTS idea to practical .NET code."
            ],
            [
                new CodeWalkthrough(
                    "Value type copy in the console demo",
                    """
                    int valueA = 10;
                    int valueB = valueA;
                    valueB = 20;
                    """,
                    "`int` is a value type. When `valueA` is copied into `valueB`, the number is copied. Changing `valueB` later does not change `valueA`.",
                    "This tiny example shows why simple numbers are safe to pass around when each variable should keep its own value."
                ),
                new CodeWalkthrough(
                    "Reference type copy in the console demo",
                    """
                    var refA = new CustomerProfile("Alice");
                    var refB = refA;
                    refB.Name = "Alice Updated";
                    """,
                    "`CustomerProfile` is a class, so the variable stores a reference to an object. `refB = refA` makes both variables point to the same object. Changing `refB.Name` is visible through `refA` too.",
                    "The demo uses this to show that objects can be shared, so developers must be careful when mutable data is passed around."
                ),
                new CodeWalkthrough(
                    "Order item as a small value",
                    """
                    public readonly record struct OrderItem(string Sku, int Quantity, decimal UnitPrice)
                    {
                        public decimal TotalPrice => Quantity * UnitPrice;
                    }
                    """,
                    "`struct` makes `OrderItem` a value type. `record` gives it useful built-in value behavior. `readonly` means its stored values should not be changed after creation. `TotalPrice` calculates quantity times unit price.",
                    "An order line is small and should be predictable during calculation, so value-style behavior fits this project."
                )
            ],
            "CTS is the ruleset that helps .NET keep types predictable across the app."
        ),
        new CourseTopic(
            "tuples",
            "Tuples",
            "Tuples group a few related values together without creating a full class.",
            "The discount engine returns both the discount amount and the reason so the API can explain the result clearly.",
            [
                "Look at the discount result as two pieces of data that belong together.",
                "Return the amount and reason from one method instead of creating extra temporary objects.",
                "Read the API response as a direct translation of that tuple into a beginner-friendly explanation.",
                "Use tuple returns when the result is small, temporary, and naturally grouped."
            ],
            [
                new CodeWalkthrough(
                    "Discount amount and reason together",
                    """
                    var (discountAmount, reason) = gross switch
                    {
                        >= 1000m => (gross * 0.10m, "10% for orders >= 1000"),
                        _ when acceptedItems.Count >= 5 => (gross * 0.05m, "5% for 5+ accepted items"),
                        _ => (0m, "No discount")
                    };
                    """,
                    "`(discountAmount, reason)` receives two values at once. Each branch returns a pair: the money amount and the explanation text. The names make the two values easy to use later.",
                    "The calculator needs both a discount number and a reason for the web response. A tuple keeps those two temporary values together without creating a separate class."
                )
            ],
            "Tuples are useful when one operation needs to return a small bundle of related values."
        ),
        new CourseTopic(
            "readonly",
            "Readonly records",
            "Readonly data types help you protect values that should not change after creation.",
            "Order lines are modeled as a readonly record struct so each item stays predictable while the order total is calculated.",
            [
                "Start with `OrderItem` as the shape for one order line.",
                "Keep its fields fixed so calculations can trust the stored values.",
                "Use the type as a stable input to the order calculator.",
                "Prefer readonly modeling when the data should be copied instead of mutated."
            ],
            [
                new CodeWalkthrough(
                    "Readonly order line",
                    """
                    public readonly record struct OrderItem(string Sku, int Quantity, decimal UnitPrice)
                    {
                        public decimal TotalPrice => Quantity * UnitPrice;
                    }
                    """,
                    "`readonly` tells readers that this small data value is not meant to change after it is created. The primary constructor lists the three pieces of data in one line. The expression-bodied property calculates the line total when asked.",
                    "Order totals should be calculated from stable input. This avoids bugs where an item changes halfway through a calculation."
                )
            ],
            "Readonly records are a simple way to keep small domain values safe and predictable."
        ),
        new CourseTopic(
            "generics",
            "Generics",
            "Generics let you write reusable code that still works with strong typing.",
            "Repository-style code and utility methods can share one implementation instead of repeating the same logic for each data type.",
            [
                "Recognize when a piece of code is repeated for multiple types.",
                "Use a generic type parameter to keep the implementation reusable.",
                "Let the compiler preserve type safety while you reduce duplication.",
                "Keep the learning focus on the shape of the API rather than on cast-heavy code."
            ],
            [
                new CodeWalkthrough(
                    "Generic repository contract",
                    """
                    public interface IRepository<T>
                    {
                        void Add(T item);
                        IReadOnlyList<T> GetAll();
                    }
                    """,
                    "`T` is a placeholder for a type. If the repository is used as `IRepository<string>`, then `T` means `string`. If it is used for another type, the same code shape still works.",
                    "The project can teach one repository idea without writing a different interface for strings, orders, payments, and every other type."
                ),
                new CodeWalkthrough(
                    "One in-memory implementation for many types",
                    """
                    public sealed class InMemoryRepository<T> : IRepository<T>
                    {
                        private readonly List<T> _items = [];

                        public void Add(T item) => _items.Add(item);
                        public IReadOnlyList<T> GetAll() => _items;
                    }
                    """,
                    "`InMemoryRepository<T>` keeps a `List<T>`, so the list holds the same type the repository was created for. `Add` stores one item. `GetAll` returns the stored items without forcing callers to cast from `object`.",
                    "This removes repeated storage code while still letting the compiler catch type mistakes."
                ),
                new CodeWalkthrough(
                    "Generic method with a rule",
                    """
                    public static T MaxByValue<T>(T left, T right) where T : IComparable<T>
                        => left.CompareTo(right) >= 0 ? left : right;
                    """,
                    "This method works for any `T` that can compare itself with another `T`. The `where` part is the rule. `CompareTo` returns which value is bigger, then the method returns the larger one.",
                    "The rule keeps the method reusable but still safe. It cannot be called with a type that has no comparison behavior."
                )
            ],
            "Generics let one implementation serve many types without losing safety."
        ),
        new CourseTopic(
            "pattern-matching",
            "Pattern matching",
            "Pattern matching chooses a branch by inspecting the runtime shape of a value.",
            "The payment processor uses it to decide which payment branch should run for each concrete payment record.",
            [
                "Check the input type instead of writing nested conditionals.",
                "Match on the concrete payment record to pick the correct behavior.",
                "Use property patterns when a decision depends on a field value.",
                "Keep the branches short so the intent stays obvious to a beginner."
            ],
            [
                new CodeWalkthrough(
                    "Payment switch expression",
                    """
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
                    """,
                    "The `switch` looks at `paymentMethod` and chooses a branch. `CardPayment(...)` matches card payments and pulls out the card number. `when` adds an extra condition. The wallet branch checks a property directly. `_` means anything that was not accepted earlier.",
                    "The project uses pattern matching so payment rules are visible in one compact place instead of spread across nested `if` statements."
                )
            ],
            "Pattern matching makes type-based decisions clearer and easier to read."
        ),
        new CourseTopic(
            "delegates-events",
            "Delegates and events",
            "Delegates describe callable behavior, and events let objects publish notifications safely.",
            "The payment completion flow shows how one part of the app can notify others without hard-coding the listeners.",
            [
                "Treat the delegate as the shape of the callback.",
                "Use the event to expose a completion signal without revealing the internal invocation details.",
                "Keep the payment decision separate from the notification step.",
                "Let the caller decide how to react when the event fires."
            ],
            [
                new CodeWalkthrough(
                    "Delegate and event declaration",
                    """
                    public delegate void PaymentCompletedHandler(string orderId, decimal finalAmount);

                    public event PaymentCompletedHandler? PaymentCompleted;
                    """,
                    "A delegate is a method shape. Here, a listener must accept an order id and a final amount, and it returns nothing because the return type is `void`. The event exposes a safe way for outside code to subscribe.",
                    "This lets `PaymentProcessor` publish a completion signal without knowing whether the console, web app, logger, or another system will listen."
                ),
                new CodeWalkthrough(
                    "Subscribing and raising the event",
                    """
                    processor.PaymentCompleted += OnPaymentCompleted;

                    public void Complete(string orderId, decimal amount) =>
                        PaymentCompleted?.Invoke(orderId, amount);
                    """,
                    "`+=` subscribes a method to the event. Later, `Complete` raises the event. `?.Invoke` prevents an error if there are no listeners.",
                    "The caller decides what to do after payment completes, while the processor only reports that completion happened."
                )
            ],
            "Delegates and events help the app communicate without tight coupling."
        ),
        new CourseTopic(
            "dotnet8",
            ".NET 8 features",
            ".NET 8 includes modern tools that improve performance, safety, and developer ergonomics.",
            "The project highlights features such as frozen collections, `TimeProvider`, and newer language conveniences in a practical app.",
            [
                "Notice where read-heavy data uses a frozen lookup structure.",
                "Use `TimeProvider` to keep time-based logic testable.",
                "See how newer language features reduce ceremony in the model code.",
                "Treat these features as quality-of-life tools, not as the core learning goal."
            ],
            [
                new CodeWalkthrough(
                    "Frozen inventory lookup",
                    """
                    private readonly FrozenDictionary<string, int> _stockBySku;

                    public InventoryService(IEnumerable<KeyValuePair<string, int>> seed)
                    {
                        _stockBySku = seed.ToFrozenDictionary();
                    }
                    """,
                    "`FrozenDictionary` is a dictionary optimized for reading after it is built. The constructor receives seed stock data and turns it into a frozen lookup. `readonly` means the field reference is assigned by construction and not replaced later.",
                    "Inventory is read many times during stock checks in this demo. Freezing the lookup fits read-heavy data and introduces a practical .NET 8 collection."
                ),
                new CodeWalkthrough(
                    "TimeProvider in the apps",
                    """
                    builder.Services.AddSingleton(TimeProvider.System);

                    app.MapPost("/api/order/calculate",
                        (OrderRequest request, OrderCalculationService calculator, TimeProvider timeProvider) =>
                        {
                            var response = calculator.Calculate(request, timeProvider.GetUtcNow().UtcDateTime);
                            return Results.Ok(response);
                        });
                    """,
                    "`TimeProvider` is the app's source of time. The web app registers the system clock once, then the endpoint asks for it when calculating an order. `GetUtcNow()` gives the current UTC time.",
                    "Using `TimeProvider` keeps time access in one place and makes time-based code easier to test than calling `DateTime.UtcNow` everywhere."
                )
            ],
            ".NET 8 features make common app code cleaner, faster, and easier to test."
        )
    ];

    public static CourseTopic GetTopic(string id) =>
        CourseTopics.First(topic => string.Equals(topic.Id, id, StringComparison.OrdinalIgnoreCase));

    public static IReadOnlyList<(string Title, string Detail)> Topics { get; } =
        CourseTopics.Select(topic => (topic.Title, topic.Description)).ToArray();

    public static IReadOnlyList<string> PracticeQuestions { get; } =
    [
        "Why use readonly record struct for OrderItem?",
        "Difference between delegate and event in this project?",
        "Where is pattern matching helping readability?",
        "Show CTS with value/reference and boxing/unboxing from output.",
        "Why choose FrozenDictionary for inventory?"
    ];

    public static IReadOnlyList<string> FeatureMap { get; } =
    [
        "Domain models: Domain/Payment.cs, Domain/OrderItem.cs",
        "Service behavior: Services/PaymentProcessor.cs, Services/DiscountEngine.cs",
        "Infrastructure-style collection: Services/InventoryService.cs",
        "Generics and repository: Common/Repository.cs",
        "Core interview topics: CTS, tuples, readonly, pattern matching, delegates/events",
        ".NET 8 highlights: FrozenDictionary, TimeProvider, required, primary constructors"
    ];
}
