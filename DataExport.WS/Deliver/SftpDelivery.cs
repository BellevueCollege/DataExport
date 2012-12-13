using System;

namespace DataExport
{
	public class SftpDelivery : DeliveryBase, IDeliveryStrategy
	{
		/// <summary>
		/// 
		/// </summary>
		public byte[] Source {get;set;}

		/// <summary>
		/// 
		/// </summary>
		public string Destination{get;set;}

		/// <summary>
		/// 
		/// </summary>
		public string Hostname{get;set;}

		/// <summary>
		/// 
		/// </summary>
		public string Username{get;set;}

		/// <summary>
		/// 
		/// </summary>
		public string Password{get;set;}

		/// <summary>
		/// 
		/// </summary>
		public string Key{get;set;}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writeMode"></param>
		/// <returns></returns>
		public bool Put(DeliveryWriteMode writeMode = DeliveryWriteMode.Exception)
		{
			throw new NotImplementedException();
		}
	}
}