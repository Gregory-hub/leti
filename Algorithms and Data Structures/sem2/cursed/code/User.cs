using System.Numerics;


namespace cursed;
public class User
{
	public struct PublicKey
	{
		public BigInteger E;
		public BigInteger M;
	}

	public struct PrivateKey
	{
		public BigInteger D;
		public BigInteger M;
	}

	public string Name;
	private PrivateKey privateKey;
	public PublicKey publicKey;
	private short BitNumber;

	public User(string name, short bit_number = 1024)
	{
		Name = name;
		BitNumber = bit_number;
		InitRSA();
	}

	public void InitRSA()
	{
		RSA.InitRSA(ref publicKey, ref privateKey, BitNumber);
	}

	public BigInteger[] SendMessage(string text, PublicKey public_key)
	{
		Message message = new Message(text);
		return message.Encode(public_key);
	}

	public string RecieveMessage(BigInteger[] encoded)
	{
		Message message = Message.FromEncoded(encoded, privateKey);
		return message.Text;
	}
}
