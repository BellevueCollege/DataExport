using System.Collections.Generic;
using System.Linq;
using System.Web;
using CtcApi;

namespace DataExport
{
	public interface IDeliveryStrategy
	{
		/// <summary>
		/// 
		/// </summary>
		ApplicationContext Context{get;set;}

		/// <summary>
		/// 
		/// </summary>
		byte[] Source{get;set;}

		/// <summary>
		/// 
		/// </summary>
		string Destination{get;set;}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writeMode"></param>
		/// <returns></returns>
		bool Put(DeliveryWriteMode writeMode = DeliveryWriteMode.Overwrite);
	}
}