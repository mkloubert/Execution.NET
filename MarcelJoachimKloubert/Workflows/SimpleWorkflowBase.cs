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

namespace MarcelJoachimKloubert.Workflows
{
    /// <summary>
    /// A basic object for a simple (method based) workflow.
    /// </summary>
    public abstract class SimpleWorkflowBase : AttributeWorkflowBase
    {
        #region Constructors (1)

        /// <summary>
        /// Initialites a new instance of the <see cref="SimpleWorkflowBase" /> class.
        /// </summary>
        /// <param name="syncRoot">The value for the <see cref="WorkflowBase.SyncRoot" /> property.</param>
        protected SimpleWorkflowBase(object syncRoot = null)
            : base(syncRoot: syncRoot)
        {
        }

        #endregion Constructors (1)

        #region Methods (4)

        /// <summary>
        /// Handles an execution error.
        /// </summary>
        /// <param name="ctx">The current execution context.</param>
        protected virtual void HandleError(IWorkflowExecutionContext ctx)
        {
            ctx.ContinueOnError = false;
            throw ctx.LastError;
        }

        /// <summary>
        /// The first action for <see cref="SimpleWorkflowBase.StartWorkflow(IWorkflowExecutionContext)" />.
        /// </summary>
        /// <param name="ctx">The current execution context.</param>
        protected abstract void ExecuteFirstStep(IWorkflowExecutionContext ctx);

        /// <summary>
        /// The next (default) action for <see cref="SimpleWorkflowBase.StartWorkflow(IWorkflowExecutionContext)" />.
        /// </summary>
        /// <param name="ctx">The current execution context.</param>
        [OnWorkflowError("HandleError")]
        protected virtual void ExecuteNextStep(IWorkflowExecutionContext ctx)
        {
            // do nothing by default
        }

        /// <summary>
        /// Starts the workflow.
        /// </summary>
        /// <param name="ctx">The current execution context.</param>
        [WorkflowStart]
        [NextWorkflowStep("ExecuteNextStep")]
        [OnWorkflowError("HandleError")]
        protected void StartWorkflow(IWorkflowExecutionContext ctx)
        {
            ExecuteFirstStep(ctx);
        }

        #endregion Methods (4)
    }
}