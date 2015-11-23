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
    #region CLASS: WorkflowWrapper<TFunc>

    /// <summary>
    /// Wraps a workflow.
    /// </summary>
    /// <typeparam name="TWorkflow">Type of the function to wrap.</typeparam>
    public class WorkflowWrapper<TWorkflow> : WorkflowBase
        where TWorkflow : IWorkflow
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowWrapper{TWorkflow}" /> class.
        /// </summary>
        /// <param name="baseWorkflow">The function to wrap.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseWorkflow" /> is <see langword="null" />.
        /// </exception>
        public WorkflowWrapper(TWorkflow baseWorkflow, object syncRoot = null)
            : base(syncRoot: syncRoot)
        {
            if (baseWorkflow == null)
            {
                throw new ArgumentNullException("baseWorkflow");
            }

            BaseWorkflow = baseWorkflow;
        }

        #endregion Constructors (1)

        #region Properties (1)

        /// <summary>
        /// Gets the wrapped workflow.
        /// </summary>
        public TWorkflow BaseWorkflow { get; private set; }

        #endregion Properties (1)

        #region Methods (4)

        /// <inheriteddoc />
        public sealed override bool Equals(object obj)
        {
            return BaseWorkflow.Equals(obj);
        }

        /// <inheriteddoc />
        public sealed override int GetHashCode()
        {
            return BaseWorkflow.GetHashCode();
        }

        /// <inheriteddoc />
        protected override IEnumerable<WorkflowFunc> GetFunctions()
        {
            return BaseWorkflow;
        }

        /// <inheriteddoc />
        public sealed override string ToString()
        {
            return BaseWorkflow.ToString();
        }

        #endregion Methods (4)
    }

    #endregion CLASS: WorkflowWrapper<TFunc>

    #region CLASS: WorkflowWrapper

    /// <summary>
    /// Wraps a workflow.
    /// </summary>
    public class WorkflowWrapper : WorkflowWrapper<IWorkflow>
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowWrapper" /> class.
        /// </summary>
        /// <param name="baseWorkflow">The workflow to wrap.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseWorkflow" /> is <see langword="null" />.
        /// </exception>
        public WorkflowWrapper(IWorkflow baseWorkflow, object syncRoot = null)
            : base(baseWorkflow: baseWorkflow,
                   syncRoot: syncRoot)
        {
        }

        #endregion Constructors (1)
    }

    #endregion CLASS: WorkflowWrapper
}