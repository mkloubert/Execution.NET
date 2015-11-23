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
using System.Linq;
using System.Reflection;

namespace MarcelJoachimKloubert.Execution.Workflows
{
    /// <summary>
    /// A basic workflow that uses attributes.
    /// </summary>
    public abstract class AttributeWorkflowBase : WorkflowBase
    {
        #region Constructors (1)

        /// <summary>
        /// Initialites a new instance of the <see cref="AttributeWorkflowBase" /> class.
        /// </summary>
        /// <param name="syncRoot">The value for the <see cref="WorkflowBase.SyncRoot" /> property.</param>
        protected AttributeWorkflowBase(object syncRoot = null)
            : base(syncRoot: syncRoot)
        {
        }

        #endregion Constructors (1)

        #region Properties (2)

        /// <summary>
        /// Gets the contract name.
        /// </summary>
        public virtual string Contract => null;

        /// <summary>
        /// Gets the start object.
        /// </summary>
        public virtual object Object
        {
            get { return this; }
        }

        #endregion Properties (2)

        #region Methods (2)

        /// <inheriteddoc />
        protected override IEnumerable<WorkflowFunc> GetFunctions()
        {
            var contract = WorkflowAttributeBase.ParseContractName(Contract);

            var occuredErrors = new List<Exception>();

            var obj = Object;

            var type = obj.GetType();
            var allMethods = GetMethodsByType(type);

            MethodInfo currentMethod = null;
            {
                // methods with 'WorkflowStartAttribute'
                var methodsWithAttribs = allMethods.Where(m => m.GetCustomAttributes(typeof(WorkflowStartAttribute), true)
                                                                .OfType<WorkflowStartAttribute>()
                                                                .Any(a => a.GetType().Equals(typeof(WorkflowStartAttribute)) && 
                                                                          a.Contract == contract));

                currentMethod = methodsWithAttribs.SingleOrDefault();
            }

            var hasBeenCanceled = false;
            Exception lastError = null;
            object previousValue = null;
            object result = null;
            var throwErrors = true;
            object value = null;
            while (!hasBeenCanceled &&
                   (currentMethod != null))
            {
                yield return (args) =>
                    {
                        var ctx = new WorkflowExecutionContext()
                            {
                                Arguments = args,
                                LastError = lastError,
                                PreviousValue = previousValue,
                                Result = result,
                                ThrowErrors = throwErrors,
                                Value = value,
                            };

                        // first try to find method for next step
                        MethodInfo nextMethod = null;
                        {
                            // search for 'NextWorkflowStepAttribute'
                            var nextStep = currentMethod.GetCustomAttributes(typeof(NextWorkflowStepAttribute), true)
                                                        .OfType<NextWorkflowStepAttribute>()
                                                        .SingleOrDefault(a => a.GetType().Equals(typeof(NextWorkflowStepAttribute)) &&
                                                                              a.Contract == contract);

                            if (nextStep != null)
                            {
                                var allPossibleNextMethods = allMethods.Where(m => m.Name == nextStep.Member)
                                                                       .ToArray();

                                // first: try find with parameter
                                nextMethod = allPossibleNextMethods.SingleOrDefault(m => m.GetParameters().Length > 0);
                                if (nextMethod == null)
                                {
                                    // now find WITHOUT parameter
                                    nextMethod = allPossibleNextMethods.Single(m => m.GetParameters().Length < 1);
                                }
                            }
                        }

                        try
                        {
                            object[] methodParams = null;
                            if (currentMethod.GetParameters().Length > 0)
                            {
                                methodParams = new object[] { ctx };
                            }

                            currentMethod.Invoke(obj, methodParams);

                            lastError = null;
                        }
                        catch (Exception ex)
                        {
                            lastError = ctx.LastError = ex.GetBaseException();

                            occuredErrors.Add(ex);

                            // search for 'OnWorkflowErrorAttribute'
                            var onError = currentMethod.GetCustomAttributes(typeof(OnWorkflowErrorAttribute), true)
                                                       .OfType<OnWorkflowErrorAttribute>()
                                                       .SingleOrDefault(a => a.GetType().Equals(typeof(OnWorkflowErrorAttribute)) &&
                                                                             a.Contract == contract);

                            if (onError != null)
                            {
                                var allPossibleNextMethods = allMethods.Where(m => m.Name == onError.Member)
                                                                       .ToArray();

                                // first: try find with parameter
                                var errorHandlerMethod = allPossibleNextMethods.SingleOrDefault(m => m.GetParameters().Length > 0);
                                if (errorHandlerMethod == null)
                                {
                                    // now find WITHOUT parameter
                                    errorHandlerMethod = allPossibleNextMethods.Single(m => m.GetParameters().Length < 1);
                                }

                                ctx.ContinueOnError = true;

                                object[] methodParams = null;
                                if (errorHandlerMethod.GetParameters().Length > 0)
                                {
                                    methodParams = new object[] { ctx };
                                }

                                errorHandlerMethod.Invoke(obj, methodParams);
                            }

                            if (!ctx.ContinueOnError)
                            {
                                throw;
                            }
                        }

                        var nextAction = ctx.Next;
                        if (nextAction == null)
                        {
                            currentMethod = nextMethod;
                        }
                        else
                        {
                            obj = nextAction.Target;
                            currentMethod = nextAction.GetMethodInfo();
                        }

                        if (currentMethod != null)
                        {
                            type = currentMethod.DeclaringType;  //TODO
                            allMethods = GetMethodsByType(type);
                        }

                        previousValue = ctx.NextValue;
                        throwErrors = ctx.ThrowErrors;
                        value = ctx.Value;

                        if (ctx.Cancel)
                        {
                            hasBeenCanceled = true;
                            ctx.HasBeenCanceled = hasBeenCanceled;
                        }

                        result = ctx.Result;

                        return ctx;
                    };
            }

            if (throwErrors &&
                (occuredErrors.Count > 0))
            {
                throw new AggregateException(occuredErrors);
            }
        }

        private static MethodInfo[] GetMethodsByType(Type type)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                   BindingFlags.Instance | BindingFlags.Static);
        }

        #endregion Methods (2)
    }
}