using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Prism.IocContainer.Wpf.Tests.Support.WPFHelpers
{
    /// <summary>
    /// Wraps test cases for FactAttribute and TheoryAttribute so the test case runs on the WPF STA thread
    /// </summary>
    [DebuggerDisplay (@"\{ class = {TestMethod.TestClass.Class.Name}, method = {TestMethod.Method.Name}, display = {DisplayName}, skip = {SkipReason} \}")]
    public class WpfTestCase : LongLivedMarshalByRefObject, IXunitTestCase
    {
        private IXunitTestCase _testCase;

        public WpfTestCase (IXunitTestCase testCase)
        {
            _testCase = testCase;
        }

        /// <summary/>
        [EditorBrowsable (EditorBrowsableState.Never)]
        [Obsolete ("Called by the de-serializer", error: true)]
        public WpfTestCase () { }

        public IMethodInfo Method => _testCase.Method;

        public Task<RunSummary> RunAsync (IMessageSink diagnosticMessageSink,
                                         IMessageBus messageBus,
                                         object [] constructorArguments,
                                         ExceptionAggregator aggregator,
                                         CancellationTokenSource cancellationTokenSource)
        {
            var tcs = new TaskCompletionSource<RunSummary> ();
            var thread = new Thread (() => {
                try {
                    // Set up the SynchronizationContext so that any awaits
                    // resume on the STA thread as they would in a GUI app.
                    SynchronizationContext.SetSynchronizationContext (new DispatcherSynchronizationContext ());

                    // Start off the test method.
                    Task<RunSummary> testCaseTask = _testCase.RunAsync (diagnosticMessageSink, messageBus, constructorArguments, aggregator, cancellationTokenSource);

                    // Arrange to pump messages to execute any async work associated with the test.
                    var frame = new DispatcherFrame ();
                    Task.Run (async delegate {
                        try {
                            await testCaseTask;
                        } finally {
                            // The test case's execution is done. Terminate the message pump.
                            frame.Continue = false;
                        }
                    });
                    Dispatcher.PushFrame (frame);

                    // Report the result back to the Task we returned earlier.
                    CopyTaskResultFrom (tcs, testCaseTask);
                } catch (Exception e) {
                    tcs.SetException (e);
                }
            });

            thread.SetApartmentState (ApartmentState.STA);
            thread.Start ();
            return tcs.Task;
        }

        public string DisplayName {
            get { return _testCase.DisplayName; }
        }

        public string SkipReason {
            get { return _testCase.SkipReason; }
        }

        public ISourceInformation SourceInformation {
            get { return _testCase.SourceInformation; }
            set { _testCase.SourceInformation = value; }
        }

        public ITestMethod TestMethod {
            get { return _testCase.TestMethod; }
        }

        public object [] TestMethodArguments {
            get { return _testCase.TestMethodArguments; }
        }

        public Dictionary<string, List<string>> Traits {
            get { return _testCase.Traits; }
        }

        public string UniqueID {
            get { return _testCase.UniqueID; }
        }

        public void Deserialize (IXunitSerializationInfo info)
        {
            _testCase = info.GetValue<IXunitTestCase> ("InnerTestCase");
        }

        public void Serialize (IXunitSerializationInfo info)
        {
            info.AddValue ("InnerTestCase", _testCase);
        }

        private static void CopyTaskResultFrom<T> (TaskCompletionSource<T> tcs, Task<T> template)
        {
            if (tcs == null)
            {
                throw new ArgumentNullException (nameof (tcs));
            }
            if (template == null)
            {
                throw new ArgumentNullException (nameof (template));
            }
            if (!template.IsCompleted)
            {
                throw new ArgumentException ("Task must be completed first.", nameof (template));
            }

            if (template.IsFaulted)
            {
                if (template.Exception != null)
                {
                    tcs.SetException (template.Exception);
                }
            }
            else if (template.IsCanceled)
            {
                tcs.SetCanceled ();
            }
            else
            {
                tcs.SetResult (template.Result);
            }
        }
    }
}
