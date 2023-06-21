using Pizza.ViewModels;

namespace Pizza.Views;

public partial class Cart : ContentPage
{
    CartViewModel ViewModel { get; set; }
    public Cart()
    {
        InitializeComponent();
        ViewModel = (CartViewModel)this.BindingContext;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ViewModel.Update();
    }
}