using System.Text;
using System.Numerics;


namespace cursed;
public static class RSA
{
	public static BigInteger Encrypt(BigInteger data, User.PublicKey public_key)
	{
		return PrimeFinder.ModulusPow(data, public_key.E, public_key.M);
	}

	public static BigInteger Decrypt(BigInteger encrypted, User.PrivateKey private_key)
	{
		return PrimeFinder.ModulusPow(encrypted, private_key.D, private_key.M);
	}

	public static void InitRSA(ref User.PublicKey public_key, ref User.PrivateKey private_key, short bit_number)
	{
		PrimeFinder prime_finder = new PrimeFinder();

        BigInteger[] primes = prime_finder.FindRSAPrimes((short)(bit_number / 2));		// prime * prime => bit_number bits
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
