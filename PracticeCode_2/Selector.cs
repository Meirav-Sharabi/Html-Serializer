using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PracticeCode_2
{
    public class Selector
    {
        public string TagName { get; set; }
        public string id { get; set; }
        public List<string> Classes = new List<string>();
        public Selector Child { get; set; }
        public Selector Parent { get; set; }

        public static Selector FromQueryString(string queryString)
        {
            // Split the query string into parts based on spaces
            string[] queryParts = Regex.Split(queryString, " ");

            Selector rootSelector = null;
            Selector currentSelector = new Selector();

            foreach (string item in queryParts)
            {
                var newSelector = new Selector();
                // Add it as a child of the current selector
                //הוספה באופן דינאמי כלומר הצבעה ולא השמה
                currentSelector.Child = (newSelector);
                newSelector.Parent = (currentSelector);
                // Update the current selector to point to the new one
                currentSelector = newSelector;
                if (rootSelector == null)
                {
                    rootSelector = newSelector;
                    rootSelector.Parent = null;
                }
                string[] parts = Regex.Split(item, "(?=[#.])").Where(S => S.Length > 0).ToArray();
                foreach (string part in parts)
                {
                    if (part.StartsWith("#"))
                    {
                        currentSelector.id = part.Substring(1, part.Length - 1);
                    }
                    else if (part.StartsWith("."))
                    {
                        // Part starts with ., update Classes property
                        currentSelector.Classes.Add(part.Substring(1));
                    }
                    else
                    {
                        if(HtmlHelper.Instance.AllTags.Contains(part)||HtmlHelper.Instance.SelfClosingTags.Contains(part.Substring(1,part.Length-1)))
                        {
                            currentSelector.TagName = part;
                        }                    
                    }
                }
            }
            return rootSelector;
        }
    }
}
