using System;
using System.Collections.Generic;

namespace module
{
    interface IPaymentMethod
    {
        string Name { get; }
        void Pay(decimal amount);
    }

    class CardPayment : IPaymentMethod
    {
        public string Name
        {
            get { return "карткою"; }
        }

        public void Pay(decimal amount)
        {
            Console.WriteLine("Оплата карткою: " + amount.ToString("F2") + " грн.");
        }
    }

    class CashPayment : IPaymentMethod
    {
        public string Name
        {
            get { return "готівкою"; }
        }

        public void Pay(decimal amount)
        {
            Console.WriteLine("Оплата готівкою: " + amount.ToString("F2") + " грн.");
        }
    }

    class DeliveryRecord
    {
        public Order Order { get; private set; }
        public Transport Transport { get; private set; }
        public decimal Cost { get; private set; }
        public string PaymentMethod { get; private set; }

        public DeliveryRecord(
            Order order,
            Transport transport,
            decimal cost,
            string paymentMethod)
        {
            Order = order;
            Transport = transport;
            Cost = cost;
            PaymentMethod = paymentMethod;
        }
    }

    class DeliveryService
    {
        private Dictionary<int, Transport> transports =
            new Dictionary<int, Transport>();

        private Dictionary<int, Order> orders =
            new Dictionary<int, Order>();

        private Dictionary<int, Transport> activeOrders =
            new Dictionary<int, Transport>();

        private List<DeliveryRecord> deliveryHistory =
            new List<DeliveryRecord>();

        public void AddTransport(Transport transport)
        {
            if (transport.Id <= 0 || transports.ContainsKey(transport.Id))
            {
                Console.WriteLine("Помилка: некоректний або повторний ID транспорту.");
                return;
            }

            transports.Add(transport.Id, transport);
        }

        public void AddOrder(Order order)
        {
            if (order.Id <= 0 || orders.ContainsKey(order.Id))
            {
                Console.WriteLine("Помилка: некоректний або повторний ID замовлення.");
                return;
            }

            if (order.WeightKg <= 0 || order.DistanceKm <= 0)
            {
                Console.WriteLine("Помилка: вага й відстань мають бути додатними.");
                return;
            }

            orders.Add(order.Id, order);
        }

        public void ShowCosts(int orderId)
        {
            Order order;

            if (!orders.TryGetValue(orderId, out order))
            {
                Console.WriteLine("Помилка: замовлення №" + orderId + " не знайдено.");
                return;
            }

            bool foundTransport = false;

            foreach (Transport transport in transports.Values)
            {
                if (transport.CanTake(order))
                {
                    string status = transport.IsBusy ? "зайнятий" : "вільний";

                    Console.WriteLine(
                        transport.Name + ": " +
                        transport.CalculateCost(order).ToString("F2") +
                        " грн. (" + status + ")");

                    foundTransport = true;
                }
            }

            if (!foundTransport)
            {
                Console.WriteLine("Немає транспорту, який може взяти це замовлення.");
            }
        }

        public void AssignOrder(int orderId, int transportId)
        {
            Order order;
            Transport transport;

            if (!orders.TryGetValue(orderId, out order))
            {
                Console.WriteLine("Помилка: замовлення №" + orderId + " не знайдено.");
                return;
            }

            if (order.Status != OrderStatus.New)
            {
                Console.WriteLine("Помилка: замовлення вже призначене або доставлене.");
                return;
            }

            if (!transports.TryGetValue(transportId, out transport))
            {
                Console.WriteLine("Помилка: транспорт не знайдено.");
                return;
            }

            if (transport.IsBusy)
            {
                Console.WriteLine("Помилка: транспорт «" + transport.Name + "» зайнятий.");
                return;
            }

            if (!transport.CanTake(order))
            {
                Console.WriteLine(
                    "Помилка: транспорт «" + transport.Name +
                    "» не може взяти це замовлення.");
                return;
            }

            transport.Occupy();
            order.SetStatus(OrderStatus.Active);
            activeOrders.Add(orderId, transport);

            Console.WriteLine(
                "Замовлення №" + orderId +
                " призначено транспорту «" + transport.Name + "».");
        }

        public void DeliverOrder(int orderId, IPaymentMethod paymentMethod)
        {
            Transport transport;

            if (!activeOrders.TryGetValue(orderId, out transport))
            {
                Console.WriteLine(
                    "Помилка: активне замовлення №" + orderId + " не знайдено.");
                return;
            }

            Order order = orders[orderId];
            decimal cost = transport.CalculateCost(order);

            paymentMethod.Pay(cost);

            deliveryHistory.Add(
                new DeliveryRecord(order, transport, cost, paymentMethod.Name));

            order.SetStatus(OrderStatus.Delivered);
            transport.Release();
            activeOrders.Remove(orderId);

            Console.WriteLine(
                "Замовлення №" + orderId +
                " доставлено. Транспорт звільнено.");
        }

        public void ShowActiveOrders()
        {
            if (activeOrders.Count == 0)
            {
                Console.WriteLine("Активних замовлень немає.");
                return;
            }

            foreach (KeyValuePair<int, Transport> item in activeOrders)
            {
                Order order = orders[item.Key];

                Console.WriteLine(
                    "№" + order.Id + ": " + order.Description +
                    ", " + order.WeightKg + " кг, " +
                    order.DistanceKm + " км; транспорт: " +
                    item.Value.Name + ".");
            }
        }

        public void ShowDeliveryHistory()
        {
            if (deliveryHistory.Count == 0)
            {
                Console.WriteLine("Історія доставок порожня.");
                return;
            }

            foreach (DeliveryRecord record in deliveryHistory)
            {
                Console.WriteLine(
                    "№" + record.Order.Id + ": " + record.Order.Description +
                    "; транспорт: " + record.Transport.Name +
                    "; вартість: " + record.Cost.ToString("F2") +
                    " грн.; оплата: " + record.PaymentMethod + ".");
            }
        }
    }
}