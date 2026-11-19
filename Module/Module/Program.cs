using System;
using System.Text;

namespace module
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            DeliveryService service = new DeliveryService();

            service.AddTransport(new Bike(1, "Велосипед", 5m));
            service.AddTransport(new Car(2, "Автомобіль", 100m));
            service.AddTransport(new Truck(3, "Вантажівка", 1000m));


            service.AddOrder(new Order(101, "Документи", 2m, 5m));
            service.AddOrder(new Order(102, "Посилка", 20m, 15m));
            service.AddOrder(new Order(103, "Важкий вантаж", 600m, 5m));
            service.AddOrder(new Order(104, "Маленька посилка", 1m, 2m));

            Console.WriteLine("=== 1. Вартість доставки ===");
            service.ShowCosts(101);
            service.ShowCosts(102);
            service.ShowCosts(103);

            Console.WriteLine("\n=== 2. Призначення замовлення ===");
            service.AssignOrder(101, 1);

            Console.WriteLine("\n=== 3. Спроби з зайнятим або невідповідним транспортом ===");
            service.AssignOrder(104, 1);
            service.AssignOrder(103, 2);

            Console.WriteLine("\n=== 4. Призначення відповідного транспорту ===");
            service.AssignOrder(102, 2);
            service.AssignOrder(103, 3);
            service.AssignOrder(102, 3);

            Console.WriteLine("\n=== 5. Активні замовлення ===");
            service.ShowActiveOrders();

            Console.WriteLine("\n=== 6. Доставка й оплата ===");
            service.DeliverOrder(101, new CardPayment());
            service.DeliverOrder(102, new CashPayment());
            service.DeliverOrder(103, new CardPayment());

            Console.WriteLine("\n=== 7. Історія доставок ===");
            service.ShowDeliveryHistory();

            Console.WriteLine("\nАктивні замовлення після доставок:");
            service.ShowActiveOrders();
        }
    }
}