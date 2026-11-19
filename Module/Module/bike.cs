namespace module
{
    class Bike : Transport
    {
        public Bike(int id, string name, decimal maxWeightKg)
            : base(id, name, maxWeightKg)
        {
        }

        public override decimal CalculateCost(Order order)
        {
            return 20m + 5m * order.DistanceKm;
        }

        public override bool CanTake(Order order)
        {
            return base.CanTake(order) && order.DistanceKm <= 10m;
        }
    }
}