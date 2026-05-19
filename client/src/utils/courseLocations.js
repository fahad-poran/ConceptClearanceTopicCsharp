export function inferWhereItAppears(topicId) {
  switch (topicId) {
    case "oop":
      return "Domain/Payment.cs and Services/PaymentProcessor.cs";
    case "cts":
      return "The shared domain and service layer, where type behavior becomes visible";
    case "tuples":
      return "Services/OrderCalculationService.cs";
    case "readonly":
      return "Domain/OrderItem.cs";
    case "generics":
      return "Common/Repository.cs and reusable utility patterns";
    case "pattern-matching":
      return "Services/PaymentProcessor.cs";
    case "delegates-events":
      return "Services/PaymentProcessor.cs and the code that subscribes to completion events";
    case "dotnet8":
      return "Services/InventoryService.cs and the app startup configuration";
    default:
      return "The shared lesson code";
  }
}
