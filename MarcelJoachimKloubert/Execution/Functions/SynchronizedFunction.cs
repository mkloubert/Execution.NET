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

namespace MarcelJoachimKloubert.Execution.Functions
{
    #region CLASS: SynchronizedFunction<TFunc>

    /// <summary>
    /// A thread safe function.
    /// </summary>
    /// <typeparam name="TFunc">Type of the function to wrap.</typeparam>
    public class SynchronizedFunction<TFunc> : FunctionWrapper<TFunc>
        where TFunc : IFunction
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedFunction{TFunc}" /> class.
        /// </summary>
        /// <param name="baseFunc">The function to wrap.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseFunc" /> is <see langword="null" />.
        /// </exception>
        public SynchronizedFunction(TFunc baseFunc, object syncRoot = null)
            : base(baseFunc: baseFunc,
                   syncRoot: syncRoot)
        {
        }

        #endregion Constructors (1)

        #region Methods (1)

        /// <inheriteddoc />
        protected sealed override void OnExecute(IDictionary<string, object> input, IDictionary<string, object> output)
        {
            lock (SyncRoot)
            {
                base.OnExecute(input, output);
            }
        }

        #endregion Methods (1)
    }

    #endregion CLASS: SynchronizedFunction<TFunc>

    #region CLASS: SynchronizedFunction

    /// <summary>
    /// A thread safe function.
    /// </summary>
    public class SynchronizedFunction : SynchronizedFunction<IFunction>
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedFunction" /> class.
        /// </summary>
        /// <param name="baseFunc">The function to wrap.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseFunc" /> is <see langword="null" />.
        /// </exception>
        public SynchronizedFunction(IFunction baseFunc, object syncRoot = null)
            : base(baseFunc: baseFunc,
                   syncRoot: syncRoot)
        {
        }

        #endregion Constructors (1)
    }

    #endregion CLASS: SynchronizedFunction
}