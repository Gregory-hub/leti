using System;

public interface IOrderable
{
    public bool Available(int number = 1);

    public void AddToOrder(Order order, int number);
}

