# DotNet8InterviewPrep

Interview preparation console application for a **.NET developer with 3+ years experience**.
It now includes a **simple menu UI** for easier learning and navigation.

## Real-world scenario
This project simulates an **e-commerce order processing flow**:
- Build order
- Validate inventory
- Apply discounts
- Process payment
- Capture interview-focused language/runtime features

## Easy navigation flow
1. Start app and choose menu options (`1..5`, `Q`).
2. Run `2` first to see end-to-end real-world flow.
3. Run `3` to focus only on CTS and frequently asked theory-to-code topics.
4. Run `4` to map each topic directly to source files.
5. Run `5` to self-practice common interview questions.

## Suggested study order (for 3+ years experience)
1. `Features/InterviewPrepApp.cs`: control flow + UI + scenario orchestration
2. `Domain/*`: business model and OOP design
3. `Services/*`: pattern matching, delegates/events, tuple logic
4. `Common/Repository.cs`: generics and reusable abstractions

## Topic coverage map
- OOP (abstraction/inheritance/polymorphism): `Domain/Payment.cs`, `Services/PaymentProcessor.cs`
- CTS (value/reference type, boxing/unboxing): `Features/InterviewPrepApp.cs`
- Tuples: `Services/DiscountEngine.cs`
- `readonly` struct: `Domain/OrderItem.cs`, `Features/InterviewPrepApp.cs`
- Generic method + generic repository: `Common/Repository.cs`
- Pattern matching: `Services/PaymentProcessor.cs`, `Services/DiscountEngine.cs`
- Delegates + Events: `Services/PaymentProcessor.cs`
- .NET 8 / modern C# features:
  - Primary constructors: `Domain/Order.cs`, `Features/InterviewPrepApp.cs`
  - Collection expressions: `Features/InterviewPrepApp.cs`
  - `FrozenDictionary`: `Services/InventoryService.cs`
  - `TimeProvider`: `Features/InterviewPrepApp.cs`
  - `required` members: `Domain/Order.cs`

## Run
```bash
dotnet run --project InterviewPrep.Console
```

## Suggested interview practice
1. Explain why `OrderItem` is `readonly record struct` instead of `class`.
2. Explain delegate vs event and where each is used.
3. Explain pros/cons of generic repository.
4. Explain pattern matching benefit over if/else chains.
5. Explain value/reference copy behavior with exact output from the program.

Sync test note: repository sync workflow configured.
