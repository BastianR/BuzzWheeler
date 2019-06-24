using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Support.Logging
{
    public static class Logger
    {
        static Logger()
        {
            EnableStdOutLogging();
        }


        #region info

        public static void Info(string message)
        {
            string assemblyName = Assembly.GetCallingAssembly().GetName().Name;
            NLog.Logger logger = LogManager.GetLogger(assemblyName);
            logger.Info(message);
        }

        #endregion



        #region debug

        public static void Debug(string message)
        {
            string assemblyName = Assembly.GetCallingAssembly().GetName().Name;
            NLog.Logger logger = LogManager.GetLogger(assemblyName);
            logger.Debug(message);
        }

        public static void Debug(string message, Exception exception)
        {
            string assemblyName = Assembly.GetCallingAssembly().GetName().Name;
            NLog.Logger logger = LogManager.GetLogger(assemblyName);
            logger.Debug(exception, message);
        }

        #endregion



        #region warning

        public static void Warn(string message)
        {
            string assemblyName = Assembly.GetCallingAssembly().GetName().Name;
            NLog.Logger logger = LogManager.GetLogger(assemblyName);
            logger.Warn(message);
        }

        public static void Warn(string message, Exception exception)
        {
            string assemblyName = Assembly.GetCallingAssembly().GetName().Name;
            NLog.Logger logger = LogManager.GetLogger(assemblyName);
            logger.Warn(exception, message);
        }

        public static void Warn(Exception exception, string message, params object[] arguments)
        {
            string assemblyName = Assembly.GetCallingAssembly().GetName().Name;
            NLog.Logger logger = LogManager.GetLogger(assemblyName);
            logger.Warn(exception, message, arguments);
        }

        #endregion



        #region error

        public static void Error(string message)
        {
            string assemblyName = Assembly.GetCallingAssembly().GetName().Name;
            NLog.Logger logger = LogManager.GetLogger(assemblyName);
            logger.Error(message);
        }

        public static void Error(string message, Exception exception)
        {
            string assemblyName = Assembly.GetCallingAssembly().GetName().Name;
            NLog.Logger logger = LogManager.GetLogger(assemblyName);
            logger.Error(exception, message);
        }

        public static void Error(Exception exception, string message, params object[] arguments)
        {
            string assemblyName = Assembly.GetCallingAssembly().GetName().Name;
            NLog.Logger logger = LogManager.GetLogger(assemblyName);
            logger.Error(exception, message, arguments);
        }

        #endregion



        #region private methods

        /// <summary>
        /// Called only in Debug configuration: resets all logging rules final property to false thus enabling the rule which
        /// writes to Visual Studio's output window.
        /// </summary>
        [Conditional("DEBUG")]
        private static void EnableStdOutLogging()
        {
            if (LogManager.Configuration == null)
            {
                return;
            }

            IList<LoggingRule> rules = LogManager.Configuration.LoggingRules;

            foreach (LoggingRule rule in rules)
            {
                rule.Final = false;
            }
        }

        #endregion
    }
}
