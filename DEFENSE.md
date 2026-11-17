# Defense prep: Shop Calc


## 1. Likely questions and answers

**Why Blazor Server and not WebAssembly?**
Code runs on the server and the browser gets small UI updates over a SignalR connection. Startup is fast, the pricing rules
are not exposed to the client, and I can use .NET directly. The trade-off is that it needs a constant connection and uses server memory per user.

**Where is the functional programming?**
`Pricing.Calculate` is a pure function: same input gives the same output, no side effects. Models are immutable `record`s,
and `CartState` never edits a list, it replaces it with a new one using LINQ and `with` expressions.

**How does state management work?**
`CartState` is registered as Scoped, so there is one per user session (circuit). Components inject it. It exposes `OnChange`;
`MainLayout` subscribes and calls `InvokeAsync(StateHasChanged)`, so the cart badge updates from any page. It unsubscribes in `Dispose` to avoid memory leaks.

**Why `InvokeAsync(StateHasChanged)`?**
The event may fire outside the component's synchronization context, and Blazor requires UI updates to run on its dispatcher.

**Why `decimal` and not `double`?**
`double` has binary rounding errors (0.1 + 0.2 != 0.3). `decimal` is exact for money. I round to 2 places with `MidpointRounding.AwayFromZero`, the usual rule for prices.

**Which components are reusable?**
`ProductCard` (Parameter `Product`, EventCallback `OnAdd`) and `PriceSummary` (Parameters `Breakdown`, `Currency`). They do not know about the cart; the parent passes data in and handles events.

**How do you handle errors?**
Expected user errors (bad quantity, bad promo code, non-numeric input) return a `Result` object with a message that is shown with `role="alert"`; no exceptions are used for normal validation.
Quantity is limited to 1..99 in `CartState`, so the rule is enforced even if the UI changes.

**How did you test it?**
xUnit tests for: empty cart, tax and shipping, bulk threshold (at and below), free shipping boundary, promo, rounding, case-insensitive promo codes, quantity validation, `OnChange`, and `Clear`. Run with `dotnet test`.

**Is it responsive?**
CSS grid with `auto-fill minmax`, a media query for small screens (2-column grid, hides the unit-price column), a scroll wrapper for the table, and dark mode through `prefers-color-scheme`.

**What about scalability?**
Honestly: Blazor Server keeps a circuit in memory per user, so it scales with memory, and you would scale out with a SignalR backplane or Azure SignalR. The catalog is in memory today, so the next step is EF Core with a database and caching.

**What would you add next?**
Database (EF Core + SQLite/PostgreSQL), user accounts and roles (admin edits products and promos), order history, integration tests with bUnit, and a real payment provider.

**Why are the rules in appsettings.json?**
Tax and shipping change without code changes. They are bound to the `PricingRules` record at start-up with the Options-style `GetSection("Pricing").Get<>()`.

**Walk me through `Calculate`.**
Use the 7 steps in README ("Calculation order"). Example: 5 x 10.00 -> subtotal 50, bulk 5, taxable 45, tax 5.40, shipping 5, total 55.40.

**Which part did you write? (team)** Be ready to explain every file; any member can be asked about any part.
