using System;

public class DrugStorage
{
    public List<Drug> Drugs = new List<Drug>();

    public void Add(Drug drug)
    {
        Drug? d = Drugs.Find(d => d.Name == drug.Name);
        if (d is not null) d.Quantity += drug.Quantity;
        else Drugs.Add(drug);
    }

    public void Remove(Drug drug)
    {
        Drugs.Remove(drug);
    }

    public bool Available(string name, int quantity = 1)
    {
        Drug? drug = Drugs.Find(d => d.Name == name);
        if (drug is null) return false;
        if (drug.Quantity < quantity) return false;
        return true;
    }

    public Drug? Get(string name)
    {
        if (!Available(name)) return null;
        return Drugs.Find(d => d.Name == name);
    }

    public Drug Extract(string name, int quantity)
    {
        Drug? drug = Drugs.Find(d => d.Name == name);
        if (drug is null) throw new ArgumentException("drug is not found");
        if (drug.Quantity < quantity) throw new ArgumentException("not enough drugs in the storage");

        Drug? new_drug = Activator.CreateInstance(drug.GetType(), new object[] { drug.Name, drug.Price, quantity }) as Drug;
        drug.Quantity -= quantity;
        if (drug.Quantity == 0) Remove(drug);

        return new_drug;
    }
}

