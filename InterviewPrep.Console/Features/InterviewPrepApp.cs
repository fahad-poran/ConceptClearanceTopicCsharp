using InterviewPrep.Shared.Common;
using InterviewPrep.Shared.Contracts;
using InterviewPrep.Shared.Domain;
using InterviewPrep.Shared.Features;
using InterviewPrep.Shared.Services;
using DomainOrderItem = InterviewPrep.Shared.Domain.OrderItem;
using SharedOrderItem = InterviewPrep.Shared.Contracts.OrderItem;

namespace InterviewPrep.Console.Features;

public sealed class InterviewPrepApp
{
    private readonly TimeProvider _timeProvider = TimeProvider.System;
    private readonly InventoryService _inventory;
    private readonly OrderCalculationService _orderCalculationService;

    public InterviewPrepApp()
    {
        // Real-world seed data that acts like a lightweight in-memory DAL/cache for demo usage.
        _inventory = new InventoryService(new Dictionary<string, int>
        {
            ["LAPTOP-001"] = 10,
            ["MOUSE-010"] = 50,
            ["BAG-200"] = 20
        });
        _orderCalculationService = new OrderCalculationService(_inventory);
    }

    public void Run()
    {
        while (true)
        {
            ShowMenu();
            var input = global::System.Console.ReadLine()?.Trim().ToUpperInvariant();

            switch (input)
            {
                case "1":
                    ShowProjectOverview();
                    Pause();
                    break;
                case "2":
                    RunOrderProcessingDemo();
                    Pause();
                    break;
                case "3":
                    ExplainCtsAndCoreInterviewTopics(1125m);
                    Pause();
                    break;
                case "4":
                    ShowFeatureMap();
                    Pause();
                    break;
                case "5":
                    ShowInterviewQuestions();
                    Pause();
                    break;
                case "Q":
                    return;
                default:
                    global::System.Console.WriteLine("Invalid option. Please choose 1-5 or Q.\n");
                    break;
            }
        }
    }

    private void ShowMenu()
    {
        global::System.Console.Clear();
        global::System.Console.WriteLine("=== .NET 8 Interview Prep Console (3+ Years Experience) ===");
        global::System.Console.WriteLine($"UTC Now (TimeProvider): {_timeProvider.GetUtcNow():u}");
        global::System.Console.WriteLine();
        global::System.Console.WriteLine("1. Project Overview");
        global::System.Console.WriteLine("2. Run Real-World Order Processing Demo");
        global::System.Console.WriteLine("3. Run CTS + Core Topic Demo");
        global::System.Console.WriteLine("4. Topic-to-File Navigation Map");
        global::System.Console.WriteLine("5. Interview Practice Questions");
        global::System.Console.WriteLine("Q. Quit");
        global::System.Console.Write("\nSelect an option: ");
    }

    private static void ShowProjectOverview()
    {
        global::System.Console.WriteLine("\n--- Project Overview ---");
        global::System.Console.WriteLine(InterviewPrepCatalog.ProjectOverview);
        global::System.Console.WriteLine("Goal: Learn interview-focused C# and .NET 8 topics with practical code.");
        global::System.Console.WriteLine("Style: Small modules, real naming, and comments explaining 'why'.");
        global::System.Console.WriteLine();
    }

    private void RunOrderProcessingDemo()
    {
        global::System.Console.WriteLine("\n--- Real-World Order Processing Demo ---");
        global::System.Console.WriteLine("Step 1: Build order using domain model...");

        var order = new Order("ORD-1001", "Alice Johnson")
        {
            ShippingAddress = "221B Baker Street, London"
        };

        // Collection expression (modern C#): concise, readable initialization for fixed demo data.
        SharedOrderItem[] cartItems =
        [
            new("LAPTOP-001", 1, 1200m),
            new("MOUSE-010", 2, 25m)
        ];

        global::System.Console.WriteLine("Step 2: Validate stock from inventory service...");
        foreach (var item in cartItems)
        {
            if (_inventory.IsInStock(item.Sku, item.Quantity))
            {
                order.AddItem(new DomainOrderItem(item.Sku, item.Quantity, item.UnitPrice));
            }
        }

        global::System.Console.WriteLine("Step 3: Calculate discount using tuple result...");
        var (discountAmount, reason) = DiscountEngine.CalculateDiscount(order);
        var finalAmount = order.Total - discountAmount;

        global::System.Console.WriteLine("Order Summary:");
        global::System.Console.WriteLine($"Order Id: {order.OrderId}");
        global::System.Console.WriteLine($"Customer: {order.CustomerName}");
        global::System.Console.WriteLine($"Address: {order.ShippingAddress}");
        global::System.Console.WriteLine($"Gross Total: {order.Total:C}");
        global::System.Console.WriteLine($"Discount: {discountAmount:C} ({reason})");
        global::System.Console.WriteLine($"Final Amount: {finalAmount:C}\n");

        global::System.Console.WriteLine("Step 4: Process payment using polymorphism + pattern matching...");
        var processor = new PaymentProcessor();

        // Event subscription: interviewers ask how delegate/event enables loose coupling.
        // PaymentProcessor publishes "payment completed"; this UI only listens and reacts.
        processor.PaymentCompleted += OnPaymentCompleted;

        var paymentStatus = processor.Process(new CardPayment("4111111111111111", "Alice Johnson"));
        global::System.Console.WriteLine($"Payment Status: {paymentStatus}");
        processor.Complete(order.OrderId, finalAmount);

        global::System.Console.WriteLine("\nOrder demo completed.");

        var apiStyleResponse = _orderCalculationService.Calculate(
            new OrderRequest(
                "Alice Johnson",
                cartItems
                    .Select(item => new SharedOrderItem(item.Sku, item.Quantity, item.UnitPrice))
                    .ToArray()),
            _timeProvider.GetUtcNow().UtcDateTime);
        global::System.Console.WriteLine($"Shared order service final total: {apiStyleResponse.FinalTotal:C}");
    }

