using System;

public class DrugStorage : IStoring
{
    private List<Drug> drugs = new List<Drug>();
    public List<Drug> Drugs
    {
        get { return drugs; }
        set { drugs = value; }
    }

    public bool Available(string name, int quantity = 1)
    {
        Drug? d = Drugs.Find(d => d.Name == name);
        if (d is null) return false;
        if (d.Quantity < quantity) return false;
        return true;
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

    public Drug Extract(string name, int quantity)
    {
        Drug? d = Drugs.Find(d => d.Name == name);
        if (d is null) throw new ArgumentException("Drug is not found");
        if (d.Quantity < quantity) throw new ArgumentException("Not enough drugs in the storage");

        Drug? new_drug = Activator.CreateInstance(d.GetType(), new object[] { d.Name, d.Price, quantity }) as Drug;
        if (new_drug is not null) Remove(new_drug);

        return new_drug;
    }
}

