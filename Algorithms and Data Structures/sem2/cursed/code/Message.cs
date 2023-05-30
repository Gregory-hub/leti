using System.Numerics;


namespace cursed;
public class Message
{
	public string Text { get; set; }

	public Message(string text)
	{
		Text = text;
	}

	public BigInteger[] Encode(User.PublicKey public_key)
	{
		return RSA.Encode(Text, public_key);
	}

	public static Message FromEncoded(BigInteger[] encoded, User.PrivateKey private_key)
	{
		return new Message(RSA.Decode(encoded, private_key));
	}
}
