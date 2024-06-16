using System;
using System.Collections.Generic;
using System.Linq;

namespace practicod2
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public List<Selector> Children { get; set; }
       
        public static Selector ParseQuery(string query)
        {
            var selectors = query.Split(' ');
            Selector root = null;
            Selector currentSelector = null;

            foreach (var selectorString in selectors)
            {
                var selector = ParseSelector(selectorString);
                if (root == null)
                {
                    root = selector;
                    currentSelector = root;
                }
                else
                {
                    currentSelector.Children = new List<Selector> { selector };
                    selector.Parent = currentSelector;
                    currentSelector = selector;
                }
            }

            return root;
        }

        private static Selector ParseSelector(string selectorString)
        {
            var selector = new Selector();

            var parts = selectorString.Split('.', '#');
            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                    continue;

                if (part.StartsWith("#"))
                {
                    selector.Id = part.Substring(1);
                }
                else if (part.StartsWith("."))
                {
                    if (selector.Classes == null)
                        selector.Classes = new List<string>();
                    selector.Classes.Add(part.Substring(1));
                }
                else
                {
                    selector.TagName = part;
                }
            }

            return selector;
        }
       
    }
}
