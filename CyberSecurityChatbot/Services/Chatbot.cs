using CyberSecurityChatbot.Services;

namespace CyberSecurityChatbot.Services
{
    public class Chatbot
    {
        private SecurityTools tools = new SecurityTools();

        public string GetResponse(string input)
        {
            string text = input.ToLower();

            // Threat detection
            if (tools.ContainsThreat(input))
            {
                return "⚠️ Warning: Potential security threat detected!";
            }

            // Knowledge responses
            if (text.Contains("phishing"))
            {
                return "Phishing is when attackers trick users into revealing sensitive information.";
            }
            else if (text.Contains("malware"))
            {
                return "Malware is harmful software designed to damage systems.";
            }
            else if (text.Contains("password"))
            {
                return "Use strong passwords with uppercase letters, numbers, and symbols.";
            }
            else if (text.Contains("social engineering"))
            {
                return "Social engineering manipulates people into giving away confidential information.";
            }
            else if (text.Contains("firewall"))
            {
                return "A firewall monitors and controls network traffic for security.";
            }
            else if (text.Contains("antivirus"))
            {
                return "Antivirus software detects and removes malicious programs.";
            }
            else if (text.Contains("vpn"))
            {
                return "A VPN encrypts your internet connection to protect your privacy.";
            }
            else if (text.Contains("simulate phishing"))
            {
                return SimulatePhishing();
            }

            return "I’m not sure about that. Try asking about cybersecurity topics.";
        }

        private string SimulatePhishing()
        {
            return @"--- Phishing Simulation ---
1. Attacker sends a fake email
2. Victim clicks a malicious link
3. Credentials are stolen

Prevention:
- Verify sender email
- Avoid suspicious links
- Use multi-factor authentication";
        }
    }
}