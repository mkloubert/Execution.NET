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
    #region CLASS: DelegateCommand<TParam>

    /// <summary>
    /// A command based on delegates.
    /// </summary>
    /// <typeparam name="TParam">Type of the parameter.</typeparam>
    public class DelegateCommand<TParam> : CommandBase<TParam>
    {
        #region Fields (2)

        private readonly CanExecutePredicate _CAN_EXECUTE;
        private readonly ExecuteAction _EXECUTE;

        #endregion Fields (2)

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{TParam}" /> class.
        /// </summary>
        /// <param name="executeAction">The execute action.</param>
        /// <param name="canExecutePredicate">The custom 'can execute' predicate to use.</param>
        public DelegateCommand(ExecuteAction executeAction,
                               CanExecutePredicate canExecutePredicate = null)
        {
            if (executeAction == null)
            {
                throw new ArgumentNullException("executeAction");
            }

            _EXECUTE = executeAction;
            _CAN_EXECUTE = canExecutePredicate ?? ((cmd, p) => true);
        }

        #endregion Constructors (1)

        #region Delegates (2)

        /// <summary>
        /// Describes a predicate for a <see cref="DelegateCommand{TParam}.CanExecute(TParam)" /> method.
        /// </summary>
        /// <param name="cmd">The parent command.</param>
        /// <param name="param">The parameter to check.</param>
        /// <returns>Command can be executed or not.</returns>
        public delegate bool CanExecutePredicate(DelegateCommand<TParam> cmd, TParam param);

        /// <summary>
        /// Describes an action for a <see cref="CommandBase{TParam}.Execute(TParam)" /> method.
        /// </summary>
        /// <param name="cmd">The parent command.</param>
        /// <param name="param">The parameter to check.</param>
        /// <returns>Command can be executed or not.</returns>
        public delegate void ExecuteAction(DelegateCommand<TParam> cmd, TParam param = default(TParam));

        #endregion Delegates (2)

        #region Methods (6)

        /// <inheriteddoc />
        public sealed override bool CanExecute(TParam parameter)
        {
            return _CAN_EXECUTE(this, parameter);
        }

        /// <inheriteddoc />
        protected sealed override void OnExecute(TParam parameter)
        {
            _EXECUTE(this, parameter);
        }

        /// <summary>
        /// Converts an <see cref="Func{T}" /> delegate to an <see cref="CanExecutePredicate" /> instance.
        /// </summary>
        /// <param name="func">The input value.</param>
        /// <returns>The output value.</returns>
        /// <remarks>
        /// Returns <see langword="null" /> if <paramref name="func" /> is also <see langword="null" />.
        /// </remarks>
        public static CanExecutePredicate ToCanExecutePredicate(Func<bool> func)
        {
            if (func == null)
            {
                return null;
            }

            return (cmd, p) => func();
        }

        /// <summary>
        /// Converts an <see cref="Func{T}" /> delegate to an <see cref="CanExecutePredicate" /> instance.
        /// </summary>
        /// <param name="func">The input value.</param>
        /// <returns>The output value.</returns>
        /// <remarks>
        /// Returns <see langword="null" /> if <paramref name="func" /> is also <see langword="null" />.
        /// </remarks>
        public static CanExecutePredicate ToCanExecutePredicate(Func<TParam, bool> func)
        {
            if (func == null)
            {
                return null;
            }

            return (cmd, p) => func(p);
        }

        /// <summary>
        /// Converts an <see cref="Action" /> delegate to an <see cref="ExecuteAction" /> instance.
        /// </summary>
        /// <param name="action">The input value.</param>
        /// <returns>The output value.</returns>
        /// <remarks>
        /// Returns <see langword="null" /> if <paramref name="action" /> is also <see langword="null" />.
        /// </remarks>
        public static ExecuteAction ToExecuteAction(Action action)
        {
            if (action == null)
            {
                return null;
            }

            return (cmd, p) => action();
        }

        /// <summary>
        /// Converts an <see cref="Action{T}" /> delegate to an <see cref="ExecuteAction" /> instance.
        /// </summary>
        /// <param name="action">The input value.</param>
        /// <returns>The output value.</returns>
        /// <remarks>
        /// Returns <see langword="null" /> if <paramref name="action" /> is also <see langword="null" />.
        /// </remarks>
        public static ExecuteAction ToExecuteAction(Action<TParam> action)
        {
            if (action == null)
            {
                return null;
            }

            return (cmd, p) => action(p);
        }

        #endregion Methods (6)
    }

    #endregion CLASS: DelegateCommand<TParam>

    #region CLASS: DelegateCommand

    /// <summary>
    /// A command based on delegates.
    /// </summary>
    public class DelegateCommand : DelegateCommand<object>
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{TParam}" /> class.
        /// </summary>
        /// <param name="executeAction">The execute action.</param>
        /// <param name="canExecutePredicate">The custom 'can execute' predicate to use.</param>
        public DelegateCommand(ExecuteAction executeAction,
                               CanExecutePredicate canExecutePredicate = null)
            : base(executeAction: executeAction,
                    canExecutePredicate: canExecutePredicate)
        {
        }

        #endregion Constructors (1)
    }

    #endregion CLASS: DelegateCommand
}