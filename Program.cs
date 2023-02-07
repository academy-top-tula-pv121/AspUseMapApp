namespace AspUseMapApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            string date = "";

            //
            //app.Use(async(context, next) =>
            //{
            //    // code before next.Invoke()
            //    date = DateTime.Now.ToString();
            //    var path = context.Request.Path.Value.ToLower();
            //    if(path == "/date")
            //    {
            //        await context.Response.WriteAsync($"Current USE date: {date}");
            //    }
            //    else
            //        await next.Invoke(context);

            //    // code after next.Invoke()
            //    //Console.WriteLine($"Current date for console: {date}");

            //});

            app.Use(GetDate);

            app.UseWhen(
                context => context.Request.Path == DateTime.Now.ToShortDateString(),
                appBuilder =>
                {

                });

            app.MapWhen(
                context => context.Request.Path == DateTime.Now.ToShortDateString(),
                appBuilder =>
                {

                });

            app.Map("/by", appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    await context.Response.WriteAsync($"By");
                    await next.Invoke(context);
                });
            });

            app.Map("/hello", appBuilder =>
            {
                appBuilder.Run(async (context) =>
                {
                    await context.Response.WriteAsync($"Hello");
                    //await next.Invoke(context);
                });
            });

            app.Map("/company", appBuilder => {         // "/company"
                appBuilder.Map("/about", About);        // "/company/about"
                appBuilder.Map("/contact", Contact);    // "/company/contact"
            });


            app.Run(async (context) =>
            {
                //await Task.Delay(10000);
                await context.Response.WriteAsync($"Current RUN date: {date}");
            });

            app.Run();
        }







        static void About(IApplicationBuilder appBuilder)
        {
            appBuilder.Run(async context => await context.Response.WriteAsync("About"));
        }

        static void Contact(IApplicationBuilder appBuilder)
        {
            appBuilder.Run(async context => await context.Response.WriteAsync("Contact"));
        }

        static async Task GetDate(HttpContext context, RequestDelegate next)
        {
            // code before next.Invoke()
            string date = DateTime.Now.ToString();
            var path = context.Request.Path.Value.ToLower();
            if (path == "/date")
            {
                await context.Response.WriteAsync($"Current USE date: {date}");
            }
            else
                await next.Invoke(context);

            // code after next.Invoke()
            //Console.WriteLine($"Current date for console: {date}");
        }
    }
}