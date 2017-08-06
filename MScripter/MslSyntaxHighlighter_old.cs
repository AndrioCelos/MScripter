using System;
using System.Collections.Generic;

using FastColoredTextBoxNS;

namespace MScripter {
    public static class OldMslSyntaxHighlighter {
        public static Style CommentStyle;
        public static Style KeywordStyle;
        public static Style CommandStyle;
        public static Style CustomCommandStyle;
        public static Style FunctionStyle;
        public static Style CustomFunctionStyle;
        public static Style FunctionPropertyStyle;
        public static Style VariableStyle;
        public static Style AliasStyle;
        public static Style ErrorStyle;
        public static Style WarningStyle;

        public delegate void ParseParameterDelegate(int lineIndex, ref int i, ref int state, ref int braceLevel, FastColoredTextBox textBox, MslBookmark bookmark, List<Error> errors);

        public static Dictionary<string, Event> events = new Dictionary<string, Event>(StringComparer.OrdinalIgnoreCase) {
            { "ACTION"     , new Event("Triggers when you receive an action message in a channel or in private.",
                                           new ParseParameterDelegate[] { ParameterParser.TextPattern, ParameterParser.TextScope }) },
            { "ACTIVE"     , new Event("Triggers when a window in mIRC is activated.",
                                           new ParseParameterDelegate[] { ParameterParser.WindowType }) },
            { "AGENT"      , new Event("Triggers when a speech agent finishes speaking.",
                                           new ParseParameterDelegate[0]) },
            { "APPACTIVE"  , new Event("Triggers when the mIRC main window is activated.",
                                           new ParseParameterDelegate[0]) },
            { "BAN"        , new Event("Triggers when a ban is set on a channel.",
                                           new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "CHAT"       , new Event("Triggers when you receive a message in a DCC CHAT session.",
                                           new ParseParameterDelegate[] { ParameterParser.TextPattern }) },
            { "CLOSE"      , new Event("Triggers when a window in mIRC is closed.",
                                           new ParseParameterDelegate[] { ParameterParser.WindowType, ParameterParser.TextPattern }) },
            { "CONNECT"    , new Event("Triggers when mIRC has logged in to an IRC server.",
                                           new ParseParameterDelegate[0]) },
            { "CONNECTFAIL", new Event("Triggers when an attempt and all retries to connect to an IRC server fail.",
                                           new ParseParameterDelegate[0]) },
            { "CTCPREPLY"  , new Event("Triggers when you receive a CTCP reply.",
                                           new ParseParameterDelegate[] { ParameterParser.TextPattern }) },
            { "DCCSERVER"  , new Event("Triggers when someone tries to connect to your DCC server.",
                                           new ParseParameterDelegate[] { ParameterParser.DCCType }) },
            { "DEHELP"     , new Event("Triggers when a user loses channel half-operator status.",
                                           new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "DEOP"       , new Event("Triggers when a user loses channel operator status.",
                                           new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "DEVOICE"    , new Event("Triggers when a user loses channel voice.",
                                           new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "DIALOG"     , new Event("Triggers when something happens to a control in a dialog.",
                                           new ParseParameterDelegate[] { ParameterParser.Anything, ParameterParser.DialogEvent, ParameterParser.DialogID }) },
            { "DISCONNECT" , new Event("Triggers when mIRC has disconnected from an IRC server.",
                                           new ParseParameterDelegate[0]) },
            { "DNS"        , new Event("Triggers when a DNS query started using /dns succeeds or fails.",
                                           new ParseParameterDelegate[0]) },
            { "ERROR"      , new Event("Triggers when you receive an IRC ERROR message from the server.",
                                           new ParseParameterDelegate[] { ParameterParser.TextPattern }) },
            { "EXIT"       , new Event("Triggers when mIRC is closing.",
                                           new ParseParameterDelegate[0]) },
            { "FILESENT"   , new Event("Triggers when you successfully finish sending a file via DCC SEND.",
                                           new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "FILERCVD"   , new Event("Triggers when you successfully finish receiving a file via DCC GET.",
                                           new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "GETFAIL"    , new Event("Triggers when a DCC GET transfer fails.",
                                           new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "HELP"       , new Event("Triggers when a user gains channel half-operator status.",
                                           new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "HOTLINK"    , new Event("Triggers when the mouse is moved over a word in a mIRC window.",
                                           new ParseParameterDelegate[] { ParameterParser.TextPattern, ParameterParser.WindowType }) },
            { "INPUT"      , new Event("Triggers when you enter text in a mIRC window's input box.",
                                           new ParseParameterDelegate[] { ParameterParser.WindowType }) },
            { "INVITE"     , new Event("Triggers when you are invited to a channel.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "JOIN"       , new Event("Triggers when someone joins a channel you are on.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "KEYDOWN"    , new Event("Triggers when a key is pressed or as one is held down in a custom window.",
                                       new ParseParameterDelegate[] { ParameterParser.CustomWindow }) },
            { "KEYUP"      , new Event("Triggers when a key is released in a custom window.",
                                       new ParseParameterDelegate[] { ParameterParser.CustomWindow }) },
            { "KICK"       , new Event("Triggers when someone is kicked out of a channel you are on.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "LOAD"       , new Event("Triggers when the script is first loaded.",
                                       new ParseParameterDelegate[0]) },
            { "LOGON"      , new Event("Triggers as (with ^) or after (without ^) mIRC sends the NICK and USER commands.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "MIDIEND"    , new Event("Triggers when a MIDI sound finishes playing.",
                                       new ParseParameterDelegate[0]) },
            { "MODE"       , new Event("Triggers when modes of a channel you are on are changed.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "MP3END"     , new Event("Triggers when an MP3 sound finishes playing.",
                                       new ParseParameterDelegate[0]) },
            { "NICK"       , new Event("Triggers when you see someone change their nickname.",
                                       new ParseParameterDelegate[0]) },
            { "NOSOUND"    , new Event("Triggers when you receive a CTCP SOUND request for a sound you don't have.",
                                       new ParseParameterDelegate[0]) },
            { "NOTICE"     , new Event("Triggers when you receive a notice message in a channel or in private.",
                                       new ParseParameterDelegate[] { ParameterParser.TextPattern, ParameterParser.TextScope }) },
            { "NOTIFY"     , new Event("Triggers when someone on your notify list joins IRC.",
                                       new ParseParameterDelegate[0]) },
            { "OP"         , new Event("Triggers when a user gives channel operator status to another user.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "OPEN"       , new Event("Triggers when a mIRC window opens.",
                                       new ParseParameterDelegate[] { ParameterParser.WindowType, ParameterParser.TextPattern }) },
            { "PARSELINE"  , new Event("Triggers before IRC lines are received or sent, and allows scripts to modify them.",
                                       new ParseParameterDelegate[] { ParameterParser.ParselineType, ParameterParser.TextPattern }) },
            { "PART"       , new Event("Triggers when someone leaves a channel you are on.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "PING"       , new Event("Triggers when you receive a PING from an IRC server.",
                                       new ParseParameterDelegate[0]) },
            { "PLAYEND"    , new Event("Triggers when a sound started by /play finishes playing.",
                                       new ParseParameterDelegate[0]) },
            { "PONG"       , new Event("Triggers when you receive a PONG from an IRC server.",
                                       new ParseParameterDelegate[0]) },
            { "QUIT"       , new Event("Triggers when someone quits the IRC server.",
                                       new ParseParameterDelegate[0]) },
            { "RAWMODE"    , new Event("Triggers when you receive a MODE line from an IRC server.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "SENDFAIL"   , new Event("Triggers when your DCC SEND transfer fails.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "SERV"       , new Event("Triggers when you receive a message in a DCC FSERVE session.",
                                       new ParseParameterDelegate[] { ParameterParser.TextPattern }) },
            { "SERVERMODE" , new Event("Triggers when the IRC server changes modes of a channel you are on.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "SERVEROP"   , new Event("Triggers when the server gives channel operator status to a user.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "SIGNAL"     , new Event("Triggered by scripts using the /signal command.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "SNOTICE"    , new Event("Triggers when you receive a notice directly from a server.",
                                       new ParseParameterDelegate[] { ParameterParser.TextPattern }) },
            { "SOCKCLOSE"  , new Event("Triggers when a socket connection is closed by the remote party.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "SOCKLISTEN" , new Event("Triggers when someone tries to connect to your socket listener.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "SOCKOPEN"   , new Event("Triggers when a connection started using /sockopen is established.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "SOCKREAD"   , new Event("Triggers when you receive data on a TCP socket conmection.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "SOCKWRITE"  , new Event("Triggers when writing data to a TCP socket connection has completed.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "START"      , new Event("Triggers when the script is first loaded and when mIRC starts.",
                                       new ParseParameterDelegate[0]) },
            { "TABCOMP"    , new Event("Triggers when you press the Tab key in a mIRC window's input box.",
                                       new ParseParameterDelegate[] { ParameterParser.WindowType }) },
            { "TEXT"       , new Event("Triggers when you receive a PRIVMSG in a channel or private.",
                                       new ParseParameterDelegate[] { ParameterParser.TextPattern, ParameterParser.TextScope }) },
            { "TOPIC"      , new Event("Triggers when a channel topic is changed.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "UDPREAD"    , new Event("Triggers when you receive data on a UDP socket.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "UNBAN"      , new Event("Triggers when a ban is removed from a channel.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "UNLOAD"     , new Event("Triggers when the script is unloaded.",
                                       new ParseParameterDelegate[0]) },
            { "UNOTIFY"    , new Event("Triggers when someone on your notify list leaves IRC.",
                                       new ParseParameterDelegate[0]) },
            { "USERMODE"   , new Event("Triggers when your user modes are changed.",
                                       new ParseParameterDelegate[0]) },
            { "VCMD"       , new Event("Triggers when the speech recognition system matches a word you speak on your command list.",
                                       new ParseParameterDelegate[] { ParameterParser.WindowType }) },
            { "VOICE"      , new Event("Triggers when a user gains channel voice.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "WALLOPS"    , new Event("Triggers when you receive a WALLOPS message.",
                                       new ParseParameterDelegate[] { ParameterParser.TextPattern }) },
            { "WAVEEND"    , new Event("Triggers when a wave sound finishes playing.",
                                       new ParseParameterDelegate[0]) },
        };

        public static LineInfo Highlight(int lineIndex, DocumentType syntaxType, FastColoredTextBox textBox, out int cursorCharType) {
            MslBookmark bookmark = null;
            return Highlight(lineIndex, syntaxType, textBox, out cursorCharType, ref bookmark);
        }
        public static LineInfo Highlight(int lineIndex, DocumentType syntaxType, FastColoredTextBox textBox, out int cursorCharType, ref MslBookmark bookmark) {
            switch (syntaxType) {
                case DocumentType.RemoteScript:
                case DocumentType.AliasScript:
                case DocumentType.ConsoleInput:
                    return HighlightMsl(lineIndex, syntaxType, textBox, out cursorCharType, ref bookmark);

                case DocumentType.Variables:
                    // TODO: HighlightVariables(lineIndex, syntaxType, textBox);
                    cursorCharType = 0;
                    bookmark = null;
                    return null;

                case DocumentType.INI:
                    HighlightIni(lineIndex, syntaxType, textBox, ref bookmark);
                    cursorCharType = 0;
                    return null;

                default:
                    cursorCharType = 0;
                    bookmark = null;
                    return null;
            }
        }
        
        public static LineInfo HighlightMsl(int lineIndex, DocumentType syntaxType, FastColoredTextBox textBox, out int cursorCharType, ref MslBookmark bookmark) {
            Line line = textBox[lineIndex];

            var lineInfoTable = ((MslDocument) textBox.Tag).lineInfoTable;
            LineInfo info;

            int i = 0; int start = -1; int state = 0; int braceLevel = 0; MslBookmark newBookmark = null; List<Error> errors = new List<Error>();
            string eventName = null;
            cursorCharType = 0;

            if (lineIndex != 0 && lineInfoTable.TryGetValue(textBox[lineIndex - 1].UniqueId, out info)) {
                state = info.State;
                braceLevel = info.BraceLevel;
            }

            if (syntaxType == DocumentType.ConsoleInput) {
                if (!sequenceCheck(line, 0, "> ", false)) return null;
                state = 1;
                i = 2;
            } else i = 0;

            for (; i < line.Count; ++i) {
                if (i < 0) throw new Exception();

                char c = line[i].c;
                switch (state) {
                    case 0:
                        if (c == ' ') continue;  // Remember that mIRC only recognises U+0020 as whitespace.
                        if (c == ';') {
                            // Comment
                            new Range(textBox, i, lineIndex, line.Count, lineIndex).SetStyle(CommentStyle);
                            i = int.MaxValue - 2; break;
                        } else if (sequenceCheck(line, i, "/*", false)) {
                            // Multi-line comment.
                            new Range(textBox, i, lineIndex, line.Count, lineIndex).SetStyle(CommentStyle);
                            state = 4;
                            i = int.MaxValue - 2; break;
                        }
                        if (syntaxType == DocumentType.RemoteScript) {
                            if (sequenceCheck(line, i, "on", true)) {
                                new Range(textBox, i, lineIndex, i + 2, lineIndex).SetStyle(KeywordStyle);
                                newBookmark = new MslBookmark(BookmarkType.Event, lineIndex);
                                i += 2;
                                state = 16;
                            }
                            if (sequenceCheck(line, i, "alias", true)) {
                                new Range(textBox, i, lineIndex, i + 5, lineIndex).SetStyle(KeywordStyle);
                                newBookmark = new MslBookmark(BookmarkType.Alias, lineIndex);
                                i += 5;
                                state = 8;
                            }
                            if (sequenceCheck(line, i, "dialog", true)) {
                                new Range(textBox, i, lineIndex, i + 6, lineIndex).SetStyle(KeywordStyle);
                                newBookmark = new MslBookmark(BookmarkType.Dialog, lineIndex);
                                i += 6;
                                state = 8;
                            }
                            if (sequenceCheck(line, i, "ctcp", true)) {
                                new Range(textBox, i, lineIndex, i + 4, lineIndex).SetStyle(KeywordStyle);
                                newBookmark = new MslBookmark(BookmarkType.CtcpEvent, lineIndex);
                                i += 4;
                                state = 24;
                            }
                            if (sequenceCheck(line, i, "error", true)) {
                                new Range(textBox, i, lineIndex, i + 5, lineIndex).SetStyle(ErrorStyle);
                                errors.Add(new Error() { Type = 0, Location = new Range(textBox, i, lineIndex, i + 5, lineIndex), Text = "This is a test." });
                                i += 4;
                                state = 8;
                            }
                        } else if (syntaxType == DocumentType.AliasScript) {
                            newBookmark = new MslBookmark(BookmarkType.Alias, lineIndex);
                            state = 9;
                            start = i;
                        }
                        break;

                    case 1:
                        if (start == -1) {
                            if (c == ' ') continue;
                            if (c == ';') {
                                // Comment
                                new Range(textBox, i, lineIndex, line.Count, lineIndex).SetStyle(CommentStyle);
                                i = int.MaxValue - 2; break;
                            } else if (sequenceCheck(line, i, "/*", false)) {
                                // Multi-line comment.
                                new Range(textBox, i, lineIndex, line.Count, lineIndex).SetStyle(CommentStyle);
                                state = 5;
                                i = int.MaxValue - 2; break;
                            }
                            start = i;
                            if (c == '$' || c == '%') {
                                --i;
                                state = 2;
                                start = -1;
                                break;
                            }

                            while (c == '/' || c == '.' || c == '!') {
                                ++i;
                                if (i >= line.Count) {
                                    new Range(textBox, start, lineIndex, i, lineIndex).SetStyle(ErrorStyle);
                                    break;
                                }
                                c = line[i].c;
                            }

                            if (sequenceCheck(line, i, "{", true)) {
                                ++braceLevel;
                                start = -1;
                                continue;
                            }
                            if (sequenceCheck(line, i, "|", true)) {
                                continue;
                            }
                            if (sequenceCheck(line, i, "}", true)) {
                                --braceLevel;
                                if (braceLevel == 0) state = 0;
                                start = -1;
                                continue;
                            }

                            if (syntaxType == DocumentType.ConsoleInput && sequenceCheck(line, i, "?", false)) {
                                new Range(textBox, start, lineIndex, i + 1, lineIndex).SetStyle(KeywordStyle);
                            } else {
                                string result = sequenceCheck(line, i, Program.commands, state);
                                if (result != null) {
                                    new Range(textBox, start, lineIndex, i + result.Length, lineIndex).SetStyle(CommandStyle);
                                    i += result.Length;
                                } else {
                                    do ++i; while (i < line.Count && line[i].c != ' ');
                                    new Range(textBox, start, lineIndex, i, lineIndex).SetStyle(CustomCommandStyle);
                                }
                            }
                            state = 2; start = -1;
                        }
                        break;

                    case 2:
                    case 3:
                        HighlightParameters(lineIndex, ref i, ref state, ref braceLevel, textBox, false);
                        break;

                    case 4:
                        if (sequenceCheck(line, i, "*/", false)) {
                            new Range(textBox, 0, lineIndex, i + 2, lineIndex).SetStyle(CommentStyle);
                            state &= ~4;
                            ++i;
                        }
                        break;

                    case 5:
                    case 6:
                        i = int.MaxValue - 1; break;

                    case 8:
                        if (c == ' ') continue;
                        if (sequenceCheck(line, i, "-l", true)) {
                            new Range(textBox, i, lineIndex, i + 2, lineIndex).SetStyle(KeywordStyle);
                            i += 2;
                            state = 9; start = -1;
                        } else {
                            i -= 1;
                            state = 9; start = -1;
                        }
                        break;

                    case 9:
                        if (c == ' ') {
                            if (start != -1) {
                                new Range(textBox, start, lineIndex, i, lineIndex).SetStyle(AliasStyle);
                                newBookmark.title = newBookmark.title.Substring(0, newBookmark.title.Length - 1) + line.Text.Substring(start, i - start);
                                state = 1; start = -1;
                            }
                        } else if (start == -1) start = i;
                        break;

                    case 16:
                        if (c == ':') {
                            state = 17;
                            start = i + 1;

                            if (textBox.Selection.Start == textBox.Selection.End && textBox.Selection.End.iLine == lineIndex && textBox.Selection.End.iChar == i + 1)
                                cursorCharType = 1;
                        }
                        break;
                    case 17:
                        if (c == ':') {
                            new Range(textBox, start, lineIndex, i, lineIndex).SetStyle(KeywordStyle);
                            eventName = line.Text.Substring(start, i - start);
                            newBookmark.title = eventName;
                            state = 18;
                            start = i + 1;
                            if (sequenceCheck(line, i + 1, "{", true)) { state = 1; start = -1; }
                        }
                        break;
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                        var eventData = events[eventName];
                        if (state - 18 >= eventData.Parameters.Length) { state = 1; --i; }
                        else eventData.Parameters[state - 18](lineIndex, ref i, ref state, ref braceLevel, textBox, newBookmark, errors);
                        start = -1;
                        break;
                    case 24:  // CTCP
                        if (c == ':') {
                            state = 25;
                            start = i + 1;
                        }
                        break;
                    case 25:  // CTCP pattern
                        ParameterParser.TextPattern(lineIndex, ref i, ref state, ref braceLevel, textBox, newBookmark, errors);
                        start = -1;
                        break;
                    case 26:  // CTCP target
                        ParameterParser.TextScope(lineIndex, ref i, ref state, ref braceLevel, textBox, newBookmark, errors);
                        state = 1;
                        start = -1;
                        break;
                    case 32:  // Bare if parameter
                    case 34:
                        break;
                }
            }

            if (state > 0 && state < 4) state = 1;
            else if (state >= 8) state = 0;
            
            if (braceLevel == 0 && state < 4) state = 0;
            if (state == 5) state = 1;
            if (state == 6) state = 2;

            // Encode the final state information.
            if (state == 4) new Range(textBox, lineIndex).SetStyle(CommentStyle);  // The entire line is part of a block comment.

            if (bookmark == null) {
                bookmark = newBookmark;
                if (newBookmark != null) Console.WriteLine($"Bookmark added to line {lineIndex}.");
            } else if (newBookmark == null) {
                bookmark = null;
                Console.WriteLine($"Bookmark removed from line {lineIndex}.");
            } else if (newBookmark.type != bookmark.type) {
                bookmark = newBookmark;
                Console.WriteLine($"Bookmark replaced on line {lineIndex}.");
            } else if (newBookmark.title != bookmark.title) {
                bookmark.title = newBookmark.title;
                Console.WriteLine($"Bookmark renamed on line {lineIndex}.");
            }

            info = new LineInfo() { BraceLevel = unchecked((sbyte) braceLevel), State = (byte) state, bookmark = bookmark,
                errors = errors.Count == 0 ? null : errors.ToArray() };
            lineInfoTable[textBox[lineIndex].UniqueId] = info;
            // unchecked prevents the program from crashing if *someone* exceeds 127 nested braces.


            return info;
        }
        /*
        public static void HighlightExpression(int lineIndex, ref int i, ref int state, ref int braceLevel, FastColoredTextBox textBox) {
            Line line = textBox[lineIndex];
            int start = -1; Style style = null;

            for (; i < line.Count; ++i) {
                char c = line[i].c;
                if (c == ' ') {
                    if (style != null) break;
                    continue;
                }

                //if (first) {
                    // Check for braces if we're not inside a function.
                    if (sequenceCheck(line, i, "{", true)) {
                        ++braceLevel;
                        state = 1;
                        return;
                    }
                    if (sequenceCheck(line, i, "|", true)) {
                        state = 1;
                        return;
                    }
                    if (sequenceCheck(line, i, "}", true)) {
                        --braceLevel;
                        if (braceLevel == 0) state = 0;
                        else state = 1;
                        return;
                    }
                //}

                if (c == '(') {
                    if (style == FunctionStyle) break;
                    ++brackets;
                } else if (c == ')') {
                    if (brackets == 0) {
                        if (!first) break;
                    }
                    --brackets;
                } else if (stopOnColon && c == ':') {
                    break;
                } else if (c == ',') {
                    if (!first) break;
                }

                if (style == null) {
                    start = i;
                    if (c == '%') {
                        style = VariableStyle;
                    } else if (c == '$') {
                        style = FunctionStyle;
                        if (sequenceCheck(line, i + 1, "&", true)) {
                            // Continuation operator.
                            new Range(textBox, i, lineIndex, i + 2, lineIndex).SetStyle(KeywordStyle);
                            state = 6;
                            return;
                        }
                    } else if (sequenceCheck(line, i, "|", true)) {
                        style = ErrorStyle;
                    }
                }
            }

            if (style == null) return;

            // Process the token.
            if (style == FunctionStyle && i < line.Count && line[i].c == '(') {
                state = 3;
                new Range(textBox, start, lineIndex, i + 1, lineIndex).SetStyle(style);
                while (i < line.Count && line[i].c != ')') {
                    ++i;
                    HighlightParameters(lineIndex, ref i, ref state, ref braceLevel, textBox, stopOnColon);
                    if (i < line.Count) {
                        if (line[i].c == ':') return;
                        if (line[i].c != ',')
                            new Range(textBox, i, lineIndex, i + 1, lineIndex).SetStyle(style);
                    }
                }
                if (i < line.Count && line[i].c == ')') {
                    new Range(textBox, i, lineIndex, i + 1, lineIndex).SetStyle(style);

                    // Check for a property.
                    start = -1;
                    ++i;
                    if (sequenceCheck(line, i, ".", false)) {
                        start = i;
                        ++i;

                        for (; i < line.Count; ++i) {
                            if (line[i].c == ' ') break;
                            else if (!first && (line[i].c == ')' || line[i].c == ',')) break;
                        }
                        new Range(textBox, start, lineIndex, i, lineIndex).SetStyle(FunctionPropertyStyle);
                    }
                }
                if (first) state = 2;
            } else if (style != null) {
                new Range(textBox, start, lineIndex, i, lineIndex).SetStyle(style);
            }
        }
        */

        public static void HighlightParameters(int lineIndex, ref int i, ref int state, ref int braceLevel, FastColoredTextBox textBox, bool stopOnColon) {
            Line line = textBox[lineIndex];
            bool first = (state == 2); int brackets = 0;
            int start = -1; Style style = null;

            for (; i < line.Count; ++i) {
                char c = line[i].c;
                if (c == ' ') {
                    if (style != null) break;
                    continue;
                }

                if (first) {
                    // Check for braces if we're not inside a function.
                    if (sequenceCheck(line, i, "{", true)) {
                        ++braceLevel;
                        state = 1;
                        return;
                    }
                    if (sequenceCheck(line, i, "|", true)) {
                        state = 1;
                        return;
                    }
                    if (sequenceCheck(line, i, "}", true)) {
                        --braceLevel;
                        if (braceLevel == 0) state = 0;
                        else state = 1;
                        return;
                    }
                }

                if (c == '(') {
                    if (style == FunctionStyle) break;
                    ++brackets;
                } else if (c == ')') {
                    if (brackets == 0) {
                        if (!first) break;
                    }
                    --brackets;
                } else if (stopOnColon && c == ':') {
                    break;
                } else if (c == ',') {
                    if (!first) break;
                }

                if (style == null) {
                    start = i;
                    if (c == '%') {
                        style = VariableStyle;
                    } else if (c == '$') {
                        style = FunctionStyle;
                        if (sequenceCheck(line, i + 1, "&", true)) {
                            // Continuation operator.
                            new Range(textBox, i, lineIndex, i + 2, lineIndex).SetStyle(KeywordStyle);
                            state = 6;
                            return;
                        }
                    } else if (sequenceCheck(line, i, "|", true)) {
                        style = ErrorStyle;
                    }
                }
            }

            if (style == null) return;

            // Process the token.
            if (style == FunctionStyle && i < line.Count && line[i].c == '(') {
                state = 3;
                new Range(textBox, start, lineIndex, i + 1, lineIndex).SetStyle(style);
                while (i < line.Count && line[i].c != ')') {
                    ++i;
                    HighlightParameters(lineIndex, ref i, ref state, ref braceLevel, textBox, stopOnColon);
                    if (i < line.Count) {
                        if (line[i].c == ':') return;
                        if (line[i].c != ',')
                            new Range(textBox, i, lineIndex, i + 1, lineIndex).SetStyle(style);
                    }
                }
                if (i < line.Count && line[i].c == ')') {
                    new Range(textBox, i, lineIndex, i + 1, lineIndex).SetStyle(style);

                    // Check for a property.
                    start = -1;
                    ++i;
                    if (sequenceCheck(line, i, ".", false)) {
                        start = i;
                        ++i;

                        for (; i < line.Count; ++i) {
                            if (line[i].c == ' ') break;
                            else if (!first && (line[i].c == ')' || line[i].c == ',')) break;
                        }
                        new Range(textBox, start, lineIndex, i, lineIndex).SetStyle(FunctionPropertyStyle);
                    }
                }
                if (first) state = 2;
            } else if (style != null) {
                new Range(textBox, start, lineIndex, i, lineIndex).SetStyle(style);
            }
        }

        public static void HighlightIni(int lineIndex, DocumentType syntaxType, FastColoredTextBox textBox, ref MslBookmark bookmark) {
            Line line = textBox[lineIndex];

            // Check for a section header.
            for (int i = 0; i < line.Count; ++i) {
                if (line[i].c == '[') {
                    for (int j = line.Count - 1; j > i; --j) {
                        if (line[j].c == ']') {
                            new Range(textBox, lineIndex).SetStyle(KeywordStyle);

                            string title = Program.GetLineSubstring(line, 1, line.Count - 1);
                            if (bookmark == null)
                                bookmark = new MslBookmark(BookmarkType.IniSection, title, lineIndex);
                            else
                                bookmark.title = title;

                            return;
                        } else if (line[j].c != ' ')
                            break;
                    }
                    break;
                } else if (line[i].c != ' ')
                    break;
            }

            // Check for a value.
            for (int i = 0; i < line.Count; ++i) {
                char c = line[i].c;
                if (c == '=') {
                    new Range(textBox, 0, lineIndex, i, lineIndex).SetStyle(CustomCommandStyle);
                    return;
                }
            }
        }

        /// <summary>Checks whether a specified string is present at a specified position in the line.</summary>
        /// <param name="i">The position to search.</param>
        /// <param name="sequence">The string to search for.</param>
        /// <param name="requireBoundary">If true, there must be whitespace or the end of the line after the string for the search to succeed.</param>
        /// <returns>A Boolean value indicating whether or not the search succeeded.</returns>
        public static bool sequenceCheck(IList<FastColoredTextBoxNS.Char> text, int i, string sequence, bool requireBoundary) {
            if (i > text.Count - sequence.Length) return false;
            for (int j = 0; j < sequence.Length; ++j) {
                char c1 = sequence[j]; char c2;
                // Set c2 to the opposite case version of c1.
                if (c1 >= 'A' && c1 <= 'Z') c2 = (char) (c1 + 0x20);
                else if (c1 >= 'a' && c1 <= 'z') c2 = (char) (c1 - 0x20);
                else c2 = c1;

                // Check each character.
                char c = text[i + j].c;
                if (c != c1 && c != c2) return false;
            }
            // Check that this is a whitespace boundary.
            return (!requireBoundary || i == text.Count - sequence.Length || text[i + sequence.Length].c == ' ');
        }
        /// <summary>Checks whether any string in a list is present at a given position in the text.</summary>
        /// <param name="i">The position to search.</param>
        /// <param name="sequences">The list of strings to search for. The list must be in lexicographical order, and all in lowercase.</param>
        /// <param name="state">Not used.</param>
        /// <returns>The string that was matched, or null if none were matched.</returns>
        public static string sequenceCheck(IList<FastColoredTextBoxNS.Char> text, int i, IEnumerable<string> sequences, int state) {
            int j; int pos; IEnumerator<string> enumerator = sequences.GetEnumerator();
            enumerator.MoveNext();

            for (j = 0; (pos = i + j) < text.Count; ++j) {
                // Set c to lower case.
                char c = text[pos].c;
                if (c == ' ') break;
                if (c >= 'A' && c <= 'Z') c += (char) 0x20;

                while (j >= enumerator.Current.Length || enumerator.Current[j] != c) {
                    // No match for this entry.
                    if (enumerator.MoveNext() == false) return null;
                    if (enumerator.Current.Length <= j) return null;
                    for (int k = j - 1; k >= 0; --k) {
                        char c2 = text[i + k].c;
                        if (c2 >= 'A' && c2 <= 'Z') c2 += (char) 0x20;
                        if (c2 != enumerator.Current[k]) return null;
                    }
                }
            }
            if (j == enumerator.Current.Length) return enumerator.Current;
            return null;
        }

        public static char skipTo(IList<FastColoredTextBoxNS.Char> text, char[] targets, ref int i) {
            for (; i < text.Count; ++i) {
                foreach (char c2 in targets) {
                    if (text[i].c == c2) return c2;
                }
            }
            return '\n';
        }

        public static class ParameterParser {
            public static void WindowType(int lineIndex, ref int i, ref int state, ref int braceLevel, FastColoredTextBox textBox, MslBookmark bookmark, List<Error> errors) {
                Line line = textBox[lineIndex]; int start = i;

                while (true) {
                    if (i >= line.Count) {
                        Range errorRange = new Range(textBox, line.Count - 1, lineIndex, line.Count, lineIndex);
                        errorRange.SetStyle(ErrorStyle);
                        errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Event missing the command." });
                        return;
                    }
                    if (line[i].c == ':') break;
                    ++i;
                }

                if (i - start == 1) {
                    if (line[start].c == '*' || line[start].c == '#' || line[start].c == '?' || line[start].c == '=' ||
                        line[start].c == '!' || line[start].c == '@') {
                        ++state;
                        return;
                    }
                }

                {
                    Range errorRange = new Range(textBox, start, lineIndex, i, lineIndex);
                    errorRange.SetStyle(ErrorStyle);
                    errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Window type parameter must be one of the following: * # ? = ! @" });
                }
                ++state;
            }

            public static void BanTarget(int lineIndex, ref int i, ref int state, ref int braceLevel, FastColoredTextBox textBox, MslBookmark bookmark, List<Error> errors) {
                Line line = textBox[lineIndex]; int start = -1; int start2 = i;

                while (true) {
                    if (i >= line.Count) {
                        Range errorRange = new Range(textBox, line.Count - 1, lineIndex, line.Count, lineIndex);
                        errorRange.SetStyle(ErrorStyle);
                        errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Event missing the command." });
                        return;
                    }

                    var c = line[i].c;
                    if (c == ',' || c == ':') {
                        if (start == -1) {
                            Range errorRange = new Range(textBox, i, lineIndex, i + 1, lineIndex);
                            errorRange.SetStyle(ErrorStyle);
                            errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Invalid channel name in ON BAN target." });
                        } else {
                            c = line[start].c;
                            if (i - start == 1 && (c == '*' || c == '#')) {
                                //wildcard = true;
                            } else if (c != '#' && c != '&' && c != '+' && c != '!') {
                                Range errorRange = new Range(textBox, start, lineIndex, i, lineIndex);
                                errorRange.SetStyle(ErrorStyle);
                                errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Invalid channel name in ON BAN target." });
                            }
                            start = -1;
                        }
                        if (c == ':') break;
                    } else {
                        if (start == -1) start = i;
                        ++i;
                    }
                }

                ++state;
            }

            public static void DCCType(int lineIndex, ref int i, ref int state, ref int braceLevel, FastColoredTextBox textBox, MslBookmark bookmark, List<Error> errors) {
                Line line = textBox[lineIndex]; int start = i;

                if (sequenceCheck(line, i, new string[] { "chat:", "fserve:", "send:" }, state) == null) {
                    Range errorRange = new Range(textBox, start, lineIndex, i, lineIndex);
                    errorRange.SetStyle(ErrorStyle);
                    errors.Add(new Error() { Type = 0, Location = errorRange, Text = "DCC type parameter must be one of the following: chat send fserve" });
                }
                ++state;
            }

            public static void DialogEvent(int lineIndex, ref int i, ref int state, ref int braceLevel, FastColoredTextBox textBox, MslBookmark bookmark, List<Error> errors) {
                Line line = textBox[lineIndex]; int start = i;

                if (sequenceCheck(line, i, new string[] { "close:", "dclick:", "drop:", "edit:", "init:", "menu:", "mouse:", "rclick:", "sclick:", "scroll:", "uclick:" }, state) == null) {
                    Range errorRange = new Range(textBox, start, lineIndex, i, lineIndex);
                    errorRange.SetStyle(ErrorStyle);
                    errors.Add(new Error() { Type = 0, Location = errorRange, Text = "DCC type parameter must be one of the following: chat send fserve" });
                }
                ++state;
            }
            public static void DialogID(int lineIndex, ref int i, ref int state, ref int braceLevel, FastColoredTextBox textBox, MslBookmark bookmark, List<Error> errors) {
                Line line = textBox[lineIndex]; int start = i; bool error = false;

                while (true) {
                    if (i >= line.Count) {
                        Range errorRange = new Range(textBox, line.Count - 1, lineIndex, line.Count, lineIndex);
                        errorRange.SetStyle(ErrorStyle);
                        errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Event missing the command." });
                        return;
                    }

                    var c = line[i].c;
                    if (c == ':') {
                        if (i == start) {
                            Range errorRange = new Range(textBox, i, lineIndex, i + 1, lineIndex);
                            errorRange.SetStyle(ErrorStyle);
                            errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Invalid control ID on ON DIALOG event." });
                        } else if (error) {
                            Range errorRange = new Range(textBox, start, lineIndex, i, lineIndex);
                            errorRange.SetStyle(ErrorStyle);
                            errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Invalid control ID on ON DIALOG event." });
                        }
                        break;
                    } else if (c < '0' || c > '9') {
                        error = true;
                    }
                }

                ++state;
            }

            public static void CustomWindow(int lineIndex, ref int i, ref int state, ref int braceLevel, FastColoredTextBox textBox, MslBookmark bookmark, List<Error> errors) {
                Line line = textBox[lineIndex]; int start;

                // Find the first non-space character.
                while (true) {
                    if (i >= line.Count) {
                        Range errorRange = new Range(textBox, line.Count - 1, lineIndex, line.Count, lineIndex);
                        errorRange.SetStyle(ErrorStyle);
                        errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Event missing the pattern parameter." });
                        return;
                    }
                    if (line[i].c != ' ') break;
                    ++i;
                }
                start = i;

                if (line[i].c != '@' && line[i].c != '*') {
                    for (; i < line.Count || line[i].c != ':'; ++i) ;
                    Range errorRange = new Range(textBox, start, lineIndex, i, lineIndex);
                    errorRange.SetStyle(ErrorStyle);
                    errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Invalid custom window name." });
                } else {
                    for (; i < line.Count || line[i].c != ':'; ++i) ;
                }

                ++state;
            }

            public static void ParselineType(int lineIndex, ref int i, ref int state, ref int braceLevel, FastColoredTextBox textBox, MslBookmark bookmark, List<Error> errors) {
                Line line = textBox[lineIndex]; int start = i;

                // Find the first non-space character.
                while (true) {
                    if (i >= line.Count) {
                        Range errorRange = new Range(textBox, line.Count - 1, lineIndex, line.Count, lineIndex);
                        errorRange.SetStyle(ErrorStyle);
                        errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Event missing the pattern parameter." });
                        return;
                    }
                    if (line[i].c != ' ') break;
                    ++i;
                }
                start = i;

                if (sequenceCheck(line, i, new string[] { "*:", "in:", "out:" }, state) == null) {
                    Range errorRange = new Range(textBox, start, lineIndex, i, lineIndex);
                    errorRange.SetStyle(ErrorStyle);
                    errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Parseline type parameter must be one of the following: * in out" });
                }
                ++state;
            }

            public static void TextPattern(int lineIndex, ref int i, ref int state, ref int braceLevel, FastColoredTextBox textBox, MslBookmark bookmark, List<Error> errors) {
                Line line = textBox[lineIndex]; bool singleVariable = false; int start;

                // Find the first non-space character.
                while (true) {
                    if (i >= line.Count) {
                        Range errorRange = new Range(textBox, line.Count - 1, lineIndex, line.Count, lineIndex);
                        errorRange.SetStyle(ErrorStyle);
                        errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Event missing the pattern parameter." });
                        return;
                    }
                    if (line[i].c != ' ') break;
                    ++i;
                }

                if (i >= line.Count) {
                    Range errorRange = new Range(textBox, line.Count - 1, lineIndex, line.Count, lineIndex);
                    errorRange.SetStyle(ErrorStyle);
                    errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Event missing the scope parameter." });
                    return;
                }

                start = i;
                var c = line[i].c;
                if (line[i].c == '%') singleVariable = true;
                if (sequenceCheck(line, i, "$(", false)) {
                    state = 3;
                    new Range(textBox, i, lineIndex, i + 2, lineIndex).SetStyle(FunctionStyle);
                    ++i;
                    while (i < line.Count && line[i].c != ')') {
                        ++i;
                        HighlightParameters(lineIndex, ref i, ref state, ref braceLevel, textBox, true);
                        if (i >= line.Count || line[i].c == ':') {
                            Range errorRange = new Range(textBox, start, lineIndex, i, lineIndex);
                            errorRange.SetStyle(ErrorStyle);
                            errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Unclosed dynamic match." });
                            state = 19;
                            return;
                        }
                    }
                    new Range(textBox, i, lineIndex, i + 1, lineIndex).SetStyle(FunctionStyle);
                    ++i;
                }

                while (true) {
                    if (i >= line.Count) {
                        Range errorRange = new Range(textBox, line.Count - 1, lineIndex, line.Count, lineIndex);
                        errorRange.SetStyle(ErrorStyle);
                        errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Event missing the pattern parameter." });
                        return;
                    }
                    if (line[i].c == ' ' && singleVariable) singleVariable = false;
                    else if (line[i].c == ':') break;
                    ++i;
                }

                if (singleVariable) new Range(textBox, start, lineIndex, i, lineIndex).SetStyle(VariableStyle);
                bookmark.title += " : " + line.Text.Substring(start, i - start);
                ++state;
            }
            public static void TextScope(int lineIndex, ref int i, ref int state, ref int braceLevel, FastColoredTextBox textBox, MslBookmark bookmark, List<Error> errors) {
                Line line = textBox[lineIndex]; bool singleVariable = false; int start;

                // Find the first non-space character.
                while (true) {
                    if (i >= line.Count) {
                        Range errorRange = new Range(textBox, line.Count - 1, lineIndex, line.Count, lineIndex);
                        errorRange.SetStyle(ErrorStyle);
                        errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Event missing the scope parameter." });
                        return;
                    }
                    if (line[i].c != ' ') break;
                    ++i;
                }

                if (i >= line.Count) {
                    Range errorRange = new Range(textBox, line.Count - 1, lineIndex, line.Count, lineIndex);
                    errorRange.SetStyle(ErrorStyle);
                    errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Event missing the command." });
                    return;
                }

                start = i;
                if (line[i].c == '%') singleVariable = true;

                while (true) {
                    if (i >= line.Count) {
                        Range errorRange = new Range(textBox, line.Count - 1, lineIndex, line.Count, lineIndex);
                        errorRange.SetStyle(ErrorStyle);
                        errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Event missing the scope parameter." });
                        return;
                    }
                    if (singleVariable && line[i].c == ' ') singleVariable = false;
                    else if (line[i].c == ':') {
                        if (singleVariable) new Range(textBox, start, lineIndex, i, lineIndex).SetStyle(VariableStyle);
                        break;
                    }
                    ++i;
                }

                ++state;
            }

            public static void Anything(int lineIndex, ref int i, ref int state, ref int braceLevel, FastColoredTextBox textBox, MslBookmark bookmark, List<Error> errors) {
                Line line = textBox[lineIndex];

                while (true) {
                    if (i >= line.Count) {
                        Range errorRange = new Range(textBox, line.Count - 1, lineIndex, line.Count, lineIndex);
                        errorRange.SetStyle(ErrorStyle);
                        errors.Add(new Error() { Type = 0, Location = errorRange, Text = "Event missing parameters or command." });
                        return;
                    }

                    var c = line[i].c;
                    if (c == ':') break;
                }

                ++state;
            }

        }

        public class Event {
            public string Description { get; }
            public ParseParameterDelegate[] Parameters { get; }

            public Event(string description, ParseParameterDelegate[] parameters) {
                this.Description = description;
                this.Parameters = parameters;
            }
        }
    }
}

