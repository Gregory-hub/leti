using System.IO;
using System;

    /// <summary>
    ///   A BankTransaction is created every time a deposit or withdrawal occurs on a BankAccount
    ///   A BankTransaction records the amount of money involved, together with the current date and time.
    /// </summary>
public class BankTransaction
{
	private readonly decimal amount;
	private readonly DateTime when;

	public BankTransaction(decimal tranAmount)
	{
		amount = tranAmount;
		when = DateTime.Now;
	}

	public decimal Amount()
	{
		return amount;
	}

	public DateTime When()
	{
		return when;
	}
	~BankTransaction()
	{
		StreamWriter swFile = File.AppendText("Transactions.Dat");
		swFile.WriteLine("Date/Time: {0}\tAmount: {1}", when, amount);
		swFile.Close();
		GC.SuppressFinalize(this);
	}
}

