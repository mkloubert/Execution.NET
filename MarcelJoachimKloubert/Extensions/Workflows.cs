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

using MarcelJoachimKloubert.Execution.Workflows;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarcelJoachimKloubert.Extensions
{
    /// <summary>
    /// Workflow extension methods.
    /// </summary>
    public static class MJKWorkflowExtensionMethods
    {
        #region Methods (3)

        /// <summary>
        /// Async execution of an <see cref="IWorkflow" />.
        /// </summary>
        /// <param name="workflow">The workflow to execute.</param>
        /// <param name="argsList">The list of arguments for the execution.</param>
        /// <returns>The running task.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="workflow" /> is <see langword="null" />.
        /// </exception>
        public static async Task<object> ExecuteAsync(this IWorkflow workflow, IEnumerable<object> argsList)
        {
            if (workflow == null)
            {
                throw new ArgumentNullException(nameof(workflow));
            }

            return await Task.Factory.StartNew(function: (state) =>
                {
                    var taskArgs = (object[])state;

                    return ((IWorkflow)taskArgs[0]).Execute(argList: (IEnumerable<object>)taskArgs[1]);
                }, state: new object[] { workflow, argsList });
        }

        /// <summary>
        /// Async execution of an <see cref="IWorkflow" />.
        /// </summary>
        /// <param name="workflow">The workflow to execute.</param>
        /// <param name="args">The list of arguments for the execution.</param>
        /// <returns>The running task.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="workflow" /> is <see langword="null" />.
        /// </exception>
        public static async Task<object> ExecuteAsync(this IWorkflow workflow, params object[] args)
        {
            if (workflow == null)
            {
                throw new ArgumentNullException(nameof(workflow));
            }

            return await Task.Factory.StartNew(function: (state) =>
                {
                    var taskArgs = (object[])state;

                    return ((IWorkflow)taskArgs[0]).Execute(args: (object[])taskArgs[1]);
                }, state: new object[] { workflow, args });
        }

        /// <summary>
        /// Returns a thread safe version of a workflow.
        /// </summary>
        /// <param name="workflow">The workflow to wrap.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <returns>
        /// The workflow wrapper or <see langword="null" /> if <paramref name="workflow" />
        /// is also <see langword="null" />.
        /// </returns>
        public static IWorkflow Synchronize(this IWorkflow workflow, object syncRoot = null)
        {
            return workflow != null ? new SynchronizedWorkflow(baseWorkflow: workflow, syncRoot: syncRoot)
                                    : null;
        }

        #endregion Methods (3)
    }
}