using System;

public abstract class Drug : IOrderable
{
    public int Price;
    public int Quantity;
    public string Type = "";
    private string name = "";
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public Drug(string name, int price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }

    bool IOrderable.Available(int number)
    {
        return Quantity >= number; 
    }

    void IOrderable.AddToOrder(Order order, int number)
    {
        if (number > Quantity)
        {
            for (int i = 0; i < number; i++) order.Add(this);
        }
        else throw new ArgumentOutOfRangeException("trying to add more than Quantity");
    }


    public class Antidepressant : Drug
    {
        public Antidepressant(string name, int price, int quantity) : base(name, price, quantity)
        {
            Type = "Antidepressant";
        }
    }


    public class Barbiturate : Drug
    {
        public Barbiturate(string name, int price, int quantity) : base(name, price, quantity)
        {
            Type = "Barbiturate";
        }
    }


    public class Hormone : Drug
    {
        public Hormone(string name, int price, int quantity) : base(name, price, quantity)
        {
            Type = "Hormone";
        }
    }
}

