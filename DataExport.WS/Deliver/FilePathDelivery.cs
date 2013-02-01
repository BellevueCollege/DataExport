using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Common.Logging;
using CtcApi;

namespace DataExport
{
    [XmlType("file")]
	public class FilePathDelivery : DeliveryStrategy
	{
		private ILog _log = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// 
		/// </summary>
		public override ApplicationContext Context{get;set;}

		/// <summary>
		/// 
		/// </summary>
		public override byte[] Source {get;set;}

		/// <summary>
		/// 
		/// </summary>
        [XmlAttribute("destination")]
        public override string Destination { get; set; }

        /// <summary>
		/// 
		/// </summary>
		/// <param name="writeMode"></param>
		/// <returns></returns>
		public override bool Put(DeliveryWriteMode writeMode = DeliveryWriteMode.Overwrite)
		{
			if (string.IsNullOrWhiteSpace(Destination))
			{
				throw new NullReferenceException("Delivery Destination cannot be NULL or empty.");
			}

			if (!Directory.Exists(Path.GetDirectoryName(Destination)))
			{
				throw new DirectoryNotFoundException(string.Format("Specified directory does not exist: '{0}'", Destination));
			}
			
			if (File.Exists(Destination))
			{
				if (DeliveryWriteMode.Overwrite == writeMode)
				{
					File.Delete(Destination);
				}
				else if (DeliveryWriteMode.Exception == writeMode)
				{
					throw new NotSupportedException(string.Format("File already exists, and Overwrite flag has not been specified: '{0}'", Destination));
				}
				else if (DeliveryWriteMode.Ignore == writeMode)
				{
					_log.Warn(m => m("File already exists and flag is set to Ignore. Skipping.\n{0}", Destination));
					return false;
				}
			}

			try
			{
				int length = Source.Length;
				using (FileStream fs = File.Create(Destination, length, FileOptions.None))
				{
					fs.Write(Source, 0, length);
					fs.Flush();
					fs.Close();

					return true;
				}
			}
			catch (Exception ex)
			{
				_log.Error(m => m("Unable to write file '{0}'...\n{1}", Destination, ex));
			}
			return false;
		}
	}
}