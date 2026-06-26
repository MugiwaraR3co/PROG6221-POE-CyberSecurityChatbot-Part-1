using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CyberSecurityChatbot.Models;
using CyberSecurityChatbot.Services;

namespace CyberSecurityChatbot.UI
{
    public partial class MainWindow : Window
    {
        private readonly Chatbot _bot;
        private readonly SecurityTools _tools;
        private readonly QuizManager _quiz;

        public ObservableCollection<MessageItem> ChatMessages { get; } = new ObservableCollection<MessageItem>();

        public MainWindow()
        {
            InitializeComponent();

            _bot = new Chatbot();
            _tools = new SecurityTools();
            _quiz = new QuizManager(_bot.Logger);

            ChatList.ItemsSource = ChatMessages;

            AddSystemMessage("Systems Online. Please register your tracking identity profile inside the command terminal prompt lines below.");

            RefreshTasksMatrix();
            RenderQuizQuestionState();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessTerminalInput();
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                ProcessTerminalInput();
            }
        }

        private void ProcessTerminalInput()
        {
            string input = InputBox.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(input)) return;

            AddUserMessage(input);
            InputBox.Clear();

            string response = _bot.GetResponse(input);
            AddBotMessage(response);

            RefreshTasksMatrix();
            InterceptNLPInterfaceTriggers();
        }

        private void InterceptNLPInterfaceTriggers()
        {
            if (_bot.Logger.SenderUINLPQuizTrigger)
            {
                _bot.Logger.SenderUINLPQuizTrigger = false;
                WorkspaceTabs.SelectedItem = QuizTab;
            }
        }

        private void CheckPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string inputPw = PasswordBox.Password ?? string.Empty;
            string evaluationResult = _tools.CheckPassword(inputPw);
            StatusText.Text = $"Analysis: {evaluationResult}";
            AddSystemMessage($"Entropy validation routine executed: {evaluationResult}");
        }

        private void RefreshTasksMatrix()
        {
            DgTasks.ItemsSource = null;
            DgTasks.ItemsSource = _bot.TaskEngine.GetAllTasks();
        }

        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            string title = TxtTaskTitle.Text.Trim();
            string desc = TxtTaskDescription.Text.Trim();
            string reminder = TxtTaskReminder.Text.Trim();

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Task identification title parameters cannot be left completely blank.", "Validation Boundary Failure", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string coreReturnMessage = _bot.TaskEngine.AddTask(title, desc, reminder);
            AddSystemMessage($"[Task Framework Notification]: {coreReturnMessage}");

            TxtTaskTitle.Clear();
            TxtTaskDescription.Clear();
            TxtTaskReminder.Clear();

            RefreshTasksMatrix();
        }

        private void BtnMarkComplete_Click(object sender, RoutedEventArgs e)
        {
            if (DgTasks.SelectedItem is CyberTask selectedItem)
            {
                _bot.TaskEngine.MarkAsComplete(selectedItem.Id);
                RefreshTasksMatrix();
            }
        }

        private void BtnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (DgTasks.SelectedItem is CyberTask selectedItem)
            {
                _bot.TaskEngine.DeleteTask(selectedItem.Id);
                RefreshTasksMatrix();
            }
        }

        private void RenderQuizQuestionState()
        {
            QuizQuestion currentQuestion = _quiz.GetCurrentQuestion();
            QuizScoreText.Text = $"Active Assessment Score Metric: {_quiz.GetScore()} / {_quiz.GetTotalQuestions()} Items Cleared";

            if (currentQuestion == null)
            {
                QuestionText.Text = _quiz.GetFinalMessage();
                OptionsPanel.Visibility = Visibility.Collapsed;
                SubmitAnswerBtn.Visibility = Visibility.Collapsed;
                NextQuestionBtn.Visibility = Visibility.Collapsed;
                ResetQuizBtn.Visibility = Visibility.Visible;
                return;
            }

            OptionsPanel.Visibility = Visibility.Visible;
            SubmitAnswerBtn.Visibility = Visibility.Visible;
            NextQuestionBtn.Visibility = Visibility.Collapsed;
            ResetQuizBtn.Visibility = Visibility.Collapsed;
            ExplanationText.Visibility = Visibility.Collapsed;

            QuestionText.Text = currentQuestion.Question;

            if (currentQuestion.IsTrueFalse)
            {
                OptA.Content = "True";
                OptB.Content = "False";
                OptC.Visibility = Visibility.Collapsed;
                OptD.Visibility = Visibility.Collapsed;
            }
            else
            {
                OptC.Visibility = Visibility.Visible;
                OptD.Visibility = Visibility.Visible;
                OptA.Content = currentQuestion.Options[0];
                OptB.Content = currentQuestion.Options[1];
                OptC.Content = currentQuestion.Options[2];
                OptD.Content = currentQuestion.Options[3];
            }

            OptA.IsChecked = false;
            OptB.IsChecked = false;
            OptC.IsChecked = false;
            OptD.IsChecked = false;
        }

        private void SubmitAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            QuizQuestion currentQuestion = _quiz.GetCurrentQuestion();
            if (currentQuestion == null) return;

            string selectedAnswerString = string.Empty;
            if (OptA.IsChecked == true) selectedAnswerString = OptA.Content.ToString();
            else if (OptB.IsChecked == true) selectedAnswerString = OptB.Content.ToString();
            else if (OptC.IsChecked == true && !currentQuestion.IsTrueFalse) selectedAnswerString = OptC.Content.ToString();
            else if (OptD.IsChecked == true && !currentQuestion.IsTrueFalse) selectedAnswerString = OptD.Content.ToString();

            if (string.IsNullOrEmpty(selectedAnswerString))
            {
                MessageBox.Show("Please select an choice option node before committing answers.", "Missing Token Input", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            bool verificationOutcome = _quiz.SubmitAnswer(selectedAnswerString);
            ExplanationText.Text = $"[(Verification Result: {(verificationOutcome ? "SUCCESS" : "FAIL")})]\n{currentQuestion.Explanation}";
            ExplanationText.Visibility = Visibility.Visible;

            SubmitAnswerBtn.Visibility = Visibility.Collapsed;
            NextQuestionBtn.Visibility = Visibility.Visible;

            QuizScoreText.Text = $"Active Assessment Score Metric: {_quiz.GetScore()} / {_quiz.GetTotalQuestions()} Items Cleared";
        }

        private void NextQuestionBtn_Click(object sender, RoutedEventArgs e)
        {
            RenderQuizQuestionState();
        }

        private void ResetQuizBtn_Click(object sender, RoutedEventArgs e)
        {
            _quiz.ResetQuiz();
            RenderQuizQuestionState();
        }

        private void AddSystemMessage(string text) => AddMessageElement("System", text);
        private void AddUserMessage(string text) => AddMessageElement("You", text);
        private void AddBotMessage(string text) => AddMessageElement("Bot", text);

        private void AddMessageElement(string sender, string msgText)
        {
            ChatMessages.Add(new MessageItem { TemplateSender = sender, Text = msgText, Timestamp = DateTime.Now.ToString("HH:mm:ss") });
            if (ChatList.Items.Count > 0)
            {
                ChatList.ScrollIntoView(ChatList.Items[ChatList.Items.Count - 1]);
            }
        }
    }

    public class MessageItem
    {
        public string TemplateSender { get; set; }
        public string Text { get; set; }
        public string Timestamp { get; set; }
    }
}