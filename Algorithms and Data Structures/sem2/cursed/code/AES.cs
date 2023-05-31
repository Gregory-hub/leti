using System.Numerics;


namespace cursed;
public static class AES
{
	public static byte[,] Sbox = new byte[16, 16]
	{
		{ 0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76 },
		{ 0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0 },
		{ 0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15 },
		{ 0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75 },
		{ 0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84 },
		{ 0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf },
		{ 0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8 },
		{ 0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2 },
		{ 0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73 },
		{ 0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb },
		{ 0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79 },
		{ 0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08 },
		{ 0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a },
		{ 0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e },
		{ 0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf },
		{ 0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16 }
	};
	public static byte[,] InverseSbox = new byte[16, 16]
	{
		{ 0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb },
		{ 0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb },
		{ 0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e },
		{ 0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25 },
		{ 0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92 },
		{ 0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84 },
		{ 0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06 },
		{ 0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b },
		{ 0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73 },
		{ 0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e },
		{ 0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b },
		{ 0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4 },
		{ 0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f },
		{ 0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef },
		{ 0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61 },
		{ 0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d }
	};
	public static byte[,] HillCipher = new byte[4, 4]
	{
		{ 2, 3, 1, 1 },
		{ 1, 2, 3, 1 },
		{ 1, 1, 2, 3 },
		{ 3, 1, 1, 2 }
	};
	public static byte[,] InverseHillCipher = new byte[4, 4]
	{
		{ 14, 11, 13, 9 },
		{ 9, 14, 11, 13 },
		{ 13, 9, 14, 11 },
		{ 11, 13, 9, 14 }
	};
	public static byte[] RCon = { 0x8d, 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36 };
	public const short KeyBitNumber = 128;		// available: 128
	public const short Rounds = 10;

	public static BigInteger GenerateKey()
	{
		PrimeFinder prime_finder = new PrimeFinder();
		return prime_finder.GetRandomBigInt(KeyBitNumber);
	}

	public static byte[,] Encode(BigInteger[] data, short bit_number, BigInteger key)
	{
		BigInteger[] keys = GenerateKeys(key);

		byte[] block = new byte[16];
		byte[] number_in_bytes;

		byte[,] encoded = new byte[data.Count() * bit_number / 128, 16];

		int index = 0;
		for (int i = 0; i < data.Count(); i++)
		{
			number_in_bytes = data[i].ToByteArray();
			for (int k = 0; k < bit_number / 8; k += 16)
			{
				for (int j = 0; j < 16; j++)
					block[j] = (k + j < number_in_bytes.Count()) ? number_in_bytes[k + j] : (byte)0;

				EncodeBlock(ref block, keys);

				for (int j = 0; j < 16; j++) encoded[index, j] = block[j];
				index++;
			}
		}

		return encoded;
	}

	private static void EncodeBlock(ref byte[] block_data, BigInteger[] keys)
	{
		if (KeyBitNumber != 128) throw new InvalidDataException("Invalid BitNumber");

		byte[,] block = new byte[4, 4];
		for (int i = 0; i < 4; i++)
			for (int j = 0; j < 4; j++) block[i, j] = block_data[i * 4 + j];

		AddRoundKey(ref block, keys[0]);
		for (int i = 1; i < Rounds; i++)
		{
			SubBytes(ref block);
			ShiftRows(ref block);
			MixColumns(ref block);
			AddRoundKey(ref block, keys[i]);
		}
		SubBytes(ref block);
		ShiftRows(ref block);
		AddRoundKey(ref block, keys[10]);

		for (int i = 0; i < 4; i++)
			for (int j = 0; j < 4; j++) block_data[i * 4 + j] = block[i, j];
	}

	private static BigInteger[] GenerateKeys(BigInteger original_key)
	{
		uint[] words = new uint[(Rounds + 1) * 4];

		for (int i = 0; i < 4; i++)
		{
			words[i] = BitConverter.ToUInt32(original_key.ToByteArray(), i * 4);
		}

		for (int i = 4; i < (Rounds + 1) * 4; i++)
		{
			if (i % 4 == 0) words[i] = words[i - 4] ^ Transform(words[i - 1], i);
			else words[i] = words[i - 4] ^ words[i - 1];
		}

		BigInteger[] keys = new BigInteger[Rounds + 1];
		for (int i = 0; i < words.Count(); i += 4)
		{
			keys[i / 4]  = (BigInteger)words[i + 0] << 96;
			keys[i / 4] += (BigInteger)words[i + 1] << 64;
			keys[i / 4] += (BigInteger)words[i + 2] << 32;
			keys[i / 4] += (BigInteger)words[i + 3];
		}

		return keys;
	}

	private static uint Transform(uint word, int index)
	{
		byte[] byte_word = BitConverter.GetBytes(word);
		RotateWord(ref byte_word);
		SubWord(ref byte_word);

		word = BitConverter.ToUInt32(byte_word);
		return word ^ ((uint)RCon[index / 4] << 24);
	}

	private static void RotateWord(ref byte[] byte_word)
	{
		byte_word = new byte[] { byte_word[1], byte_word[2], byte_word[3], byte_word[0] };
	}

	private static void SubWord(ref byte[] byte_word)
	{
		for (int i = 0; i < 4; i++) byte_word[i] = Sbox[((byte_word[i] & 0xf0) >> 4), (byte_word[i] & 0x0f)];
	}

