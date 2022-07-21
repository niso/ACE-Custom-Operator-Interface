// Copyright © Omron Robotics and Safety Technologies, Inc. All rights reserved.
//

using Ace.Server.Core.Event;
using Ace.Server.Core.References;
using Ace.Util;
using System;

namespace Ace.OperatorInterface
{
    /// <summary>
    /// OperatorInterface
    /// </summary>    
    [TaskStatusComponent(typeof(OperatorInterfaceStatusComponent))]
    class OperatorInterface : IAceObject
    {
        public bool IsDisposed => throw new NotImplementedException();

        public bool IsBeingDisposed => throw new NotImplementedException();

        public string RuntimeIdentifier => throw new NotImplementedException();

        public string VisualizationBaseName => throw new NotImplementedException();

        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Guid Identifier
        {
            get; private set;
        }

		public bool EmulationMode => throw new NotImplementedException();

		/// <summary>
		/// Disposing
		/// </summary>
		public event EventHandler<EventArgs> Disposing;


        /// <summary>
        /// PropertyModified
        /// </summary>
        public event EventHandler<PropertyModifiedEventArgs> PropertyModified;

        public OperatorInterface()
        {
            this.Identifier = System.Guid.NewGuid();
        }

        public IAceObject Clone()
        {
            return null;
        }

        public void Dispose()
        {            
        }

        public ReferenceCollection GetReferences()
        {
            return null;
        }
    }
}
