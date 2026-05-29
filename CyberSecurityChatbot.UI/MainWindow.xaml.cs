using CyberSecurityChatbot.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Input;

csharp CyberSecurityChatbot.UI\MainWindow.xaml.cs
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CyberSecurityChatbot.Services;

namespace CyberSecurityChatbot.UI
{
    public partial class MainWindow : Window
    {
        private readonly Chatbot _bot;
        private readonly SecurityTools _tools;
        private int _interactionCount = 0;
        private readonly string _logFolder = "Logs";

        // Observable collection bound to the ListBox for modern UI
        public ObservableCollection<MessageItem> Messages { get; } = new();

        public MainWindow()
        {
            InitializeComponent();

            _bot = new Chatbot();
            _tools = new SecurityTools();

            DataContext = this;

            TryPlayGreeting();
            AddSystemMessage("Hello! Type a question or check a password. I remember interests and detect simple sentiment.");
        }

        private void TryPlayGreeting()
        {
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "welcome.wav");
                if (File.Exists(path))
                {
                    using var player = new SoundPlayer(path);
                    player.Play();
                }
            }
            catch
            {
                AddSystemMessage("Voice greeting file not found.");
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await SendInputAsync();
        }

        private async void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                await SendInputAsync();
            }
        }

        private async Task SendInputAsync()
        {
            var input = InputBox.Text?.Trim();
            if (string.IsNullOrEmpty(input))
                return;

            AddMessage("You", input);
            _interactionCount++;
            Log("User", input);
            InputBox.Clear();

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                AddSystemMessage($"You asked {_interactionCount} questions. Stay safe online!");
                return;
            }

            // Show a typing indicator
            var typing = new MessageItem { Sender = "Bot", Text = "Bot is typing...", Timestamp = DateTime.Now, IsTyping = true };
            Messages.Add(typing);
            ScrollToEnd();

            // Small delay for realism
            await Task.Delay(600);

            // Get response and replace typing indicator with actual message
            string response = _bot.GetResponse(input);

            // replace typing entry
            var index = Messages.IndexOf(typing);
            if (index >= 0)
            {
                Messages[index] = new MessageItem { Sender = "Bot", Text = response, Timestamp = DateTime.Now };
            }
            else
            {
                AddMessage("Bot", response);
            }

            ScrollToEnd();
        }

        private void PhishSimButton_Click(object sender, RoutedEventArgs e)
        {
            _ = SimulatePhishingAsync();
        }

        private async Task SimulatePhishingAsync()
        {
            AddMessage("You", "simulate phishing");
            Log("User", "simulate phishing");

            var typing = new MessageItem { Sender = "Bot", Text = "Running phishing simulation...", Timestamp = DateTime.Now, IsTyping = true };
            Messages.Add(typing);
            ScrollToEnd();

            await Task.Delay(700);

            string resp = _bot.GetResponse("simulate phishing");
            var idx = Messages.IndexOf(typing);
            if (idx >= 0)
                Messages[idx] = new MessageItem { Sender = "Bot", Text = resp, Timestamp = DateTime.Now };
            else
                AddMessage("Bot", resp);

            ScrollToEnd();
        }

        private void CheckPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var pw = PasswordBox.Password ?? "";
            var result = _tools.CheckPassword(pw);
            StatusText.Text = $"Result: {result}";
            AddSystemMessage($"Password checked: {result}");
            Log("User", "Checked password");
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Messages.Clear();
            AddSystemMessage("Chat cleared.");
        }

        private void AddSystemMessage(string text)
        {
            Messages.Add(new MessageItem { Sender = "System", Text = text, Timestamp = DateTime.Now });
            ScrollToEnd();
        }

        private void AddMessage(string sender, string text)
        {
            Messages.Add(new MessageItem { Sender = sender, Text = text, Timestamp = DateTime.Now });
            ScrollToEnd();
        }

        private void ScrollToEnd()
        {
            if (ChatList.Items.Count > 0)
            {
                ChatList.ScrollIntoView(ChatList.Items[ChatList.Items.Count - 1]);
            }
        }

        private void Log(string user, string action)
        {
            try
            {
                Directory.CreateDirectory(_logFolder);
                File.AppendAllText(Path.Combine(_logFolder, "log.txt"),
                    $"[{DateTime.Now}] {user}: {action}{Environment.NewLine}");
            }
            catch
            {
                // keep UI stable if logging fails
            }
        }
    }

    // Simple message model used by the UI
    public class MessageItem
    {
        public string Sender { get; set; } = "Bot"; // "You", "Bot", or "System"
        public string Text { get; set; } = "";
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public bool IsTyping { get; set; }
    }
}