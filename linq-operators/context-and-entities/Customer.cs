namespace Nutshell.EntityModels;

public class Customer
{
    public int ID { get; set; }
    public required string Name { get; set; }

    public virtual List<Purchase> Purchases{ get; set; } = new List<Purchase>();
}
