using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using FastColoredTextBoxNS;

namespace MIDE {
    public class MslDocument {
        /// <summary>Returns the text in this document.</summary>
        public string Text {
            get { return this.textBoxes[0].Text; }
            private set { this.textBoxes[0].Text = value; }
        }
        /// <summary>Returns or sets the file path of this document.</summary>
        public string Path { get; set; }
        /// <summary>Returns or sets a value indicating whether the file is in INI format.</summary>
        public bool IsINI { get; set; }
        /// <summary>Returns or sets the type of the document.</summary>
        public DocumentType Type { get; set; }
        /// <summary>Returns the type of the document as of the last save.</summary>
        public DocumentType OriginalType { get; private set; }
        /// <summary>Returns or sets a value indicating whether this document is read-only.</summary>
        public bool ReadOnly { get; set; }
        /// <summary>Returns or sets the handle to the mIRC window that this document should be synchronised with.</summary>
        public IntPtr SyncTarget { get; set; }

        /// <summary>Returns or sets a value indicating whether the document has unsaved changes.</summary>
        public bool Modified => this.textBoxes[0].IsChanged;
        /// <summary>Returns or sets a value indicating whether the document is associated with a file.</summary>
        public bool Saved { get; set; }

        public FastColoredTextBox[] textBoxes = new FastColoredTextBox[2];
        public int SelectedIndex { get; set; }
        public FastColoredTextBox SelectedTextBox {
            get { return this.textBoxes[this.SelectedIndex]; }
            set {
                int index = Array.IndexOf(this.textBoxes, value);
                if (index == -1) throw new ArgumentException("The given text box does not belong to this document.");
                this.SelectedIndex = index;
            }
        }
        public AutocompleteMenu autoCompleteMenu;

        public List<MslBookmark> bookmarks { get; } = new List<MslBookmark>();
        public List<Error> errors { get; } = new List<Error>();
        public Dictionary<int, LineInfo> lineInfoTable { get; } = new Dictionary<int, LineInfo>(256);
        internal int lineCount => this.textBoxes[0].LinesCount;

        public FastColoredTextBox TextBox {
            get { return this.textBoxes[0]; }
            set {
                this.textBoxes[0] = value;
                this.textBoxes[0].TextChanged += TextBox_TextChanged;

                this.textBoxes[0].ClearStyle(StyleIndex.All);
                this.textBoxes[0].ClearStylesBuffer();
            }
        }

        public EventHandler<TextChangedEventArgs> TextChanged;
        public EventHandler<BookmarkEventArgs> BookmarkChanged;

        public MslDocument(DocumentType type) : this(type, null, false, false) { }
        public MslDocument(DocumentType type, string path, bool isINI, bool readOnly) {
            this.Type = type;
            this.Path = path;
            this.IsINI = isINI;
            this.ReadOnly = readOnly;
            this.textBoxes[0] = new FastColoredTextBox() { Tag = this };
        }
        public MslDocument(DocumentType type, string path, bool isINI, bool readOnly, IntPtr syncTarget, bool saved) {
            this.Type = type;
            this.Path = path;
            this.IsINI = isINI;
            this.ReadOnly = readOnly;
            this.SyncTarget = syncTarget;
            this.Saved = saved;
            this.textBoxes[0] = new FastColoredTextBox() { Tag = this };
        }
        public MslDocument(DocumentType type, string path, bool isINI, bool readOnly, IntPtr syncTarget, bool saved, string text)
            : this(type, path, isINI, readOnly, syncTarget, saved) {
            this.Text = text;
        }

