#region Apache License
//
// Licensed to the Apache Software Foundation (ASF) under one or more 
// contributor license agreements. See the NOTICE file distributed with
// this work for additional information regarding copyright ownership. 
// The ASF licenses this file to you under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with 
// the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NJetty.Util.Component
{
    /// <summary>
    /// TODO: Class/Interface Information here
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// November 2008
    /// </date>
    public interface ILifeCycle
    {

        
        /// <summary>
        /// Starts the component.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the component.
        /// The component may wait for current activities to complete
        /// normally, but it can be interrupted.
        /// </summary>
        void Stop();

        /// <summary>
        /// true if the component is starting or has been started.
        /// </summary>
        bool IsRunning
        {
            get;
        }

        /// <summary>
        /// true if the component has been started.
        /// </summary>
        bool IsStarted
        {
            get;
        }

        /// <summary>
        /// true if the component is starting.
        /// </summary>
        bool IsStarting
        {
            get;
        }

        /// <summary>
        /// true if the component is stopping.
        /// </summary>
        bool IsStopping
        {
            get;
        }

        /// <summary>
        /// true if the component has been stopped.
        /// </summary>
        bool IsStopped
        {
            get;
        }

        /// <summary>
        /// true if the component has failed to start or has failed to stop.
        /// </summary>
        bool IsFailed
        {
            get;
        }

        void AddLifeCycleListener(IListener listener);

        void RemoveLifeCycleListener(IListener listener);

    }
}
