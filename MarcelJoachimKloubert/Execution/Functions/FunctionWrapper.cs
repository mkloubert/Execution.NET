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
    #region CLASS: FunctionWrapper<TFunc>

    /// <summary>
    /// Wraps a function.
    /// </summary>
    /// <typeparam name="TFunc">Type of the function to wrap.</typeparam>
    public class FunctionWrapper<TFunc> : FunctionBase
        where TFunc : IFunction
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionWrapper{TFunc}" /> class.
        /// </summary>
        /// <param name="baseFunc">The function to wrap.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseFunc" /> is <see langword="null" />.
        /// </exception>
        public FunctionWrapper(TFunc baseFunc, object syncRoot = null)
            : base(syncRoot: syncRoot)
        {
            if (baseFunc == null)
            {
                throw new ArgumentNullException(nameof(baseFunc));
            }

            BaseFunction = baseFunc;
        }

        #endregion Constructors (1)

        #region Properties (4)

        /// <summary>
        /// Gets the wrapped function.
        /// </summary>
        public TFunc BaseFunction { get; private set; }

        /// <inheriteddoc />
        public sealed override Guid Id
        {
            get { return BaseFunction.Id; }
        }

        /// <inheriteddoc />
        public sealed override string Name
        {
            get { return BaseFunction.Name; }
        }

        /// <inheriteddoc />
        public sealed override string Namespace
        {
            get { return base.Namespace; }
        }

        #endregion Properties (4)

        #region Methods (4)

        /// <inheriteddoc />
        public sealed override bool Equals(object obj)
        {
            return BaseFunction.Equals(obj);
        }

        /// <inheriteddoc />
        public override int GetHashCode()
        {
            return BaseFunction.GetHashCode();
        }

        /// <inheriteddoc />
        protected override void OnExecute(IDictionary<string, object> input, IDictionary<string, object> output)
        {
            var result = BaseFunction.Execute(input);
            if (result == null)
            {
                return;
            }

            using (var e = result.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    output.Add(e.Current);
                }
            }
        }

        /// <inheriteddoc />
        public override string ToString()
        {
            return BaseFunction.ToString();
        }

        #endregion Methods (4)
    }

    #endregion CLASS: FunctionWrapper<TFunc>

    #region CLASS: FunctionWrapper

    /// <summary>
    /// Wraps a function.
    /// </summary>
    public class FunctionWrapper : FunctionWrapper<IFunction>
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionWrapper" /> class.
        /// </summary>
        /// <param name="baseFunc">The function to wrap.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseFunc" /> is <see langword="null" />.
        /// </exception>
        public FunctionWrapper(IFunction baseFunc, object syncRoot = null)
            : base(baseFunc: baseFunc,
                   syncRoot: syncRoot)
        {
        }

        #endregion Constructors (1)
    }

    #endregion CLASS: FunctionWrapper
}