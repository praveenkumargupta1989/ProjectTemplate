using log4net;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace ACIPL.Template.Core.Logging
{
    /// <summary>
    ///     Provides Logging functionality configurable through config file.
    /// </summary>
    public class Logger : ILogger
    {
        //Text separator string. This will be used in forming exception message details parsing.
        private const string TextSeparator = "*********************************************";
        private readonly ILog logger;

        /// <summary>
        ///     Initializes new instance of log object
        /// </summary>
        public Logger(ILog log)
        {
            logger = log;
        }

        #region ILogger Members

        public string Name
        {
            get { return logger.Logger.Name; }
        }

        /// <summary>
        ///     Provide DEBUG log enable status for conditional logging.
        /// </summary>
        public bool IsDebugEnabled
        {
            get { return logger.IsDebugEnabled; }
        }

        /// <summary>
        ///     Provide ERROR log enable status for conditional logging.
        /// </summary>
        public bool IsErrorEnabled
        {
            get { return logger.IsErrorEnabled; }
        }

        /// <summary>
        ///     Provide FATAL log enable status for conditional logging.
        /// </summary>
        public bool IsFatalEnabled
        {
            get { return logger.IsFatalEnabled; }
        }

        /// <summary>
        ///     Provide INFO log enable status for conditional logging.
        /// </summary>
        public bool IsInfoEnabled
        {
            get { return logger.IsInfoEnabled; }
        }

        /// <summary>
        ///     Provide WARN log enable status for conditional logging.
        /// </summary>
        public bool IsWarnEnabled
        {
            get { return logger.IsWarnEnabled; }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void DebugFormat(string format, params object[] args)
        {
            PrepareSourceClassDetails();
            logger.DebugFormat(format, args);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void InfoFormat(string format, params object[] args)
        {
            PrepareSourceClassDetails();
            logger.InfoFormat(format, args);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void WarnFormat(string format, params object[] args)
        {
            PrepareSourceClassDetails();
            logger.WarnFormat(format, args);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ErrorFormat(string format, params object[] args)
        {
            PrepareSourceClassDetails();
            logger.ErrorFormat(format, args);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void FatalFormat(string format, params object[] args)
        {
            PrepareSourceClassDetails();
            logger.FatalFormat(format, args);
        }

        /// <summary>
        ///     Log DEBUG message to configured adapters.
        /// </summary>
        /// <param name="message"></param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Debug(object message)
        {
            PrepareSourceClassDetails();
            logger.Debug(message);
        }

        /// <summary>
        ///     Log INFO message to configured adapters.
        /// </summary>
        /// <param name="message"></param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Info(object message)
        {
            PrepareSourceClassDetails();
            logger.Info(message);
        }

        /// <summary>
        ///     Log WARN message to configured adapters.
        /// </summary>
        /// <param name="message"></param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Warn(object message)
        {
            PrepareSourceClassDetails();
            logger.Warn(message);
        }

        /// <summary>
        ///     Log ERROR message to configured adapters.
        /// </summary>
        /// <param name="message"></param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Error(object message)
        {
            PrepareSourceClassDetails();
            logger.Error(message);
        }

        /// <summary>
        ///     Log FATAL message to configured adapters.
        /// </summary>
        /// <param name="message"></param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Fatal(object message)
        {
            PrepareSourceClassDetails();
            logger.Fatal(message);
        }

        /// <summary>
        ///     Log DEBUG message by parsing given exception object to configured adapters.
        /// </summary>
        /// <param name="exception"></param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Debug(Exception exception)
        {
            string exceptionMessage = ParseException(exception);
            PrepareSourceClassDetails();
            logger.Debug(exceptionMessage);
        }

        /// <summary>
        ///     Log INFO message by parsing given exception object to configured adapters.
        /// </summary>
        /// <param name="exception"></param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Info(Exception exception)
        {
            string exceptionMessage = ParseException(exception);
            PrepareSourceClassDetails();
            logger.Info(exceptionMessage);
        }

        /// <summary>
        ///     Log WARN message by parsing given exception object to configured adapters.
        /// </summary>
        /// <param name="exception"></param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Warn(Exception exception)
        {
            string exceptionMessage = ParseException(exception);
            PrepareSourceClassDetails();
            logger.Warn(exceptionMessage);
        }

        /// <summary>
        ///     Log ERROR message by parsing given exception object to configured adapters.
        /// </summary>
        /// <param name="exception"></param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Error(Exception exception)
        {
            string exceptionMessage = ParseException(exception);
            PrepareSourceClassDetails();
            logger.Error(exceptionMessage);
        }

        /// <summary>
        ///     Log FATAL message by parsing given exception object to configured adapters.
        /// </summary>
        /// <param name="exception"></param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Fatal(Exception exception)
        {
            string exceptionMessage = ParseException(exception);
            PrepareSourceClassDetails();
            logger.Fatal(exceptionMessage);
        }

        #endregion

        /// <summary>
        ///     Prepares Source class details to indicate origin of logger method call.
        /// </summary>
        private static void PrepareSourceClassDetails()
        {
            var stack = new StackTrace();
            StackFrame frame = stack.GetFrame(2);
            MethodBase method = frame.GetMethod();

            // This property will be get used in Log4Net config file as part of PatternLayout definition
            //to get source class details from which logger method has been invoked.
            ThreadContext.Properties["SourceClass"] = method.DeclaringType + "." + method.Name;
        }

        /// <summary>
        ///     Parse given exception object to prepare formatted log message.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>string</returns>
        private string ParseException(Exception exception)
        {
            var exceptionInfo = new StringBuilder();

            try
            {
                Exception currentException = exception; // Temp variable to hold InnerException object during the loop.

                // Count variable to track the number of exceptions in the chain.
                int exceptionCount = 1;

                // Loop through each exception class in the chain of exception objects.
                do
                {
                    // Write title information for the exception object.
                    exceptionInfo.AppendFormat("{0}{0}{1}) Exception Information{0}{2}", Environment.NewLine,
                                               exceptionCount, TextSeparator);
                    exceptionInfo.AppendFormat("{0}Exception Type: {1}", Environment.NewLine,
                                               currentException.GetType().FullName);

                    // Loop through the public properties of the exception object and record their value.
                    PropertyInfo[] publicProperties = currentException.GetType().GetProperties();

                    foreach (PropertyInfo propInfo in publicProperties)
                    {
                        // Here InnerException or StackTrace is not capturing. And This information is 
                        // captured later in the process.
                        if (propInfo.Name != "InnerException" && propInfo.Name != "StackTrace")
                        {
                            if (propInfo.GetValue(currentException, null) == null)
                            {
                                exceptionInfo.AppendFormat("{0}{1}: NULL", Environment.NewLine, propInfo.Name);
                            }
                            else
                            {
                                // writing the ToString() value of the property.
                                exceptionInfo.AppendFormat("{0}{1}: {2}", Environment.NewLine, propInfo.Name,
                                                           propInfo.GetValue(currentException, null));
                            }
                        }
                    }

                    // Record the StackTrace with separate label.
                    if (currentException.StackTrace != null)
                    {
                        exceptionInfo.AppendFormat("{0}{0}StackTrace Information{0}{1}", Environment.NewLine, null);
                        exceptionInfo.AppendFormat("{0}{1}", Environment.NewLine, currentException.StackTrace);
                    }

                    // Reset the temp exception object and iterate the counter.
                    currentException = currentException.InnerException;
                    exceptionCount++;
                } while (currentException != null);

                exceptionInfo.AppendFormat("{0}{1}", Environment.NewLine, TextSeparator);
            }
            catch (Exception exobj)
            {
                logger.Fatal(exobj.ToString());
            }

            return exceptionInfo.ToString();
        }
    }
}