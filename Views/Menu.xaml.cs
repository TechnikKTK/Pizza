using Pizza.ViewModels;

namespace Pizza.Views;

public partial class Menu : ContentPage
{
	MainViewModel Context { get; set; }

    List<int> types = new List<int>();

	public Menu()
	{
		InitializeComponent();
		Context = (MainViewModel)this.BindingContext;
        types = new List<int>(new int[] { 1, 2, 3 });
    }

    private async void Pizza_Clicked(object sender, EventArgs e)
    {
        var button = (sender as Button);
        if (button.BackgroundColor.ToHex() == "#be311f".ToUpper())
        {
            button.BackgroundColor = Color.FromArgb("#ffffff");
            button.TextColor = Color.FromArgb("#be311f");
            
            types.Remove(1);
        }
        else
        {
            button.BackgroundColor = Color.FromArgb("#be311f");
            button.TextColor = Color.FromArgb("#ffffff");

            types.Add(1);
        }

        types.Sort();
        await Task.Run(()=>Context.Select(types.ToArray()));
    }

    private async void Drink_Clicked(object sender, EventArgs e)
    {
        var button = (sender as Button);
        if (button.BackgroundColor.ToHex() == "#be311f".ToUpper())
        {
            button.BackgroundColor = Color.FromArgb("#ffffff");
            button.TextColor = Color.FromArgb("#be311f");

            types.Remove(2);
        }
        else
        {
            button.BackgroundColor = Color.FromArgb("#be311f");
            button.TextColor = Color.FromArgb("#ffffff");

            types.Add(2);
        }

        types.Sort();
        await Task.Run(() => Context.Select(types.ToArray()));
    }

    private async void Sous_Clicked(object sender, EventArgs e)
    {
        var button = (sender as Button);
        if (button.BackgroundColor.ToHex() == "#be311f".ToUpper())
        {
            button.BackgroundColor = Color.FromArgb("#ffffff");
            button.TextColor = Color.FromArgb("#be311f");

            types.Remove(3);
        }
        else
        {
            button.BackgroundColor = Color.FromArgb("#be311f");
            button.TextColor = Color.FromArgb("#ffffff");

            types.Add(3);
        }

        types.Sort();
        await Task.Run(() => Context.Select(types.ToArray()));
    }
}