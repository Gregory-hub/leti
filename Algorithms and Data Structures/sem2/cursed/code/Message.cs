namespace cursed;


class Message
{
	public string Text { get; set; }

	public Message(string text)
	{
		Text = text;
	}

	public string Encode()
	{
		string encoded = "";

		PrimeFinder prime_finder = new PrimeFinder();
        ulong[] primes = prime_finder.FindRSAPrimes(100000000, 1000000);

		UInt128 m = primes[0] * primes[1];
		UInt128 fi = (primes[0] - 1) * (primes[1] - 1);		// Fi(m) - Euler function

		return encoded;
	}
	
	public string Decode()
	{
		string decoded = "";

		return decoded;
	}
}
