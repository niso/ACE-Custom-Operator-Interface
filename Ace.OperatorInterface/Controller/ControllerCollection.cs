// Copyright © Omron Robotics and Safety Technologies, Inc. All rights reserved.
//

using Ace.OperatorInterface.Controller.ViewModel;
using Ace.OperatorInterface.ViewModel;
using Ace.Server.Adept.Controllers;
using Ace.Util;
using System;

namespace Ace.OperatorInterface.Controller
{
    /// <summary>
	/// ControllerCollection class that manages the listing of controllers 
	/// </summary>
	/// <seealso cref="ControllerCollection" />
	public class ControllerCollection : ItemCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerHelper"/> class.
        /// </summary>
        /// <param name="connectionHelper">The connection helper.</param>
        public ControllerCollection(ICustomLibraryUtil customLibraryUtil) : base(customLibraryUtil)  { }

        /// <summary>
        /// Method to update the items in the item collection
        /// </summary>
        public void UpdateControllerList()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                lock (Items)
                {
                    Clear();

                    try
                    {
                        var controllers = this.NameLookupService[typeof(IAdeptController)];
                        foreach (var controller in controllers)
                        {
                            var item = new ControllerViewModel(this.NameLookupService, controller as IAdeptController, this.ConnectionHelper);
                            item.ReportError = (text) => OnReportError(text);
                            Items.Add(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        OnReportError(ex);
                    }
                }
            });
        }
    }
}
