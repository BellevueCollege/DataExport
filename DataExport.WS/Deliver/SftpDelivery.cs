using System;
using System.IO;
using Common.Logging;
using CtcApi;
using Renci.SshNet;
using Renci.SshNet.Common;
using CtcApi.Extensions;
using Tamir.SharpSsh;
using Tamir.SharpSsh.java.io;
using Tamir.SharpSsh.jsch;
using Tamir.Streams;
using Session = Renci.SshNet.Session;
using String = Tamir.SharpSsh.java.String;

namespace DataExport
{
	public class SftpDelivery : DeliveryBase, IDeliveryStrategy
	{
		private ILog _log = LogManager.GetCurrentClassLogger();
		private ISftpClient _sftp;

		/// <summary>
		/// 
		/// </summary>
		public ApplicationContext Context{get;set;}

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
		public string KeyFile{get;set;}

		public SftpDelivery()
		{
		}

		public SftpDelivery(ISftpClient client)
		{
			_log.Trace(m => m("Initializing SftpDelivery with an ISftpClient object."));
			_sftp = client;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writeMode"></param>
		/// <returns></returns>
		public bool Put(DeliveryWriteMode writeMode = DeliveryWriteMode.Exception)
		{
			if (SaveFileCopy)
			{
				string filename = Path.Combine(Context.BaseDirectory, string.Format("SftpDeliveryFile-{0:yyyyMMdd-HHmmss}.csv", DateTime.Now));
				_log.Info(m => m("Saving copy of SFTP upload file to '{0}'", filename));
				Source.ToFile(filename, Source.Length);
			}

			_log.Trace(m => m("Beginning SFTP communication."));
			using (Stream dataStream = new MemoryStream(Source))
			{
				_log.Trace(m => m("Getting SFTP client object..."));
				_sftp = GetClient();

				using (_sftp)
				{
					try
					{
						_log.Debug(m => m("Connecting to SFTP server ({0}@{1})...", Username, Hostname));
						_sftp.Connect();

						_log.Trace(m => m("Does file exist at destination (and Overwrite is disabled)?"));
						if (DeliveryWriteMode.Overwrite != writeMode && _sftp.Exists(Destination))
						{
							if (DeliveryWriteMode.Exception == writeMode)
							{
								_log.Info(m => m("Destination file exists and Overwrite flag has not been specified: '{0}'", Destination));
								throw new SftpPermissionDeniedException("File already exists and Overwrite is not enabled.");
							}
							// DeliverWriteMode.Ignore
							_log.Info(m => m("Destination file exists and flag is set to Ignore. Skipping.\n{0}", Destination));
							return false;
						}

						_log.Info(m => m("Uploading file via SFTP ({0}@{1}:{2}) WriteMode: '{3}'", Username, Hostname, Destination, writeMode.ToString("F")));
						_sftp.UploadFile(dataStream, Destination, (DeliveryWriteMode.Overwrite == writeMode));
						// if nothing blew up we succeeded (?)
						return true;
					}
					catch (SshConnectionException ex)
					{
						_log.Warn(m => m("Unable to establish an SFTP connection to '{0}@{1}'\n{2}", Username, Hostname, ex));
						throw;
					}
					catch(SftpPermissionDeniedException ex)
					{
						_log.Warn(m => m("Failed to upload file to '{0}@{1}:{2}'\n{3}", Username, Hostname, Destination, ex));
						throw;
					}
					catch(SshException ex)
					{
						_log.Warn(m => m("SSH server returned the following error '{0}'\n{1}", ex.Message, ex));
						throw;
					}
					finally
					{
						_log.Debug(m => m("Disconnecting from SFTP server..."));
						if (_sftp != null && _sftp.IsConnected)
						{
							_sftp.Disconnect();
						}
					}
				}
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		private ISftpClient GetClient()
		{
			if (_sftp != null)
			{
				_log.Trace(m => m("Using existing SFTP client object."));
				return _sftp;
			}

			if (string.IsNullOrWhiteSpace(KeyFile))
			{
				if (string.IsNullOrWhiteSpace(Password))
				{
					throw new SshAuthenticationException("Password cannot be blank if no KeyFile is provided.");
				}
				return new SftpClientProxy(Hostname, Username, Password);
			}

			// if we haven't already returned...
			// Include Password if it was provided
			PrivateKeyFile[] keys = new[]
			                        	{
			                        			string.IsNullOrWhiteSpace(Password)
			                        					? new PrivateKeyFile(KeyFile)
			                        					: new PrivateKeyFile(KeyFile, Password)
			                        	};

			return new SftpClientProxy(Hostname, Username, keys);
		}
	}
}