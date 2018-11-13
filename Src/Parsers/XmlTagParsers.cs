using System;
using System.Collections.Generic;

namespace Licenator
{
    public class XmlTag
    {
        public string Name { get ;set; }
        public string Value { get; set; }
    }

    public static class XmlTagParser
    {
        public static IEnumerable<XmlTag> Parse(string line)
        {
            var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var token in tokens)
            {
                if (token.Contains("=\"") && token.EndsWith("\""))
                {
                    var equalIndex = token.IndexOf('=');
                    
                    var name = token.Substring(0, equalIndex);
                    var value = token.Substring(equalIndex + 2, token.Length - (equalIndex + 3));

                    yield return new XmlTag
                    {
                        Name = name,
                        Value = value
                    };
                }
            }
        }
    }
}