using CyberSecurityChatbot.Services;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Input;

csharp CyberSecurityChatbot.UI\MainWindow.xaml.cs
using System;
using System.IO;
using System.Media;
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

        public MainWindow()
        {
            InitializeComponent();

            _bot = new Chatbot();
            _tools = new SecurityTools();

            TryPlayGreeting();
            AddSystemMessage("Hello! Type a question or check a password. I remember interests and can detect simple sentiment.");
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

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendInput();
        }

        // New: handle Enter key to send
        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendInput();
                e.Handled = true;
            }
        }

        private void SendInput()
        {
            var input = InputBox.Text?.Trim();
            if (string.IsNullOrEmpty(input))
                return;

            AddUserMessage(input);
            _interactionCount++;

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                AddSystemMessage($"You asked {_interactionCount} questions. Stay safe online!");
                InputBox.Clear();
                return;
            }

            string response = _bot.GetResponse(input);
            AddBotMessage(response);
            Log("User", input);
            InputBox.Clear();
        }

        private void PhishSimButton_Click(object sender, RoutedEventArgs e)
        {
            AddUserMessage("simulate phishing");
            string resp = _bot.GetResponse("simulate phishing");
            AddBotMessage(resp);
            Log("User", "simulate phishing");
        }

        private void CheckPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var pw = PasswordBox.Password ?? "";
            var result = _tools.CheckPassword(pw);
            StatusText.Text = $"Result: {result}";
            AddSystemMessage($"Password checked: {result}");
            Log("User", "Checked password");
        }

        // New: clear chat
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ChatList.Items.Clear();
            AddSystemMessage("Chat cleared.");
        }

        private void AddUserMessage(string text)
        {
            ChatList.Items.Add($"You: {text}");
        }

        private void AddBotMessage(string text)
        {
            ChatList.Items.Add($"Bot: {text}");
        }

        private void AddSystemMessage(string text)
        {
            ChatList.Items.Add($"* {text}");
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
}