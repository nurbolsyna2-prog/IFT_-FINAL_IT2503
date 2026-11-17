# Online Shop Calculation

A classroom shopping application built with **C#, .NET 8, and Blazor Server**. It demonstrates a product catalog, a stateful cart, promo codes, and a transparent price calculation.

Team: Atshybai Nurbolsyn and Chekalin Tomiris

## Features

- 10 sample products, grouped by category
- Live search by product name or category
- Add and remove products; change quantities from 1 to 99
- Promo codes: `SAVE10` (10%), `WELCOME5` (5%), and `NURBOLSYN` (50%); matching is case-insensitive
- Price summary with subtotal, bulk and promo discounts, tax, shipping, and total
- Responsive CSS Grid layout, a small-screen media query, and dark mode based on device preference
- Blazor Server updates the cart badge and totals when cart state changes

## Pricing rules

Defaults are configured in `onlineShopp/appsettings.json`:

- Currency: `$`
- Tax rate: 12%, calculated after discounts
- Bulk discount: 10% on an individual product line when its quantity is 5 or more
- Promo discount: applied after the bulk discount
- Shipping: $5 when the discounted taxable amount is below $100; free at or above $100
- Money is represented by `decimal`; tax and discounts are rounded to two places using midpoint rounding away from zero

Calculation order: subtotal → bulk discount → promo discount → tax → shipping → total.

## Run the app

Install the .NET 8 SDK. From the repository root (the folder containing `IFT_FINAL_IT2503.sln`), run:

```powershell
dotnet run --project .\onlineShopp\OnlineShop.csproj
```

Open the `Now listening on:` URL printed in the terminal, for example `http://localhost:5000`. Keep the terminal open while using the app; press `Ctrl+C` to stop it.

In VS Code, open the repository root, then choose **Terminal → New Terminal** and run the command above.

## Run the tests

The xUnit test project is `OnlineShop.Tests`. From the repository root, run:

```powershell
dotnet test .\IFT_FINAL_IT2503.sln
```

The tests cover empty-cart totals, tax and shipping, the bulk-discount boundary, free shipping, promo-code matching and order, midpoint rounding, quantity validation, cart removal and clearing, and change notifications.

## Project structure

```text
IFT_FINAL_IT2503.sln
├── onlineShopp/                 # Blazor Server application
│   ├── Components/              # Layout, pages, and reusable UI components
│   ├── Models/                  # Product, cart, pricing rules, and results
│   ├── services/                # Catalog, cart state, and pricing calculation
│   ├── wwwroot/app.css          # Responsive styling and dark mode
│   └── Program.cs               # App setup and dependency registration
└── OnlineShop.Tests/            # xUnit tests for pricing and cart behavior
```

## Functional programming concepts

`Pricing.Calculate` is a pure function: the same items, pricing rules, and promo produce the same `PriceBreakdown`, without changing application state. Models such as `Product`, `CartItem`, and `PriceBreakdown` are immutable records. Cart changes create new lists with LINQ and `with` expressions instead of mutating existing records in place.

## Current scope

The catalog and cart are stored in memory. “Place order” is a demo action: it clears the cart and shows a confirmation message, but does not save an order or process a payment. There is no login, database, or payment integration.

