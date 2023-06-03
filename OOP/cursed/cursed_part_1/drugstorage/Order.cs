using System;

public class Order
{
    public List<Drug> Goods = new List<Drug>();
    public int TotalPrice
    {
        get
        {
            int total_price = 0;
            foreach (Drug drug in Goods) total_price += drug.Price * drug.Quantity;
            return total_price;
        }
    }

    public void Add(Drug drug)
    {
        Drug? d = Goods.Find(d => d.Name == drug.Name);
        if (d is not null) d.Quantity += drug.Quantity;
        else Goods.Add(drug);
    }

    public void Remove(string name, int quantity)
    {
        Drug? drug = Goods.Find(d => d.Name == name);
        if (drug is null) throw new ArgumentException("drug is not found");
        if (drug.Quantity < quantity) throw new ArgumentException("not enough drugs in the storage");

        if (drug.Quantity == quantity) Goods.Remove(drug);
        else drug.Quantity -= quantity;
    }

    public void Clear()
    {
        Goods = new List<Drug>();
    }

    public void Send(int balance)
    {
        if (Goods.Count == 0)
        {
            Console.WriteLine("Cannot send order. Order is empty");
            return;
        }
        if (balance < TotalPrice) Console.WriteLine("Cannot send order. Balance is too low");
        Console.WriteLine();
        Console.WriteLine("SENDNIG ORDER:");
        foreach (Drug drug in Goods) Console.WriteLine($"{drug.Name}: {drug.Quantity} units");
        Clear();
    }
}

