// Copyright © Omron Robotics and Safety Technologies, Inc. All rights reserved.
//

using Ace.Services.LogService;
using System;
using System.Threading;

namespace Ace.Client {
	/// <summary>
	/// Utility class that manages a background thread 
	/// </summary>
	internal class BackgroundCommandMonitor {

		private ILogService logService;
		private bool run;
		private Thread thread;
		private Action operation;
		private int defaultDelay = 100;

		/// <summary>
		/// Should the monitor thread continue to run
		/// </summary>
		public bool ShouldRun {
			get { return run; }
		}

		/// <summary>
		/// Gets or sets the default delay (milliseconds).
		/// </summary>
		public int DefaultDelay {
			get { return defaultDelay; }
			set {
				if (value < 0)
					return;
				defaultDelay = value;
			}
		}

		/// <summary>
		/// Occurs when an error is detected.
		/// </summary>
		public event Action<object, Exception> ErrorDetected;

		/// <summary>
		/// Initializes a new instance of the <see cref="BackgroundCommandMonitor"/> class.
		/// </summary>
		/// <param name="logService">The log service.</param>
		/// <param name="monitorOperation">The monitor operation.</param>
		public BackgroundCommandMonitor(ILogService logService,
										Action monitorOperation) {
			this.logService = logService;
			this.operation = monitorOperation;
		}

		/// <summary>
		/// Starts the monitor running
		/// </summary>
		public void Start() {
			if ((thread != null) && (thread.IsAlive))
				return;
			run = true;
			thread = new Thread(new ThreadStart(BackgroundMain));
			thread.Start();
		}

		/// <summary>
		/// Stops the monitor from running.
		/// </summary>
		public void Stop() {
			run = false;
			thread?.Join(1000);
			thread = null;
		}

		private void BackgroundMain() {

			try {
				while (ShouldRun) {
					operation.Invoke();
					Thread.Sleep(DefaultDelay);
				}
			} catch (Exception ex) {
				ErrorDetected?.Invoke(this, ex);
				logService.Log("BackgroundCommandMonitor", ex);
			}

		}

	}
}
