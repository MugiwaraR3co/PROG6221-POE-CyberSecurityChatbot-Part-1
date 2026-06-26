using System;
using System.Collections.Generic;
using CyberSecurityChatbot.Models;

namespace CyberSecurityChatbot.Services
{
    public class QuizManager
    {
        private List<QuizQuestion> _questions;
        private int _currentIndex = 0;
        private int _score = 0;

        // Constructor populates more than 10 mandatory syllabus questions
        public QuizManager(object logger placeholder = null)
        {
            InitializeQuestions();
        }

        private void InitializeQuestions()
        {
            _questions = new List<QuizQuestion>
            {
                // Topic 1: Phishing (Multiple Choice)
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                    CorrectAnswer = "Report the email as phishing",
                    Explanation = "Reporting phishing emails helps security teams block malicious senders and protects the rest of the network from scams.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "An urgent email from your 'bank' claims your account is locked and provides a link to log in. What is this an example of?",
                    Options = new List<string> { "Spear phishing", "Whaling", "Phishing simulation", "Ransomware deployment" },
                    CorrectAnswer = "Spear phishing",
                    Explanation = "Phishing attempts often invent a fake sense of urgency or fear to trick individuals into revealing login credentials on cloned portals.",
                    IsTrueFalse = false
                },

                // Topic 2: Password Safety (True/False and Multiple Choice)
                new QuizQuestion
                {
                    Question = "Using the exact same strong password across multiple online accounts is completely safe.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "If one platform suffers a database data breach, hackers will attempt credential stuffing to access your other profiles using that same password.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "Which of the following characters helps create the highest entropy for a strong password?",
                    Options = new List<string> { "Only lower-case letters", "Sequential numbers like 12345", "A mix of uppercase, numbers, and special symbols", "Your birth year" },
                    CorrectAnswer = "A mix of uppercase, numbers, and special symbols",
                    Explanation = "Mixing sets increases character variety combinations, heavily raising the cryptographic work factor against automated brute-force attacks.",
                    IsTrueFalse = false
                },

                // Topic 3: Safe Browsing (HTTPS, Public Wi-Fi)
                new QuizQuestion
                {
                    Question = "When browsing, what does the padlock icon and the 'HTTPS' prefix in the browser address bar mean?",
                    Options = new List<string> { "The website content is 100% safe and verified", "Data traffic between your browser and the site is securely encrypted", "The site is hosted locally", "Your computer is immune to malware downloads" },
                    CorrectAnswer = "Data traffic between your browser and the site is securely encrypted",
                    Explanation = "HTTPS guarantees transport-layer encryption, preventing external network snoopers from reading data in transit, though the host itself could still be malicious.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "It is perfectly secure to conduct online banking on a public open Wi-Fi network without a VPN.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Public Wi-Fi networks leave transmission frames unencrypted, exposing users to Man-in-the-Middle (MitM) traffic packet capture attacks.",
                    IsTrueFalse = true
                },

                // Topic 4: Social Engineering (True/False)
                new QuizQuestion
                {
                    Question = "Social engineering bypasses physical firewalls by exploiting human trust and behavior flaws.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "True",
                    Explanation = "Social engineering tricks people into violating security protocols voluntarily through manipulation, fear, or technical authority impersonation.",
                    IsTrueFalse = true
                },

                // Topic 5: Two-Factor Authentication (2FA)
                new QuizQuestion
                {
                    Question = "Which option represents a multi-factor authentication layer factor?",
                    Options = new List<string> { "A secondary backup password string", "An authenticator app time-based token code", "A public username variation", "An security profile hint answer" },
                    CorrectAnswer = "An authenticator app time-based token code",
                    Explanation = "Multi-factor authentication requires combining distinct pillars: something you know (password) and something you physically have (authenticator token).",
                    IsTrueFalse = false
                },

                // Topic 6: Malware and Ransomware
                new QuizQuestion
                {
                    Question = "Ransomware is a specific classification of malicious software designed to encrypt user files and demand financial payment for the decryption key.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "True",
                    Explanation = "Ransomware locks local access to operating assets using encryption algorithms, holding production workflows hostage for ransom payments.",
                    IsTrueFalse = true
                },

                // Topic 7: Privacy Settings
                new QuizQuestion
                {
                    Question = "What is a primary benefit of managing app tracking permissions on smart devices?",
                    Options = new List<string> { "It accelerates internet hardware speeds", "It restricts unauthorized corporations from harvesting and profiling behavioral data", "It deletes duplicate pictures", "It deactivates local background firewall rules" },
                    CorrectAnswer = "It restricts unauthorized corporations from harvesting and profiling behavioral data",
                    Explanation = "Minimizing telemetry and data sharing access prevents background tracking profiles from mapping structural identity vectors.",
                    IsTrueFalse = false
                },

                // Topic 8: Data Backup
                new QuizQuestion
                {
                    Question = "What is the industry standard '3-2-1' backup strategy recommendation rule?",
                    Options = new List<string> { "3 hours of work, 2 local saves, 1 password reset", "3 copies of data, across 2 different media types, with 1 copy stored completely offsite", "3 clouds, 2 local computers, 1 system password", "3 active users, 2 encryption keys, 1 backup file" },
                    CorrectAnswer = "3 copies of data, across 2 different media types, with 1 copy stored completely offsite",
                    Explanation = "Maintaining offsite disaster copies isolates critical operational data from ransomware or localized physical infrastructure failures.",
                    IsTrueFalse = false
                }
            };
        }

        public QuizQuestion GetCurrentQuestion()
        {
            if (IsFinished()) return null;
            return _questions[_currentIndex];
        }

        public bool SubmitAnswer(string answer)
        {
            QuizQuestion current = GetCurrentQuestion();
            if (current == null) return false;

            bool isCorrect = (current.CorrectAnswer.Trim().ToLower() == answer.Trim().ToLower());
            if (isCorrect)
            {
                _score++;
            }

            _currentIndex++;
            return isCorrect;
        }

        public bool IsFinished()
        {
            return _currentIndex >= _questions.Count;
        }

        public int GetScore() => _score;
        public int GetTotalQuestions() => _questions.Count;

        public string GetFinalMessage()
        {
            double percentage = ((double)_score / _questions.Count) * 100;
            if (percentage >= 75)
                return $"Assessment Concluded. Great job! Final clear rate: {percentage:F0}%. Systems parameters fully compliant.";
            else
                return $"Assessment Concluded. Keep learning... Final clear rate: {percentage:F0}%. Review remediation frameworks.";
        }

        public void ResetQuiz()
        {
            _currentIndex = 0;
            _score = 0;
        }
    }
}