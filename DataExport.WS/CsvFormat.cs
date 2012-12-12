using System;
using System.Data;
using System.Text;
using CtcApi;
using DataExport.WS.Config;

namespace DataExport.WS
{
	public class CsvFormat : CsvFormatConfig, IExportFormat
	{
		public ApplicationContext Context {get;set;}

		public string Serialize(DataSet ds)
		{
			StringBuilder csv = new StringBuilder();

			// TODO: are column headings required?

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
					// TODO: should only TitleCase fields that are so specified in the config settings
					csv.Append(fieldData);
				}
				csv.Append(Environment.NewLine);
			}

			return csv.ToString();
		}
	}
}