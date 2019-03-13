using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace emotitron.Compression
{
	public static class BitstreamExtensions
	{
		#region Bitstream Reader/Writers

		/// <summary>
		/// Compress a value with this crusher, and Write to the bitstream.
		/// </summary>
		/// <param name="f">Uncompressed value.</param>
		/// <param name="bitstream"></param>
		/// <param name="bcl"></param>
		/// <returns></returns>
		[System.Obsolete("Use Bitstream<T> instead.")]
		public static CompressedFloat Write(this FloatCrusher fc, float f, ref Bitstream bitstream, BitCullingLevel bcl = BitCullingLevel.NoCulling)
		{
			int bits = fc._bits[(int)bcl];
			uint c = fc.Compress(f);
			bitstream.Write(c, bits);
			return new CompressedFloat(fc, c);
		}
		/// <summary>
		/// Write a previously compressed value to the bitstream.
		/// </summary>
		/// <param name="c">Compressed value.</param>
		/// <param name="bitstream"></param>
		/// <param name="bcl"></param>
		/// <returns></returns>
		[System.Obsolete("Use Bitstream<T> instead.")]
		public static CompressedFloat Write(this FloatCrusher fc, CompressedFloat c, ref Bitstream bitstream, BitCullingLevel bcl = BitCullingLevel.NoCulling)
		{
			int bits = fc._bits[(int)bcl];
			bitstream.Write(c, bits);
			return c;
		}
		/// <summary>
		/// Write a previously compressed value to the bitstream.
		/// </summary>
		/// <param name="c">Compressed value.</param>
		/// <param name="bitstream"></param>
		/// <param name="bcl"></param>
		/// <returns></returns>
		[System.Obsolete("Use Bitstream<T> instead.")]
		public static CompressedFloat Write(this FloatCrusher fc, uint c, ref Bitstream bitstream, BitCullingLevel bcl = BitCullingLevel.NoCulling)
		{
			int bits = fc._bits[(int)bcl];
			bitstream.Write(c, bits);
			return new CompressedFloat(fc, c);
		}

		/// <summary>
		/// Read a CompressedValue from the bitstream.
		/// </summary>
		/// <param name="bitstream">Serialized source for read.</param>
		/// <param name="bcl"></param>
		[System.Obsolete("Use Bitstream<T> instead.")]
		public static CompressedFloat Read(this FloatCrusher fc, ref Bitstream bitstream, BitCullingLevel bcl = BitCullingLevel.NoCulling)
		{
			int bits = fc._bits[(int)bcl];
			return new CompressedFloat(fc, (uint)bitstream.Read(bits));
		}

		/// <summary>
		/// Read a CompressedValue from the bitstream, decompress it, and return the uncompressed float.
		/// </summary>
		/// <param name="bitstream">erialized source for read.</param>
		/// <param name="bcl"></param>
		/// <returns></returns>
		[System.Obsolete("Use Bitstream<T> instead.")]
		public static float ReadAndDecompress(this FloatCrusher fc, ref Bitstream bitstream, BitCullingLevel bcl = BitCullingLevel.NoCulling)
		{
			CompressedFloat c = fc.Read(ref bitstream, bcl);
			return fc.Decompress(c);
		}

		#endregion


		/// <summary>
		/// Read this entire bitstream (from bit 0 to the current writePtr position) to the supplied byte[]. Bitposition will be incremented accordingly.
		/// </summary>
		/// <param name="target">Target array buffer.</param>
		/// <param name="bitposition">Reference to the bitpointer of the target array buffer.</param>
		[System.Obsolete()]
		public static void ReadOut(this Bitstream bs, byte[] target, ref int bitposition)
		{
			int remainingbits = bs.WritePtr;
			int index = 0;
			while (remainingbits > 0)
			{
				ulong frag = bs[index];
				for (int i = 0; i < 64; i += 8)
				{
					int bits = remainingbits > 8 ? 8 : remainingbits;
					target.Write(frag >> i, ref bitposition, bits);
					remainingbits -= bits;

					if (remainingbits == 0)
						return;
				}
				index++;
			}
		}

		/// <summary>
		/// Read this entire bitstream (from bit 0 to the current writePtr position) to the supplied byte[].
		/// </summary>
		/// <param name="target">Target array buffer.</param>
		/// <returns>Returns the number of bits that were written.</returns>
		[System.Obsolete()]
		public static int ReadOut(this Bitstream bs, byte[] target)
		{
			int bitsused = 0;
			bs.ReadOut(target, ref bitsused);
			return bitsused;
		}
	}
}
