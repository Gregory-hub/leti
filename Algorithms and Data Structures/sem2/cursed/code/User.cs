using System.Numerics;


namespace cursed;


class User
{
	public struct PublicKey
	{
		public long E;
		public long M;
	}

	public struct PrivateKey
	{
		public long D;
		public long M;
	}

	public string Name;
	private PrivateKey privateKey;
	public PublicKey publicKey;
	int bit_number = 12;

	public User(string name)
	{
		Name = name;
		InitRSA();
	}

	public void InitRSA()
	{
		int lower_limit = (int)Math.Pow(2, bit_number);
		int limit = (int)Math.Pow(2, bit_number + 1) - 1;

		PrimeFinder prime_finder = new PrimeFinder();
        long[] primes = prime_finder.FindRSAPrimes(limit, lower_limit);

		long m = (long)primes[0] * (long)primes[1];
		long fi = ((long)primes[0] - 1) * ((long)primes[1] - 1);		// Fi(m) - Euler function
		long e = prime_finder.FindCoprime(fi, limit, lower_limit);

		publicKey.E = e;
		publicKey.M = m;

		long d = prime_finder.FindPrivateKey(e, fi);
		privateKey.M = m;
		privateKey.D = d;
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
