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
	public PrivateKey privateKey;
	public PublicKey publicKey;
	public BigInteger AESEncryptKey;
	public BigInteger AESDecryptKey;
	public short BitNumber;

	public User(string name, short bit_number = 1024)
	{
		Name = name;
		BitNumber = bit_number;
		RSA.InitRSA(ref publicKey, ref privateKey, BitNumber);
		AESEncryptKey = AES.GenerateKey();
	}

	public void ReceiveAESKey(BigInteger key)
	{
		AESDecryptKey = key;
	}

	public byte[,] SendMessage(string text, PublicKey public_key)
	{
		Message message = new Message(text);
		return message.Encode(public_key, BitNumber, AESEncryptKey);
	}

	public string RecieveMessage(byte[,] encoded)
	{
		Message message = Message.FromEncoded(encoded, privateKey, BitNumber, AESDecryptKey);
		return message.Text;
	}
}
