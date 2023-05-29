namespace cursed;

public class PrimeFinder
{
	private int Seed 
	{
		get { return DateTime.Now.Microsecond; }
	}

	public long[] FindRSAPrimes(long limit, long lower_limit)
	{
		// sieve of Atkin
		if (limit < lower_limit) throw new InvalidDataException("limit cannot be smaller than lower_limit");

		long[] s = { 1, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 49, 53, 59 };
		bool[] is_prime = new bool[limit + 1];

		var sqrt = Math.Sqrt(limit);
		for (long x = 1; x <= sqrt; x++)
			for (long y = 1; y <= sqrt; y++)
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

		for (long n = 5; n <= sqrt; n++)
			if (is_prime[n])
			{
				var j = n * n;
				for (long k = j; k <= limit; k += j)
					is_prime[k] = false;
			}

		List<long> primes = new List<long>();
		primes.Add(2);
		primes.Add(3);
		for (long n = lower_limit + lower_limit % 2 + 1; n <= limit; n += 2)
			if (is_prime[n])
				primes.Add(n);

		long[] result_primes = new long[2];
		Random rand = new Random(Seed);

		result_primes[0] = primes[rand.Next(primes.Count)];
		result_primes[1] = primes[rand.Next(primes.Count)];

		return result_primes;
	}

	public long FindCoprime(long num, int limit, int lower_limit)
	{
		Random rand = new Random(Seed);
		long candidate = rand.Next(lower_limit, limit);
		while (FindGCD(num, (long)candidate) != 1)
		{
			candidate++;
			if (candidate > limit) candidate = lower_limit;
		}
		return candidate;
	}

	public long FindPrivateKey(long e, long fi)
	{
		long x_dio = SolveDiophantineEquation(e, -fi, 1)[0];
		return x_dio + fi;
	}

	public long[] SolveDiophantineEquation(long a, long b, long c)
	{
		// ax + by = c
		// finds particular solution

		long[] euclidus = Euclidus((long)Math.Abs((decimal)a), (long)Math.Abs((decimal)b));
		long d = euclidus[0];
		long[] result = new long[2];
		if (c % d == 0)
		{
			result[0] = (c / d) * euclidus[1] * Math.Sign(a);
			result[1] = (c / d) * euclidus[2] * Math.Sign(b);
		}
		else throw new InvalidOperationException("Equation cannot be solved");

		return result;
	}

	private long FindGCD(long a, long b)
	{
		return Euclidus(a, b)[0];
	}

	private long[] Euclidus(long a, long b)
	{
		// returns 3 values: D(a, b) and Bezu coefficients
		if (b == 0) return new long[] { a, 1, 0 };
		long[] euclidus = Euclidus(b, a % b);
		return new long[] { euclidus[0], euclidus[2], euclidus[1] - (a / b) * euclidus[2] };
	}
}
