using log4net;
using log4net.Config;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ACIPL.Template.Core.Logging
{
    /// <summary>
    ///     Provides instance of a Logger class with respective dependencies.
    /// </summary>
    public static class LoggerFactory
    {
        static LoggerFactory()
        {
            string[] commandLine = Environment.GetCommandLineArgs();
            var commandLineArgs = new CommandLineArguments(commandLine);
            Assembly entryAssembly = Assembly.GetEntryAssembly();

            string defaultLogFileName = entryAssembly == null
                                            ? string.Empty
                                            : entryAssembly.GetName().Name + ".log";

            string logFileName = commandLineArgs["LogFile"] ?? defaultLogFileName;

            GlobalContext.Properties["logFile"] = logFileName;
            XmlConfigurator.Configure();
        }

        /// <summary>
        ///     Returns a new instance of the Logger class.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Logger GetLogger()
        {
            //Getting Caller class type by using stack trace details.
            //This type information will be used in getting logger component from log4net.
            var frame = new StackFrame(1, false);
            MethodBase method = frame.GetMethod();
            Type declaringType = method.DeclaringType;
            return new Logger(LogManager.GetLogger(declaringType));
        }

        /// <summary>
        ///     Returns a new instance of the Logger class.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Logger GetLogger(string name)
        {
            return new Logger(LogManager.GetLogger(name));
        }
    }
}