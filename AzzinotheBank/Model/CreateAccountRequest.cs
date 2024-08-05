namespace AzzinotheBank.Model
{
    public class CreateAccountRequest
    {
        public int Nif { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}
