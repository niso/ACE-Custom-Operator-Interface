// Copyright © Omron Robotics and Safety Technologies, Inc. All rights reserved.
//

using Ace.Server.Adept.Controllers;
using Ace.Services.NameLookup;
using Ace.Util;
using System;
using System.Linq;

namespace RemoteAccess {

	/// <summary>
	/// Demonstrates making a remote connection to an existing
	/// ACE instance and calling functions on it.
	/// <list type="bullet">
	/// <item>Set the <c>RemotingPort</c> value to the port number on which ACE should 
	/// listen for connections, such as "12543" (default is "0", which disables remote access).</item>
	/// <item>Set the <c>RemotingName</c> value to the name by which the ACE instance should respond (default is "ace").</item>
	/// </list>
	/// </summary>
	public class RemoteAccessDemo {

		const string RemotingName = "ace";
		const int AceInstance = 1;		

		static void Main(string[] args) {

			// Initialize remoting infrastructure
			RemotingUtil.InitializeRemotingSubsystem(true, 0);

			// Assign the remoting port based on the default port and the 
			// instance of ACE we want to access
			int RemotingPort = RemotingUtil.DefaultRemotingPortBase + (100 * AceInstance);

			// Connect to ACE.
			INameLookupService ace = (INameLookupService) RemotingUtil.GetRemoteServerObject(typeof(INameLookupService), RemotingName, "localhost", RemotingPort);

			// Get access to the controller using the name
			var controllers = ace[typeof(IAdeptController)].Cast<IAdeptController>();
			foreach (var controller in controllers) {
				controller.HighPower = true;
				controller.Calibrate();
			}

		}
	}
}
