﻿using System;

namespace BizHawk.Emulation.Common
{
	/// <summary>
	/// This object facilitates communications between client and core
	/// and is used by the IEmulator interface
	/// The primary use is to provide a client => core communication, such as providing client-side callbacks for a core to use
	/// Any communications that can be described as purely a Core -> Client system, should be provided as an <seealso cref="IEmulatorService"/> instead
	/// </summary>
	/// <seealso cref="IEmulator" />
	public class CoreComm
	{
		public CoreComm(Action<string> showMessage, Action<string> notifyMessage)
		{
			ShowMessage = showMessage;
			Notify = notifyMessage;
		}

		public CoreComm Clone() => (CoreComm)MemberwiseClone();

		public ICoreFileProvider CoreFileProvider { get; set; }

		/// <summary>
		/// Gets a message to show. reasonably annoying (dialog box), shouldn't be used most of the time
		/// </summary>
		public Action<string> ShowMessage { get; }

		/// <summary>
		/// Gets a message to show. less annoying (OSD message). Should be used for ignorable helpful messages
		/// </summary>
		public Action<string> Notify { get; }
	}
}
