using Microsoft.Maui.Hosting;
using Pizza.Models;
using Pizza.ViewModels;

namespace Pizza.Views;

public partial class OrderDetails : ContentPage
{
    public CartViewModel Context { get; }

    public OrderDetails()
	{
		InitializeComponent();
        
        var display = DeviceDisplay.Current.MainDisplayInfo;
        var dpi = display.Density;
        Table.HeightRequest = Convert.ToInt32(DeviceDisplay.Current.MainDisplayInfo.Height / dpi - 60);
    }

    public OrderDetails(CartViewModel client):this()
    {
        Context = client;
        this.BindingContext = Context;
        Context.Client.CurrentOrder.DeliveryPhone = Context.Client.Phone;
        CartCell.Height = Context.Client.Cart.Count * 60;
    }
}