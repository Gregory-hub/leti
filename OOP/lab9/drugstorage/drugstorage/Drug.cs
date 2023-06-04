using System;

public abstract class Drug
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

