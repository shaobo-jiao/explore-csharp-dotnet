namespace Nutshell.EntityModels;

public class Purchase
{
    public int ID { get; set; }
    public required string Description { get; set; }
    public DateTime Date { get; set; }
    public decimal Price {get; set; }

    public int? CustomerID {get; set; }
    public virtual Customer? Customer {get; set; }

}
