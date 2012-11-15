using System;

namespace DataExport.WS.Controllers
{
	public class Exporter : IExporter
	{
		public IExportFormat Format
		{
			get {throw new NotImplementedException();}
			set {throw new NotImplementedException();}
		}

		public string Name
		{
			get {throw new NotImplementedException();}
			set {throw new NotImplementedException();}
		}

		public IDataInput Data
		{
			get {throw new NotImplementedException();}
			set {throw new NotImplementedException();}
		}

		public string Export()
		{
			throw new NotImplementedException();
		}
	}
}