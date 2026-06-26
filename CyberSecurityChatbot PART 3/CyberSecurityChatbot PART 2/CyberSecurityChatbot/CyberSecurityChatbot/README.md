# CyberSecurityChatbot

A modern cybersecurity awareness chatbot developed in C# using both a Console Application and a WPF Graphical User Interface (GUI). The chatbot helps users learn about cybersecurity topics such as password security, phishing attacks, online privacy, and scam prevention through interactive conversations and security tools.

---

# Features

## User Interface (WPF)

The application includes a modern and interactive WPF chat interface designed to provide an engaging user experience.

### Features

* Modern chat interface with message bubbles and avatars.
* Message timestamps for all conversations.
* Typing indicator to simulate real chatbot interactions.
* Smooth fade-in animation for incoming messages.
* Automatic scrolling to the latest message.
* Keyboard-friendly functionality (Press Enter to send messages).
* Clear Chat button.
* Dedicated Send and Phishing Simulation buttons.
* Password strength checking interface with results display.
* Welcome voice greeting playback using `welcome.wav`.
* Safe audio handling with exception management.

---

## Chatbot Intelligence and Behaviour

The chatbot provides cybersecurity-focused assistance through keyword recognition, memory, and conversational context tracking.

### Features

* Recognises cybersecurity topics including:

  * Password Security
  * Phishing
  * Privacy
  * Scams
* Randomised responses to prevent repetitive answers.
* Follow-up conversation support using topic tracking.
* Context-aware replies for requests such as:

  * "Tell me more"
  * "Another one"
  * "More information"
* Memory and recall functionality for user details:

  * User name
  * User interests
* Personalised responses based on stored information.
* Basic sentiment detection using predefined sentiment cues.
* Empathetic and encouraging responses when users express concern.
* Built-in phishing simulation functionality.
* Safe fallback responses for unknown input.

---

## Security Tools

The application includes practical cybersecurity tools to assist users.

### Password Strength Checker

Evaluates passwords and provides feedback on password strength.

### Threat Detection

Detects potentially suspicious input patterns including:

* Suspicious URLs
* SQL Injection attempts
* Embedded scripts

---

## Data Models and Binding

The WPF application uses data binding for responsive user interface updates.

### Features

* MessageItem model containing:

  * Sender
  * Message Text
  * Timestamp
  * Typing Status
* ObservableCollection binding for real-time chat updates.

---

## Logging and Robustness

The application includes safety mechanisms to improve reliability.

### Features

* Lightweight logging to `Logs/log.txt`.
* Error handling to prevent application crashes.
* Safe file and asset handling.
* Graceful exception management throughout the application.

---

## Project Structure

```text
CyberSecurityChatbot/
│
├── Program.cs
├── Services/
│   ├── Chatbot.cs
│   └── SecurityTools.cs
│
├── Logs/
│   └── log.txt
│
└── CyberSecurityChatbot.UI/
    ├── MainWindow.xaml
    ├── MainWindow.xaml.cs
    ├── Assets/
    │   └── welcome.wav
    └── CyberSecurityChatbot.UI.csproj
```

---

# Installation

## Prerequisites

* Visual Studio 2022 or later
* .NET Framework / .NET version specified in the project
* Windows Operating System

## Steps

1. Clone the repository:

```bash
git clone https://github.com/YourUsername/CyberSecurityChatbot.git
```

2. Open the solution in Visual Studio.

3. Build the solution.

4. Run either:

   * CyberSecurityChatbot (Console Application)
   * CyberSecurityChatbot.UI (WPF Application)

---

# Usage

## WPF Application

Users can:

* Ask cybersecurity-related questions.
* Receive advice on passwords, phishing, privacy, and scams.
* Continue conversations using follow-up questions.
* Run phishing simulations.
* Check password strength.
* Receive personalised responses based on stored information.

### Example Questions

* How can I create a strong password?
* What is phishing?
* How do I protect my privacy online?
* Tell me more about scams.
* Another phishing tip.

---

## Console Application

The original console application remains fully functional and supports:

* Cybersecurity question answering.
* Phishing simulations.
* Password strength checking.
* Conversation memory and follow-up interactions.

---

# Technologies Used

* C#
* .NET
* Windows Presentation Foundation (WPF)
* XAML
* ObservableCollection
* Object-Oriented Programming (OOP)

---

# Key Learning Outcomes

This project demonstrates:

* GUI development using WPF.
* Event-driven programming.
* Object-oriented software design.
* Data binding in WPF.
* Exception handling and logging.
* Basic cybersecurity awareness concepts.
* User interaction design.
* Application integration between console and graphical interfaces.

---

# Developer Notes

* The WPF application references the core CyberSecurityChatbot project.
* Assets are automatically copied to the output directory.
* Logging is implemented in both the console and GUI applications.
* The original console functionality has been preserved.
* Recommended GitHub submission:

  * Minimum of 6 commits.
  * Minimum of 2 releases.

---

# Future Improvements

Potential enhancements include:

* AI-powered responses using external APIs.
* Speech-to-text support.
* Text-to-speech responses.
* Enhanced phishing simulations.
* User authentication and profiles.
* Database-backed memory storage.
* Dark mode and theme customisation.

---

# License

This project is developed for educational purposes and cybersecurity awareness training.