        public static MslDocument FromFile(string path) {
            var document = FromFile(path, DocumentType.RemoteScript, false, IntPtr.Zero);
            document.DetectType();
            return document;
        }
        public static MslDocument FromFile(string path, DocumentType type)
            => FromFile(path, type, false, IntPtr.Zero);
        public static MslDocument FromFile(string path, DocumentType type, IntPtr syncTarget)
            => FromFile(path, type, false, syncTarget);
        public static MslDocument FromFile(string path, DocumentType type, bool readOnly)
            => FromFile(path, type, readOnly, IntPtr.Zero);
        public static MslDocument FromFile(string path, DocumentType type, bool readOnly, IntPtr syncTarget) {
            bool INI = path.EndsWith(".ini");

            // Open the file.
            StreamReader reader = new StreamReader(File.Open(path, FileMode.Open, FileAccess.Read));

            var document = new MslDocument(type, path, INI, readOnly, syncTarget, true, String.Empty);
            var builder = new StringBuilder();

            while (!reader.EndOfStream) {
                var line = reader.ReadLine();

                if (INI && document.TextBox.LinesCount == 0) {
                    // Read the INI section header.
                    line = line.Trim();
                    if (line.Length != 0) {
                        if (line[0] == '[' && line.EndsWith("]")) {
                            line = line.Substring(1, line.Length - 2).Trim();
                            if (line.Equals("script", StringComparison.OrdinalIgnoreCase))
                                type = DocumentType.RemoteScript;
                            else if (line.Equals("aliases", StringComparison.OrdinalIgnoreCase))
                                type = DocumentType.AliasScript;
                            else if (line.Equals("popups", StringComparison.OrdinalIgnoreCase))
                                type = DocumentType.Popup;
                            else if (line.Equals("users", StringComparison.OrdinalIgnoreCase))
                                type = DocumentType.Users;
                            else if (line.Equals("vars", StringComparison.OrdinalIgnoreCase))
                                type = DocumentType.Variables;

                            continue;
                        }
                    }
                }

                // Read the text.
                if (INI) {
                    var delimiter = line.IndexOf('=');
                    if (delimiter != -1)
                        line = line.Substring(delimiter + 1);
                }
                builder.AppendLine(line);
            }

            document.TextBox.Text = builder.ToString();
            document.TextBox.ClearUndo();
            document.TextBox.IsChanged = false;

            // Finish up.
            reader.Close();
            return document;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            mSLSyntaxHighlight(textBoxes[0], e.ChangedRange, this.Type);
        }

