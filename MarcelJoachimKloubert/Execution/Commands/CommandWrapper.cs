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
    #region CLASS: CommandWrapper<TCmd>

    /// <summary>
    /// Wraps a command.
    /// </summary>
    /// <typeparam name="TCmd">Type of the command.</typeparam>
    public class CommandWrapper<TCmd> : CommandBase<object>
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
        public CommandWrapper(TCmd baseCmd, object syncRoot = null)
            : base(syncRoot: syncRoot)
        {
            if (baseCmd == null)
            {
                throw new ArgumentNullException(nameof(baseCmd));
            }

            BaseCommand = baseCmd;
        }

        #endregion Constructors (1)

        #region Properties (1)

        /// <summary>
        /// Gets the wrapped command.
        /// </summary>
        public TCmd BaseCommand { get; }

        #endregion Properties (1)

        #region Methods (5)

        /// <inheriteddoc />
        public override bool CanExecute(object parameter)
        {
            return BaseCommand.CanExecute(parameter);
        }

        /// <inheriteddoc />
        public override bool Equals(object obj)
        {
            return BaseCommand.Equals(obj);
        }

        /// <inheriteddoc />
        public override int GetHashCode()
        {
            return BaseCommand.GetHashCode();
        }

        /// <inheriteddoc />
        protected sealed override void OnExecute(object parameter)
        {
            BaseCommand.Execute(parameter);
        }

        /// <inheriteddoc />
        public override string ToString()
        {
            return BaseCommand.ToString();
        }

        #endregion Methods (5)
    }

    #endregion CLASS: CommandWrapper<TCmd>

    #region CLASS: CommandWrapper

    /// <summary>
    /// Wraps a command.
    /// </summary>
    public class CommandWrapper : CommandWrapper<ICommand>
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandWrapper" /> class.
        /// </summary>
        /// <param name="baseCmd">The command to wrap.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseCmd" /> is <see langword="null" />.
        /// </exception>
        public CommandWrapper(ICommand baseCmd, object syncRoot = null)
            : base(baseCmd: baseCmd,
                   syncRoot: syncRoot)
        {
        }

        #endregion Constructors (1)
    }

    #endregion CLASS: CommandWrapper
}