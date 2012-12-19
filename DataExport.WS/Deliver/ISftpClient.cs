using System;
using System.IO;
using Renci.SshNet;

namespace DataExport
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// This interface primarily exists so that we can create mock objects for unit testing.
	/// </remarks>
	public interface ISftpClient : IDisposable
	{
		void Connect();
		void Disconnect();
		bool Exists(string path);
		void UploadFile(Stream input, string path);
		void UploadFile(Stream input, string path, bool canOverride);
		bool IsConnected {get;set;}
		// these properties are exposed in order for mock frameworks to have access to them.
		string Hostname{get;set;}
		string Username{get;set;}
		PrivateKeyFile[] KeyFiles{get;set;}
	}
}