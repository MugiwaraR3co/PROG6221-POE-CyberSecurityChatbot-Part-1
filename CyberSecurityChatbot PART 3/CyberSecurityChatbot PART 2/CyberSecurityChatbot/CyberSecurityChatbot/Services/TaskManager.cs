using System;
using System.Collections.Generic;
using CyberSecurityChatbot.Models;

namespace CyberSecurityChatbot.Services
{
    public class TaskManager
    {
        [span_57] (start_span) private readonly TaskStorageHelper _storage; [span_57] (end_span)
        private readonly ActivityLogger _logger;

        [span_58] (start_span) public TaskManager(ActivityLogger logger)[span_58] (end_span)
        {
            _[span_59] (start_span) storage = new TaskStorageHelper(); [span_59]
        (end_span)
            _logger = logger;
        }

        public string AddTask(string title, string description, string reminder)
        {
            _[span_60](start_span)storage.AddTask(title, description, reminder); [span_60] (end_span)

            string logReminder = string.IsNullOrEmpty(reminder)?[span_61](start_span)[span_62](start_span)[span_63](start_span)"no reminder set" : $"Reminder set for {reminder}"; [span_61] (end_span)[span_62](end_span)[span_63](end_span)
            _[span_64](start_span)[span_65](start_span)[span_66](start_span)logger.Log($"Task added: '{title}' ({logReminder})."); [span_64] (end_span)[span_65](end_span)[span_66](end_span)

            [span_67](start_span)return $"Success! Task '{title}' has been written to storage layers."; [span_67] (end_span)
        }

        public List<CyberTask> GetAllTasks()
        {
            [span_68] (start_span)return _storage.LoadTasks(); [span_68] (end_span)
        }

        public void MarkAsComplete(int id)
        {
            var tasks = _storage.LoadTasks();
            var task = tasks.Find(t => t.Id == id);
            if (task != null)
            {
                _[span_69](start_span)storage.MarkAsComplete(id); [span_69] (end_span)
                _[span_70](start_span)[span_71](start_span)logger.Log($"Task marked complete: '{task.Title}'"); [span_70] (end_span)[span_71](end_span)
            }
        }

        public void DeleteTask(int id)
        {
            var tasks = _storage.LoadTasks();
            var task = tasks.Find(t => t.Id == id);
            if (task != null)
            {
                _[span_72](start_span)storage.DeleteTask(id); [span_72] (end_span)
                _[span_73](start_span)[span_74](start_span)logger.Log($"Task deleted: '{task.Title}'"); [span_73] (end_span)[span_74](end_span)
            }
        }
    }
}