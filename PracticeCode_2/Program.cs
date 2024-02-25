using PracticeCode_2;

HtmlSerializer s= new HtmlSerializer();
var html =await s.Load("https://learn.malkabruk.co.il/");
var rootHtml = s.Serializer(html);
var selector = Selector.FromQueryString("h1 #e");
var result = HtmlElementExtensions.FindElements(rootHtml, selector);
foreach (var item in result)
{
    Console.WriteLine(item.Id+" "+item.Name+" " + item.Classes[0]);
}
