// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;
using System.Text.Json;
using practicod2;
async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
var html =await Load("https://hebrewbooks.org/beis");
var cleanHtml = new Regex("\\s").Replace(html, "");
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(a => a.Length > 0);
var htmlElement = "<div id=\"my-id\" class= \"my=class-1 my-class-2\" width=\"100%\">text</div>";
var attributes=new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlElement);
//foreach (var attribute in HtmlHelper.Instance.array)
//{
//   Console.WriteLine(attribute);
//
HtmlHelper htmlHelper = HtmlHelper.Instance;
HtmlElement root = htmlHelper.BuildTree(htmlLines.ToList());
string query = "div #mydiv .class-name";
Selector rootSelector = Selector.ParseQuery(query);

Console.ReadLine();
