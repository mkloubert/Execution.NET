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

namespace MarcelJoachimKloubert.Execution.Workflows
{
    /// <summary>
    /// A basic workflow attribute.
    /// </summary>
    public abstract class WorkflowAttributeBase : Attribute
    {
        #region Constructors (3)

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowAttributeBase"/> class.
        /// </summary>
        /// <param name="contract">The value for the <see cref="WorkflowAttributeBase.Contract" /> property.</param>
        protected WorkflowAttributeBase(Type contract)
            : this(contractName: GetContractName(contract))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowAttributeBase"/> class.
        /// </summary>
        /// <param name="contractName">The value for the <see cref="WorkflowAttributeBase.Contract" /> property.</param>
        protected WorkflowAttributeBase(string contractName)
        {
            Contract = ParseContractName(contractName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NextWorkflowStepAttribute"/> class.
        /// </summary>
        protected WorkflowAttributeBase()
            : this(contractName: null)
        {
        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gets or sets the contract.
        /// </summary>
        public string Contract { get; set; }

        #endregion Properties (1)

        #region Methods (2)

        /// <summary>
        /// Returns the contract name from a <see cref="Type" /> object.
        /// </summary>
        /// <param name="type">The type from where to get the contract name from.</param>
        /// <returns>
        /// The contract name or <see langword="null" /> is <paramref name="type" /> is also <see langword="null" />.
        /// </returns>
        public static string GetContractName(Type type)
        {
            return type != null ? string.Format("{0}{1}{2}", type.AssemblyQualifiedName
                                                           , "\n"
                                                           , type.FullName)
                                : null;
        }

        /// <summary>
        /// Parses a contract name.
        /// </summary>
        /// <param name="contract">The input value.</param>
        /// <returns>The parsed value.</returns>
        public static string ParseContractName(string contract)
        {
            contract = (contract ?? string.Empty).ToUpper().Trim();
            if (contract == string.Empty)
            {
                contract = null;
            }

            return contract;
        }

        #endregion Methods (2)
    }
}