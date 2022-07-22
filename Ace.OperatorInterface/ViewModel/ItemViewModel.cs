// Copyright © Omron Robotics and Safety Technologies, Inc. All rights reserved.
//

using System;
using System.Windows;

namespace Ace.OperatorInterface.ViewModel
{
    /// <summary>
    /// Base class for any items displayed by the helpers
    /// </summary>
    public class ItemViewModel : PropertyModifyBase
    {                
        /// <summary>
        /// Gets the object handle.
        /// </summary>
        protected IAceObject ObjectHandle { get; private set; }

        private string _displayName;

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public virtual string DisplayName
        {
            get => _displayName;
            private set
            {      
                if(_displayName != value)
                {
                    _displayName = value;
                    this.OnObjectPropertyModified(nameof(DisplayName));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemViewModel" /> class.
        /// </summary>
        /// <param name="aceObject">The ace object.</param>
        public ItemViewModel(IAceObject aceObject)
        {
            this.ObjectHandle = aceObject;
            this.DisplayName = aceObject.Name;

            this.ObjectHandle.PropertyModified += ObjectHandle_PropertyModified;
        }

        private void ObjectHandle_PropertyModified(object sender, Server.Core.Event.PropertyModifiedEventArgs e)
        {
            OnObjectPropertyModified(e.PropertyName);
        }
                
        /// <summary>
        /// Called when a property in the object is modified.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnObjectPropertyModified(string propertyName)
        {
        }

        /// <summary>
        /// Raises the events on the UI thread
        /// </summary>
        protected void RaiseEvents()
        {
            Delegate del = new Action(() => RaiseEventsInternal());
            Application.Current.Dispatcher.BeginInvoke(del, new object[] { });
        }

        /// <summary>
        /// Defines the events that will be raised
        /// </summary>
        protected virtual void RaiseEventsInternal()
        {
        }

        /// <summary>
        /// Unhooks the events.
        /// </summary>
        public virtual void UnhookEvents()
        {
            try
            {
                this.ObjectHandle.PropertyModified -= ObjectHandle_PropertyModified;
            }
            catch (Exception ex)
            {
                OnReportError(ex);
            }
        }
    }
}
