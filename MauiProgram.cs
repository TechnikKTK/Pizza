namespace Pizza
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //CreateTemp();


            return builder.Build();
        }

        private static async void CreateTemp()
        {

            Models.Product product = new Models.Product();

            product.Cost = 590;
            product.Count = 1;
            product.ImagePath = "pizza/student.png";
            product.Name = "Студенческая";
            product.Description = "Томатный соус, моцарелла, ветчина, салями, шампиньоны\r\n30 см, 600 г\r\n";

            await product.Save();
            product = new Models.Product();

            product.Cost = 480;
            product.Count = 1;
            product.ImagePath = "pizza/salyami.png";
            product.Name = "Салями";
            product.Description = "Томатный соус, моцарелла, салями\r\n30 см, 570 г\r\n";

            await product.Save();
            product = new Models.Product();

            product.Cost = 550;
            product.Count = 1;
            product.ImagePath = "pizza/vetchina.png";
            product.Name = "Ветчина и грибы";
            product.Description = "Томатный соус, моцарелла, ветчина, шампиньоны \r\n30 см, 620 г\r\n";

            await product.Save();
            product = new Models.Product();

            product.Cost = 500;
            product.Count = 1;
            product.ImagePath = "pizza/chesse.png";
            product.Name = "Сырная";
            product.Description = "Моцарелла, брынза, чесночное масло\r\n30 см, 560 г\r\n";

            await product.Save();
            product = new Models.Product();

            product.Cost = 90;
            product.Count = 1;
            product.ImagePath = "pizza/dushes.png";
            product.TypeProduct = Models.TypeProduct.Напитки;
            product.Name = "Напиток";
            product.Description = "Черноголовка Дюшес 0,5 л";

            await product.Save();
            product = new Models.Product();

            product.Cost = 90;
            product.Count = 1;
            product.TypeProduct = Models.TypeProduct.Напитки;
            product.ImagePath = "pizza/cola.png";
            product.Name = "Напиток";
            product.Description = "Черноголовка Кола 0,5 л";

            await product.Save();
            product = new Models.Product();

            product.Cost = 130;
            product.Count = 1;
            product.TypeProduct = Models.TypeProduct.Напитки;
            product.ImagePath = "pizza/mors_brusnich.png";
            product.Name = "Морс";
            product.Description = "Морс брусничный 0,45 л";

            await product.Save();
            product = new Models.Product();

            product.Cost = 130;
            product.Count = 1;
            product.TypeProduct = Models.TypeProduct.Напитки;
            product.ImagePath = "pizza/mors_smorod.png";
            product.Name = "Морс";
            product.Description = "Морс черная смородина 0,45 л";

            await product.Save();
            product = new Models.Product();


            product.Cost = 50;
            product.Count = 1;
            product.TypeProduct = Models.TypeProduct.Соусы;
            product.ImagePath = "pizza/syr.png";
            product.Name = "Соус";
            product.Description = "Сырный соус 40 г";

            await product.Save();
            product = new Models.Product();


            product.Cost = 50;
            product.Count = 1;
            product.TypeProduct = Models.TypeProduct.Соусы;
            product.ImagePath = "pizza/chesnok.png";
            product.Name = "Соус";
            product.Description = "Чесночный соус 40 г";

            await product.Save();
            product = new Models.Product();

            product.Cost = 50;
            product.Count = 1;
            product.TypeProduct = Models.TypeProduct.Соусы;
            product.ImagePath = "pizza/barbeku.png";
            product.Name = "Соус";
            product.Description = "Соус барбекю 40 г";

            await product.Save();
            product = new Models.Product();
        }
    }
}