        private void mSLSyntaxHighlight(FastColoredTextBox sender, Range range, DocumentType syntaxType) {
            range.ClearStyle(MslSyntaxHighlighter.CommandStyle,
                             MslSyntaxHighlighter.CommentStyle,
                             MslSyntaxHighlighter.CustomCommandStyle,
                             MslSyntaxHighlighter.ErrorStyle,
                             MslSyntaxHighlighter.FunctionStyle,
                             MslSyntaxHighlighter.FunctionPropertyStyle,
                             MslSyntaxHighlighter.KeywordStyle,
                             MslSyntaxHighlighter.AliasStyle,
                             MslSyntaxHighlighter.VariableStyle);

            // Highlight the text.
            bool bookmarksChanged = false; bool errorsChanged = false;
            // If the changed line was a continuation, find the start.
            int i = range.FromLine;
            LineInfo lineInfo;
            while (i != 0 && this.lineInfoTable.TryGetValue(this.TextBox[i - 1].UniqueId, out lineInfo) && lineInfo.State == (byte) MslSyntaxHighlighter.ParseStateIndex.Continuation)
                --i;

            for (; i < range.tb.LinesCount; ++i) {
                LineInfo oldLineInfo; MslBookmark oldBookmark = null; string oldTitle = null;
                if (this.lineInfoTable.TryGetValue(range.tb[i].UniqueId, out oldLineInfo)) {
                    oldBookmark = oldLineInfo.bookmark;
                    oldTitle = oldBookmark?.title;
                }

                var newLineInfo = MslSyntaxHighlighter.Highlight(range.tb, i, syntaxType);

                if (this.Type != DocumentType.Text) {
                    // Deal with bookmarks.
                    if (oldBookmark != null) {
                        if (newLineInfo.bookmark != null) {
                            if (oldBookmark != newLineInfo.bookmark) {
                                // A bookmark was replaced.
                                this.bookmarks[this.bookmarks.IndexOf(oldBookmark)] = newLineInfo.bookmark;
                                bookmarksChanged = true;
                            } else if (oldTitle != newLineInfo.bookmark.title) {
                                // A bookmark was renamed.
                                this.BookmarkChanged?.Invoke(this, new BookmarkEventArgs(newLineInfo.bookmark));
                            }
                        } else {
                            // A bookmark was removed.
                            this.bookmarks.Remove(oldLineInfo.bookmark);
                            bookmarksChanged = true;
                        }
                    } else {
                        if (newLineInfo?.bookmark != null) {
                            // A bookmark was added.
                            this.bookmarks.Add(newLineInfo.bookmark);
                            bookmarksChanged = true;
                        }
                    }
                }

                if (this.Type != DocumentType.Text && this.Type != DocumentType.INI) {

                    if (oldLineInfo != null && oldLineInfo.errors.Length != 0) {
                        errorsChanged = true;
                        foreach (var error in oldLineInfo.errors)
                            this.errors.Remove(error);
                    }

                    // Skip past continued lines.
                    while (i < range.tb.LinesCount && this.lineInfoTable.TryGetValue(this.TextBox[i].UniqueId, out lineInfo) && lineInfo.State == (byte) MslSyntaxHighlighter.ParseStateIndex.Continuation) {
                        // Always reparse the next line if there is a continuation.
                        // This avoids glitches when continuations are added or removed.
                        oldLineInfo = null;
                        ++i;

                        if (lineInfo.errors.Length != 0) errorsChanged = true;
                        foreach (Error error in lineInfo.errors)
                            this.errors.Remove(error);
                    }

                    // Deal with errors.
                    if (newLineInfo != null && newLineInfo.errors.Length != 0) {
                        errorsChanged = true;
                        foreach (var error in newLineInfo.errors)
                            this.errors.Add(error);
                    }

                    // (Normally we only reparse the next line if it was changed, or the current line was changed in such
                    //  a way as to affect its meaning, such as adding braces.)
                    if (i >= range.ToLine && oldLineInfo != null &&
                        newLineInfo.BraceLevel == oldLineInfo.BraceLevel && newLineInfo.State == oldLineInfo.State) break;
                } else {
                    // Such cascading isn't an issue for INI files.
                    if (i >= range.ToLine) break;
                }
            }

            if (bookmarksChanged) {
                //functionList.Items.Clear();
                //if (currentItem == document) RebuildFunctionList();
            }

            if (errorsChanged) {
                //errorList.Items.Clear();
                //RebuildErrorList();
            }

            range.SetStyle(ControlCharStyle.Instance, @"[\x01-\x03\x0F\x13\x16\x1C\x1F]");

            range.ClearFoldingMarkers();
            range.SetFoldingMarkers("(?<![^ :\r\n]){(?![^ \r\n])", "(?<![^ \r\n])}(?![^ \r\n])");

            this.TextChanged?.Invoke(this, new TextChangedEventArgs(range));
        }

        public DocumentType DetectType() {
            bool comment = false;

            this.Type = DocumentType.RemoteScript;
            foreach (var line in this.TextBox.Lines) {
                var line2 = line.Trim();
                if (line2 == string.Empty) continue;

                if (comment) {
                    if (line2.StartsWith("*/"))
                        comment = false;
                } else {
                    if (line2.StartsWith("/*"))
                        comment = true;
                    else if (line2.StartsWith(";"))
                        continue;
                    else if (line2.StartsWith("%")) {
                        this.Type = DocumentType.Variables;
                        break;
                    } else if (line2.StartsWith(".")) {
                        this.Type = DocumentType.Popup;
                        break;
                    } else if (line2.StartsWith("on "    , StringComparison.OrdinalIgnoreCase) ||
                               line2.StartsWith("alias " , StringComparison.OrdinalIgnoreCase) ||
                               line2.StartsWith("menu "  , StringComparison.OrdinalIgnoreCase) ||
                               line2.StartsWith("dialog ", StringComparison.OrdinalIgnoreCase) ||
                               line2.StartsWith("raw "   , StringComparison.OrdinalIgnoreCase) ||
                               line2.StartsWith("ctcp "  , StringComparison.OrdinalIgnoreCase)) {
                        this.Type = DocumentType.RemoteScript;
                        break;
                    } else if (line2.IndexOf(' ') != -1) {
                        this.Type = DocumentType.AliasScript;
                        break;
                    }
                }
            }
            return this.Type;
        }

