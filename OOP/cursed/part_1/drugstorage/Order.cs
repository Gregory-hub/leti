using System;

public class Order : IStoring
{
    private List<Drug> drugs = new List<Drug>();
    public List<Drug> Drugs
    {
        get { return drugs; }
        set { drugs = value; }
    }

    public int TotalPrice
    {
        get
        {
            int total_price = 0;
            foreach (Drug drug in Drugs) total_price += drug.Price * drug.Quantity;
            return total_price;
        }
    }

    public void Add(Drug drug)
    {
        Drug? d = Drugs.Find(d => d.Name == drug.Name);
        if (d is not null) d.Quantity += drug.Quantity;
        else
        {
            Drug? new_drug = Activator.CreateInstance(drug.GetType(), new object[] { drug.Name, drug.Price, drug.Quantity }) as Drug;
            if (new_drug is not null) Drugs.Add(new_drug);
        }
    }

    public void Remove(Drug drug)
    {
        Drug? d = Drugs.Find(d => d.Name == drug.Name);
        if (d is null) throw new ArgumentException("Drug is not found");
        if (d.Quantity < drug.Quantity) throw new ArgumentException("Trying to remove more drugs than storage has");

        if (d.Quantity == drug.Quantity) Drugs.Remove(d);
        else d.Quantity -= drug.Quantity;
    }

    public void Clear()
    {
        Drugs = new List<Drug>();
    }

    public void Send(int balance)
    {
        if (Drugs.Count == 0)
        {
            Console.WriteLine("Cannot send order. Order is empty");
            return;
        }
        if (balance < TotalPrice) Console.WriteLine("Cannot send order. Balance is too low");
        Console.WriteLine();
        Console.WriteLine("SENDNIG ORDER:");
        foreach (Drug drug in Drugs) Console.WriteLine($"{drug.Name}: {drug.Quantity} units");
        Console.WriteLine($"Total price: {TotalPrice}$");
        Clear();
    }
}

