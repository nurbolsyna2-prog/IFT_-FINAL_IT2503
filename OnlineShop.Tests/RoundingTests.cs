using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Tests;

public class RoundingTests
{
    [Fact]
    public void RoundsTaxMidpointsAwayFromZero()
    {
        var item = new CartItem(new Product(1, "Sticker", "Stationery", 0.05m), 1);
        var rules = PricingRules.Default with { TaxRate = 0.10m, ShippingFee = 0m };

        var result = Pricing.Calculate([item], rules, null);

        Assert.Equal(0.01m, result.Tax);
        Assert.Equal(0.06m, result.Total);
    }
}
