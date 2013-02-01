using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using CtcApi;

namespace DataExport
{
	public abstract class DeliveryStrategy
	{
		/// <summary></summary>
		public abstract ApplicationContext Context { get; set; }

		/// <summary></summary>
		public abstract byte[] Source { get; set; }

        /// <summary></summary>
        [XmlAttribute("destination")]
        public abstract string Destination { get; set; }

        /// <summary></summary><param name="writeMode"></param><returns></returns>
		public abstract bool Put(DeliveryWriteMode writeMode = DeliveryWriteMode.Exception);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
		public static byte[] ConvertToSource(string data)
		{
			return Encoding.UTF8.GetBytes(data);
		}
	}

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

}