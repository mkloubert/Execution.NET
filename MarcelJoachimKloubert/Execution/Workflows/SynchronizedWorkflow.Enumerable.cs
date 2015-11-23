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

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MarcelJoachimKloubert.Execution.Workflows
{
    partial class SynchronizedWorkflow<TWorkflow>
    {
        #region STRUCT: WorkflowEnumerable

        private struct WorkflowEnumerable : IEnumerable<WorkflowFunc>
        {
            #region Fields (1)

            private readonly SynchronizedWorkflow<TWorkflow> _WORKFLOW;

            #endregion Fields (1)

            #region Constructors (1)

            internal WorkflowEnumerable(SynchronizedWorkflow<TWorkflow> workflow)
            {
                _WORKFLOW = workflow;
            }

            #endregion Constructors (1)

            #region Methods (2)

            public IEnumerator<WorkflowFunc> GetEnumerator()
            {
                return new WorkflowEnumerator(_WORKFLOW);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion Methods (2)
        }

        #endregion STRUCT: WorkflowEnumerable

        #region STRUCT: WorkflowEnumerator

        private struct WorkflowEnumerator : IEnumerator<WorkflowFunc>
        {
            #region Fields (2)

            private readonly IEnumerator<WorkflowFunc> _ENUMERATOR;
            private readonly object _SYNC;

            #endregion Fields (2)

            #region Constructors (1)

            internal WorkflowEnumerator(SynchronizedWorkflow<TWorkflow> workflow)
            {
                _ENUMERATOR = workflow.BaseWorkflow.GetEnumerator();
                _SYNC = workflow.SyncRoot;
            }

            #endregion Constructors (1)

            #region Properties (2)

            public WorkflowFunc Current
            {
                get
                {
                    var syncRoot = _SYNC;

                    WorkflowFunc currrentFunc;
                    lock (syncRoot)
                    {
                        currrentFunc = _ENUMERATOR.Current;
                    }

                    return currrentFunc == null ? null : new WorkflowFunc((args) =>
                        {
                            lock (syncRoot)
                            {
                                return currrentFunc(args);
                            }
                        });
                }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            #endregion Properties (2)

            #region Methods (3)

            public void Dispose()
            {
                lock (_SYNC)
                {
                    _ENUMERATOR.Dispose();
                }
            }

            public bool MoveNext()
            {
                lock (_SYNC)
                {
                    return _ENUMERATOR.MoveNext();
                }
            }

            public void Reset()
            {
                lock (_SYNC)
                {
                    _ENUMERATOR.Reset();
                }
            }

            #endregion Methods (3)
        }

        #endregion STRUCT: WorkflowEnumerator
    }
}