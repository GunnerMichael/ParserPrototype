using QueryMap;
using SqlDocumentRender;
using SqlParserWrapper;
using SqlParserWrapper.Model;
using System;
using System.IO;
using System.Linq;

namespace SQLParser
{
    class Program 
    {
        static void Main(string[] args)
        {

            //SqlParser parser = new SqlParser(new QueryMapper(new CsvRender(@"c:\michaelxx\testing-map.csv")));

            //SqlParser parser = new SqlParser(new QueryMapper(new MarkdownRender(@"c:\michaelxx\sample-map.md", "Sample Proc")));

            //parser.Parser(File.ReadAllText(@"C:\michaelxx\repos\SQLWalk\sampleproc.sql"));


            ///System.Diagnostics.Debug.ReadLine();
            ///

            //parser = new SqlParser(new QueryMapper(new MarkdownRender(@"c:\michaelxx\stageEvents-map.md", "StageEvents")));
            //parser.Parser(File.ReadAllText(@"C:\michaelxx\uspStageEvents.sql"));


            //Generate(
            //    @"\\sbsfiles\BI_ServiceCentre\Projects\Front End Bureau Migration\Michael Handover Documents\LoanApplication.sql",
            //    @"c:\michaelxx",
            //    "_ApplicationDataMart.LoanApplication");

            Generate(
                @"C:\michaelxx\_applicationDataMart.property.sql",
                @"c:\michaelxx",
            "_ApplicationDataMart.Property");

            Generate(
                @"C:\michaelxx\uspDimMortgageApplication.sql",
                @"c:\michaelxx",
                "DW.dimMortgageApplication");

            Generate(
                @"C:\michaelxx\uspStageEvents.sql",
                @"c:\michaelxx",
                "ServiceTick.StageEvents");

        }


        public static void Generate(string sourceFile, string outputFolder, string title)
        {
            SqlParser parser = new SqlParser(new QueryMapper());
            ParsedContainer container = parser.Parser(File.ReadAllText(sourceFile));
            QueryMapContainerBuilder builder = new QueryMapContainerBuilder();
            var output = builder.Build(container);

            string fileName = $"{outputFolder}\\{title}.md";

            new ColumnMappingMarkdownRender().CreateOutput(output,fileName, title,false);

        }

    }
}
