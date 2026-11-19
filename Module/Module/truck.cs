namespace module
{
    class Truck : Transport
    {
        public Truck(int id, string name, decimal maxWeightKg)
            : base(id, name, maxWeightKg)
        {
        }

        public override decimal CalculateCost(Order order)
        {
            return 200m + 15m * order.DistanceKm + 0.5m * order.WeightKg;
        }
    }
}