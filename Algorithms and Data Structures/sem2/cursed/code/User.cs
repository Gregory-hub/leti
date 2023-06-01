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
	private BigInteger AESKey;
	private BigInteger AESOtherSideKey;
	public short BitNumber;

	public User(string name, short bit_number = 1024)
	{
		Name = name;
		BitNumber = bit_number;
		RSA.InitRSA(ref publicKey, ref privateKey, BitNumber);
		AESKey = AES.GenerateKey();
	}

	public BigInteger SendAESKey(PublicKey rsa_key)
	{
		return RSA.Encrypt(AESKey, rsa_key);
	}

	public void ReceiveAESKey(BigInteger key)
	{
		AESOtherSideKey = RSA.Decrypt(key, privateKey);
	}

	public byte[,] SendMessage(string text)
	{
		Message message = new Message(text);
		return message.Encrypt(BitNumber, AESKey);
	}

	public string RecieveMessage(byte[,] encrypted)
	{
		Message message = Message.FromEncrypted(encrypted, BitNumber, AESOtherSideKey);
		return message.Text;
	}
}