	private static void AddRoundKey(ref byte[,] block, BigInteger key)
	{
		byte[] key_bytes = key.ToByteArray();	// 128 bit => 16 bytes
		for (int i = 0; i < 4; i++)
			for (int j = 0; j < 4; j++) block[i, j] ^= key_bytes[i + j * 4];
	}

	private static void SubBytes(ref byte[,] block)
	{
		for (int i = 0; i < 4; i++)
			for (int j = 0; j < 4; j++)
				block[i, j] = Sbox[((block[i, j] & (15 << 4)) >> 4), (block[i, j] & 15)];
	}

	private static void ShiftRows(ref byte[,] block)
	{
		byte[,] block_tmp = new byte[4, 4];
		for (int i = 0; i < 4; i++)
			for (int j = 0; j < 4; j++)
				block_tmp[i, j] = block[i, j];

		for (int i = 1; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				block[i, j] = block_tmp[i, (j + i) % 4];
			}
		}
	}

	private static void MixColumns(ref byte[,] block)
	{
		for (int col_i = 0; col_i < 4; col_i++)
		{
			byte[,] col = new byte[4, 1];
			for (int i = 0; i < 4; i++) col[i, 0] = block[i, col_i];

			byte[,] res = new byte[4, 1];
			for (int i = 0; i < 4; i++) {
				res[i, 0] = 0;
				for (int j = 0; j < 4; j++) {
					res[i, 0] ^= MultiplyGF(HillCipher[i, j], col[j, 0]);
				}
			}
			for (int i = 0; i < 4; i++) block[i, col_i] = res[i, 0];
		}
	}

	private static byte MultiplyGF(byte a, byte b)
	{
		// Galois Field (2^8) Multiplication of two Bytes
		byte p = 0;

		for (int i = 0; i < 8; i++) {
			if ((b & 1) != 0) {
				p ^= a;
			}

			bool hi_bit_set = (a & 0x80) != 0;
			a <<= 1;
			if (hi_bit_set) {
				a ^= 0x1B;		// x^8 + x^4 + x^3 + x + 1
			}
			b >>= 1;
		}

		return p;
	}

	public static BigInteger[] Decode(byte[,] encoded, short bit_number, BigInteger key)
	{
		BigInteger[] keys = GenerateKeys(key);

		byte[] block = new byte[16];
		byte[] decoded_bytes = new byte[encoded.Length];

		for (int i = 0; i < encoded.GetLength(0); i++)
		{
			for (int j = 0; j < 16; j++) block[j] = encoded[i, j];

			DecodeBlock(ref block, keys);

			for (int j = 0; j < 16; j++) decoded_bytes[i * 16 + j] = block[j];
		}

		BigInteger[] decoded = new BigInteger[encoded.GetLength(0) * 128 / bit_number];
		byte[] bytes = new byte[bit_number / 8 + 1];
		for (int i = 0; i < decoded.Length; i++)
		{
			for (int j = 0; j < bytes.Length - 1; j++) bytes[j] = decoded_bytes[i * bit_number / 8 + j];
			decoded[i] = new BigInteger(bytes);
		}

		return decoded;
	}

	private static void DecodeBlock(ref byte[] block_data, BigInteger[] keys)
	{
		byte[,] block = new byte[4, 4];
		for (int i = 0; i < 4; i++)
			for (int j = 0; j < 4; j++) block[i, j] = block_data[i * 4 + j];

		AddRoundKey(ref block, keys[10]);
		for (int i = Rounds - 1; i > 0; i--)
		{
			InverseShiftRows(ref block);
			InverseSubBytes(ref block);
			AddRoundKey(ref block, keys[i]);
			InverseMixColumns(ref block);
		}
			InverseShiftRows(ref block);
			InverseSubBytes(ref block);
			AddRoundKey(ref block, keys[0]);

		for (int i = 0; i < 4; i++)
			for (int j = 0; j < 4; j++) block_data[i * 4 + j] = block[i, j];
	}

	private static void InverseShiftRows(ref byte[,] block)
	{
		byte[,] block_tmp = new byte[4, 4];
		for (int i = 0; i < 4; i++)
			for (int j = 0; j < 4; j++)
				block_tmp[i, j] = block[i, j];

		for (int i = 1; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				block[i, j] = block_tmp[i, (j - i + 4) % 4];
			}
		}
	}

	private static void InverseSubBytes(ref byte[,] block)
	{
		for (int i = 0; i < 4; i++)
			for (int j = 0; j < 4; j++)
				block[i, j] = InverseSbox[((block[i, j] & 0xf0) >> 4), (block[i, j] & 0x0f)];
	}

	private static void InverseMixColumns(ref byte[,] block)
	{
		for (int col_i = 0; col_i < 4; col_i++)
		{
			byte[,] col = new byte[4, 1];
			for (int i = 0; i < 4; i++) col[i, 0] = block[i, col_i];

			byte[,] res = new byte[4, 1];
			for (int i = 0; i < 4; i++) {
				res[i, 0] = 0;
				for (int j = 0; j < 4; j++) {
					res[i, 0] ^= MultiplyGF(InverseHillCipher[i, j], col[j, 0]);
				}
			}
			for (int i = 0; i < 4; i++) block[i, col_i] = res[i, 0];
		}
	}
}
