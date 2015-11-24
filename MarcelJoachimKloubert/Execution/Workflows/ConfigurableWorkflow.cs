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
using System.Collections.Generic;

namespace MarcelJoachimKloubert.Execution.Workflows
{
    /// <summary>
    /// A workflow that uses a fluend API to be build.
    /// </summary>
    public partial class ConfigurableWorkflow : WorkflowBase
    {
        #region Fields (1)

        private WorkflowConfig _start;

        #endregion Fields (1)

        #region Methods (3)

        /// <inheriteddoc />
        protected override IEnumerable<WorkflowFunc> GetFunctions()
        {
            var occuredErrors = new List<Exception>();

            var current = _start; 
            var hasBeenCanceled = false;
            Exception lastError = null;
            object previousValue = null;
            object result = null;
            var throwErrors = true;
            object value = null;
            while (!hasBeenCanceled &&
                   current != null)
            {
                yield return (args) =>
                    {
                        var ctx = new WorkflowExecutionContext()
                            {
                                Arguments = args,
                                LastError = lastError,
                                PreviousValue = previousValue,
                                Result = result,
                                ThrowErrors = throwErrors,
                                Value = value,
                            };

                        try
                        {
                            current.ACTION(ctx);

                            lastError = null;
                            current = current.Next;
                        }
                        catch (Exception ex)
                        {
                            ctx.LastError = lastError = ex;

                            occuredErrors.Add(ex);

                            var onError = current.OnError;
                            if (onError != null)
                            {
                                current = onError;
                            }
                            else
                            {
                                if (!ctx.ContinueOnError)
                                {
                                    throw;
                                }
                            }
                        }

                        var nextAction = ctx.Next;
                        if (nextAction != null)
                        {
                            current = new WorkflowConfig(current.Workflow, nextAction, current);
                        }

                        if (ctx.Cancel)
                        {
                            hasBeenCanceled = true;
                            ctx.HasBeenCanceled = hasBeenCanceled;
                        }

                        previousValue = ctx.NextValue;
                        throwErrors = ctx.ThrowErrors;
                        result = ctx.Result;
                        value = ctx.Value;

                        return ctx;
                    };
            }

            if (throwErrors &&
                occuredErrors.Count > 0)
            {
                throw new AggregateException(occuredErrors);
            }
        }

        /// <summary>
        /// Starts building the workflow.
        /// </summary>
        /// <param name="startAction">The start action.</param>
        /// <returns>The config of the start action.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="startAction" /> is <see langword="null" />.
        /// </exception>
        public IWorkflowConfig StartWith(WorkflowAction startAction)
        {
            if (startAction == null)
            {
                throw new ArgumentNullException("startAction");
            }

            return _start = new WorkflowConfig(this, startAction);
        }

        /// <summary>
        /// Starts building the workflow.
        /// </summary>
        /// <param name="action">The start action.</param>
        /// <returns>The config of the start action.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public IWorkflowConfig StartWith(Action action)
        {
            return StartWith(action != null ? new WorkflowAction((ctx) => action())
                                            : null);
        }

        #endregion Methods (3)
    }
}