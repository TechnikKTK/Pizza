using Pizza.Models;

namespace Pizza.Views;

public partial class OrderView : ContentView
{
    public Orders Context { get; set; }

    public OrderView()
    {
        InitializeComponent();
    }

    private void ExpandFrame(object sender, EventArgs e)
    {
        if (Context == null)
        {
            Context = (Orders)BindingContext;
        }

        Context.IsExpand = !Context.IsExpand;
        if (Context.IsExpand)
        {
            Expander.RotateTo(180);
            CartCell.HeightRequest = Context.Products.Count * 60;
            CartFrame.HeightRequest = Context.Products.Count * 60 + 80;
        }
        else
        {
            Expander.RotateTo(0);
            CartCell.HeightRequest = 0;
            CartFrame.HeightRequest = 0;
        }

        Context.Update();
    }
}