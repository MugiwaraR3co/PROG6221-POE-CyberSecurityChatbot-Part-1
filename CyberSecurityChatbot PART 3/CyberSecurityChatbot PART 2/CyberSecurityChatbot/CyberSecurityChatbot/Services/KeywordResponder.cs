using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatbot.Services
{
    public class KeywordResponder
    {
        // Part 2 Basic Knowledge Base Core
        private readonly Dictionary<string, string> _cyberKnowledgeBase = new Dictionary<string, string>
        {
            { "phishing", "Phishing involves fraudulent emails or clones mimicking legitimate vendors to steal user credentials. Never click unexpected links." },
            { "password", "A resilient defense configuration requires passwords with high entry entropy—utilizing uppercase characters, numeric rows, and special designators." },
            { "privacy", "Data tracking telemetry vectors profile user behaviors. Audit structural application permissions to isolate exposure channels." },
            { "scam", "Malicious actors exploit artificial urgency windows. Validate cross-channel identity paths before authorizing asset transactions." },
            { "malware", "Malware or ransomware encrypts device payloads to hold structural assets hostage. Maintain isolated, offline offsite backups." },
            { "2fa", "Two-Factor Authentication forces dual-pillar confirmation rules (combining something you know with a hardware authenticator token)." }
        };

        // --- PART 3 NLP INTENT GROUP DETECTION DICTIONARIES ---
        private readonly List<string> _addTaskKeywords = new List<string> { "add task", "add a task", "create task", "i need to", "enable", "set up" };
        private readonly List<string> _reminderKeywords = new List<string> { "remind me", "reminder", "set a reminder", "remind me to", "don't forget" };
        private readonly List<string> _startQuizKeywords = new List<string> { "start quiz", "take quiz", "test my knowledge", "quiz me", "play the game" };
        private readonly List<string> _showLogKeywords = new List<string> { "show activity log", "what have you done", "what did you do", "show log", "recent actions" };

        public string MatchKnowledgeBase(string input)
        {
            foreach (var key in _cyberKnowledgeBase.Keys)
            {
                if (input.Contains(key))
                {
                    return _cyberKnowledgeBase[key];
                }
            }
            return null;
        }

        // Methods to parse user sentences into automated workspace intents
        public bool IsAddTaskIntent(string input) => _addTaskKeywords.Any(phrase => input.Contains(phrase));
        public bool IsReminderIntent(string input) => _reminderKeywords.Any(phrase => input.Contains(phrase));
        public bool IsStartQuizIntent(string input) => _startQuizKeywords.Any(phrase => input.Contains(phrase));
        public bool IsShowLogIntent(string input) => _showLogKeywords.Any(phrase => input.Contains(phrase));
    }
}