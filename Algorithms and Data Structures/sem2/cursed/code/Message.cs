using System.Text;
using System.Numerics;


namespace cursed;


class Message
{
	public string Text { get; set; }

	public Message(string text)
	{
		Text = text;
	}

	public BigInteger[] Encode(User.PublicKey public_key)
	{
		BigInteger[] encoded = new BigInteger[Text.Length];
		for (int i = 0; i < Text.Length; i++)
		{
			encoded[i] = ModulusPow(Text[i], public_key.E, public_key.M);
		}

		return encoded;
	}

	public static Message FromEncoded(BigInteger[] encoded, User.PrivateKey private_key)
	{
		StringBuilder decoded = new StringBuilder();

		for (int i = 0; i < encoded.Length; i++)
		{
			decoded.Append((char)ModulusPow(encoded[i], private_key.D, private_key.M));
		}

		return new Message(decoded.ToString());
	}

	public static BigInteger ModulusPow(BigInteger num, long pow, long modulus)
	{
		BigInteger result = 1;
		// for (int n = 1; n <= pow; n++)
		while (pow > 0)
		{
			if (pow % 2 == 1) result = (result * num) % modulus;
			num = num * num % modulus;
			pow /= 2;
		}

		return result;
	}
}