        public void Save() {
            this.Save(this.Path, this.IsINI);
            this.Saved = true;
        }
        public void Save(string path, bool INI) {
            // Open the file.
            using (var writer = new StreamWriter(File.Open(path, FileMode.Create, FileAccess.Write))) {
                int n = 1;

                // Write the text.
                if (INI) {
                    switch (this.Type) {
                        case DocumentType.RemoteScript: writer.WriteLine("[script]" ); break;
                        case DocumentType.AliasScript : writer.WriteLine("[aliases]"); break;
                        case DocumentType.Popup       : writer.WriteLine("[popups]" ); break;
                        case DocumentType.Users       : writer.WriteLine("[users]"  ); break;
                        case DocumentType.Variables   : writer.WriteLine("[vars]"   ); break;
                        default: writer.WriteLine("[script]"); break;
                    }
                }
                foreach (string line in this.TextBox.Lines) {
                    if (INI) {
                        writer.Write("n");
                        writer.Write(n);
                        writer.Write("=");
                        ++n;
                    }
                    writer.WriteLine(line);
                }
            }
        }

        public int EndOfBlock(int lineIndex) {
            int i;
            for (i = lineIndex; i < this.TextBox.LinesCount; ++i) {
                LineInfo lineInfo;
                if (this.lineInfoTable.TryGetValue(this.TextBox[i].UniqueId, out lineInfo) && lineInfo.BraceLevel == 0) return i;
            }
            return i - 1;
        }

        public Tag GetTag(Place position, params TagType[] types) {
            var lineInfoTable = this.lineInfoTable;
            LineInfo lineInfo;
            if (lineInfoTable.TryGetValue(this.TextBox[position.iLine].UniqueId, out lineInfo)) {
                foreach (var tag in lineInfo.tags) {
                    if ((types == null || types.Length == 0 || types.Contains(tag.type)) && tag.range.Contains(position)) {
                        return tag;
                    }
                }
            }
            return null;
        }

        public Tag GetTag(Range position, params TagType[] types) {
            var lineInfoTable = this.lineInfoTable;
            int lineNumber = position.Start.iLine;
            if (position.End.iLine == lineNumber) {
                LineInfo lineInfo;
                if (lineInfoTable.TryGetValue(this.TextBox[lineNumber].UniqueId, out lineInfo)) {
                    foreach (var tag in lineInfo.tags) {
                        if ((types == null || types.Length == 0 || types.Contains(tag.type)) &&
                            tag.range.Contains(position.Start) && tag.range.Contains(position.End)) {
                            return tag;
                        }
                    }
                }
            }
            return null;
        }

        public Tag GetSelectedTag(params TagType[] types) => GetTag(this.TextBox.Selection, types);

        public LineInfo GetLineInfo(int lineIndex) {
            LineInfo result;
            this.lineInfoTable.TryGetValue(this.TextBox[lineIndex].UniqueId, out result);
            return result;
        }

        public MslBookmark GetBookmark(int lineNumber) {
            MslBookmark bookmark;
            switch (this.Type) {
                case DocumentType.Text: return null;  // Those don't have bookmarks.
                case DocumentType.INI:
                    for (int i = lineNumber; i >= 0; --i) {
                        bookmark = GetLineInfo(i).bookmark;
                        if (bookmark != null) return bookmark;
                    }
                    return null;
                case DocumentType.RemoteScript:
                case DocumentType.AliasScript:
                    // Is there a bookmark on the current line?
                    bookmark = GetLineInfo(lineNumber).bookmark;
                    if (bookmark != null) return bookmark;

                    // We continue from the line above the starting line.
                    // This allows a result to be found if there is a '}' on the starting line.
                    for (int i = lineNumber - 1; i >= 0; --i) {
                        var lineInfo = GetLineInfo(i);
                        if (lineInfo.BraceLevel <= 0) break;
                        if (lineInfo.bookmark != null) return lineInfo.bookmark;
                    }
                    return null;
                default: return null;
            }
        }
    }

    public class BookmarkEventArgs {
        public MslBookmark Bookmark { get; }

        public BookmarkEventArgs(MslBookmark bookmark) {
            this.Bookmark = bookmark;
        }
    }

    public enum DocumentType {
        RemoteScript,
        AliasScript,
        Popup,
        Variables,
        Users,
        ConsoleInput,
        Expression,
        Text,
        INI
    }
}
