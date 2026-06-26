using CyberSecurityChatbot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
[span_30] (start_span)using Newtonsoft.Json;[span_30] (end_span)
using CyberSecurityChatbot.Models;

namespace CyberSecurityChatbot.Services
{
    public class TaskStorageHelper
    {
        [span_31] (start_span) private const string FilePath = "tasks.json"; [span_31]
        (end_span)

        [span_32] (start_span)// READ: Deserializes the local JSON file on launch[span_32](end_span)
        public List<CyberTask> LoadTasks()
        {
            try
            {
                if (!File.Exists(FilePath))
                    [span_33] (start_span)return new List<CyberTask>(); [span_33] (end_span)

                string json = File.ReadAllText(FilePath);
                [span_34] (start_span)return JsonConvert.DeserializeObject<List<CyberTask>>(json) ?? new List<CyberTask>(); [span_34] (end_span)
            }
            catch (Exception)
            {
                [span_35] (start_span)return new List<CyberTask>(); [span_35] (end_span)
            }
        }

        [span_36] (start_span)// Utility method to write the updated task list back to the local drive[span_36](end_span)
        public void SaveTasks(List<CyberTask> tasks)
        {
            try
            {
                [span_37] (start_span)string json = JsonConvert.SerializeObject(tasks, Formatting.Indented); [span_37] (end_span)
                [span_38](start_span)File.WriteAllText(FilePath, json); [span_38] (end_span)
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Storage Error: {ex.Message}");
            }
        }

        [span_39] (start_span)// CREATE: Generates a new task record with an auto-incremented ID[span_39](end_span)
        public void AddTask(string title, string description, string reminder)
        {
            [span_40] (start_span)var tasks = LoadTasks(); [span_40] (end_span)
            [span_41](start_span)int nextId = tasks.Any() ? tasks.Max(t => t.Id) + 1 : 1; [span_41] (end_span)

            var newTask = new CyberTask
            {
                [span_42](start_span)Id = nextId,
                [span_42](end_span)
                Title = title,
                Description = description,
                Reminder = reminder,
                IsComplete = false,
                CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            };

            [span_43] (start_span)tasks.Add(newTask); [span_43] (end_span)
            [span_44](start_span)SaveTasks(tasks); [span_44] (end_span)
        }

        [span_45] (start_span)// UPDATE: Flips the completion status flag[span_45](end_span)
        public void MarkAsComplete(int id)
        {
            [span_46] (start_span)var tasks = LoadTasks(); [span_46] (end_span)
            [span_47](start_span)var task = tasks.FirstOrDefault(t => t.Id == id); [span_47] (end_span)
            if (task != null)
            {
                [span_48] (start_span)task.IsComplete = true; [span_48] (end_span)
                [span_49](start_span)SaveTasks(tasks); [span_49] (end_span)
            }
        }

        [span_50] (start_span)// DELETE: Purges an existing entry by its ID[span_50](end_span)
        public void DeleteTask(int id)
        {
            [span_51] (start_span)var tasks = LoadTasks(); [span_51] (end_span)
            [span_52](start_span)var task = tasks.FirstOrDefault(t => t.Id == id); [span_52] (end_span)
            if (task != null)
            {
                [span_53] (start_span)tasks.Remove(task); [span_53] (end_span)
                [span_54](start_span)SaveTasks(tasks); [span_54] (end_span)
            }
        }
    }
}