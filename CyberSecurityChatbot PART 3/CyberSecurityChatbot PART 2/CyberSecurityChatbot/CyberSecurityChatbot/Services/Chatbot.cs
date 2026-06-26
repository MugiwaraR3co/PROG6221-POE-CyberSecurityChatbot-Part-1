using System;
using System.Text.RegularExpressions;
using CyberSecurityChatbot.Models;

namespace CyberSecurityChatbot.Services
{
    public class Chatbot
    {
        public ActivityLogger Logger { get; }
        public TaskManager TaskEngine { get; }
        private readonly KeywordResponder _responder;

        // Conversation state flags to enable natural multi-turn dialogue loops
        private string _lastCreatedTaskTitle = string.Empty;
        private bool _awaitingReminderConfirmation = false;

        public Chatbot()
        {
            Logger = new ActivityLogger();
            TaskEngine = new TaskManager(Logger);
            _responder = new KeywordResponder();
        }

        public string GetResponse(string userInput)
        {
            string cleanInput = userInput?.Trim().ToLower() ?? string.Empty;

            // --- STATE TRACER A: INTERCEPT REMINDER RESPONSES ---
            if (_awaitingReminderConfirmation)
            {
                _awaitingReminderConfirmation = false;

                if (cleanInput.Contains("yes") || _responder.IsReminderIntent(cleanInput))
                {
                    // Attempt to cleanly extract the reminder text phrasing from the raw text line
                    string reminderPhrase = userInput;
                    if (cleanInput.StartsWith("yes,")) reminderPhrase = userInput.Substring(4).Trim();
                    else if (cleanInput.StartsWith("yes")) reminderPhrase = userInput.Substring(3).Trim();

                    if (string.IsNullOrEmpty(reminderPhrase) || cleanInput == "yes")
                    {
                        reminderPhrase = "Configured Alert Schedule Profile";
                    }

                    // Reload the task from database storage layers to append the extracted reminder context
                    var allTasks = TaskEngine.GetAllTasks();
                    foreach (var task in allTasks)
                    {
                        if (task.Title == _lastCreatedTaskTitle && string.IsNullOrEmpty(task.Reminder))
                        {
                            TaskEngine.DeleteTask(task.Id); // Swap entry rows out
                            TaskEngine.AddTask(task.Title, task.Description, reminderPhrase);
                            break;
                        }
                    }

                    Logger.Log($"Reminder set: '{_lastCreatedTaskTitle}' on validation schedule context.");
                    return $"Got it! I'll remind you to complete '{_lastCreatedTaskTitle}' on your scheduled timeline criteria.";
                }

                return "Understood. Task registration pipeline committed to the database array without setting background reminders.";
            }

            // --- NLP INTENT MATRIX ROUTER LAYER ---

            // 1. SHOW ACTIVITY LOG PULLS
            if (_responder.IsShowLogIntent(cleanInput))
            {
                Logger.Log("NLP recognised log intent request string.");
                if (cleanInput.Contains("show more"))
                {
                    return Logger.GetFullLog();
                }

                string recentLogsOutput = Logger.GetRecentLog(10);
                if (Logger.GetCount() > 10)
                {
                    recentLogsOutput += "\n\n[💡 System Parameter Notification: Type 'show more' to print out the full historical log matrix history]";
                }
                return recentLogsOutput;
            }

            if (cleanInput == "show more")
            {
                Logger.Log("Full log history printed.");
                return Logger.GetFullLog();
            }

            // 2. QUIZ PANEL WORKSPACE TRIGGER REDIRECTS
            if (_responder.IsStartQuizIntent(cleanInput))
            {
                Logger.Log("NLP recognised quiz intent string.");
                Logger.Log("Quiz started");

                // Toggle the shared UI memory trigger so MainWindow redirects focus tabs
                Logger.SenderUINLPQuizTrigger = true;
                return "Initializing compliance literacy parameters. Activating the Quiz verification environment on your main control layout panel now!";
            }

            // 3. TASK CREATION INTERFACE
            if (_responder.IsAddTaskIntent(cleanInput))
            {
                Logger.Log($"NLP recognised task intent from: '{userInput}'");

                // Clean up the string to extract the raw task title out of phrases
                string taskTitle = Regex.Replace(userInput, @"(?i)^(add task|add a task|create task|i need to|enable|set up)\s*", "").Trim();
                if (string.IsNullOrEmpty(taskTitle)) taskTitle = "Unspecified Compliance Objective Operational Node";

                // Formulate a clean, technical description parameter based on context keywords
                string generatedDesc = "Review and inspect asset security configurations to verify integrity protections.";
                if (cleanInput.Contains("2fa") || cleanInput.Contains("factor"))
                    generatedDesc = "Set up multi-factor security access controls on all administrator entry channels.";
                else if (cleanInput.Contains("privacy"))
                    generatedDesc = "Review account privacy settings to ensure your data is protected.";
                else if (cleanInput.Contains("password"))
                    generatedDesc = "Cycle operational credentials to apply high entropy defensive strings.";

                // Save directly into JSON database storage
                TaskEngine.AddTask(taskTitle, generatedDesc, "");

                // Keep local state records active to handle multi-turn reminder attachment next
                _lastCreatedTaskTitle = taskTitle;
                _awaitingReminderConfirmation = true;

                return $"Task added with the description '{generatedDesc}'\n\nWould you like to set a verification reminder for this task?";
            }

            // --- FALLBACK MATRIX: PART 2 FLOW KNOWLEDGE SEARCH ---
            string kbMatch = _responder.MatchKnowledgeBase(cleanInput);
            if (kbMatch != null)
            {
                Logger.Log($"Keyword matched: Identification phrase processed -> response delivered.");
                return kbMatch;
            }

            // Basic Fallback Catchment Layer
            return "Command sequence unrecognised. Please phrase your directive towards registering asset tasks, pulling session log matrices, or initiating compliance quizzes.";
        }
    }
}