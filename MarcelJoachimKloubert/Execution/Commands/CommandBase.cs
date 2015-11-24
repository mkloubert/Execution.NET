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
    /// <summary>
    /// A basic command.
    /// </summary>
    /// <typeparam name="TParam">Type of the parameter.</typeparam>
    public abstract class CommandBase<TParam> : ICommand
    {
        #region Fields (1)

        private readonly object _SYNC_ROOT;

        #endregion Fields (1)

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase{TParam}" /> class.
        /// </summary>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        protected CommandBase(object syncRoot = null)
        {
            _SYNC_ROOT = syncRoot ?? new object();
        }

        #endregion Constructors (1)

        #region Events (1)

        /// <inheriteddoc />
        public event EventHandler CanExecuteChanged;

        #endregion Events (1)

        #region Properties (2)

        /// <summary>
        /// Gets the object for thread safe operations.
        /// </summary>
        public virtual object SyncRoot
        {
            get { return _SYNC_ROOT; }
        }

        /// <summary>
        /// Gets or sets an object that should be linked with that instance.
        /// </summary>
        public virtual object Tag { get; set; }

        #endregion Properties (2)

        #region Methods (7)

        /// <inheriteddoc />
        public abstract bool CanExecute(TParam parameter);

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(ConvertParam(parameter));
        }

        /// <summary>
        /// Converts an object to the underlying parameter type.
        /// </summary>
        /// <param name="parameter">The value to convert.</param>
        /// <returns></returns>
        protected virtual TParam ConvertParam(object parameter)
        {
            if (null == parameter)
            {
                if (typeof(TParam).IsValueType)
                {
                    return default(TParam);
                }
            }

            return (TParam)parameter;
        }

        /// <summary>
        /// <see cref="ICommand.Execute(object)" />
        /// </summary>
        public virtual void Execute(TParam parameter = default(TParam))
        {
            if (!CanExecute(parameter))
            {
                return;
            }

            OnExecute(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute(ConvertParam(parameter));
        }

        /// <summary>
        /// The logic for the <see cref="CommandBase{TParam}.Execute(TParam)" /> method.
        /// </summary>
        /// <param name="parameter">The parameter to handle.</param>
        protected abstract void OnExecute(TParam parameter);

        /// <summary>
        /// Raises the <see cref="CommandBase{TParam}.CanExecuteChanged" /> event.
        /// </summary>
        /// <returns>Event was raised or not.</returns>
        public bool RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
                return true;
            }

            return false;
        }

        #endregion Methods (7)
    }
}