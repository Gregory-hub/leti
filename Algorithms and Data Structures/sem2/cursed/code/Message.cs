using System.Numerics;


namespace cursed;
public class Message
{
	public string Text { get; set; }

	public Message(string text)
	{
		Text = text;
	}

	public byte[,] Encode(User.PublicKey public_key, short bit_number)
	{
		BigInteger[] rsa_encoded = RSA.Encode(Text, public_key);
		return AES.Encode(rsa_encoded, bit_number);
	}

	public static Message FromEncoded(byte[,] encoded, User.PrivateKey private_key)
	{
		BigInteger[] aes_decoded = AES.Decode(encoded);
		return new Message(RSA.Decode(aes_decoded, private_key));
	}
}
