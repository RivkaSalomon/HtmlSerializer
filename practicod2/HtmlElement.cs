using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practicod2
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }
        public void AddChild(HtmlElement child)
        {
            if (Children == null)
            {
                Children = new List<HtmlElement>();
            }
            child.Parent = this;
            Children.Add(child);
        }
        // פונקצית Descendants
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var element = queue.Dequeue();
                yield return element;

                if (element.Children != null)
                {
                    foreach (var child in element.Children)
                    {
                        queue.Enqueue(child);
                    }
                }
            }
        }

        // פונקצית Ancestors
        public IEnumerable<HtmlElement> Ancestors()
        {
            var current = this.Parent;

            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }
        // פונקציה הממצאה אלמנטים על פי סלקטור
        public IEnumerable<HtmlElement> FindElementsBySelector(Selector selector)
        {
            HashSet<HtmlElement> resultSet = new HashSet<HtmlElement>();
            FindElementsBySelectorRecursive(this, selector, resultSet);
            return resultSet;
        }

        private void FindElementsBySelectorRecursive(HtmlElement element, Selector selector, HashSet<HtmlElement> resultSet)
        {
            if (element == null)
                return;

            // קוד קיים שבודק האם האלמנט תואם לסלקטור
            if (IsElementMatchSelector(element, selector))
            {
                resultSet.Add(element);
            }

            if (element.Children != null)
            {
                foreach (var child in element.Children)
                {
                    FindElementsBySelectorRecursive(child, selector, resultSet);
                }
            }
        }

        private bool IsElementMatchSelector(HtmlElement element, Selector selector)
        {
            // קוד קיים שבודק האם האלמנט תואם לסלקטור
            if (selector.TagName != null && element.Name != selector.TagName)
                return false;

            if (selector.Id != null && element.Id != selector.Id)
                return false;

            if (selector.Classes != null && !selector.Classes.All(c => element.Classes.Contains(c)))
                return false;

            return true;
        }
    }
}
