using System.Text;
using System.Numerics;


namespace cursed;
public static class RSA
{
	public static BigInteger[] Encode(string text, User.PublicKey public_key)
	{
		BigInteger[] encoded = new BigInteger[text.Length];
		for (int i = 0; i < text.Length; i++)
		{
			encoded[i] = PrimeFinder.ModulusPow(text[i], public_key.E, public_key.M);
		}

		return encoded;
	}

	public static string Decode(BigInteger[] encoded, User.PrivateKey private_key)
	{
		StringBuilder decoded = new StringBuilder();

		for (int i = 0; i < encoded.Length; i++)
		{
			BigInteger value = PrimeFinder.ModulusPow(encoded[i], private_key.D, private_key.M);
			decoded.Append((char)value);
		}

		return decoded.ToString();
	}

	public static void InitRSA(ref User.PublicKey public_key, ref User.PrivateKey private_key, short bit_number)
	{
		PrimeFinder prime_finder = new PrimeFinder();

        BigInteger[] primes = prime_finder.FindRSAPrimes((short)(bit_number / 2));	// prime * prime => bit_number bits
		BigInteger m = (BigInteger)primes[0] * (BigInteger)primes[1];
		BigInteger fi = ((BigInteger)primes[0] - 1) * ((BigInteger)primes[1] - 1);		// Fi(m) - Euler function
		BigInteger e = prime_finder.FindCoprime(fi, bit_number);

		public_key.E = e;
		public_key.M = m;

		BigInteger d = prime_finder.FindPrivateKey(e, fi);
		private_key.M = m;
		private_key.D = d;
	}

}
