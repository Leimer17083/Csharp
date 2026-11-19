namespace module
{
    class Car : Transport
    {
        public Car(int id, string name, decimal maxWeightKg)
            : base(id, name, maxWeightKg)
        {
        }

        public override decimal CalculateCost(Order order)
        {
            return 50m + 8m * order.DistanceKm + order.WeightKg;
        }
    }
}