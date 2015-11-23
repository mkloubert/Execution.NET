/**********************************************************************************************************************
 * Execution.NET (https://github.com/mkloubert/Execution.NET)                                                         *
 *                                                                                                                    *
 * Copyright (c) 2015, Marcel Joachim Kloubert <marcel.kloubert@gmx.net>                                              *
 * All rights reserved.                                                                                               *
 *                                                                                                                    *
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the   *
 * following conditions are met:                                                                                      *
 *                                                                                                                    *
 * 1. Redistributions of source code must retain the above copyright notice, this list of conditions and the          *
 *    following disclaimer.                                                                                           *
 *                                                                                                                    *
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the       *
 *    following disclaimer in the documentation and/or other materials provided with the distribution.                *
 *                                                                                                                    *
 * 3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote    *
 *    products derived from this software without specific prior written permission.                                  *
 *                                                                                                                    *
 *                                                                                                                    *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, *
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE  *
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, *
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR    *
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,  *
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE   *
 * USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.                                           *
 *                                                                                                                    *
 **********************************************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MarcelJoachimKloubert.Workflows
{
    /// <summary>
    /// A basic workflow.
    /// </summary>
    public abstract partial class WorkflowBase : IWorkflow
    {
        #region Fields (1)

        private readonly object _SYNC_ROOT;

        #endregion Fields (1)

        #region Constructors (1)

        /// <summary>
        /// Initialites a new instance of the <see cref="WorkflowBase" /> class.
        /// </summary>
        /// <param name="syncRoot">The value for the <see cref="WorkflowBase.SyncRoot" /> property.</param>
        protected WorkflowBase(object syncRoot = null)
        {
            _SYNC_ROOT = syncRoot ?? new object();
        }

        #endregion Constructors (1)

        #region Properties (2)

        /// <summary>
        /// Gets the object for thread safe operations.
        /// </summary>
        public virtual object SyncRoot
        {
            get { return _SYNC_ROOT; }
        }

        /// <summary>
        /// Gets or sets an object that should be linked with that instance.
        /// </summary>
        public virtual object Tag { get; set; }

        #endregion Properties (2)

        #region Methods (5)

        /// <summary>
        /// Converts a delegates that requires no arguments to a <see cref="WorkflowAction" /> instance.
        /// </summary>
        /// <param name="action">The input value.</param>
        /// <returns>The output value.</returns>
        /// <remarks>
        /// Returns <see langword="null" /> id <paramref name="action" /> is also <see langword="null" />.
        /// </remarks>
        public static WorkflowAction ToAction(Action action)
        {
            if (action == null)
            {
                return null;
            }

            return (ctx) => action();
        }

        /// <inheriteddoc />
        public object Execute(IEnumerable<object> argList)
        {
            var args = new object[0];
            if (argList != null)
            {
                args = argList as object[];
                if (args == null)
                {
                    args = argList.ToArray();
                }
            }

            return Execute(args: args);
        }

        /// <inheriteddoc />
        public object Execute(params object[] args)
        {
            args = args ?? new object[] { null };

            object result = null;

            using (var e = GetEnumerator())
            {
                while (e.MoveNext())
                {
                    var ctx = e.Current(args);
                    if (ctx != null)
                    {
                        result = ctx.Result;
                    }
                }
            }

            return result;
        }

        /// <inheriteddoc />
        public IEnumerator<WorkflowFunc> GetEnumerator()
        {
            return (GetFunctions() ?? Enumerable.Empty<WorkflowFunc>()).OfType<WorkflowFunc>()
                                                                       .GetEnumerator();
        }

        /// <summary>
        /// Returns the functions for <see cref="WorkflowBase.GetEnumerator()" /> method.
        /// </summary>
        /// <returns>The list of functions.</returns>
        protected abstract IEnumerable<WorkflowFunc> GetFunctions();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion Methods (5)
    }
}