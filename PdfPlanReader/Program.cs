// See https://aka.ms/new-console-template for more information
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig;
using PdfPlanReader;
using System.Text.Json.Serialization;
using System.Text.Json;

Console.WriteLine("Hello, World!");
var lst = new List<ExtractedData>();
using (var pdf = PdfDocument.Open(@"c:\Users\jalki\Downloads\FlightGroup.pdf"))
{
    foreach (var page in pdf.GetPages())
    {
        var text = ContentOrderTextExtractor.GetText(page);
        //var rawText = page.Text;

        //Console.WriteLine(text);
        var ed = ExtractedData.ReadFromPage(text);
        if(ed!=null)
            lst.Add(ed); 
    }
}
foreach(var ed in lst)
{
    Console.WriteLine(JsonSerializer.Serialize(ed));
}

Console.ReadKey();
