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

namespace MarcelJoachimKloubert.Execution.Workflows
{
    /// <summary>
    /// Describes a workflow execution context.
    /// </summary>
    public interface IWorkflowExecutionContext
    {
        #region Properties (12)

        /// <summary>
        /// Gets the list of arguments that were submitted to the <see cref="IWorkflow.Execute(object[])" /> method.
        /// </summary>
        object[] Arguments { get; }

        /// <summary>
        /// Gets or sets if operation should be canceled or not.
        /// </summary>
        bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets if execution should be continued after an exception was thrown or not.
        /// </summary>
        bool ContinueOnError { get; set; }

        /// <summary>
        /// Gets if the operation has been canceled or not.
        /// </summary>
        bool HasBeenCanceled { get; }

        /// <summary>
        /// Gets the error from previous execution or <see langword="null" /> if no error occured.
        /// </summary>
        Exception LastError { get; }

        /// <summary>
        /// Gets or sets the next action.
        /// </summary>
        WorkflowAction Next { get; set; }

        /// <summary>
        /// Sets an <see cref="Action" /> for the <see cref="IWorkflowExecutionContext.Next" /> property.
        /// </summary>
        Action NextAction { set; }

        /// <summary>
        /// Gets or sets the value for the next execution.
        /// </summary>
        /// <remarks>This value value is resetted before each execution.</remarks>
        object NextValue { get; set; }

        /// <summary>
        /// Gets the value from the previous execution that was set in <see cref="IWorkflowExecutionContext.NextValue" /> property.
        /// </summary>
        object PreviousValue { get; }

        /// <summary>
        /// Gets or sets the result object for an <see cref="IWorkflow.Execute(object[])" /> method.
        /// </summary>
        object Result { get; set; }

        /// <summary>
        /// Gets or sets if all collected errors should be executed at the end of the execution chain or not.
        /// </summary>
        bool ThrowErrors { get; set; }

        /// <summary>
        /// Gets or sets the value for whole execution chain.
        /// </summary>
        object Value { get; set; }

        #endregion Properties (12)
    }
}