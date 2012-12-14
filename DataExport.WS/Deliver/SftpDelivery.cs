using System;
using System.IO;
using Common.Logging;
using Renci.SshNet;
using Renci.SshNet.Common;
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writeMode"></param>
		/// <returns></returns>
		public bool Put(DeliveryWriteMode writeMode = DeliveryWriteMode.Exception)
		{
			Stream dataStream = new MemoryStream(Source);

			using (SftpClient sftp = CreateClient())
			{
				try
				{
					sftp.Connect();

					if (DeliveryWriteMode.Overwrite != writeMode && sftp.Exists(Destination))
					{
						if (DeliveryWriteMode.Exception == writeMode)
						{
							throw new SftpPermissionDeniedException(string.Format("Destination file exists and Overwrite flag has not been specified: '{0}'", Destination));
						}
						// DeliverWriteMode.Ignore
						_log.Info(m => m("Destination file exists and flag is set to Ignore. Skipping.\n{0}", Destination));
					}

					sftp.UploadFile(dataStream, Destination, (DeliveryWriteMode.Overwrite == writeMode));
					// if nothing blew up we succeeded (?)
					return true;
				}
				catch (SshConnectionException ex)
				{
					_log.Warn(m => m("Unable to establish an SFTP connection to '{0}@{1}'\n{2}", Username, Hostname, ex));
				}
				catch(SftpPermissionDeniedException ex)
				{
					_log.Warn(m => m("Failed to upload file to '{0}@{1}:{2}'\n{3}", Username, Hostname, Destination, ex));
				}
				catch(SshException ex)
				{
					_log.Warn(m => m("SSH server returned the following error '{0}'\n{1}", ex.Message, ex));
				}
				finally
				{
					if (sftp != null && sftp.IsConnected)
					{
						sftp.Disconnect();
					}
				}
			}

#region SharpSSH/JSch
/*
			Sftp sftp = new Sftp(Hostname, Username);
			sftp.AddIdentityFile(KeyFile);

			// TODO: write bytes to a file so it can be passed to sftp
*/

/* The following code should upload a stream of byte[] data, but is throwing the following error:
 * 
Tamir.SharpSsh.jsch.JSchException: Session.connect: System.IO.FileNotFoundException: Could not load file or assembly 'DiffieHellman, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies. The system cannot find the file specified.
File name: 'DiffieHellman, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
   at Tamir.SharpSsh.jsch.jce.DH.getE()
   at Tamir.SharpSsh.jsch.DHG1.init(Session session, Byte[] V_S, Byte[] V_C, Byte[] I_S, Byte[] I_C)
   at Tamir.SharpSsh.jsch.Session.receive_kexinit(Buffer buf)
   at Tamir.SharpSsh.jsch.Session.connect(Int32 connectTimeout)

 * 
 * Even though I've installed the DiffieHellman library from here: https://nuget.org/packages/DiffieHellman/1.0.0
 * 
			_log.Trace(m => m("Initializing SSH object and setting key file: '{0}'...", KeyFile));
			JSch ssh = new JSch();
			ssh.addIdentity(KeyFile);	// not sure if this is a file or the text of the key - 12/13/2012, shawn.south@bellevuecollege.edu

			Session session = null;
			try
			{
				_log.Info(m => m("Initiating SSH session: {0}@{1}", Username, Hostname));
				session = ssh.getSession(Username, Hostname);
				_log.Trace(m => m("Connecting to '{0}' as '{1}'...", Hostname, Username));
				session.connect();
				_log.Trace(m => m("Connected."));

				Channel channel = null;
				try
				{
					channel = session.openChannel("sftp");
					_log.Trace(m => m("Connecting to SFTP channel..."));
					channel.connect();
					_log.Trace(m => m("Connected."));

					ChannelSftp sftp = null;
					try
					{
						sftp = channel as ChannelSftp;

						if (sftp != null)
						{
							_log.Trace(m => m("Uploading data stream to '{0}'...", Destination));
							using (InputStreamWrapper dataStream = new InputStreamWrapper(new MemoryStream(Source)))
							{
								sftp.put(dataStream, Destination);
								_log.Trace(m => m("Upload complete."));

								return true;
							}
						}
						else
						{
							_log.Warn(m => m("Unable to create SFTP channel."));
						}
					}
					finally
					{
						if (sftp != null && sftp.isConnected())
						{
							sftp.disconnect();
						}
					}
				}
				finally
				{
					if (channel != null && channel.isConnected())
					{
						channel.disconnect();
					}
				}
			}
			finally
			{
				if (session != null && session.isConnected())
				{
					session.disconnect();
				}
			}
 */
#endregion
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private SftpClient CreateClient()
		{

			if (string.IsNullOrWhiteSpace(KeyFile))
			{
				if (string.IsNullOrWhiteSpace(Password))
				{
					throw new SshAuthenticationException("Password cannot be blank if no KeyFile is provided.");
				}
				return new SftpClient(Hostname, Username, Password);
			}
			
			// if we haven't already returned...
			// Include Password if it was provided
			PrivateKeyFile[] keys = new[] {string.IsNullOrWhiteSpace(Password) ? new PrivateKeyFile(KeyFile) : new PrivateKeyFile(KeyFile, Password)};
			
			return new SftpClient(Hostname, Username, keys);
		}
	}
}