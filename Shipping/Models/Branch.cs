namespace Shipping.Models
{
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; } = true;
        public bool IsDeleted { get; set; } = false;


        public virtual List<Employee> Employees { get; set; }
        public virtual List<Merchant> Merchants { get; set; }

    }
}
