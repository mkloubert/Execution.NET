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

namespace MarcelJoachimKloubert.Execution.Commands
{
    #region CLASS: SynchromizedCommand<TCmd>

    /// <summary>
    /// A thread safe command.
    /// </summary>
    /// <typeparam name="TCmd">Type of the command.</typeparam>
    public class SynchromizedCommand<TCmd> : CommandWrapper<TCmd>
        where TCmd : ICommand
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandWrapper{TCmd}" /> class.
        /// </summary>
        /// <param name="baseCmd">The command to wrap.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseCmd" /> is <see langword="null" />.
        /// </exception>
        public SynchromizedCommand(TCmd baseCmd, object syncRoot = null)
            : base(baseCmd: baseCmd,
                   syncRoot: syncRoot)
        {
        }

        #endregion Constructors (1)

        #region Methods (2)

        /// <inheriteddoc />
        public sealed override bool CanExecute(object parameter)
        {
            lock (SyncRoot)
            {
                return base.CanExecute(parameter);
            }
        }

        /// <inheriteddoc />
        public sealed override void Execute(object parameter = null)
        {
            lock (SyncRoot)
            {
                base.Execute(parameter);
            }
        }

        #endregion Methods (2)
    }

    #endregion CLASS: SynchromizedCommand<TCmd>

    #region CLASS: SynchromizedCommand

    /// <summary>
    /// A thread safe command.
    /// </summary>
    public class SynchronizedCommand : SynchromizedCommand<ICommand>
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedCommand" /> class.
        /// </summary>
        /// <param name="baseCmd">The command to wrap.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseCmd" /> is <see langword="null" />.
        /// </exception>
        public SynchronizedCommand(ICommand baseCmd, object syncRoot = null)
            : base(baseCmd: baseCmd,
                   syncRoot: syncRoot)
        {
        }

        #endregion Constructors (1)
    }

    #endregion CLASS: SynchromizedCommand
}