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
using MarcelJoachimKloubert.Execution.Commands;
using System.Threading.Tasks;

namespace MarcelJoachimKloubert.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ICommand" />s.
    /// </summary>
    public static class MJKCommandExtensionMethods
    {
        #region Methods (2)

        /// <summary>
        /// Executes a command async.
        /// </summary>
        /// <param name="cmd">The command to execute.</param>
        /// <param name="parameter">The optional parameter to submit.</param>
        /// <returns>The running task.</returns>
        public static async Task ExecuteAsync(this ICommand cmd, object parameter = null)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            await Task.Factory.StartNew(action: (state) =>
                {
                    var taskArgs = (object[])state;

                    ((ICommand)taskArgs[0]).Execute(taskArgs[1]);
                }, state: new object[] { cmd, parameter });
        }

        /// <summary>
        /// Returns a thread safe version of a command.
        /// </summary>
        /// <param name="cmd">The command to wrap.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <returns>
        /// The function wrapper or <see langword="null" /> if <paramref name="cmd" />
        /// is also <see langword="null" />.
        /// </returns>
        public static ICommand Synchronize(this ICommand cmd, object syncRoot = null)
        {
            return cmd != null ? new SynchronizedCommand(cmd, syncRoot: syncRoot)
                               : null;
        }

        #endregion Methods (2)
    }
}