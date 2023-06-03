namespace cursed_part_1
{
    internal class Program
    {
        static int ParseInt(string message)
        {
            while (true)
            {
                Console.Write(message);
                try
                {
                    int input = int.Parse(Console.ReadLine());
                    return input;
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid input. Try again");
                    Console.WriteLine();
                }
            }
        }

        static void AddDrug(ref DrugStorage drugStorage, ref Order order)
        {
            Console.WriteLine();
            Console.WriteLine("Add drug:");
            if (drugStorage.Drugs.Count == 0)
            {
                Console.WriteLine("Cannot add drugs. Storage is empty");
                return;
            }

            for (int i = 0; i < drugStorage.Drugs.Count; i++) Console.WriteLine($"{i + 1}. {drugStorage.Drugs[i].Type}: {drugStorage.Drugs[i].Name} ({drugStorage.Drugs[i].Price}$, {drugStorage.Drugs[i].Quantity} available)");

            int input = ParseInt("Enter your choice: ") - 1;
            if (input < 0 || input >= drugStorage.Drugs.Count)
            {
                Console.WriteLine("\nInvalid choice. Try again");
                AddDrug(ref drugStorage, ref order);
                return;
            }
            Drug drug = drugStorage.Drugs[input];

            int quantity = ParseInt("Enter quantity: ");
            if (quantity <= 0 || quantity > drug.Quantity) 
            {
                Console.WriteLine("\nInvalid choice. Try again");
                AddDrug(ref drugStorage, ref order);
                return;
            }

            if (drugStorage.Available(drug.Name, quantity)) order.Add(drugStorage.Extract(drug.Name, quantity));
        }

        static void RemoveDrug(ref DrugStorage drugStorage, ref Order order)
        {
            Console.WriteLine();
            Console.WriteLine("Remove drugs:");
            if (order.Goods.Count == 0)
            {
                Console.WriteLine("Cannot remove drugs. Order is empty");
                return;
            }

            for (int i = 0; i < order.Goods.Count; i++) Console.WriteLine($"{i + 1}. {order.Goods[i].Type}: {order.Goods[i].Name} ({order.Goods[i].Price}$, {order.Goods[i].Quantity} units)");

            int input = ParseInt("Enter your choice: ") - 1;
            if (input < 0 || input >= order.Goods.Count)
            {
                Console.WriteLine("\nInvalid choice. Try again");
                RemoveDrug(ref drugStorage, ref order);
                return;
            }
            Drug drug = order.Goods[input];

            int quantity = ParseInt("Enter quantity: ");
            if (quantity <= 0 || quantity > drug.Quantity) 
            {
                Console.WriteLine("\nInvalid choice. Try again");
                RemoveDrug(ref drugStorage, ref order);
                return;
            }
            order.Remove(drug.Name, quantity);
            drug.Quantity = quantity;
            drugStorage.Add(drug);
        }

        static void Main(string[] args)
        {
            DrugStorage drugStorage = new DrugStorage();

            drugStorage.Add(new Drug.Antidepressant("Amitriptyline", 200, 2000));
            drugStorage.Add(new Drug.Antidepressant("Remeron", 500, 800));
            drugStorage.Add(new Drug.Barbiturate("Phenobarbital", 20, 6000));
            drugStorage.Add(new Drug.Hormone("Dehydroepiandrosterone", 15, 4000));

            Order order = new Order();
            int input;
            Drug drug;

            int balance = ParseInt("Enter your balance: ");
            while (balance < 0)
            {
                Console.WriteLine("Balance cannot be less than 0. Try again");
                balance = ParseInt("Enter your balance: ");
            }

            Console.WriteLine();
            Console.WriteLine($"Your balance: {balance}");
            Console.WriteLine("NEW ORDER");
            AddDrug(ref drugStorage, ref order);

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Your order:");
                for (int i = 0; i < order.Goods.Count; i++)
                {
                    drug = order.Goods[i];
                    Console.WriteLine($"{drug.Type}: {drug.Name}, {drug.Quantity} units");
                }
                Console.WriteLine($"Total price: {order.TotalPrice}$");

                Console.WriteLine();
                Console.WriteLine($"1. Add more");
                Console.WriteLine($"2. Remove drugs");
                Console.WriteLine($"3. Send order");
                input = ParseInt("Enter your choice: ");

                switch (input)
                {
                    case 1:
                        AddDrug(ref drugStorage, ref order);
                        break;
                    case 2:
                        RemoveDrug(ref drugStorage, ref order);
                        break;
                    case 3:
                        if (balance >= order.TotalPrice)
                        {
                            int total_price = order.TotalPrice;
                            order.Send(balance);
                            balance -= total_price;
                            Environment.Exit(0);
                        }
                        else Console.WriteLine("\nCannot send order. Balance is too low");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again");
                        break;
                }
            }
        }
    }
}

