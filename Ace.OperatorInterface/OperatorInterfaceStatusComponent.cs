// Copyright © Omron Robotics and Safety Technologies, Inc. All rights reserved.
//

using Ace.OperatorInterface.View;
using Ace.OperatorInterface.ViewModel;
using Ace.Services.NameLookup;
using Ace.Services.TaskManager;
using Ace.Util;
using Omron.Cxap.DataLayer.Entities.Core;
using System;
using System.ComponentModel;
using System.Windows;

namespace Ace.OperatorInterface
{
    /// <summary>
    /// OperatorInterfaceStatusComponent
    /// </summary>
    public class OperatorInterfaceStatusComponent : ICustomTaskStatusComponent, IDisposable
    {
        /// <summary>
        /// assembly name
        /// </summary>
        private const string AssemblyName = "Ace.OperatorInterface";

        private object entity;
                
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the name of the group where the component will be displayed
        /// </summary>
        public string GroupingName
        {
            get
            {
                return this.CustomGroupingName;
            }
        }

        /// <summary>
        /// Gets the name of the section under the group where the component will be displayed
        /// </summary>
        public string SectionName
        {
            get
            {
                return this.CustomSectionName;
            }
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this.CustomDisplayName;
            }
        }                

        /// <summary>
        /// Gets the element for representing the item in the UI
        /// </summary>
        public FrameworkElement Element
        {
            get;
            private set;
        }

        /// <summary>
        /// ViewModel
        /// </summary>
        public OperatorInterfaceViewModel ViewModel { get; set; }

        public int SortOrder => throw new NotImplementedException();
                
        /// <summary>
        /// CustomGroupingName
        /// </summary>
        public string CustomGroupingName { get; set; }

        /// <summary>
        /// CustomSectionName
        /// </summary>
        public string CustomSectionName { get; set; }

        /// <summary>
        /// CustomDisplayName
        /// </summary>
        public string CustomDisplayName { get; set; }

        /// <summary>
        /// CustomLibraryUtil
        /// </summary>
        public ICustomLibraryUtil CustomLibraryUtil { get; set; }

        public Entity Entity => throw new NotImplementedException();

        public bool IsSupported => throw new NotImplementedException();

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="nameLookupService"></param>
        public OperatorInterfaceStatusComponent()
        {            
        }
        
        /// <summary>
        /// The component is being activated
        /// </summary>
        public void Activate()
        {
            if (ViewModel == null)
            {
                OperatorInterfaceViewModel viewModel = null;
                FrameworkElement view = null;
                OnCreateUIComponent(out viewModel, out view);
                ViewModel = viewModel;
                Element = view;
                Element.DataContext = ViewModel;
            }
        }

        /// <summary>
        /// Notifies the component that the associated entity has been renamed.
        /// </summary>
        public void NotifyNameChange()
        {
            OnPropertyModified(nameof(DisplayName));
            OnPropertyModified(nameof(SectionName));
        }

        /// <summary>
        /// The component should enter edit mode
        /// </summary>
        public void Edit()
        {
            return;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            OnDispose();
        }

        /// <summary>
        /// Called when the UI components should be created.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="view">The element.</param>
        protected void OnCreateUIComponent(out OperatorInterfaceViewModel viewModel, out FrameworkElement view)
        {
            viewModel = new OperatorInterfaceViewModel(this);
            view = new OperatorInterfaceView();                        
        }
                
        /// <summary>
        /// The component is being deactivated
        /// </summary>
        public void Deactivate()
        {            
        }

        /// <summary>
        /// Called when the operation is being disposed.
        /// </summary>
        protected void OnDispose()
        {         
            Element = null;
            ViewModel?.ClearAllControllers();
            ViewModel = null;
        }

        /// <summary>
        /// Called when a property is modified
        /// </summary>
        /// <param name="property">The property.</param>
        protected void OnPropertyModified(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }                       
        
        public void UpdateTarget(Entity entity)
        {
            this.entity = entity;
        }

        /// <summary>
        /// UpdateComponents
        /// </summary>
        public void UpdateComponents()
        {
            if (ViewModel != null)
            {
                ViewModel.UpdateControllerCollection();
            }
        }

        public void OnAdded()
        {
            throw new NotImplementedException();
        }
    }
}
