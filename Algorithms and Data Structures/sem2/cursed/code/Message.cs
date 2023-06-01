using System.Numerics;


namespace cursed;
public class Message
{
	public string Text { get; set; }

	public Message(string text)
	{
		Text = text;
	}

	public byte[,] Encrypt(short bit_number, BigInteger aes_key)
	{
		return AES.Encrypt(Text, bit_number, aes_key);
	}

	public static Message FromEncrypted(byte[,] encoded, short bit_number, BigInteger aes_key)
	{
		return new Message(AES.Decrypt(encoded, bit_number, aes_key));
	}
}
