using System;
using System.Diagnostics;
using Renci.SshNet;

namespace DataExport
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// This class is a wrapper around the actual <see cref="SftpClient"/> object so that 
	/// we can create a mock object to simulate SFTP uploads for unit testing.
	/// </remarks>
	internal class SftpClientProxy : SftpClient, ISftpClient
	{
		#region Properties
		public new bool IsConnected
		{
			get {return base.IsConnected;}
			set {Debug.Print("Pretending to set IsConnected = '{0}' (Property is read-only in underlying class.)", value);}
		}
		public string Hostname{get;set;}
		public string Username{get;set;}
		public PrivateKeyFile[] KeyFiles{get;set;}
		#endregion

		#region Constructors
		public SftpClientProxy(ConnectionInfo connectionInfo) : base(connectionInfo)
		{
		}

		public SftpClientProxy(string host, int port, string username, string password) : base(host, port, username, password)
		{
			Hostname = host;
			Username = username;
		}

		public SftpClientProxy(string host, string username, string password) : base(host, username, password)
		{
			Hostname = host;
			Username = username;
		}

		public SftpClientProxy(string host, int port, string username, params PrivateKeyFile[] keyFiles) : base(host, port, username, keyFiles)
		{
			Hostname = host;
			Username = username;
			KeyFiles = keyFiles;
		}

		public SftpClientProxy(string host, string username, params PrivateKeyFile[] keyFiles) : base(host, username, keyFiles)
		{
			Hostname = host;
			Username = username;
			KeyFiles = keyFiles;
		}
		#endregion
	}
}