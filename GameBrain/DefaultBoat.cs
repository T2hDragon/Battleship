namespace GameBrain
{
    public class DefaultBoat
    {
        public DefaultBoat(string name, int length, int amount)
        {
            Length = length;
            Name = name;
            Amount = amount;
        }

        public int Amount { get; set; }

        public string Name { get; set; }

        public int Length { get; set; }
    }
}