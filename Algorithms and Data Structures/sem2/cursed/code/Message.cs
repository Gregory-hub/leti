using System.Numerics;


namespace cursed;
public class Message
{
	public string Text { get; set; }

	public Message(string text)
	{
		Text = text;
	}

	public byte[,] Encrypt(BigInteger aes_key)
	{
		return AES.Encrypt(Text, aes_key);
	}

	public static Message FromEncrypted(byte[,] encoded, BigInteger aes_key)
	{
		return new Message(AES.Decrypt(encoded, aes_key));
	}
}
