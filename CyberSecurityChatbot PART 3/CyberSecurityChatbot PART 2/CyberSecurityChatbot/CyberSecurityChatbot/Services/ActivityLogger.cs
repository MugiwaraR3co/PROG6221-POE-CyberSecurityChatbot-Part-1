using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatbot.Services
{
    public class ActivityLogger
    {
        [span_16] (start_span) private List<string> _log = new List<string>(); [span_16] (end_span)

        // Shared global triggers to synchronize UI pane redirects across application state layers
        public bool SenderUINLPQuizTrigger { get; set; } = false;

        public void Log(string action)
        {
            [span_17] (start_span)// Appends a localized operational timestamp prefix[span_17](end_span)
            [span_18](start_span)string entry = DateTime.Now.ToString("[HH:mm] ") + action; [span_18] (end_span)
            _[span_19](start_span)log.Add(entry); [span_19] (end_span)
        }

        [span_20] (start_span)// Returns only the most recent 'count' entries as a numbered list[span_20](end_span)
        public string GetRecentLog(int count = 10)
        {
            if (!_log.Any()) return "No historical activity records found in session logs.";

            var items = _log.Skip(Math.Max(0, _log.Count - count)).ToList();
            [span_21] (start_span)[span_22](start_span)return "Here's a summary of recent actions:\n" + string.Join("\n", items.Select((val, idx) => $"{idx + 1}. {val.Substring(8)}")); [span_21] (end_span)[span_22](end_span)
        }

        [span_23] (start_span)// Pulls the entire compiled list of events[span_23](end_span)
        public string GetFullLog()
        {
            if (!_log.Any()) return "No historical activity records found in session logs.";
            [span_24] (start_span)return "Complete Historical Workspace Activity Log:\n" + string.Join("\n", _log.Select((val, idx) => $"{idx + 1}. {val}")); [span_24] (end_span)
        }

        [span_25] (start_span) public int GetCount() => _log.Count; [span_25] (end_span)
    }
}