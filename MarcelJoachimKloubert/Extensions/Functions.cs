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

using MarcelJoachimKloubert.Execution.Functions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarcelJoachimKloubert.Extensions
{
    /// <summary>
    /// Function extension methods.
    /// </summary>
    public static class MJKFunctionExtensionMethods
    {
        #region Methods (1)

        /// <summary>
        /// Async execution of an <see cref="IFunction" />.
        /// </summary>
        /// <param name="func">The function to execute.</param>
        /// <param name="params">The list of parameters for the execution.</param>
        /// <returns>The running task.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="func" /> is <see langword="null" />.
        /// </exception>
        public static Task<IDictionary<string, object>> ExecuteAsync(this IFunction func, IEnumerable<KeyValuePair<string, object>> @params = null)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            return Task.Factory.StartNew(function: (state) =>
                {
                    var taskArgs = (object[])state;

                    return ((IFunction)taskArgs[0]).Execute(@params: (IEnumerable<KeyValuePair<string, object>>)taskArgs[1]);
                }, state: new object[] { func, @params });
        }

        /// <summary>
        /// Returns a thread safe version of a function.
        /// </summary>
        /// <param name="func">The function to wrap.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <returns>
        /// The function wrapper or <see langword="null" /> if <paramref name="func" />
        /// is also <see langword="null" />.
        /// </returns>
        public static IFunction Synchronize(this IFunction func, object syncRoot = null)
        {
            return func != null ? new SynchronizedFunction(baseFunc: func, syncRoot: syncRoot)
                                : null;
        }

        #endregion Methods (1)
    }
}