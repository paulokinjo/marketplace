using Marketplace.Framework;
using System.Text.RegularExpressions;

namespace Marketplace.Domain
{
    public class ClassifiedAdTitle : Value<ClassifiedAdTitle>
    {
        public string Value { get; }

        public ClassifiedAdTitle(string value)
        {
            CheckValidity(value);
            Value = value;
        }

        public static ClassifiedAdTitle FromString(string title)
        {
            CheckValidity(title);
            return new ClassifiedAdTitle(title);
        }

        public static ClassifiedAdTitle FromHtml(string htmlTitle)
        {
            var supportedTagsReplaced = htmlTitle
                .Replace("<i>", "*")
                .Replace("</i>", "*")
                .Replace("<b>", "**")
                .Replace("</b>", "**");

            var value = Regex.Replace(supportedTagsReplaced, "<.*?>", string.Empty);

            CheckValidity(value);
            return new ClassifiedAdTitle(value);
        }

        public static implicit operator string(ClassifiedAdTitle self) => self.Value; 

        private static void CheckValidity(string value)
        {
            if (value.Length > 100)
            {
                throw new ArgumentOutOfRangeException(
                    "Title cannot be longer than 100 characters",
                    nameof(value));
            }
        }
    }
}
