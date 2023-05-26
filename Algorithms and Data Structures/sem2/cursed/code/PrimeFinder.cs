namespace cursed;

public class PrimeFinder
{
	public ulong[] FindRSAPrimes(ulong limit, ulong lower_limit = 1000)
	{
		// sieve of Atkin
		if (limit < lower_limit) throw new InvalidDataException("limit cannot be smaller than lower_limit");

		ulong[] s = { 1, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 49, 53, 59 };
		bool[] is_prime = new bool[limit + 1];

		var sqrt = Math.Sqrt(limit);
		for (ulong x = 1; x <= sqrt; x++)
			for (ulong y = 1; y <= sqrt; y++)
			{
				var n = 4 * x * x + y * y;
				if (n <= limit && (n % 12 == 1 || n % 12 == 5))
					is_prime[n] ^= true;

				n = 3 * x * x + y * y;
				if (n <= limit && n % 12 == 7)
					is_prime[n] ^= true;

				n = 3 * x * x - y * y;
				if (x > y && n <= limit && n % 12 == 11)
					is_prime[n] ^= true;
			}

		for (ulong n = 5; n <= sqrt; n++)
			if (is_prime[n])
			{
				var j = n * n;
				for (ulong k = j; k <= limit; k += j)
					is_prime[k] = false;
			}

		List<ulong> primes = new List<ulong>();
		primes.Add(2);
		primes.Add(3);
		for (ulong n = lower_limit + lower_limit % 2 + 1; n <= limit; n += 2)
			if (is_prime[n])
				primes.Add(n);

		ulong[] result_primes = new ulong[2];
		Random rand = new Random((int)primes[0]);

		result_primes[0] = primes[rand.Next(primes.Count)];
		result_primes[1] = primes[rand.Next(primes.Count)];

		return result_primes;
	}
}
