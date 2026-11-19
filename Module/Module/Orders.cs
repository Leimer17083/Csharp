namespace module
{
    enum OrderStatus
    {
        New,
        Active,
        Delivered
    }

    class Order
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public decimal WeightKg { get; private set; }
        public decimal DistanceKm { get; private set; }
        public OrderStatus Status { get; private set; }

        public Order(int id, string description, decimal weightKg, decimal distanceKm)
        {
            Id = id;
            Description = description;
            WeightKg = weightKg;
            DistanceKm = distanceKm;
            Status = OrderStatus.New;
        }

        internal void SetStatus(OrderStatus status)
        {
            Status = status;
        }
    }
}