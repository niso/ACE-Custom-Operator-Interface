// Copyright © Omron Robotics and Safety Technologies, Inc. All rights reserved.
//

using Ace.OperatorInterface.Controller;
using Ace.Services.NameLookup;
using Ace.Services.TaskManager;
using Ace.Client;
using System;
using Ace.OperatorInterface.Controller.ViewModel;

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

        private ControllerViewModel selectedControllerItem;
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
		/// Gets or sets the selected controller item.
		/// </summary>
		public ControllerViewModel SelectedControllerItem {
			get {
                return selectedControllerItem;
            }
			set {
                selectedControllerItem = value;
                OnPropertyChanged(nameof(ControllerViewModel));
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

        public void ClearAllControllers() {
            this.ControllerItems.Clear();
		}
    }
}
