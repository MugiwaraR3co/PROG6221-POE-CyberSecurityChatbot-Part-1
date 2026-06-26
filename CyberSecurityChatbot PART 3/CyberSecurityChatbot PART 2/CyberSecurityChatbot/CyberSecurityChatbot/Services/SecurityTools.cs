using System.Linq;

namespace CyberSecurityChatbot.Services
{
    public class SecurityTools
    {
        public string CheckPassword(string password)
        {
            int score = 0;

            if (password.Length >= 8) score++;
            if (password.Any(char.IsUpper)) score++;
            if (password.Any(char.IsDigit)) score++;
            if (password.Any(ch => !char.IsLetterOrDigit(ch))) score++;

            if (score == 4) return "Strong ✅";
            if (score == 3) return "Moderate ⚠️";
            return "Weak ❌";
        }

        public bool ContainsThreat(string input)
        {
            string text = input.ToLower();

            if (text.Contains("http://") || text.Contains("bit.ly"))
                return true;

            if (text.Contains("' or 1=1"))
                return true;

            if (text.Contains("<script>"))
                return true;

            return false;
        }
    }
}