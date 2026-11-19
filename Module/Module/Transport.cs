namespace module
{
    abstract class Transport
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal MaxWeightKg { get; private set; }
        public bool IsBusy { get; private set; }

        protected Transport(int id, string name, decimal maxWeightKg)
        {
            Id = id;
            Name = name;
            MaxWeightKg = maxWeightKg;
            IsBusy = false;
        }

        public abstract decimal CalculateCost(Order order);

        public virtual bool CanTake(Order order)
        {
            return order.WeightKg <= MaxWeightKg;
        }

        internal void Occupy()
        {
            IsBusy = true;
        }

        internal void Release()
        {
            IsBusy = false;
        }
    }
}