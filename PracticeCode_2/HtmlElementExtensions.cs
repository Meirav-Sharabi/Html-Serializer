using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeCode_2
{
    public static class HtmlElementExtensions
    {
        public static List<HtmlElement> FindElements(this HtmlElement root, Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>(); // HashSet כדי למנוע כפילויות
            FindElementsRecursive(root, selector, result);
            return new List<HtmlElement>(result);
        }
        // פונקציה רקורסיבית לחיפוש אלמנטים בעץ על פי הסלקטור
        private static void FindElementsRecursive(HtmlElement element, Selector selector, HashSet<HtmlElement> result)
        {
            if (selector == null)
                return;
            if (SelectorMatchesElement(element, selector))
            {
                result.Add(element);
                selector = selector.Child;
            }
            foreach (var child in element.Children)
            {
                FindElementsRecursive(child, selector, result);
            }
        }
        private static bool SelectorMatchesElement(HtmlElement element, Selector selector)
        {
            // בדיקה האם שם התג של האלמנט מתאים לשם התג של הסלקטור
            if (!string.IsNullOrEmpty(selector.TagName) && element.Name != selector.TagName)
            {
                return false;
            }

            // בדיקה האם ה-ID של האלמנט מתאים ל-ID של הסלקטור
            if (!string.IsNullOrEmpty(selector.id) && element.Id != selector.id)
            {
                return false;
            }

            // בדיקה האם האלמנט מכיל את כל ה-classes שצוינו בסלקטור
            foreach (var cls in selector.Classes)
            {
                if (!element.Classes.Contains(cls))
                {
                    return false;
                }
            }

            // אם כל התנאים מתקיימים, האלמנט מתאים לסלקטור
            return true;
        }

    }
}
