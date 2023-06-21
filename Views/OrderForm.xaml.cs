using Pizza.Models;

namespace Pizza.Views;

public partial class OrderForm : ContentPage
{
    public Orders Order { get; }

    public OrderForm()
	{
		InitializeComponent();
        var display = DeviceDisplay.Current.MainDisplayInfo;
        var dpi = display.Density;
        Table.HeightRequest = Convert.ToInt32(DeviceDisplay.Current.MainDisplayInfo.Height / dpi - 60);
    }

    public OrderForm(Orders order):this()
    {
        Order = order;
        CartCell.Height = Order.Products.Count * 61;
        CollectCell.HeightRequest = Order.Products.Count * 60;

        this.BindingContext = Order;
    }

    private async void SetState(object sender, EventArgs e)
    {
        List<string> data = new List<string>(Enum.GetNames(typeof(OrderState)));

        var result = await Application.Current.MainPage.DisplayActionSheet("Выберите статус", "Отмена", null, data.ToArray());
        if (data.Contains(result))
        {
            Order.OrderState = (OrderState)(data.IndexOf(result) + 1);
            Order.Update();
            await Order.Save();
        }
    }

    private async void Close(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PopModalAsync(true);
    }
}