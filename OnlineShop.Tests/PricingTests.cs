using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Tests;

public class PricingTests
{
    [Fact]
    public void EmptyCartHasZeroTotal()
    {
        var result = Pricing.Calculate([], PricingRules.Default, null);

        Assert.Equal(0m, result.Subtotal);
        Assert.Equal(0m, result.Tax);
        Assert.Equal(0m, result.Shipping);
        Assert.Equal(0m, result.Total);
    }

    [Fact]
    public void CalculatesTaxAndShippingForRegularPurchase()
    {
        var item = new CartItem(new Product(1, "Laptop Stand", "Accessories", 24m), 2);

        var result = Pricing.Calculate([item], PricingRules.Default, null);

        Assert.Equal(48m, result.Subtotal);
        Assert.Equal(5.76m, result.Tax);
        Assert.Equal(5m, result.Shipping);
        Assert.Equal(58.76m, result.Total);
    }

    [Fact]
    public void AppliesBulkDiscountAtQuantityThreshold()
    {
        var item = new CartItem(new Product(1, "Cable", "Accessories", 10m), 5);

        var result = Pricing.Calculate([item], PricingRules.Default, null);

        Assert.Equal(50m, result.Subtotal);
        Assert.Equal(5m, result.BulkDiscount);
        Assert.Equal(45m, result.Taxable);
        Assert.Equal(5.40m, result.Tax);
        Assert.Equal(55.40m, result.Total);
    }

    [Fact]
    public void DoesNotApplyBulkDiscountBelowQuantityThreshold()
    {
        var item = new CartItem(new Product(1, "Cable", "Accessories", 10m), 4);

        var result = Pricing.Calculate([item], PricingRules.Default, null);

        Assert.Equal(0m, result.BulkDiscount);
        Assert.Equal(40m, result.Subtotal);
    }

    [Fact]
    public void GivesFreeShippingAtThreshold()
    {
        var item = new CartItem(new Product(1, "Monitor", "Displays", 100m), 1);

        var result = Pricing.Calculate([item], PricingRules.Default, null);

        Assert.Equal(0m, result.Shipping);
        Assert.Equal(112m, result.Total);
    }

    [Theory]
    [InlineData("SAVE10", 0.10)]
    [InlineData("welcome5", 0.05)]
    [InlineData("nurbolsyn", 0.50)]
    public void FindsPromoCodesWithoutCaseSensitivity(string code, double expectedRate)
    {
        var promo = Pricing.FindPromo(code);

        Assert.NotNull(promo);
        Assert.Equal((decimal)expectedRate, promo.Rate);
    }

    [Fact]
    public void AppliesPromoAfterBulkDiscount()
    {
        var item = new CartItem(new Product(1, "Cable", "Accessories", 10m), 5);
        var promo = Pricing.FindPromo("SAVE10");

        var result = Pricing.Calculate([item], PricingRules.Default, promo);

        Assert.Equal(5m, result.BulkDiscount);
        Assert.Equal(4.50m, result.PromoDiscount);
        Assert.Equal(40.50m, result.Taxable);
        Assert.Equal(4.86m, result.Tax);
        Assert.Equal(50.36m, result.Total);
    }

    [Fact]
    public void RejectsUnknownPromoCode()
    {
        Assert.Null(Pricing.FindPromo("NOT-A-CODE"));
    }
}

