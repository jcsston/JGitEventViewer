using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGitEventViewer
{
    /**
     * Logging class based on Sample https://code.msdn.microsoft.com/windowsapps/Logging-Sample-for-Windows-0b9dffd7#content
     */
    sealed class JEventSource : EventSource
    {
        public static JEventSource Log = new JEventSource();

        [Event(1, Level = EventLevel.Verbose)]
        public void Debug(string message)
        {
            this.WriteEvent(1, message);
        }

        [Event(2, Level = EventLevel.Informational)]
        public void Info(string message)
        {
            this.WriteEvent(2, message);
        }

        [Event(3, Level = EventLevel.Warning)]
        public void Warn(string message)
        {
            this.WriteEvent(3, message);
        }

        [Event(4, Level = EventLevel.Error)]
        public void Error(string message)
        {
            this.WriteEvent(4, message);
        }

        [Event(5, Level = EventLevel.Critical)]
        public void Critical(string message)
        {
            this.WriteEvent(5, message);
        }
    }
}
