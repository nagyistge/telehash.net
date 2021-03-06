﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Crypto.Digests;

namespace Telehash
{
	public class Helpers
	{
		public static byte[] Fold(byte[] foldingBytes, int folds = 1)
		{
			// It must be an even length
			if (foldingBytes.Length % 2 != 0) {
				return null;
			}

			byte[] foldedBuffer = null;
			for (int i = 0; i < folds; ++i) {
				foldedBuffer = FoldOnce (foldedBuffer == null ? foldingBytes : foldedBuffer);
			}
			return foldedBuffer;
		}

		public static byte[] FoldOnce(byte[] foldingBytes)
		{
			// It must be an even length
			if (foldingBytes.Length % 2 != 0) {
				return null;
			}
			
			int halfLength = foldingBytes.Length / 2;
			byte[] outBuffer = new byte[halfLength];
			for (int i = 0; i < halfLength; ++i) {
				outBuffer [i] = (byte)(foldingBytes [i] ^ foldingBytes [i + halfLength]);
			}
			
			return outBuffer;
		}


		// From http://stackoverflow.com/questions/623104/byte-to-hex-string/18574846#18574846
		public static string[] HexTbl = Enumerable.Range(0, 256).Select(v => v.ToString("x2")).ToArray();
		public static string ToHexSring(IEnumerable<byte> array)
		{
			StringBuilder s = new StringBuilder();
			foreach (var v in array)
				s.Append(HexTbl[v]);
			return s.ToString();
		}
		public static string ToHexSring(byte[] array)
		{
			StringBuilder s = new StringBuilder(array.Length*2);
			foreach (var v in array)
				s.Append(HexTbl[v]);
			return s.ToString();
		}

		public static byte[] ToByteArray(Org.BouncyCastle.Math.BigInteger bigint, int expectedLength)
		{
			byte[] data = bigint.ToByteArray ();
			if (data.Length == expectedLength + 1) {
				return data.Skip (1).Take (expectedLength).ToArray ();
			}
			return data;
		}

		public static byte[] SHA256Hash(byte[] data)
		{
			var shaHash = new Sha256Digest ();
			shaHash.BlockUpdate (data, 0, data.Length);
			byte[] hashedValue = new byte[shaHash.GetDigestSize ()];
			shaHash.DoFinal(hashedValue, 0);
			return hashedValue;
		}
	}
}

