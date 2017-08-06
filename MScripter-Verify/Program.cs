using System;
using System.IO;

namespace MScripter.Verify {
    internal static class Program {
        internal static int Main(string[] args) {
            string[] files = null; int result = 0;
            int errors = 0, warnings = 0, notices = 0;
            ErrorType level = ErrorType.Note;

            if (args.Length == 1) {
                Console.WriteLine("Usage: MScripter-Verify <files>");
                Console.WriteLine();
                return 1;
            }

            for (int i = 0; i < args.Length; ++i) {
                if (args[i] == "--" || !args[i].StartsWith("-")) {
                    ++i;
                    files = new string[args.Length - i];
                    Array.Copy(args, i, files, 0, files.Length);
                    break;
                }
            }

            foreach (var file in files) {
                try {
                    var document = MslDocument.FromFile(file);

                    for (int lineIndex = 0; lineIndex < document.TextBox.LinesCount; ++lineIndex) {
                        var lineInfo = MslSyntaxHighlighter.Highlight(document.TextBox, lineIndex, document.Type);
                        string label = "Error  ";
                        foreach (var error in lineInfo.errors) {
                            switch (error.Type) {
                                case ErrorType.Error:
                                    label = "Error  ";
                                    ++errors;
                                    break;
                                case ErrorType.Warning:
                                    label = "Warning";
                                    ++warnings;
                                    break;
                                case ErrorType.Note:
                                    label = "Notice ";
                                    ++notices;
                                    break;
                            }
                            if (error.Type <= level) {
                                Console.WriteLine(label + " in file " + Path.GetFileName(document.Path) + ":" + (error.Location.FromLine + 1) + ": " + error.Text);
                                Console.WriteLine("  " + document.TextBox[error.Location.FromLine].Text);
                                Console.WriteLine(new string(' ', error.Location.Start.iChar + 2) + "^");
                                Console.WriteLine();

                                switch (error.Type) {
                                    case ErrorType.Error: if (result < 4) result = 4; break;
                                    case ErrorType.Warning: if (result < 3) result = 3; break;
                                    case ErrorType.Note: if (result < 2) result = 2; break;
                                }
                            }

                            while (document.lineInfoTable.TryGetValue(document.TextBox[lineIndex].UniqueId, out lineInfo) && lineInfo.State == (byte) MslSyntaxHighlighter.ParseStateIndex.Continuation)
                                ++lineIndex;
                        }
                    }
                } catch (IOException ex) {
                    Console.WriteLine("Error opening file " + Path.GetFileName(file) + ": " + ex.Message);
                    Console.WriteLine();
                    result = 5;
                }
            }

            Console.WriteLine($"Analysis complete: {errors} error(s), {warnings} warning(s), {notices} notice(s).");

            return result;
        }
    }
}
