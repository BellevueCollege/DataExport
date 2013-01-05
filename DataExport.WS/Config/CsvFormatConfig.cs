namespace DataExport.WS.Config
{
	public class CsvFormatConfig
	{
		/// <summary>
		/// 
		/// </summary>
		public string FieldSeparator {get;set;}

		/// <summary>
		/// Array of char values to trim from the end of each field
		/// </summary>
		public char[] FieldTrimEndChars{get;set;}

		/// <summary>
		/// Array of char values to trim from the beginning of each field
		/// </summary>
		public char[] FieldTrimLeadingChars{get;set;}

		/// <summary>
		/// 
		/// </summary>
		public bool IncludeHeader{get;set;}
	}
}