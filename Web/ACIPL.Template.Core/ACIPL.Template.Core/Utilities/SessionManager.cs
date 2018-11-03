using ACIPL.Template.Core.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.SessionState;

namespace ACIPL.Template.Core.Utilities
{
    public static class SessionManager
    {
        public static object Get(string key)
        {
            ILogger Logger = LoggerFactory.GetLogger();
            var sessionId = key.Split('~')[0].ToString();
            var token = key.Split('~')[1].ToString();
            var clientSessionId = HttpContext.Current.Session.SessionID;

            if (clientSessionId != sessionId)
            {
                Logger.Error("Session Id is not valid clientSessionId=" + clientSessionId + " SessionId=" + sessionId);
            }

            object obj = null;
            if (HttpContext.Current.Session[token] != null)
            {
                obj = HttpContext.Current.Session[token];
            }
            else
            {
                var sessionData = GetSessionById(sessionId);
                if (sessionData != null)
                {
                    obj = sessionData.Items[token];
                }
                else
                {
                    Logger.Error("No valid session with valid key");
                }
            }
            return obj;
        }

        public static string Save(object data)
        {
            var token = Guid.NewGuid().ToString().Replace("-", "");
            HttpContext.Current.Session[token] = data;
            token = string.Concat(HttpContext.Current.Session.SessionID, "~", token);
            return token;
        }

        private static SessionStateStoreData GetSessionById(string sessionId)
        {
            HttpApplication httpApplication = HttpContext.Current.ApplicationInstance;

            // Black magic #1: getting to SessionStateModule
            HttpModuleCollection httpModuleCollection = httpApplication.Modules;
            SessionStateModule sessionHttpModule = httpModuleCollection["Session"] as SessionStateModule;
            if (sessionHttpModule == null)
            {
                // Couldn't find Session module
                return null;
            }

            // Black magic #2: getting to SessionStateStoreProviderBase through reflection
            FieldInfo fieldInfo = typeof(SessionStateModule).GetField("_store", BindingFlags.NonPublic | BindingFlags.Instance);
            SessionStateStoreProviderBase sessionStateStoreProviderBase = fieldInfo.GetValue(sessionHttpModule) as SessionStateStoreProviderBase;
            if (sessionStateStoreProviderBase == null)
            {
                // Couldn't find sessionStateStoreProviderBase
                return null;
            }

            // Black magic #3: generating dummy HttpContext out of the thin air. sessionStateStoreProviderBase.GetItem in #4 needs it.
            SimpleWorkerRequest request = new SimpleWorkerRequest("dummy.html", null, new StringWriter());
            HttpContext context = new HttpContext(request);

            // Black magic #4: using sessionStateStoreProviderBase.GetItem to fetch the data from session with given Id.
            bool locked;
            TimeSpan lockAge;
            object lockId;
            SessionStateActions actions;
            SessionStateStoreData sessionStateStoreData = sessionStateStoreProviderBase.GetItem(
                context, sessionId, out locked, out lockAge, out lockId, out actions);
            return sessionStateStoreData;
        }
    }
}