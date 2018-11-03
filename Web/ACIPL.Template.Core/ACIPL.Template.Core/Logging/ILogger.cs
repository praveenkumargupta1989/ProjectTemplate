using System;

namespace ACIPL.Template.Core.Logging
{
    /// <summary>
    ///     Define methods for Logger functionality.
    /// </summary>
    public interface ILogger
    {
        string Name { get; }

        /// <summary>
        ///     Provide DebugEnable status information for conditional logging.
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        ///     Provide InfoEnable status information for conditional logging.
        /// </summary>
        bool IsInfoEnabled { get; }

        /// <summary>
        ///     Provide WarnEnable status information for conditional logging.
        /// </summary>
        bool IsWarnEnabled { get; }

        /// <summary>
        ///     Provide ErrorEnable status information for conditional logging.
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        ///     Provide FatalEnable status information for conditional logging.
        /// </summary>
        bool IsFatalEnabled { get; }

        /// <summary>
        ///     Log Debug message
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void DebugFormat(string format, params object[] args);

        /// <summary>
        ///     Log Info message
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void InfoFormat(string format, params object[] args);

        /// <summary>
        ///     Log Warn message
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void WarnFormat(string format, params object[] args);

        /// <summary>
        ///     Log Error message
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void ErrorFormat(string format, params object[] args);

        /// <summary>
        ///     Log Fatal message
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void FatalFormat(string format, params object[] args);

        /// <summary>
        ///     Log Debug message
        /// </summary>
        /// <param name="message"></param>
        void Debug(object message);

        /// <summary>
        ///     Log Info message
        /// </summary>
        /// <param name="message"></param>
        void Info(object message);

        /// <summary>
        ///     Log Warn message
        /// </summary>
        /// <param name="message"></param>
        void Warn(object message);

        /// <summary>
        ///     Log Error message
        /// </summary>
        /// <param name="message"></param>
        void Error(object message);

        /// <summary>
        ///     Log Fatal message
        /// </summary>
        /// <param name="message"></param>
        void Fatal(object message);

        /// <summary>
        ///     Log formatted Debug message from exception object.
        /// </summary>
        /// <param name="exception"></param>
        void Debug(Exception exception);

        /// <summary>
        ///     Log formatted Info message from exception object.
        /// </summary>
        /// <param name="exception"></param>
        void Info(Exception exception);

        /// <summary>
        ///     Log formatted Warn message from exception object.
        /// </summary>
        /// <param name="exception"></param>
        void Warn(Exception exception);

        /// <summary>
        ///     Log formatted Error message from exception object.
        /// </summary>
        /// <param name="exception"></param>
        void Error(Exception exception);

        /// <summary>
        ///     Log formatted Fatal message from exception object.
        /// </summary>
        /// <param name="exception"></param>
        void Fatal(Exception exception);
    }
}