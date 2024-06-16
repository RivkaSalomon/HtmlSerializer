using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Xml.Linq;
using System.Text.RegularExpressions;
namespace practicod2
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance=new HtmlHelper();
        public static HtmlHelper Instance => _instance;   
        public string[] array1 { get; set; }
        public string[] array2 { get; set; }

        private HtmlHelper()
        {
            array1 = Load("HtmlTags.json");
            array2= Load("HtmlVoidTags.json");
        }
        private string[] Load(string path)
        {
            try
            {
                var contect = File.ReadAllText("HtmlTags.json");
                string[] arr = JsonSerializer.Deserialize<string[]>(contect);
                return arr;
            }
            catch (Exception e)
            {
                Console.WriteLine(  e);
                return null;
            }
        }
        public HtmlElement BuildTree(List<string> htmlStrings)
        {
            HtmlElement root = new HtmlElement();
            HtmlElement currentElement = root;

            foreach (string htmlString in htmlStrings)
            {
                string firstWord = htmlString.Split(' ')[0];

                if (firstWord == "html/")
                {
                    // End of HTML
                    break;
                }
                else if (firstWord.StartsWith("/"))
                {
                    // Closing tag - go to the previous level in the tree
                    currentElement = currentElement.Parent;
                }
                else
                {
                    // Create a new object and add it to the Children list of the current element
                    HtmlElement newElement = new HtmlElement();
                    currentElement.AddChild(newElement);

                    newElement.Name = firstWord.ToLower();
                    // Extract the rest of the string and create the Attributes list
                    // Extract the rest of the string and create the Attributes list
                    string restOfString = htmlString.Substring(firstWord.Length).Trim();

                    // Use a Regular Expression to parse the continuation of the string correctly
                    var attributeMatches = Regex.Matches(restOfString, "([^\\s]*?)=\"(.*?)\"");

                    foreach (Match match in attributeMatches)
                    {
                        string attributeName = match.Groups[1].Value;
                        string attributeValue = match.Groups[2].Value;

                        // Update the newElement properties accordingly
                        if (attributeName.ToLower() == "class")
                        {
                            // If the attribute is 'class', split it into parts according to space and update the Classes property
                            newElement.Classes = attributeValue.Split(' ').ToList();
                        }
                        else if (attributeName.ToLower() == "id")
                        {
                            // If the attribute is 'id', update the Id property
                            newElement.Id = attributeValue;
                        }
                        else
                        {
                            // For other attributes, add them to the Attributes list
                            newElement.Attributes ??= new List<string>();
                            newElement.Attributes.Add($"{attributeName}=\"{attributeValue}\"");
                        }
                    }

                    // Extract InnerHtml using a regular expression
                    var innerHtmlMatch = Regex.Match(restOfString, ">(.*?)<");

                    if (innerHtmlMatch.Success)
                    {
                        newElement.InnerHtml = innerHtmlMatch.Groups[1].Value;
                    }
                    // Check if the tag is self-closing
                    if (htmlString.EndsWith("/") || Array.Exists(array2, tag => tag == firstWord))
                    {
                        //סימן שזה תג עם מילה אחת
                        // Leave the current element as it is
                    }
                    else
                    {
                        // Separate closing tag - put the new object in the current element
                        //סימן שאולי יש לה ילדים
                        currentElement = newElement;
                    }
                }
            }

            return root;
        }

    }
}
