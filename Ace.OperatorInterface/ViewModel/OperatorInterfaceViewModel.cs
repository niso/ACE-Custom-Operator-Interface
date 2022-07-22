// Copyright © Omron Robotics and Safety Technologies, Inc. All rights reserved.
//

using Ace.OperatorInterface.Controller;
using Ace.Services.NameLookup;
using Ace.Services.TaskManager;
using Ace.Client;
using System;

namespace Ace.OperatorInterface.ViewModel
{
    /// <summary>
    /// OperatorInterfaceViewModel
    /// </summary>
    public class OperatorInterfaceViewModel : PropertyModifyBase, IOperatorInterfaceViewModel
    {
        /// <summary>
        /// CustomTaskStatusComponent
        /// </summary>
        public ICustomTaskStatusComponent CustomTaskStatusComponent { get; private set; }
                
        public ControllerCollection _controllerItems;

        /// <summary>
        /// ControllerItems
        /// </summary>
        public ControllerCollection ControllerItems
        {
            get => _controllerItems;
            set
            {
                if (_controllerItems != value)
                    _controllerItems = value;
                    OnPropertyChanged(nameof(CustomTaskStatusComponent));
            }
        }

        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get
            {
                return CustomTaskStatusComponent?.CustomDisplayName;
            }
        }

        public static object DataContext { get; internal set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="customTaskStatusComponent"></param>
        /// <param name="nameLookupService"></param>
        public OperatorInterfaceViewModel(ICustomTaskStatusComponent customTaskStatusComponent)
        {
            this.CustomTaskStatusComponent = customTaskStatusComponent;            
            this.ControllerItems = new ControllerCollection(customTaskStatusComponent.CustomLibraryUtil);
            this.OnPropertyChanged(nameof(ControllerItems));
            this.OnPropertyChanged(nameof(Title));
            this.UpdateControllerCollection();
        }

        /// <summary>
        /// UpdateControllerCollection
        /// </summary>
        public void UpdateControllerCollection()
        {
            this.ControllerItems.UpdateControllerList();
            this.OnPropertyChanged(nameof(ControllerItems));
        }
    }
}
