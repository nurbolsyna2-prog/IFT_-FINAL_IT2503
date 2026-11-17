using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Tests;

public class CartStateTests
{
    private static readonly Product Mouse = new(1, "Mouse", "Accessories", 14.99m);

    [Fact]
    public void AddingSameProductIncrementsQuantity()
    {
        var cart = new CartState(PricingRules.Default);

        cart.Add(Mouse);
        cart.Add(Mouse);

        var item = Assert.Single(cart.Items);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(2, cart.TotalQuantity);
    }

    [Fact]
    public void SetQuantityRejectsValuesOutsideAllowedRange()
    {
        var cart = new CartState(PricingRules.Default);
        cart.Add(Mouse);

        var tooSmall = cart.SetQuantity(Mouse.Id, 0);
        var tooLarge = cart.SetQuantity(Mouse.Id, CartState.MaxQuantity + 1);

        Assert.False(tooSmall.IsOk);
        Assert.False(tooLarge.IsOk);
        Assert.Equal(1, Assert.Single(cart.Items).Quantity);
    }

    [Fact]
    public void SetQuantityAcceptsValueWithinAllowedRange()
    {
        var cart = new CartState(PricingRules.Default);
        cart.Add(Mouse);

        var result = cart.SetQuantity(Mouse.Id, 3);

        Assert.True(result.IsOk);
        Assert.Equal(3, Assert.Single(cart.Items).Quantity);
    }

    [Fact]
    public void RemoveDeletesProductFromCart()
    {
        var cart = new CartState(PricingRules.Default);
        cart.Add(Mouse);

        cart.Remove(Mouse.Id);

        Assert.Empty(cart.Items);
        Assert.Equal(0, cart.TotalQuantity);
    }

    [Fact]
    public void AppliesValidPromoAndRejectsUnknownPromo()
    {
        var cart = new CartState(PricingRules.Default);

        var invalid = cart.ApplyPromo("UNKNOWN");
        var valid = cart.ApplyPromo("SAVE10");

        Assert.False(invalid.IsOk);
        Assert.True(valid.IsOk);
        Assert.Equal("SAVE10", cart.AppliedPromo?.Code);
    }

    [Fact]
    public void ClearEmptiesItemsAndRemovesPromo()
    {
        var cart = new CartState(PricingRules.Default);
        cart.Add(Mouse);
        cart.ApplyPromo("SAVE10");

        cart.Clear();

        Assert.Empty(cart.Items);
        Assert.Null(cart.AppliedPromo);
    }

    [Fact]
    public void NotifiesSubscribersWhenCartChanges()
    {
        var cart = new CartState(PricingRules.Default);
        var notifications = 0;
        cart.OnChange += () => notifications++;

        cart.Add(Mouse);

        Assert.Equal(1, notifications);
    }
}
