using System.Text;

namespace DataExport
{
	/// <summary>
	/// Defines what action to take if the target already exists
	/// </summary>
	public enum DeliveryWriteMode
	{
		/// <summary>
		/// Replace the existing target, if it exists
		/// </summary>
		Overwrite,

		/// <summary>
		/// Throw an Exception if the target exists
		/// </summary>
		Exception,

		/// <summary>
		/// Do nothing (do not overwrite the target)
		/// </summary>
		Ignore,
	}

	public class DeliveryBase
	{
		private bool _saveFileCopy;

		/// <summary>
		/// 
		/// </summary>
		public bool SaveFileCopy
		{
			get {
				return _saveFileCopy;
			}
			set {
				_saveFileCopy = value;
			}
		}

		public static byte[] ConvertToSource(string data)
		{
			return Encoding.UTF8.GetBytes(data);
		}
	}
}