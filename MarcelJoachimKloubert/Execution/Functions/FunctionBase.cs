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
    /// <summary>
    /// A basic dedicated function.
    /// </summary>
    public abstract class FunctionBase : IFunction
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionBase" /> class.
        /// </summary>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        protected FunctionBase(object syncRoot = null)
        {
            SyncRoot = syncRoot ?? new object();
        }

        #endregion Constructors (1)

        #region Properties (6)

        /// <inheriteddoc />
        public string Fullname
        {
            get
            {
                var ns = Namespace;
                ns = string.IsNullOrWhiteSpace(ns) ? null : ns.Trim();

                var name = Name;
                name = string.IsNullOrWhiteSpace(name) ? null : name.Trim();

                return string.Format("{0}{1}{2}",
                                     ns,
                                     ns != null ? "." : null,
                                     name);
            }
        }

        /// <inheriteddoc />
        public abstract Guid Id { get; }

        /// <inheriteddoc />
        public virtual string Name
        {
            get { return GetType().Name; }
        }

        /// <inheriteddoc />
        public virtual string Namespace
        {
            get { return GetType().Namespace; }
        }

        /// <summary>
        /// Gets the object for thread safe operations.
        /// </summary>
        public virtual object SyncRoot { get; }

        /// <summary>
        /// Gets or sets an object that should be linked with that instance.
        /// </summary>
        public virtual object Tag { get; set; }

        #endregion Properties (6)

        #region Methods (9)

        /// <summary>
        /// Creates an empty storage for input parameters.
        /// </summary>
        /// <returns>The created, empty dictionary.</returns>
        protected virtual IDictionary<string, object> CreateEmptyInputParamDictionary()
        {
            return null;
        }

        /// <summary>
        /// Creates an empty storage for output parameters.
        /// </summary>
        /// <returns>The created, empty dictionary.</returns>
        protected virtual IDictionary<string, object> CreateEmptyOutputParamDictionary()
        {
            return CreateEmptyInputParamDictionary();
        }

        /// <inheriteddoc />
        public override bool Equals(object obj)
        {
            var other = obj as IFunction;

            return other != null ? Equals(other: other)
                                 : base.Equals(obj);
        }

        /// <inheriteddoc />
        public bool Equals(IFunction other)
        {
            return other != null &&
                   other.Id == Id;
        }

        /// <inheriteddoc />
        public IDictionary<string, object> Execute(IEnumerable<KeyValuePair<string, object>> @params = null)
        {
            var input = ToInputParamDictionary(@params) ?? new Dictionary<string, object>();

            var result = CreateEmptyOutputParamDictionary() ?? new Dictionary<string, object>();
            OnExecute(input, result);

            return ToOutputParamDictionary(result) ?? new Dictionary<string, object>();
        }

        /// <inheriteddoc />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// The logic for the <see cref="FunctionBase.Execute(IEnumerable{KeyValuePair{string, object}})" /> method.
        /// </summary>
        /// <param name="input">The input parameters.</param>
        /// <param name="output">The dictionary where to save the result / output parameters.</param>
        protected abstract void OnExecute(IDictionary<string, object> input, IDictionary<string, object> output);

        /// <summary>
        /// Returns a list of key/value pairs as dictionary for use as input parameters.
        /// </summary>
        /// <param name="params">The input sequence.</param>
        /// <returns>The sequence as dictionary.</returns>
        protected virtual IDictionary<string, object> ToInputParamDictionary(IEnumerable<KeyValuePair<string, object>> @params)
        {
            return ToParamDictionary(@params);
        }

        /// <summary>
        /// Returns a list of key/value pairs as dictionary for use as output / result parameters.
        /// </summary>
        /// <param name="params">The input sequence.</param>
        /// <returns>The sequence as dictionary.</returns>
        protected virtual IDictionary<string, object> ToOutputParamDictionary(IEnumerable<KeyValuePair<string, object>> @params)
        {
            return ToParamDictionary(@params);
        }

        /// <summary>
        /// Returns a list of key/value pairs as dictionary.
        /// </summary>
        /// <param name="params">The input sequence.</param>
        /// <returns>The sequence as dictionary.</returns>
        protected virtual IDictionary<string, object> ToParamDictionary(IEnumerable<KeyValuePair<string, object>> @params)
        {
            if (@params == null)
            {
                return null;
            }

            var result = @params as IDictionary<string, object>;
            if (result == null)
            {
                result = new Dictionary<string, object>();
                using (var e = @params.GetEnumerator())
                {
                    while (e.MoveNext())
                    {
                        result.Add(e.Current);
                    }
                }
            }

            return result;
        }

        #endregion Methods (9)
    }
}