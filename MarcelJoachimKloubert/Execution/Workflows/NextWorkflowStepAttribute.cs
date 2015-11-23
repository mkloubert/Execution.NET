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
    /// Marks a method as start point for a workflow.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method,
                    AllowMultiple = true,
                    Inherited = false)]
    public class NextWorkflowStepAttribute : WorkflowAttributeBase
    {
        #region Constructors (3)

        /// <summary>
        /// Initializes a new instance of the <see cref="NextWorkflowStepAttribute" /> class.
        /// </summary>
        /// <param name="member">The value for the <see cref="NextWorkflowStepAttribute.Member" /> property.</param>
        /// <param name="contract">The value for the <see cref="WorkflowAttributeBase.Contract" /> property.</param>
        public NextWorkflowStepAttribute(string member, Type contract)
            : this(member: member,
                   contractName: GetContractName(contract))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NextWorkflowStepAttribute" /> class.
        /// </summary>
        /// <param name="member">The value for the <see cref="NextWorkflowStepAttribute.Member" /> property.</param>
        /// <param name="contractName">The value for the <see cref="WorkflowAttributeBase.Contract" /> property.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="member" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="member" /> is <see langword="null" />.
        /// </exception>
        public NextWorkflowStepAttribute(string member, string contractName)
            : base(contractName: contractName)
        {
            if (member == null)
            {
                throw new ArgumentNullException("member");
            }

            Member = member.Trim();
            if (Member == string.Empty)
            {
                throw new ArgumentException("member");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NextWorkflowStepAttribute" /> class.
        /// </summary>
        /// <param name="member">The value for the <see cref="NextWorkflowStepAttribute.Member" /> property.</param>
        public NextWorkflowStepAttribute(string member)
            : this(member: member,
                   contractName: null)
        {
        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gets or sets the name of the target member.
        /// </summary>
        public string Member { get; set; }

        #endregion
    }
}