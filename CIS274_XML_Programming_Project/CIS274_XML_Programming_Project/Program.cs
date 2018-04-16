/*
 * Student: Adam Wight
 * Class: CIS274M XML Programming
 * Instructor: Ed Cauthorn
 */

using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Saxon.Api;
using CIS274_XML_Programming_Project.Conversion;

namespace CIS274_XML_Programming_Project
{
    class Program
    {
        // Keeping paths here is (much) easier than writing an actual user interface, and it's functional enough for this project
        private static string _rootPath = @"H:\Projects\School\CIS274-XML-Programming\CIS274_XML_Programming_Project\CIS274_XML_Programming_Project";

        private static string _currentWeek = "Week11";

        private static string _outputFolderPath = @"ScriptOutput\";
        private static string _currentWeekOutputPath = $@"ScriptOutput\{_currentWeek}.xml";
        private static string _currentWeekOutputFolderPath = $@"ScriptOutput\{_currentWeek}\";

        private static string _xsltPath = @"Resources\XML\XSLT\SummerBase.xslt";
        private static string _schedulePath= @"Resources\XML\SummerBase.xml";
        private static string _xQueryPath = @"Resources\XML\XQuery";

        private static string _xmlFolderPath = @"Resources\XML";
        private static string _csvFolderPath = @"Resources\CSV";
        private static string _textFolderPath = @"Resources\Text";

        internal static string[] supportedConversionFormats = { ".csv", ".txt", ".xml" };

        static void Main(string[] args)
        {
            Environment.CurrentDirectory = _rootPath;
            
            ContinuePrompt();
        }

        public static XmlDocument PerformQuery(XmlDocument input, string query)
        {
            var processor = new Processor();
            XdmNode inputXml = processor.NewDocumentBuilder().Build(new XmlNodeReader(input));
            XQueryCompiler compiler = processor.NewXQueryCompiler();
            XQueryExecutable executable = compiler.Compile(query);
            XQueryEvaluator evaluator = executable.Load();
            evaluator.ContextItem = inputXml;
            var domOut = new DomDestination();
            evaluator.Run(domOut);
            return domOut.XmlDocument;
        }

        public static XmlDocument PerformQuery(string inputPath, string query)
        {
            var input = new XmlDocument();
            input.Load(inputPath);
            
            return PerformQuery(input, query);
        }

        /// <summary>
        /// Creates an XDocument with a set of documents from the given <paramref name="inputFolderPath"/> and saves it to the <paramref name="outputPath"/>.
        /// </summary>
        public static void ConvertDocumentsToXml(string extension, string inputFolderPath, string outputPath)
        {
            if (!extension.StartsWith("."))
            {
                extension = $".{extension}";
            }

            XElement rootElement;
            ToXml converter;

            switch (extension)
            {
                case ".txt":
                    rootElement = new XElement("DocumentSet");
                    converter = new TxtToXml();
                    break;
                case ".csv":
                    rootElement = new XElement("SheetSet");
                    converter = new CsvToXml();
                    break;
                case ".xml":
                    rootElement = new XElement("XmlSet");
                    converter = new ToXmlViaXslt(_xsltPath);
                    break;
                default:
                    throw new NotSupportedException($"{extension} is not a supported file format\r\n" +
                                                    $"Supported formats: {supportedConversionFormats.ShowElements()}.");
            }

            string[] files = Directory.GetFiles(inputFolderPath);
            if (files
                .Where(file => Path.HasExtension(file))
                .All(file => Path.GetExtension(file) == extension))
            {
                var aggregator = new FileAggregator(inputFolderPath);

                foreach (string file in files)
                {
                    var doc = new XDocument();
                    doc.Add(aggregator.ConvertFilesToXml(rootElement, converter));
                    doc.Save(outputPath);
                }
            }
            else
            {
                throw new NotSupportedException($"{inputFolderPath} must only contain files with this extension: {extension} and subfolders");
            }
        }

        /// <summary>
        /// Takes a <paramref name="inputPath"/> and converts using the XSLT at <paramref name="xsltPath"/>, then outputs it to <paramref name="outputPath"/>
        /// </summary>
        /// <param name="inputPath">An XML file.</param>
        /// <param name="xsltPath">An XSLT file.</param>
        /// <param name="outputPath">The file location for the output file.</param>
        public static void PerformXslTransform(string inputPath, string xsltPath, string outputPath)
        {
            var transformer = new XslCompiledTransform();
            transformer.Load(xsltPath);
            transformer.Transform(inputPath, outputPath);
        }

        /// <summary>
        /// Takes an input XML file from <paramref name="inputPath"/> and extracts some information from it using the XQuery(s) at <paramref name="queryFolder"/>, then outputs the result of each query to <paramref name="outputFolderPath"/>
        /// </summary>
        /// <param name="inputPath">An XML file of a course schedule.</param>
        /// <param name="queryFolder">A folder containing XQuery (.xqy) files to be executed on <paramref name="inputPath"/>.</param>
        /// <param name="outputFolderPath">The location to output query results at.</param>
        public static void PerformQueriesInFolder(string inputPath, string queryFolder, string outputFolderPath)
        {
            string[] queryFiles = Directory.GetFiles(queryFolder);
            foreach(string path in queryFiles)
            {
                using (var reader = new StreamReader(path))
                {
                    PerformQuery(inputPath, reader.ReadToEnd())
                        .Save(outputFolderPath + Path.GetFileName(path) + "-results.xml");
                }
            }
        }

        /// <summary>
        /// Gets a new document (poem) from wikisource.org via a GET HTTPrequest, then parses the HTTPresponse.
        /// </summary>
        public static void GetNewDocumentFromWeb(string outputFolderPath)
        {
            var handler = new WebHandler();
            handler.GetResponse(handler.SendRequest(@"https://en.wikisource.org/wiki/Ozymandias_(Shelley)", "GET"));
            var converter = new WikiTextPuller();
            converter.ConvertWikiPoemToPlainText(handler.SavedResponse, outputFolderPath);
        }

        /// <summary>
        /// Uses the XML DOM to find and remove all elements matching <paramref name="identifiers"/> (which are either a DocumentTitle element or an ID attribute)
        /// </summary>
        /// <param name="identifiers">A set of identifiers for XML nodes; each string can be an ID attribute value OR a DocumentTitle element value</param>
        public static void RemoveDocumentsViaDom(string[] identifiers)
        {
            var doc = new XmlDocument();
            string inputPath = @"H:\Projects\School\CIS274 XML Programming\Resources\XML\week6.xml";
            doc.Load(inputPath);

            XmlNode root = doc.DocumentElement;

            var handler = new DomHandler(doc, root);

            foreach (string id in identifiers)
            {
                XmlNode node = handler.FindNode(id);
                if (node != null)
                {
                    root.RemoveChild(node);
                }
                else
                {
                    Console.WriteLine($"Could not find any matches to ID: {id} in {inputPath}");
                }
            }

            doc.Save(_currentWeekOutputPath);
        }

        /// <summary>
        /// Prompts the user to continue by pressing enter.
        /// </summary>
        public static void ContinuePrompt()
        {
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}