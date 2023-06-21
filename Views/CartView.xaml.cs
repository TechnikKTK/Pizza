using Pizza.Models;

namespace Pizza.Views;

public partial class CartView : ContentView
{
    public static readonly BindableProperty ProductProperty =
      BindableProperty.Create("Product", typeof(Product), typeof(Product), null);

    public Product Product
    {
        get { return (Product)GetValue(ProductProperty); }
        set { SetValue(ProductProperty, value); }
    }

    public CartView()
	{
		InitializeComponent();
	}

    private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        if(Product == null) { return; }
        
        Product.CountInCart = Convert.ToInt32(e.NewValue);
        Product.Update();
    }
}