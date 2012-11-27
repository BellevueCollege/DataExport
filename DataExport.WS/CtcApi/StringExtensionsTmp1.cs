using System.IO;
using System.Text;

namespace CtcApi.Extensions
{
	public static class StringExtensionsTmp1
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="str"></param>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static bool ToFile(this string str, string filename)
		{
			byte[] data = Encoding.UTF8.GetBytes(str);
			return data.ToFile(filename, str.Length);
		}
	}
}