using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueEditor
{
    public class ExporterCsv
    {
        //--------------------------------------------------------------------------------------------------------------
        // Helper Class

        public class CsvLineWriter
        {
            private StringBuilder text = new StringBuilder();
            private bool first = true;

            public void AddField(string field)
            {
                if (!first)
                    text.Append(",");
                first = false;

                text.Append("\"");
                text.Append(field.Replace("\"", "\"\""));
                text.Append("\"");
            }

            public void AddField(int field)
            {
                if (!first)
                    text.Append(",");
                first = false;

                text.Append(field);
            }

            public void AddField(DateTime field)
            {
                if (!first)
                    text.Append(",");
                first = false;

                text.Append(field.ToString("o"));   // "o" == "yyyy-MM-ddTHH:mm:ss.fffffffZ", useful to serialize/deserialize without loss and to keep some readability
            }

            public void AddEmptyField()
            {
                if (!first)
                    text.Append(",");
                first = false;
            }

            public void WriteLine(System.IO.StreamWriter file)
            {
                file.WriteLine(text.ToString());
            }
        }

        public class CsvFileReader
        {
            //Based on http://www.blackbeltcoder.com/Articles/files/reading-and-writing-csv-files-in-c
            //Updated interface for a better usability

            private enum EmptyLineBehavior
            {
                //Empty lines are interpreted as a line with zero columns.
                NoColumns,
                //Empty lines are interpreted as a line with a single empty column.
                EmptyColumn,
                //Empty lines are skipped over as though they did not exist.
                Ignore,
                //An empty line is interpreted as the end of the input file.
                EndOfFile,
            }

            private StreamReader file;
            private string CurrLine;
            private int CurrPos;
            private EmptyLineBehavior emptyLineBehavior = EmptyLineBehavior.Ignore;

            private List<string> headers = new List<string>();
            private Dictionary<string, string> redirectors = new Dictionary<string, string>();
            private Dictionary<string, string> lineCells = new Dictionary<string,string>();

            //These are special characters in CSV files. If a column contains any
            //of these characters, the entire column is wrapped in double quotes.
            protected char[] SpecialChars = new char[] { ',', '"', '\r', '\n' };

            //Indexes into SpecialChars for characters with specific meaning
            private const int DelimiterIndex = 0;
            private const int QuoteIndex = 1;

            //Gets/sets the character used for column delimiters.
            private char Delimiter
            {
                get { return SpecialChars[DelimiterIndex]; }
                set { SpecialChars[DelimiterIndex] = value; }
            }

            //Gets/sets the character used for column quotes.
            private char Quote
            {
                get { return SpecialChars[QuoteIndex]; }
                set { SpecialChars[QuoteIndex] = value; }
            }

            //Init the reader and parse headers
            public bool ParseHeaders(StreamReader inFile)
            {
                return ParseHeaders(inFile, new Dictionary<string, string>());
            }
            public bool ParseHeaders(StreamReader inFile, Dictionary<string, string> headerRedirects)
            {
                file = inFile;

                if (ParseNextLine_Impl(ref headers))
                {
                    redirectors = headerRedirects;
                    return true;
                }
                return false;
            }

            //Parse a new effective line and store its cells (handles multi-lined cells)
            public bool ParseNextLine()
            {
                lineCells.Clear();
                List<string> cells = new List<string>();
                if (ParseNextLine_Impl(ref cells))
                {
                    for (int i = 0; i < Math.Min(headers.Count, cells.Count); ++i)
                    {
                        lineCells.Add(headers[i], cells[i]);
                    }
                    return true;
                }
                return false;
            }

            //Get a cell value from the current line, considering headers redirection
            public string GetCell(string header)
            {
                if (redirectors.ContainsKey(header))
                {
                    header = redirectors[header];
                }
                if (lineCells.ContainsKey(header))
                {
                    return lineCells[header];
                }
                return String.Empty;
            }

            //GetCell variant for DateTime
            public DateTime GetCellAsDate(string header)
            {
                string value = GetCell(header);
                if (value != String.Empty)
                {
                    return DateTime.Parse(value, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind);
                }
                return DateTime.MinValue;
            }

            //Reads a row of columns from the current CSV file. Returns false if no
            //more data could be read because the end of the file was reached.
            private bool ParseNextLine_Impl(ref List<string> cells)
            {
                while (true)
                {
                    // Read next line from the file
                    CurrLine = file.ReadLine();
                    CurrPos = 0;

                    // Test for end of file
                    if (CurrLine == null)
                        return false;

                    // Test for empty line
                    if (CurrLine.Length == 0)
                    {
                        switch (emptyLineBehavior)
                        {
                            case EmptyLineBehavior.NoColumns:
                                cells.Clear();
                                return true;
                            case EmptyLineBehavior.Ignore:
                                continue;
                            case EmptyLineBehavior.EndOfFile:
                                return false;
                        }
                    }

                    // Parse line
                    string column;
                    int numColumns = 0;
                    while (true)
                    {
                        // Read next column
                        if (CurrPos < CurrLine.Length && CurrLine[CurrPos] == Quote)
                            column = ReadQuotedColumn();
                        else
                            column = ReadUnquotedColumn();

                        // Add column to list
                        if (numColumns < cells.Count)
                            cells[numColumns] = column;
                        else
                            cells.Add(column);
                        numColumns++;

                        // Break if we reached the end of the line
                        if (CurrLine == null || CurrPos == CurrLine.Length)
                            break;

                        // Otherwise skip delimiter
                        System.Diagnostics.Debug.Assert(CurrLine[CurrPos] == Delimiter);
                        CurrPos++;
                    }

                    // Remove any unused columns from collection
                    if (numColumns < cells.Count)
                        cells.RemoveRange(numColumns, cells.Count - numColumns);

                    return true;
                }
            }

            //Reads a quoted column by reading from the current line until a
            //closing quote is found or the end of the file is reached. On return,
            //the current position points to the delimiter or the end of the last
            //line in the file. Note: CurrLine may be set to null on return.
            private string ReadQuotedColumn()
            {
                // Skip opening quote character
                System.Diagnostics.Debug.Assert(CurrPos < CurrLine.Length && CurrLine[CurrPos] == Quote);
                CurrPos++;

                // Parse column
                StringBuilder builder = new StringBuilder();
                while (true)
                {
                    while (CurrPos == CurrLine.Length)
                    {
                        // End of line so attempt to read the next line
                        CurrLine = file.ReadLine();
                        CurrPos = 0;

                        // Done if we reached the end of the file
                        if (CurrLine == null)
                            return builder.ToString();

                        // Otherwise, treat as a multi-line field
                        builder.Append(Environment.NewLine);
                    }

                    // Test for quote character
                    if (CurrLine[CurrPos] == Quote)
                    {
                        // If two quotes, skip first and treat second as literal
                        int nextPos = (CurrPos + 1);
                        if (nextPos < CurrLine.Length && CurrLine[nextPos] == Quote)
                            CurrPos++;
                        else
                            break;  // Single quote ends quoted sequence
                    }

                    // Add current character to the column
                    builder.Append(CurrLine[CurrPos++]);
                }

                if (CurrPos < CurrLine.Length)
                {
                    // Consume closing quote
                    System.Diagnostics.Debug.Assert(CurrLine[CurrPos] == Quote);
                    CurrPos++;

                    // Append any additional characters appearing before next delimiter
                    builder.Append(ReadUnquotedColumn());
                }

                // Return column value
                return builder.ToString();
            }

            //Reads an unquoted column by reading from the current line until a
            //delimiter is found or the end of the line is reached. On return, the
            //current position points to the delimiter or the end of the current
            //line.
            private string ReadUnquotedColumn()
            {
                int startPos = CurrPos;
                CurrPos = CurrLine.IndexOf(Delimiter, CurrPos);
                if (CurrPos == -1)
                    CurrPos = CurrLine.Length;
                if (CurrPos > startPos)
                    return CurrLine.Substring(startPos, CurrPos - startPos);
                return String.Empty;
            }
        }
    }
}
