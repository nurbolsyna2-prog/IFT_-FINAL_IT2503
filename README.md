# Online Shop Calculation

This is a shopping web application built with C#, .NET 8, and Blazor Server. It includes a product catalog, a stateful shopping cart, promo codes, and automatic price calculation.

**Team:** Atshybai Nurbolsyn and Chekalin Tomiris

## Overview

The application is built using a modular component-based architecture. Its main goal is to demonstrate catalog filtering, cart state management, and transparent price calculations with bulk discounts, promo codes, taxes, and shipping rules.

## Features

- 10 sample products organized by category
- Real-time search bar to filter products by name or category
- Adding and removing items in the cart, with quantity adjustments from 1 to 99
- Case-insensitive promo codes: `SAVE10` (10%), `WELCOME5` (5%), and `NURBOLSYN` (50%)
- Detailed price breakdown showing subtotal, bulk discount, promo discount, tax, shipping, and total
- Responsive layout using CSS Grid with dark mode support based on system preferences
- Instant UI updates for the cart badge and totals via Blazor Server

## Pricing Rules

Default pricing parameters are configured in `onlineShopp/appsettings.json`:

- **Currency:** `$`
- **Tax rate:** 12% (calculated after all discounts are applied)
- **Bulk discount:** 10% off an individual product line when buying 5 or more units of that item
- **Promo discount:** applied after the bulk discount
- **Shipping:** `$5` if the discounted amount before tax is below `$100`, and free at `$100` or above
- Financial values use the `decimal` type, with discounts and taxes rounded to two decimal places using midpoint rounding away from zero

**Calculation order:** Subtotal → Bulk discount → Promo discount → Tax → Shipping → Total.

## Technologies and Architecture

- **Tech Stack:** C# 12, .NET 8, Blazor Server, and xUnit for automated testing.
- **Functional Programming Concepts:** The `Pricing.Calculate` method is implemented as a pure function, meaning identical inputs always return the same `PriceBreakdown` without modifying external state. Data models such as `Product`, `CartItem`, and `PriceBreakdown` are immutable records. Instead of mutating lists in place, cart operations produce new lists using LINQ and `with` expressions.

## How to Run the Application

Make sure the .NET 8 SDK is installed on your machine. From the root directory of the repository (where `IFT_FINAL_IT2503.sln` is located), run:

```powershell
dotnet run --project .\onlineShopp\OnlineShop.csproj
