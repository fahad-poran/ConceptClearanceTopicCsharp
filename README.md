# DotNet8InterviewPrep

Interview preparation solution for a **.NET developer with 3+ years experience**.
Now it includes both:
- Console learning app
- Browser-based web UI (HTML/CSS/JS)

## Projects
- `InterviewPrep.Console`: menu-driven console learning flow
- `InterviewPrep.Web`: interactive browser app with order calculator + topic cards

## Browser app (new)
The web project provides a clean client view built with:
- HTML for structure
- CSS for polished responsive layout
- Vanilla JavaScript for interactivity
- ASP.NET Core Minimal API for real-time order calculation

### Features in browser
- Hero landing with guided actions
- Real-world order calculator
- Live table of cart items
- Discount rules demonstration via API
- Interview-topic navigator cards

## Run in browser
From solution root:

```bash
dotnet run --project InterviewPrep.Web --urls http://localhost:5157
```

Then open:
- `http://localhost:5157`

## Interview topic coverage map
- OOP: Payment model + processing flow
- CTS: value/reference + boxing/unboxing (console module)
- Tuples: discount return values
- `readonly`: `OrderItem` as readonly record struct
- Generic method/repository: console module
- Pattern matching: pricing/payment rules
- Delegates/events: console module
- .NET 8: `FrozenDictionary`, primary constructors, `TimeProvider`, `required`

## Quick git sync
```bash
./sync.sh "your commit message"
```
