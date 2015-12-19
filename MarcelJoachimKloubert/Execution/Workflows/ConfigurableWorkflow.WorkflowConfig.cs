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
    partial class ConfigurableWorkflow
    {
        #region INTERFACE: IWorkflowConfig

        /// <summary>
        /// Describes a workflow config.
        /// </summary>
        public interface IWorkflowConfig
        {
            #region Properties (1)

            /// <summary>
            /// Gets the underlying workflow.
            /// </summary>
            ConfigurableWorkflow Workflow { get; }

            #endregion Properties (1)

            #region Methods (7)

            /// <summary>
            /// Defines what should be the next action to invoke.
            /// </summary>
            /// <param name="nextAction">The next action.</param>
            /// <returns>The new config.</returns>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="nextAction" /> is <see langword="null" />.
            /// </exception>
            IWorkflowConfig ContinueWith(WorkflowAction nextAction);

            /// <summary>
            /// Defines what should be the next action to invoke.
            /// </summary>
            /// <param name="action">The next action.</param>
            /// <returns>The new config.</returns>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="action" /> is <see langword="null" />.
            /// </exception>
            IWorkflowConfig ContinueWith(Action action);

            /// <summary>
            /// Executes the underlying workflow.
            /// </summary>
            /// <param name="argList">The arguments for the execution.</param>
            /// <returns>The final result of the execution.</returns>
            object Execute(IEnumerable<object> argList);

            /// <summary>
            /// Executes the underlying workflow.
            /// </summary>
            /// <param name="args">The arguments for the execution.</param>
            /// <returns>The final result of the execution.</returns>
            object Execute(params object[] args);

            /// <summary>
            /// Defines what should be the action to handle an error.
            /// </summary>
            /// <param name="errorAction">The next action.</param>
            /// <returns>The new config.</returns>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="errorAction" /> is <see langword="null" />.
            /// </exception>
            IWorkflowConfig IfFails(WorkflowAction errorAction);

            /// <summary>
            /// Defines what should be the action to handle an error.
            /// </summary>
            /// <param name="action">The next action.</param>
            /// <returns>The new config.</returns>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="action" /> is <see langword="null" />.
            /// </exception>
            IWorkflowConfig IfFails(Action action);

            /// <summary>
            /// Stops the execution and returns the parent config.
            /// </summary>
            /// <returns>The parent config or this object if it is the root.</returns>
            IWorkflowConfig Stop();

            #endregion Methods (7)
        }

        #endregion INTERFACE: IWorkflowConfig

        #region CLASS: WorkflowConfig

        internal class WorkflowConfig : IWorkflowConfig
        {
            #region Fields (2)

            internal readonly WorkflowAction ACTION;
            internal readonly WorkflowConfig PARENT;

            #endregion Fields (2)

            #region Constructors (1)

            internal WorkflowConfig(ConfigurableWorkflow workflow, WorkflowAction action, WorkflowConfig parent = null)
            {
                Workflow = workflow;
                ACTION = action;
                PARENT = parent;
            }

            #endregion Constructors (1)

            #region Properties (3)

            internal WorkflowConfig Next { get; private set; }

            internal WorkflowConfig OnError { get; private set; }

            public ConfigurableWorkflow Workflow { get; }

            #endregion Properties (3)

            #region Methods (11)

            internal WorkflowConfig ContinueWith(WorkflowAction nextAction)
            {
                if (nextAction == null)
                {
                    throw new ArgumentNullException(nameof(nextAction));
                }

                return Next = new WorkflowConfig(Workflow, nextAction, this);
            }

            IWorkflowConfig IWorkflowConfig.ContinueWith(WorkflowAction nextAction) => ContinueWith(nextAction: nextAction);

            internal WorkflowConfig ContinueWith(Action action) => ContinueWith(nextAction: ToAction(action));

            IWorkflowConfig IWorkflowConfig.ContinueWith(Action action) => ContinueWith(action: action);

            public object Execute(IEnumerable<object> argList) => Workflow.Execute(argList: argList);

            public object Execute(params object[] args) => Workflow.Execute(args: args);

            internal WorkflowConfig IfFails(WorkflowAction errorAction)
            {
                if (errorAction == null)
                {
                    throw new ArgumentNullException(nameof(errorAction));
                }

                return OnError = new WorkflowConfig(Workflow, errorAction, this);
            }

            IWorkflowConfig IWorkflowConfig.IfFails(WorkflowAction errorAction) => IfFails(errorAction: errorAction);

            internal WorkflowConfig IfFails(Action action) => IfFails(errorAction: ToAction(action));

            IWorkflowConfig IWorkflowConfig.IfFails(Action action) => IfFails(action: action);

            internal WorkflowConfig Stop() => PARENT ?? this;

            IWorkflowConfig IWorkflowConfig.Stop() => Stop();

            private static WorkflowAction ToAction(Action action)
            {
                return action != null ? new WorkflowAction((ctx) => action())
                                      : null;
            }

            #endregion Methods (11)
        }

        #endregion CLASS: WorkflowConfig
    }
}