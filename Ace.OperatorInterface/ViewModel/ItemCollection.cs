// Copyright © Omron Robotics and Safety Technologies, Inc. All rights reserved.
//

using Ace.Server.Adept.Controllers;
using Ace.Services.NameLookup;
using Ace.Util;
using System.Collections.ObjectModel;

namespace Ace.OperatorInterface.ViewModel
{
    /// <summary>
    /// Represents the base class for helpers that contain item collections
    /// </summary>
    public class ItemCollection : PropertyModifyBase
    {
        /// <summary>
        /// CustomLibraryUtil
        /// </summary>
        protected ICustomLibraryUtil CustomLibraryUtil { get; private set; }

        /// <summary>
        /// NameLookupService
        /// </summary>
        protected INameLookupService NameLookupService
        {
            get
            {
                return this.CustomLibraryUtil?.NameLookupService;
            }
        }

        /// <summary>
        /// ConnectionHelper
        /// </summary>
        protected IConnectionHelper ConnectionHelper
        {
            get
            {
                return this.CustomLibraryUtil?.ConnectionHelper;
            }
        }

        /// <summary>
        /// Gets the listing of items in the display
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCollection"/> class.
        /// </summary>
        public ItemCollection(ICustomLibraryUtil customLibraryUtil)
        {
            this.CustomLibraryUtil = customLibraryUtil;            

            this.Items = new ObservableCollection<ItemViewModel>();                               
        }        
                
        /// <summary>
        /// Clears all data from the helper
        /// </summary>
        public virtual void Clear()
        {
            foreach (var item in Items)
            {
                item.UnhookEvents();
            }
            Items.Clear();
        }
    }
}