    private static void OnPaymentCompleted(string orderId, decimal amount)
    {
        global::System.Console.WriteLine($"[Delegate/Event] Payment completed for {orderId}: {amount:C}");
    }

    private static void ExplainCtsAndCoreInterviewTopics(decimal finalAmount)
    {
        global::System.Console.WriteLine("\n--- CTS + Core Interview Topics ---");

        // CTS: Value type vs Reference type.
        int valueA = 10;
        int valueB = valueA;
        valueB = 20;
        global::System.Console.WriteLine($"Value Type Copy -> valueA: {valueA}, valueB: {valueB}");

        var refA = new CustomerProfile("Alice");
        var refB = refA;
        refB.Name = "Alice Updated";
        global::System.Console.WriteLine($"Reference Type Copy -> refA.Name: {refA.Name}, refB.Name: {refB.Name}");

        // Boxing/Unboxing under CTS.
        object boxed = finalAmount;     // Boxing value type to object.
        var unboxed = (decimal)boxed;   // Unboxing back to decimal.
        global::System.Console.WriteLine($"Boxing/Unboxing -> boxed type: {boxed.GetType().Name}, unboxed: {unboxed}");

        // readonly usage in field-like context.
        var pricing = new PricingConfig(0.15m);
        global::System.Console.WriteLine($"Readonly TaxRate: {pricing.TaxRate:P}");

        // Generic method usage.
        var maxAmount = GenericUtility.MaxByValue(250m, unboxed);
        global::System.Console.WriteLine($"Generic MaxByValue(250, {unboxed}) => {maxAmount}");

        // IReadOnlyList from generic repository.
        IRepository<string> logs = new InMemoryRepository<string>();
        logs.Add("Order created");
        logs.Add("Payment processed");

        foreach (var log in logs.GetAll())
        {
            global::System.Console.WriteLine($"Log: {log}");
        }
    }

    private static void ShowFeatureMap()
    {
        global::System.Console.WriteLine("\n--- Topic-to-File Navigation Map ---");
        global::System.Console.WriteLine("This view is grouped by learning area so OOP/polymorphism is easier to trace.");
        foreach (var line in InterviewPrepCatalog.FeatureMap)
        {
            global::System.Console.WriteLine(line);
        }
        global::System.Console.WriteLine();
        global::System.Console.WriteLine("--- Learning Sections ---");
        foreach (var topic in InterviewPrepCatalog.CourseTopics)
        {
            global::System.Console.WriteLine(topic.Title);
            global::System.Console.WriteLine($"- {topic.Definition}");
            foreach (var step in topic.Steps)
            {
                global::System.Console.WriteLine($"  * {step}");
            }
            global::System.Console.WriteLine($"  Recap: {topic.Recap}");
        }
    }

    private static void ShowInterviewQuestions()
    {
        global::System.Console.WriteLine("\n--- Interview Practice Questions ---");
        for (var i = 0; i < InterviewPrepCatalog.PracticeQuestions.Count; i++)
        {
            global::System.Console.WriteLine($"{i + 1}. {InterviewPrepCatalog.PracticeQuestions[i]}");
        }
    }

    private static void Pause()
    {
        global::System.Console.WriteLine("\nPress Enter to return to menu...");
        _ = global::System.Console.ReadLine();
    }

    private sealed class CustomerProfile(string name)
    {
        public string Name { get; set; } = name;
    }

    private readonly struct PricingConfig(decimal taxRate)
    {
        public decimal TaxRate { get; } = taxRate;
    }
}
