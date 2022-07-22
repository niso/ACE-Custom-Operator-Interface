// Copyright © Omron Robotics and Safety Technologies, Inc. All rights reserved.
//

using System;
using System.ComponentModel;

namespace Ace.OperatorInterface.ViewModel
{
    /// <summary>
    /// Base class for any items displayed by the helpers
    /// </summary>
    public class PropertyModifyBase : INotifyPropertyChanged
    {

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Report that an error has been detected.
        /// </summary>
        public Action<string> ReportError { get; set; }

        /// <summary>
        /// Method to get executed in BackgroundCommndMonitor class background task
        /// </summary>
        public Action BackGroundMonitorDelegate;


        /// <summary>
        /// Log exceptions and other messages to C:\Temp\log.txt"
        /// </summary>
        public static Action<string> LogMethodDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemBase"/> class.
        /// </summary>
        public PropertyModifyBase()
        {

        }

        /// <summary>
        /// Called when an error should be reported
        /// </summary>
        protected void OnReportError(string text)
        {
            //if (logMethod != null)
            //    logMethod("OnReportError text " + text);

            ReportError?.Invoke(text);
        }

        /// <summary>
        /// Called when an error should be reported
        /// </summary>
        protected void OnReportError(Exception ex)
        {
            if (LogMethodDelegate != null)
                LogMethodDelegate("OnReportError Exception " + ex.Message);

            OnReportError(ex.Message);
        }

        /// <summary>
        /// Called when a property is changed
        /// </summary>
        /// <param name="property">The property.</param>
        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}



