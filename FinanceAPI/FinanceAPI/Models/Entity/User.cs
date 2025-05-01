namespace FinanceAPI.Models.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
    }
}
