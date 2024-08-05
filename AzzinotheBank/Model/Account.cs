namespace AzzinotheBank.Model
{
    public class Account
    {
        public int Nif { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public Guid AccountNumer { get; set; }
        public List<Statement> Statements { get; set; }
    }
}
