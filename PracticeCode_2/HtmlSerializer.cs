using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PracticeCode_2
{
    public class HtmlSerializer
    {
        public async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }

        public HtmlElement Serializer(string html)
        {
            var cleanHtml = Regex.Replace(html, @"<!--[\s\S]*?-->", ""); // Remove comments, including multiline comments
            cleanHtml = Regex.Replace(cleanHtml, @"\s*\n\s*|\s{2,}", " "); // Replace multiple spaces with a single space
            var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(x => x.Length > 0);

            HtmlElement rootElement = null;
            HtmlElement currentElement = new HtmlElement();
            //מכיון שיכול להיות שהמשתנה הנוכחי יעמוד על שורה ריקה עלי לבדוק בכל פעם שהוא לא עומד על שורה כזו
            //כי אז יווצרו לי שיבושים ולכן אבדוק בכל פעם שהוא שונה מנל
            foreach (var line in htmlLines)
            {
                string[] words = line.Split(' ');

                if (line == "/html")
                {
                    break;
                }
                else if (line.StartsWith("/"))
                {
                    if (currentElement != null && HtmlHelper.Instance.AllTags.Contains(words[0].Substring(1)))
                    {
                        currentElement = currentElement.Parent;
                    }
                }
                else if (HtmlHelper.Instance.AllTags.Contains(words[0], StringComparer.OrdinalIgnoreCase) || HtmlHelper.Instance.SelfClosingTags.Contains(words[0]))
                {
                    if (words.Length > 1 && words[1].StartsWith('{'))
                    {
                        currentElement!.InnerHtml = line;
                    }
                    else
                    {
                        var newElement = new HtmlElement { Name = words[0] };
                        currentElement.Children.Add(newElement);
                        newElement.Parent = currentElement;
                        //שורה זו מחלצת את המאפיינים
                        var attributesRegex = new Regex(@"(\w+)(?:=""([^""]*)""|$)");
                        //שורה זו מחלצת רק את המאפינים הנמצאים בשורה הנוכחית
                        var attributesMatch = attributesRegex.Matches(line);
                        foreach (Match attribute in attributesMatch)
                        {
                            var attributeName = attribute.Groups[1].Value;
                            var attributeValue = attribute.Groups[2].Value;
                            if (attributeName.ToLower() == "class")
                            {
                                // Split the class attribute into parts
                                newElement.Classes.AddRange(attributeValue.Split(' '));
                            }
                            else if (attributeName.ToLower() == "id")
                            {
                                newElement.Id = attributeValue;
                            }
                            newElement.Attributes.Add(attributeValue);
                        }
                        bool isSelfClosing = line.EndsWith("/>") || (HtmlHelper.Instance.SelfClosingTags.Contains(words[0]));
                        if (rootElement == null)
                        {
                            rootElement = newElement;
                        }
                        if (!isSelfClosing)
                        {
                            currentElement = newElement;
                        }
                    }
                }
                else
                {
                    if (currentElement != null)
                        currentElement.InnerHtml = line;
                }
            }
            return rootElement;
        }
    }

}
