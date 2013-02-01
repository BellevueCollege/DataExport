using System;
using System.Data;
using System.Text;
using System.Xml.Serialization;
using CtcApi;
using DataExport.WS.Config;

namespace DataExport.WS.Config
{
	[XmlType("csv")]
	public class CsvFormat : ExportFormatStrategy
	{
		public override ApplicationContext Context {get;set;}

		#region .config properties
		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute("fieldSeparator")]
		public string FieldSeparator { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute("trimFromFieldStart")]
		public string TrimFromFieldStart
		{
			get { return new string(FieldTrimLeadingChars); }
			set { FieldTrimLeadingChars = value.ToCharArray(); }
		}

		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute("trimFromFieldEnd")]
		public string TrimFromFieldEnd
		{
			get { return new string(FieldTrimEndChars); }
			set { FieldTrimEndChars = value.ToCharArray(); }
		}

		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute("includeHeader")]
		public bool IncludeHeader { get; set; }

		#endregion

		public CsvFormat()
		{
			// establish default values
			FieldSeparator = ",";
			IncludeHeader = false;
			TrimFromFieldEnd = string.Empty;
			TrimFromFieldStart = string.Empty;
		}

		/// <summary>
		/// Array of char values to trim from the end of each field
		/// </summary>
		public char[] FieldTrimEndChars{get;set;}

		/// <summary>
		/// Array of char values to trim from the beginning of each field
		/// </summary>
		public char[] FieldTrimLeadingChars{get;set;}

		public override string Serialize(DataSet ds)
		{
			StringBuilder csv = new StringBuilder();

			if (IncludeHeader)
			{
				StringBuilder header = new StringBuilder();
				foreach (DataColumn column in ds.Tables[0].Columns)
				{
					header.AppendFormat("{0}{1}", header.Length > 0 ? FieldSeparator : string.Empty, column.ColumnName);
				}
				// start with the column headers
				csv.Append(header.ToString()).Append(Environment.NewLine);
			}

			// Add rows
			foreach (DataRow row in ds.Tables[0].Rows)
			{
				for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
				{
					if (i != 0)	// don't leave a leading or trailing separator
					{
						csv.Append(FieldSeparator);
					}
					
					string fieldData = row[i].ToString();

					if (FieldTrimLeadingChars != null && FieldTrimLeadingChars.Length > 0)
					{
						fieldData = fieldData.TrimStart(FieldTrimLeadingChars);
					}
					if (FieldTrimEndChars != null && FieldTrimEndChars.Length > 0)
					{
						fieldData = fieldData.TrimEnd(FieldTrimEndChars);
					}
					csv.Append(fieldData);
				}
				csv.Append(Environment.NewLine);
			}

			return csv.ToString();
		}
	}
}