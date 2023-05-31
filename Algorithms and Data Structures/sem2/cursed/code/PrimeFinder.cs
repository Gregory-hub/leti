using System.Numerics;


namespace cursed;
public class PrimeFinder
{
	private int Seed 
	{
		get { return DateTime.Now.Microsecond; }
	}

	public BigInteger GetRandomBigInt(short bit_number, bool odd = false)
	{
		if (bit_number < 2) throw new InvalidDataException("Bit number cannot be less than 2");

		Random rand = new Random(Seed);
		BigInteger big_int = 1;
		short len = (short)(bit_number - 1);
		while (len > 30)
		{
			big_int = (big_int << 30) + rand.Next((int)Math.Pow(2, 30));
			len -= 30;
		}
		big_int = (big_int << len) + rand.Next((int)Math.Pow(2, len));
		return big_int | ((odd) ? 1 : 0);
	}

	public BigInteger[] FindRSAPrimes(short bit_number)
	{
		BigInteger[] result_primes = new BigInteger[2] { 0, 0 };
		Random rand = new Random(Seed);

		int number_of_checks = 1;

		do
		{
			result_primes[0] = GetRandomBigInt(bit_number, odd: true);
		}
		while(!IsPrime(result_primes[0], number_of_checks));

		do
		{
			result_primes[1] = GetRandomBigInt(bit_number, odd: true);
		}
		while(!IsPrime(result_primes[1], number_of_checks));

		return result_primes;
	}

	public bool IsPrime(BigInteger num, int number_of_checks)
	{
		// Miller-Rabin

		if (num == 2 || num == 3) return true;
		if (num % 2 == 0 || num < 2) return false;

		BigInteger s = 0;
		BigInteger d = num - 1;
		while ((d & 1) == 0)
		{
			s += 1;
			d /= 2;
		}

		Random rand = new Random(Seed);

		Byte[] bytes = new Byte[num.ToByteArray().LongLength];
		BigInteger a;

		for (int i = 0; i < number_of_checks; i++) {
			do {
				rand.NextBytes(bytes);
				a = new BigInteger(bytes);
			}
			while (a < 2 || a >= num - 2);

			BigInteger x = ModulusPow(a, d, num);
			if (x == 1 || x == num - 1) continue;

			for (int j = 1; j < s; j++) {
				x = ModulusPow(x, 2, num);

				if (x == 1)
					return false;
				if (x == num - 1)
					break;
			}

			if (x != num - 1) return false;
		}

		return true;
	}

	public static BigInteger ModulusPow(BigInteger num, BigInteger pow, BigInteger modulus)
	{
		BigInteger result = 1;
		while (pow > 0)
		{
			if (pow % 2 == 1) result = (result * num) % modulus;
			num = num * num % modulus;
			pow /= 2;
		}

		return result;
	}

	public BigInteger FindCoprime(BigInteger num, short bit_number)
	{
		Random rand = new Random(Seed);
		BigInteger candidate = GetRandomBigInt(bit_number);
		while (FindGCD(num, (BigInteger)candidate) != 1) candidate++;
		return candidate;
	}

	public BigInteger FindPrivateKey(BigInteger e, BigInteger fi)
	{
		BigInteger x_dio = SolveDiophantineEquation(e, -fi, 1)[0];
		return x_dio + fi;
	}

	public BigInteger[] SolveDiophantineEquation(BigInteger a, BigInteger b, BigInteger c)
	{
		// ax + by = c
		// finds particular solution

		BigInteger[] euclidus = Euclidus(BigInteger.Abs(a), BigInteger.Abs(b));
		BigInteger d = euclidus[0];
		BigInteger[] result = new BigInteger[2];
		if (c % d == 0)
		{
			result[0] = (c / d) * euclidus[1] * a.Sign;
			result[1] = (c / d) * euclidus[2] * b.Sign;
		}
		else throw new InvalidOperationException("Equation cannot be solved");

		return result;
	}

	private BigInteger FindGCD(BigInteger a, BigInteger b)
	{
		return Euclidus(a, b)[0];
	}

	private BigInteger[] Euclidus(BigInteger a, BigInteger b)
	{
		// returns 3 nums: D(a, b) and Bezu coefficients
		if (b == 0) return new BigInteger[] { a, 1, 0 };
		BigInteger[] euclidus = Euclidus(b, a % b);
		return new BigInteger[] { euclidus[0], euclidus[2], euclidus[1] - (a / b) * euclidus[2] };
	}
}
