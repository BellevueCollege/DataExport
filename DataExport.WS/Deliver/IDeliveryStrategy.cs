using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataExport
{
	public interface IDeliveryStrategy
	{
		/// <summary>
		/// 
		/// </summary>
		byte[] Source{get;set;}

		/// <summary>
		/// 
		/// </summary>
		string Destination{get;set;}

		bool Put(DeliveryWriteMode writeMode = DeliveryWriteMode.Overwrite);
	}
}