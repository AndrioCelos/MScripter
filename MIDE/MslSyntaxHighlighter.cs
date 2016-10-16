using System;
using System.Collections.Generic;
using System.Linq;
using FastColoredTextBoxNS;

using static MIDE.Program;

namespace MIDE {
    public static class MslSyntaxHighlighter {
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
        public static Style NoticeStyle;

        internal delegate void ParseParameterDelegate(ParseState state);

        #region Events
        internal static Dictionary<string, Event> events = new Dictionary<string, Event>(StringComparer.OrdinalIgnoreCase) {
            { "ACTION"     , new Event("ACTION\t{0}"        , "Triggers when you receive an action message in a channel or in private.",
                                 new ParseParameterDelegate[] { ParameterParser.TextPattern, ParameterParser.TextScope }) },
            { "ACTIVE"     , new Event("ACTIVE"             , "Triggers when a window in mIRC is activated.",
                                           new ParseParameterDelegate[] { ParameterParser.WindowType }) },
            { "AGENT"      , new Event("AGENT"              , "Triggers when a speech agent finishes speaking.",
                                           new ParseParameterDelegate[0]) },
            { "APPACTIVE"  , new Event("APPACTIVE"          , "Triggers when the mIRC main window is activated.",
                                           new ParseParameterDelegate[0]) },
            { "BAN"        , new Event("BAN\t{0}"           , "Triggers when a ban is set on a channel.",
                                           new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "CHAT"       , new Event("CHAT\t{0}"          , "Triggers when you receive a message in a DCC CHAT session.",
                                           new ParseParameterDelegate[] { ParameterParser.TextPattern }) },
            { "CLOSE"      , new Event("CLOSE\t{1}"         , "Triggers when a window in mIRC is closed.",
                                           new ParseParameterDelegate[] { ParameterParser.WindowType, ParameterParser.TextPattern }) },
            { "CONNECT"    , new Event("CONNECT"            , "Triggers when mIRC has logged in to an IRC server.",
                                           new ParseParameterDelegate[0]) },
            { "CONNECTFAIL", new Event("CONNECTFAIL"        , "Triggers when an attempt and all retries to connect to an IRC server fail.",
                                           new ParseParameterDelegate[0]) },
            { "CTCPREPLY"  , new Event("CTCPREPLY\t{0}"     , "Triggers when you receive a CTCP reply.",
                                           new ParseParameterDelegate[] { ParameterParser.TextPattern }) },
            { "DCCSERVER"  , new Event("DCCSERVER"          , "Triggers when someone tries to connect to your DCC server.",
                                           new ParseParameterDelegate[] { ParameterParser.DCCType }) },
            { "DEHELP"     , new Event("DEHELP\t{0}"        , "Triggers when a user loses channel half-operator status.",
                                           new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "DEOP"       , new Event("DEOP\t{0}"          , "Triggers when a user loses channel operator status.",
                                           new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "DEVOICE"    , new Event("DEVOICE\t{0}"       , "Triggers when a user loses channel voice.",
                                           new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "DIALOG"     , new Event("DIALOG\t{0}\t{1}\t{2}"                              , "Triggers when something happens to a control in a dialog.",
                                           new ParseParameterDelegate[] { ParameterParser.Anything, ParameterParser.DialogEvent, ParameterParser.DialogID }) },
            { "DISCONNECT" , new Event("DISCONNECT"         , "Triggers when mIRC has disconnected from an IRC server.",
                                           new ParseParameterDelegate[0]) },
            { "DNS"        , new Event("DNS"                , "Triggers when a DNS query started using /dns succeeds or fails.",
                                           new ParseParameterDelegate[0]) },
            { "ERROR"      , new Event("ERROR\t{0}"         , "Triggers when you receive an IRC ERROR message from the server.",
                                           new ParseParameterDelegate[] { ParameterParser.TextPattern }) },
            { "EXIT"       , new Event("EXIT"               , "Triggers when mIRC is closing.",
                                           new ParseParameterDelegate[0]) },
            { "FILESENT"   , new Event("FILESENT\t{0}"      , "Triggers when you successfully finish sending a file via DCC SEND.",
                                           new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "FILERCVD"   , new Event("FILERCVD\t{0}"      , "Triggers when you successfully finish receiving a file via DCC GET.",
                                           new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "GETFAIL"    , new Event("GETFAIL\t{0}"       , "Triggers when a DCC GET transfer fails.",
                                           new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "HELP"       , new Event("HELP\t{0}"          , "Triggers when a user gains channel half-operator status.",
                                           new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "HOTLINK"    , new Event("HOTLINK\t{0}"       , "Triggers when the mouse is moved over a word in a mIRC window.",
                                           new ParseParameterDelegate[] { ParameterParser.TextPattern, ParameterParser.WindowType }) },
            { "INPUT"      , new Event("INPUT"              , "Triggers when you enter text in a mIRC window's input box.",
                                           new ParseParameterDelegate[] { ParameterParser.WindowType }) },
            { "INVITE"     , new Event("INVITE\t{0}"        , "Triggers when you are invited to a channel.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "JOIN"       , new Event("JOIN\t{0}"          , "Triggers when someone joins a channel you are on.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "KEYDOWN"    , new Event("KEYDOWN\t{0}"       , "Triggers when a key is pressed or as one is held down in a custom window.",
                                       new ParseParameterDelegate[] { ParameterParser.CustomWindow }) },
            { "KEYUP"      , new Event("KEYUP\t{0}"         , "Triggers when a key is released in a custom window.",
                                       new ParseParameterDelegate[] { ParameterParser.CustomWindow }) },
            { "KICK"       , new Event("KICK\t{0}"          , "Triggers when someone is kicked out of a channel you are on.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "LOAD"       , new Event("LOAD"               , "Triggers when the script is first loaded.",
                                       new ParseParameterDelegate[0]) },
            { "LOGON"      , new Event("LOGON"              , "Triggers as (with ^) or after (without ^) mIRC sends the NICK and USER commands.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "MIDIEND"    , new Event("MIDIEND"            , "Triggers when a MIDI sound finishes playing.",
                                       new ParseParameterDelegate[0]) },
            { "MODE"       , new Event("MODE\t{0}"          , "Triggers when modes of a channel you are on are changed.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "MP3END"     , new Event("MP3END"             , "Triggers when an MP3 sound finishes playing.",
                                       new ParseParameterDelegate[0]) },
            { "NICK"       , new Event("NICK"               , "Triggers when you see someone change their nickname.",
                                       new ParseParameterDelegate[0]) },
            { "NOSOUND"    , new Event("NOSOUND"            , "Triggers when you receive a CTCP SOUND request for a sound you don't have.",
                                       new ParseParameterDelegate[0]) },
            { "NOTICE"     , new Event("NOTICE\t{0}"        , "Triggers when you receive a notice message in a channel or in private.",
                                       new ParseParameterDelegate[] { ParameterParser.TextPattern, ParameterParser.TextScope }) },
            { "NOTIFY"     , new Event("NOTIFY"             , "Triggers when someone on your notify list joins IRC.",
                                       new ParseParameterDelegate[0]) },
            { "OP"         , new Event("OP\t{0}"            , "Triggers when a user gives channel operator status to another user.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "OPEN"       , new Event("OPEN\t{1}"          , "Triggers when a mIRC window opens.",
                                       new ParseParameterDelegate[] { ParameterParser.WindowType, ParameterParser.TextPattern }) },
            { "PARSELINE"  , new Event("PARSELINE\t{0}\t{1}", "Triggers before IRC lines are received or sent, and allows scripts to modify them.",
                                       new ParseParameterDelegate[] { ParameterParser.ParselineType, ParameterParser.TextPattern }) },
            { "PART"       , new Event("PART\t{0}"          , "Triggers when someone leaves a channel you are on.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "PING"       , new Event("PING"               , "Triggers when you receive a PING from an IRC server.",
                                       new ParseParameterDelegate[0]) },
            { "PLAYEND"    , new Event("PLAYEND"            , "Triggers when a sound started by /play finishes playing.",
                                       new ParseParameterDelegate[0]) },
            { "PONG"       , new Event("PONG"               , "Triggers when you receive a PONG from an IRC server.",
                                       new ParseParameterDelegate[0]) },
            { "QUIT"       , new Event("QUIT"               , "Triggers when someone quits the IRC server.",
                                       new ParseParameterDelegate[0]) },
            { "RAWMODE"    , new Event("RAWMODE\t{0}"       , "Triggers when you receive a MODE line from an IRC server.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "SENDFAIL"   , new Event("SENDFAIL\t{0}"      , "Triggers when your DCC SEND transfer fails.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "SERV"       , new Event("SERV\t{0}"          , "Triggers when you receive a message in a DCC FSERVE session.",
                                       new ParseParameterDelegate[] { ParameterParser.TextPattern }) },
            { "SERVERMODE" , new Event("SERVERMODE\t{0}"    , "Triggers when the IRC server changes modes of a channel you are on.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "SERVEROP"   , new Event("SERVEROP\t{0}"      , "Triggers when the server gives channel operator status to a user.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "SIGNAL"     , new Event("SIGNAL\t{0}"        , "Triggered by scripts using the /signal command.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "SNOTICE"    , new Event("SNOTICE\t{0}"       , "Triggers when you receive a notice directly from a server.",
                                       new ParseParameterDelegate[] { ParameterParser.TextPattern }) },
            { "SOCKCLOSE"  , new Event("SOCKCLOSE\t{0}"     , "Triggers when a socket connection is closed by the remote party.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "SOCKLISTEN" , new Event("SOCKLISTEN\t{0}"    , "Triggers when someone tries to connect to your socket listener.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "SOCKOPEN"   , new Event("SOCKOPEN\t{0}"      , "Triggers when a connection started using /sockopen is established.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "SOCKREAD"   , new Event("SOCKREAD\t{0}"      , "Triggers when you receive data on a TCP socket conmection.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "SOCKWRITE"  , new Event("SOCKWRITE\t{0}"     , "Triggers when writing data to a TCP socket connection has completed.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "START"      , new Event("START"              , "Triggers when the script is first loaded and when mIRC starts.",
                                       new ParseParameterDelegate[0]) },
            { "TABCOMP"    , new Event("TABCOMP\t{0}"       , "Triggers when you press the Tab key in a mIRC window's input box.",
                                       new ParseParameterDelegate[] { ParameterParser.WindowType }) },
            { "TEXT"       , new Event("TEXT\t{0}"          , "Triggers when you receive a PRIVMSG in a channel or private.",
                                       new ParseParameterDelegate[] { ParameterParser.TextPattern, ParameterParser.TextScope }) },
            { "TOPIC"      , new Event("TOPIC\t{0}"         , "Triggers when a channel topic is changed.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "UDPREAD"    , new Event("UDPREAD\t{0}"       , "Triggers when you receive data on a UDP socket.",
                                       new ParseParameterDelegate[] { ParameterParser.Anything }) },
            { "UNBAN"      , new Event("UNBAN\t{0}"         , "Triggers when a ban is removed from a channel.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "UNLOAD"     , new Event("UNLOAD"             , "Triggers when the script is unloaded.",
                                       new ParseParameterDelegate[0]) },
            { "UNOTIFY"    , new Event("UNOTIFY"            , "Triggers when someone on your notify list leaves IRC.",
                                       new ParseParameterDelegate[0]) },
            { "USERMODE"   , new Event("USERMODE"           , "Triggers when your user modes are changed.",
                                       new ParseParameterDelegate[0]) },
            { "VCMD"       , new Event("VCMD\t{0}"          , "Triggers when the speech recognition system matches a word you speak on your command list.",
                                       new ParseParameterDelegate[] { ParameterParser.WindowType }) },
            { "VOICE"      , new Event("VOICE\t{0}"         , "Triggers when a user gains channel voice.",
                                       new ParseParameterDelegate[] { ParameterParser.BanTarget }) },
            { "WALLOPS"    , new Event("WALLOPS\t{0}"       , "Triggers when you receive a WALLOPS message.",
                                       new ParseParameterDelegate[] { ParameterParser.TextPattern }) },
            { "WAVEEND"    , new Event("WAVEEND"            , "Triggers when a wave sound finishes playing.",
                                       new ParseParameterDelegate[0]) },
        };
        #endregion

        public static LineInfo Highlight(FastColoredTextBox textBox, int lineIndex, DocumentType syntaxType) {
            new Range(textBox, lineIndex).ClearStyle(StyleIndex.All ^ (StyleIndex) (1 << textBox.GetStyleIndex(ControlCharStyle.Instance)));

            switch (syntaxType) {
                case DocumentType.RemoteScript:
                case DocumentType.AliasScript:
                case DocumentType.Popup:
                case DocumentType.ConsoleInput:
                    return HighlightMsl(textBox, lineIndex, syntaxType);

                case DocumentType.Variables:
                    HighlightVariables(lineIndex, syntaxType, textBox);
                    return null;

                case DocumentType.INI:
                    return HighlightIni(textBox, lineIndex, syntaxType);

                default:
                    return null;
            }
        }

        public static LineInfo HighlightMsl(FastColoredTextBox textBox, int lineIndex, DocumentType syntaxType) {
            ParseState state = new ParseState() { type = syntaxType, textBox = textBox, lineIndex = lineIndex, x = 0 };
            int start;

            var document = (MslDocument) textBox.Tag;
            var lineInfoTable = document.lineInfoTable;
            LineInfo info;

            while (lineIndex > 0) {
                info = document.GetLineInfo(lineIndex - 1);
                if (info != null && (ParseStateIndex) info.State != ParseStateIndex.Continuation) {
                    state.index = (ParseStateIndex) info.State;
                    state.braceLevel = info.BraceLevel;
                    break;
                }
                --lineIndex;
            }

            // Skip the > prefix in console input.
            if (syntaxType == DocumentType.ConsoleInput) {
                if (!sequenceCheck(state, "> ", false)) return null;
                state.index = ParseStateIndex.FunctionBody;
                state.x = 2;
            }

            // Skip leading whitespace.
            if (nextToken(state, false)) {
                if (state.c == ';') {
                    // Comment
                    setStyle(state, state.x, state.line.Count, CommentStyle);
                } else {
                    switch (state.index) {
                        case 0:
                            Program.impossible(4);
                            break;
                        case ParseStateIndex.CommentRoot:
                        case ParseStateIndex.CommentFunction:
                        case ParseStateIndex.CommentDialog:
                        case ParseStateIndex.CommentMenu:
                            // The comment ends at a line starting with */
                            if (sequenceCheck(state, "*/", true)) {
                                state.index += (ParseStateIndex.Root - ParseStateIndex.CommentRoot);
                                setStyleAndAdvance(state, 2, CommentStyle);

                                if (nextToken(state, false)) {
                                    addError(state, state.x, state.line.Count, ErrorType.Warning, "Unexpected characters after the end of a block comment.");
                                }
                            } else
                                setStyle(state, state.x, state.line.Count, CommentStyle);
                            break;
                        case ParseStateIndex.Root:
                            switch (syntaxType) {
                                case DocumentType.RemoteScript:
                                    if (sequenceCheck(state, "on", true)) {
                                        setStyleAndAdvance(state, 2, KeywordStyle);
                                        ParseEvent(state);
                                    } else if (sequenceCheck(state, "alias", true)) {
                                        setStyleAndAdvance(state, 5, KeywordStyle);
                                        ParseAlias(state);
                                    } else if (sequenceCheck(state, "raw", true)) {
                                        setStyleAndAdvance(state, 3, KeywordStyle);
                                        ParseRaw(state);
                                    } else if (sequenceCheck(state, "ctcp", true)) {
                                        setStyleAndAdvance(state, 4, KeywordStyle);
                                        ParseCtcp(state);
                                    } else if (sequenceCheck(state, "menu", true)) {
                                        setStyleAndAdvance(state, 4, KeywordStyle);
                                        ParseMenu(state);
                                    } else if (sequenceCheck(state, "dialog", true)) {
                                        setStyleAndAdvance(state, 6, KeywordStyle);
                                        ParseDialog(state);
                                    } else if (sequenceCheck(state, "/*", true)) {
                                        setStyle(state, state.x, state.line.Count, CommentStyle);
                                        state.index = ParseStateIndex.CommentRoot;
#if (DEBUG)
                                    } else if (sequenceCheck(state, "f", true)) {
                                        setStyleAndAdvance(state, 1, KeywordStyle);
                                        nextToken(state, false);
                                        start = state.x;
                                        skipTo(state, ' ');
                                        var stateName = Program.GetLineSubstring(state.line, start, state.x);
                                        ParseStateIndex state2;
                                        if (Enum.TryParse(stateName, out state2))
                                            state.index = state2;
                                        else
                                            addError(state, start, state.x, ErrorType.Warning, "Invalid state.");

                                        nextToken(state, false);
                                        ParseFunction(state, int.MaxValue);
                                    } else if (sequenceCheck(state, "c", true)) {
                                        setStyleAndAdvance(state, 1, KeywordStyle);
                                        nextToken(state, false);
                                        start = state.x;
                                        skipTo(state, ' ');
                                        var stateName = Program.GetLineSubstring(state.line, start, state.x);
                                        ParseStateIndex state2;
                                        if (Enum.TryParse(stateName, out state2))
                                            state.index = state2;
                                        else
                                            addError(state, start, state.x, ErrorType.Warning, "Invalid state.");

                                        nextToken(state, false);
                                        ParseCondition(state);
#endif
                                    }
                                    break;
                                case DocumentType.AliasScript:
                                    start = state.x;
                                    skipTo(state, ' ');
                                    setStyle(state, start, state.x, AliasStyle);

                                    var aliasName = GetLineSubstring(state.line, start, state.x);
                                    state.SetBookmark(BookmarkType.Alias, aliasName);
                                    state.tags.Add(new Tag() { range = new Range(state.textBox, start, state.lineIndex, state.x, state.lineIndex), type = TagType.AliasName });

                                    state.index = ParseStateIndex.FunctionBody;
                                    ParseCommand(state);
                                    break;
                                case DocumentType.Popup:
                                    ParseMenuItem(state);
                                    break;
                            }
                            if (state.braceLevel == 0 && state.index > ParseStateIndex.Root) state.index = ParseStateIndex.Root;
                            break;
                        case ParseStateIndex.FunctionBody:
                            if (sequenceCheck(state, "/*", true)) {
                                setStyle(state, state.x, state.line.Count, CommentStyle);
                                state.index = ParseStateIndex.CommentFunction;
                                break;
                            }
                            ParseCommand(state);
                            if (state.braceLevel == 0) state.index = ParseStateIndex.Root;
                            break;
                        case ParseStateIndex.MenuBody:
                            ParseMenuItem(state);
                            break;
                        case ParseStateIndex.DialogBody:
                            ParseDialogItem(state);
                            break;
                        default:
#if (DEBUG)
                            Program.impossible(6);
#endif
                            break;
                    }
                }
            }

            if (state.index > ParseStateIndex.FunctionBody) state.index = (state.braceLevel == 0 ? ParseStateIndex.Root : ParseStateIndex.FunctionBody);

            info = new LineInfo() { bookmark = state.bookmark, BraceLevel = (sbyte) state.braceLevel, errors = state.errors.ToArray(), State = (byte) state.index, tags = state.tags };
            lineInfoTable[state.line.UniqueId] = info;
            return info;
        }

        private static void setStyle(ParseState state, int start, int end, Style style) {
            new Range(state.textBox, start, state.lineIndex, end, state.lineIndex).SetStyle(style);
        }
        private static void setStyleAndAdvance(ParseState state, int length, Style style) {
            setStyle(state, state.x, state.x + length, style);
            state.x += length;
        }

        private static void addError(ParseState state, int start, int end, ErrorType type, string message) {
            var range = new Range(state.textBox, start, state.lineIndex, end, state.lineIndex);
            Style style;

            switch (type) {
                case ErrorType.Error: style = ErrorStyle; break;
                case ErrorType.Warning: style = WarningStyle; break;
                case ErrorType.Note: style = NoticeStyle; break;
                default: throw new ArgumentException("type is not a valid ErrorType value.");
            }

            range.SetStyle(style);
            state.errors.Add(new Error() { Location = range, Type = type, Text = message });
        }

        private static void ParseAlias(ParseState state) {
            if (!nextToken(state, false, "Expected an alias name.")) return;
            if (sequenceCheck(state, "-l", true)) {
                // Local alias.
                setStyleAndAdvance(state, 2, KeywordStyle);
                if (!nextToken(state, false, "Expected an alias name.")) return;
            }

            int start = state.x;
            skipTo(state, ' ');
            setStyle(state, start, state.x, AliasStyle);

            var aliasName = GetLineSubstring(state.line, start, state.x);
            state.SetBookmark(BookmarkType.Alias, aliasName);
            state.tags.Add(new Tag() { range = new Range(state.textBox, start, state.lineIndex, state.x, state.lineIndex), type = TagType.AliasName });

            state.index = ParseStateIndex.FunctionBody;
            ParseCommand(state);
            if (state.braceLevel == 0) state.index = ParseStateIndex.Root;
        }

        private static void ParseEvent(ParseState state) {
            if (!nextToken(state, false, "Expected a level or 'me'.")) return;

            if (sequenceCheck(state, "me:", false)) {
                setStyleAndAdvance(state, 2, KeywordStyle);
                ++state.x;
            }
            if (skipTo(state, ':') != ':') {
                addError(state, state.x - 1, state.x, ErrorType.Error, "Event missing the name.");
                return;
            }
            ++state.x;
            var start = state.x;
            if (skipTo(state, ':') != ':') {
                addError(state, state.x - 1, state.x, ErrorType.Error, "Event missing the parameters.");
                return;
            }
            state.tags.Add(new Tag() { range = new Range(state.textBox, start, state.lineIndex, state.x, state.lineIndex), type = TagType.EventName });
            var eventName = state.line.Text.Substring(start, state.x - start);

            Event eventData;
            if (events.TryGetValue(eventName, out eventData)) {
                var parameters = new string[eventData.Parameters.Length];
                state.index = ParseStateIndex.EventParameterList;

                for (int index = 0; index < eventData.Parameters.Length; ++index) {
                    ++state.x;
                    start = state.x;
                    eventData.Parameters[index].Invoke(state);
                    state.tags.Add(new Tag() { range = new Range(state.textBox, start, state.lineIndex, state.x, state.lineIndex), type = TagType.EventParameter });
                    if (state.c == '\n') return;
                    skipTo(state, ':');

                    parameters[index] = GetLineSubstring(state.line, start, state.x);
                }

                state.SetBookmark(BookmarkType.Event, string.Format(eventData.BookmarkFormat, parameters));

                ++state.x;
                state.index = ParseStateIndex.FunctionBody;
                ParseCommand(state);
            } else {
                addError(state, start, state.x, ErrorType.Error, "Unknown event '" + eventName + "'.");
                // Look for '{' anyway.
                while (skipTo(state, ':') == ':') {
                    if (sequenceCheck(state, "{", true)) {
                        ParseCommand(state);
                        return;
                    }
                    ++state.x;
                }
            }
        }

        private static void ParseMenu(ParseState state) {
            if (!nextToken(state, false, "Expected an menu name.")) return;

            int start = state.x;
            skipTo(state, ' ');
            setStyle(state, start, state.x, AliasStyle);
            state.SetBookmark(BookmarkType.Menu, GetLineSubstring(state.line, start, state.x));

            if (!nextToken(state, false, "Expected '{'.")) return;
            start = state.x;
            if (!sequenceCheck(state, "{", true)) {
                addError(state, state.x, state.x + 1, ErrorType.Error, "Expected '{' after menu name.");
                return;
            }
            state.index = ParseStateIndex.MenuBody;
            state.braceLevel = 1;
        }

        private static void ParseMenuItem(ParseState state) {
            if (state.braceLevel == 1 && sequenceCheck(state, "}", true)) {
                state.braceLevel = 0;
                state.index = ParseStateIndex.Root;
                return;
            }

            if (state.c == '-') {
                ++state.x;
                if (state.c == '\n') {
                    // Separator
                    setStyle(state, state.x - 1, state.x, KeywordStyle);
                    return;
                }
            }

            while (state.c == '.') ++state.x;

            var start = state.x;
            skipTo(state, ':');
            setStyle(state, start, state.x, AliasStyle);

            if (state.c != '\n') {
                ++state.x;
                ParseCommand(state);
                if (state.braceLevel == 1) state.index = ParseStateIndex.MenuBody;
            }
        }

        private static void ParseDialog(ParseState state) {
            if (!nextToken(state, false, "Expected a dialog name.")) return;
            if (sequenceCheck(state, "-l", true)) {
                // Local dialog.
                setStyleAndAdvance(state, 2, KeywordStyle);
                if (!nextToken(state, false, "Expected a dialog name.")) return;
            }

            int start = state.x;
            skipTo(state, ' ');
            var name = GetLineSubstring(state.line, start, state.x);
            setStyle(state, start, state.x, AliasStyle);
            state.SetBookmark(BookmarkType.Dialog, name);

            if (!nextToken(state, false, "Expected '{'.")) return;
            start = state.x;
            if (!sequenceCheck(state, "{", true)) {
                addError(state, state.x, state.x + 1, ErrorType.Error, "Expected '{' after dialog name.");
                return;
            }
            state.SetBookmark(BookmarkType.Dialog, name, false);
            state.index = ParseStateIndex.DialogBody;
            state.braceLevel = 1;
        }

        private static void ParseDialogItem(ParseState state) {
            if (state.braceLevel == 1 && sequenceCheck(state, "}", true)) {
                state.braceLevel = 0;
                state.index = ParseStateIndex.Root;
                return;
            }
        }

        private static void ParseRaw(ParseState state) {
            if (!nextToken(state, false, "Expected a reply.")) return;

            var start = state.x;
            if (skipTo(state, ':') != ':') {
                addError(state, state.x - 1, state.x, ErrorType.Error, "Event missing the pattern.");
                return;
            }
            var reply = GetLineSubstring(state.line, start, state.x);
            ++state.x;

            start = state.x;
            ParameterParser.TextPattern(state);
            if (state.c == '\n') return;
            if (skipTo(state, ':') != ':') {
                addError(state, state.x - 1, state.x, ErrorType.Error, "Event missing the command.");
                return;
            }
            state.SetBookmark(BookmarkType.RawEvent, reply + " " + GetLineSubstring(state.line, start, state.x));
            ++state.x;
            state.index = ParseStateIndex.FunctionBody;
            ParseCommand(state);
        }

        private static void ParseCtcp(ParseState state) {
            if (!nextToken(state, false, "Expected a reply.")) return;

            if (skipTo(state, ':') != ':') {
                addError(state, state.x - 1, state.x, ErrorType.Error, "Event missing the pattern.");
                return;
            }
            ++state.x;

            var start = state.x;
            ParameterParser.TextPattern(state);
            if (state.c == '\n') return;
            if (skipTo(state, ':') != ':') {
                addError(state, state.x - 1, state.x, ErrorType.Error, "Event missing the scope.");
                return;
            }
            state.SetBookmark(BookmarkType.CtcpEvent, GetLineSubstring(state.line, start, state.x));
            ++state.x;

            start = state.x;
            ParameterParser.TextScope(state);
            if (state.c == '\n') return;
            if (skipTo(state, ':') != ':') {
                addError(state, state.x - 1, state.x, ErrorType.Error, "Event missing the command.");
                return;
            }
            ++state.x;

            state.index = ParseStateIndex.FunctionBody;
            ParseCommand(state);
        }

        private static void ParseCommand(ParseState state) {
            var startState = state.index;
            if (!nextToken(state, true, "Expected a command or '{'.")) return;

            do {
                int start = state.x;
                state.index = startState;

                if (sequenceCheck(state, "{", true)) {
                    ++state.braceLevel;
                    ++state.x;
                    if (!nextToken(state, true)) return;
                    continue;
                } else if (sequenceCheck(state, "}", true)) {
                    --state.braceLevel;
                    ++state.x;
                    if (!nextToken(state, true)) return;
                    if (state.braceLevel == 0) return;
                    continue;
                } else if (sequenceCheck(state, "|", true)) {
                    ++state.x;
                    if (!nextToken(state, true)) return;
                    continue;
                } else {
                    if (state.c == '%' || state.c == '$') {
                        state.index = ParseStateIndex.ParameterList;
                        ParseParameters(state);
                        continue;
                    }

                    // Skip '/', '.' and '!' prefixes.
                    while (state.c == '/' || state.c == '.' || state.c == '!') {
                        ++state.x;
                        if (state.c == '\n' || state.c == ' ') {
                            addError(state, start, state.x, ErrorType.Error, "Expected a command.");
                            break;
                        }
                    }

                    var result = sequenceCheck(state, Program.commands);
                    if (result != null) {
                        // TODO: Fix this for aliases in remote scripts.
                        if (state.type == DocumentType.RemoteScript && result == "say")
                            addError(state, state.x, state.x + 3, ErrorType.Warning, "The say command cannot be used in events. Use `/msg $chan` instead.");

                        setStyle(state, start, state.x + result.Length, CommandStyle);
                        state.x += result.Length;
                    } else {
                        skipTo(state, ' ');
                        setStyle(state, start, state.x, CustomCommandStyle);
                    }

                    ++state.x;
                    if (!nextToken(state, true)) return;
                    if (result == "if" || result == "elseif" || result == "while") {
                        // This is a special case because of how different it is from other commands.
                        state.index = ParseStateIndex.Condition;
                        ParseCondition(state);
                        ParseCommand(state);
                    } else if (result == "else") {
                        ParseCommand(state);
                    } else if (result == "var") {
                        // This is another special case.
                        state.index = ParseStateIndex.VarParameterList;
                        ParseVar(state);
                    } else {
                        state.index = ParseStateIndex.ParameterList;
                        ParseParameters(state);
                        // This terminates at a `{`, `|` or `}`.
                    }
                }
            } while (state.c != '\n');
        }

        private static void ParseVar(ParseState state) {
            if (!nextToken(state, true, "Expected a variable name.")) return;
            do {
                var start = state.x;
                var c = state.c;

                if (sequenceCheck(state, "{", true) || sequenceCheck(state, "|", true) || sequenceCheck(state, "}", true)) {
                    // These always terminate /var
                    if (state.braceLevel == 0 && state.c == '}') {
                        addError(state, state.x, state.x + 1, ErrorType.Error, "'}' without '{'.");
                        state.x = state.line.Count;
                    }
                    return;
                }

                skipTo(state, ' ', ',');
                if (c == '%') {
                    setStyle(state, start, state.x, VariableStyle);
                    state.tags.Add(new Tag() { range = new Range(state.textBox, start, state.lineIndex, state.x, state.lineIndex), type = TagType.Variable });
                }

                if (!nextToken(state, true)) return;

                if (state.c == ',') {
                    ++state.x;
                    continue;
                }

                if (sequenceCheck(state, "=", true)) {
                    ++state.x;
                    if (!nextToken(state, true, "Expected an expression.")) return;
                    if (state.c == ',') {
                        addError(state, start, state.x, ErrorType.Error, "Expected an expression.");
                        ++state.x;
                        continue;
                    }
                } else {
                    addError(state, state.x, state.x + 1, ErrorType.Warning, "/var assignment without '='; this can cause problems with old versions of mIRC.");
                }

                ParseParameters(state);
                if (state.c == ',') ++state.x;
            } while (nextToken(state, true));
        }

        private static void ParseParameters(ParseState state) {
            ParseParameters(state, int.MaxValue);
        }
        private static void ParseParameters(ParseState state, int end) {
#if (DEBUG)
            if (end < state.line.Count && state.line[end].c != ':') throw new ArgumentException("The character at the end point was not the expected ':'.");
#endif
            var startBrackets = state.brackets;

            while (state.x < end && nextToken(state, end == int.MaxValue)) {
                var start = state.x;
                if (sequenceCheck(state, "{", true) || sequenceCheck(state, "|", true) || sequenceCheck(state, "}", true)) {
                    // These terminate a 'bare' parameter list only.
                    if (state.index == ParseStateIndex.ParameterList || state.index == ParseStateIndex.VarParameterList) {
                        if (state.braceLevel == 0 && state.c == '}') {
                            addError(state, state.x, state.x + 1, ErrorType.Error, "'}' without '{'.");
                            state.x = state.line.Count;
                        }
                        return;
                    }
                }
                ParseToken(state, startBrackets);

                if ((state.c == ')' || state.c == ',') && state.brackets == startBrackets) return;
            }
        }

        private static void ParseFunction(ParseState state, int end) {
            var start = state.x;
            var startBrackets = state.brackets;
            // TODO: Handle $? properly.
            skipToken(state, end, startBrackets, true);
            setStyle(state, start, state.x, FunctionStyle);
            state.tags.Add(new Tag() { range = new Range(state.textBox, start, state.lineIndex, state.x, state.lineIndex), type = TagType.FunctionCall });

            var functionName = GetLineSubstring(state.line, start, state.x);
            if (functionName.StartsWith("$?")) addError(state, start, start + 2, ErrorType.Note, "`$?` functions are currently unimplemented.");

            if (state.c == '(') {
                setStyleAndAdvance(state, 1, FunctionStyle);

                // Recurse brackets.
                var first = state.index;
                ++state.brackets;
                state.index = ParseStateIndex.ParameterListBracketed;

                if (functionName.Equals("$iif", StringComparison.OrdinalIgnoreCase)) {
                    // $iif works differently.
                    ParseCondition(state, true);
                    if (state.c != ',') {
                        addError(state, state.x, state.x + 1, ErrorType.Error, "Expected a logical operator or `,`.");
                        if (skipTo(state, ',') != ',') {
                            addError(state, state.x - 1, state.x, ErrorType.Error, "Missing ')'.");
                        }
                    }
                    ++state.x;
                    nextToken(state, true);
                    ParseParameters(state);
                } else {
                    ParseParameters(state);
                }


                while (state.c == ',') {
                    ++state.x;
                    ParseParameters(state);
                }

                state.index = first;
                if (state.c == ')') {
#if (DEBUG)
                    if (state.brackets != startBrackets + 1) impossible(9);
#endif
                    setStyleAndAdvance(state, 1, FunctionStyle);
                    --state.brackets;
                    if (state.c == '.') {
                        // Function property
                        start = state.x + 1;
                        skipToken(state, end, startBrackets, false);
                        setStyle(state, start, state.x, FunctionPropertyStyle);
                    } else {
                        start = state.x;
                        skipToken(state, end, startBrackets, false);
                        if (state.x != start)
                            addError(state, start, state.x, ErrorType.Error, "Unexpected characters after a function closing bracket.");
                    }
                } else {
                    addError(state, state.x - 1, state.x, ErrorType.Error, "Missing ')'.");
                    return;
                }
            }
        }

        private static void ParseCondition(ParseState state) {
            ParseCondition(state, false);
        }
        private static void ParseCondition(ParseState state, bool delimited) {
            var startState = state.index;
            var startBrackets = state.brackets;
            int operatorType = 0, start = state.x;
            // Ignore evaluation brackets.
            while (sequenceCheck(state, "[", true) || sequenceCheck(state, "]", true)) { ++state.x; nextToken(state, true); }

            //if (delimited) {
            // Without brackets, the condition must be of the form <word> <operator> <word>,
            //   or <word> <operator> with a type-3 operator.
            // <word> && <word> <operator> <word> is valid, too.
            // Bracketed conditions are different, because the operands can have any length, and there may be no operator at all.

            if (state.c == '(') {
                // Bracketed condition
                ++state.brackets;
                setStyleAndAdvance(state, 1, KeywordStyle);
                state.index = ParseStateIndex.Condition;
                ParseCondition(state, true);
                state.index = startState;
                if (state.c == ')') {
                    --state.brackets;
                    setStyleAndAdvance(state, 1, KeywordStyle);
                }
                nextToken(state, true);

                var result = sequenceCheck(state, operators4);
                if (result != null) {
                    operatorType = 4;
                    setStyleAndAdvance(state, result.Length, KeywordStyle);
                    nextToken(state, true, "Expected an expression.");
                }
            } else {
                // First operand
                bool multipleTokens = false;
                do {
                    if (!nextToken(state, true, "Expected an expression.")) return;
                    ParseToken(state, state.brackets);
                    if (!nextToken(state, true, "Expected an operator.")) return;

                    if (!delimited) {
                        while (sequenceCheck(state, "[", true) || sequenceCheck(state, "]", true)) { ++state.x; nextToken(state, true, "Expected an operator."); }

                        while (sequenceCheck(state, "$+", true)) {
                            setStyleAndAdvance(state, 2, FunctionStyle);
                            nextToken(state, true, "Expected an expression.");
                            ParseToken(state, state.brackets);
                            if (!nextToken(state, true)) return;
                        }
                    }

                    if (state.c == ')' || state.c == ',') {
                        if (multipleTokens || !delimited)
                            addError(state, state.x, state.x + 1, ErrorType.Error, "No recognised operator was found.");
                        return;
                    }

                    // Operator?
                    string result;
                    start = state.x;
                    if (state.c == '!') ++state.x;

                    if ((result = sequenceCheck(state, operators1)) != null) operatorType = 1;
                    else if ((result = sequenceCheck(state, operators2)) != null) operatorType = 2;
                    else if ((result = sequenceCheck(state, operators3)) != null) operatorType = 3;
                    else if (state.x == start && (result = sequenceCheck(state, operators4)) != null) operatorType = 4;

                    if (operatorType != 0) {
                        setStyleAndAdvance(state, result.Length, KeywordStyle);
                        nextToken(state, true, "Expected an expression.");
                    } else {
                        if (!delimited) {
                            skipToken(state, int.MaxValue, startBrackets, false);
                            addError(state, start, state.x, ErrorType.Error, "Unknown operator.");
                            break;
                        } else {
                            state.x = start;
                        }
                    }

                    multipleTokens = true;
                } while (operatorType == 0 && state.c != ')' && state.c != ',' && state.c != '\n');

                if (state.c == ')' || state.c == ',') {
                    if (operatorType == 1 || operatorType == 4) {
                        // mIRC does strange things here.
                        addError(state, state.x, state.x + 1, ErrorType.Error, "Expected an expression.");
                    }
                    return;
                }

                if (operatorType != 4 && operatorType != 3) {
                    // Second operand
                    do {
                        if (!nextToken(state, true, "Expected an expression.")) return;
                        ParseToken(state, state.brackets);
                        if (!nextToken(state, true, "Expected an operator.")) return;

                        if (!delimited) {
                            while (sequenceCheck(state, "[", true) || sequenceCheck(state, "]", true)) { ++state.x; nextToken(state, true, "Expected an operator."); }

                            while (sequenceCheck(state, "$+", true)) {
                                setStyleAndAdvance(state, 2, FunctionStyle);
                                nextToken(state, true, "Expected an expression.");
                                ParseToken(state, state.brackets);
                                if (!nextToken(state, true)) return;
                            }

                            break;
                        }

                        var result = sequenceCheck(state, operators4);
                        if (result != null) {
                            operatorType = 4;
                            setStyleAndAdvance(state, result.Length, KeywordStyle);
                            nextToken(state, true, "Expected an expression.");
                        }
                    } while (operatorType != 4 && state.c != ')' && state.c != ',' && state.c != '\n');
                }
            }

            if (operatorType == 4) {
                // Recurse for && and || operators.
                ParseCondition(state, false);
            }
        }

        private static void ParseToken(ParseState state, int startBrackets) {
            ParseToken(state, startBrackets, int.MaxValue);
        }
        private static void ParseToken(ParseState state, int startBrackets, int end) {
            var start = state.x;
            switch (state.c) {
                case '$':  // Function
                    ParseFunction(state, end);
                    return;
                case '%':  // Variable
                    skipToken(state, end, state.brackets, false);
                    state.tags.Add(new Tag() { range = new Range(state.textBox, start, state.lineIndex, state.x, state.lineIndex), type = TagType.Variable });
                    setStyle(state, start, state.x, VariableStyle);
                    return;
                case ')':
                    if (state.brackets == 0) { ++state.x; break; }
                    if (state.brackets != startBrackets) {
                        --state.brackets;
                        ++state.x;
                        break;
                    }
                    if (state.index == ParseStateIndex.ParameterListBracketed || state.index == ParseStateIndex.EventParameterList ||
                        state.index == ParseStateIndex.Condition) return;
                    break;
                case ',':  // Function parameter list delimiter, or /var delimiter.
                    if (state.brackets == 0) {
                        if (state.index == ParseStateIndex.ParameterListBracketed) {
                            ++state.x;
                            return;
                        } else if (state.index == ParseStateIndex.VarParameterList)
                            return;
                    }
                    ++state.x;
                    break;
            }
            if (sequenceCheck(state, "{", true) || sequenceCheck(state, "|", true) || sequenceCheck(state, "}", true)) {
                // These terminate a 'bare' parameter list only.
                if (state.index == ParseStateIndex.ParameterList) {
                    if (state.braceLevel == 0 && state.c == '}') {
                        addError(state, state.x, state.x + 1, ErrorType.Error, "'}' without '{'.");
                        state.x = state.line.Count;
                        return;
                    }
                }
            }
            // Anything else is simply text.
            skipToken(state, end, startBrackets, false);
        }

        /// <summary>Advances the parser past the current name.</summary>
        /// <remarks>
        /// What this should terminate at depends on the situation.
        /// For example, inside a function, a comma immediately after a variable name isn't part of the variable name.
        /// </remarks>
        private static void skipToken(ParseState state, int end, int startBrackets, bool function) {
            if (end == int.MaxValue) end = state.line.Count;
            if (end > state.line.Count) throw new ArgumentOutOfRangeException("end");

            // Outside of brackets, a token ends at a space only.
            // Inside a function, a token can also end at ',' or ')'.
            for (; state.x < end; ++state.x) {
                switch (state.c) {
                    case ' ': return;
                    case ',':
                        if (state.brackets == startBrackets &&
                            (state.index == ParseStateIndex.ParameterListBracketed || state.index == ParseStateIndex.VarParameterList))
                            return;
                        break;
                    case '(':
                        if (function) return;
                        ++state.brackets;
                        break;
                    case ')':
                        if (state.brackets != 0 &&
                            (state.index == ParseStateIndex.ParameterListBracketed ||
                             state.index == ParseStateIndex.EventParameterList ||
                             state.index == ParseStateIndex.Condition)) return;
                        --state.brackets;
                        break;
                }
            }
        }

        /// <summary>Checks whether a specified string is present at a specified position in the line.</summary>
        /// <param name="i">The position to search.</param>
        /// <param name="sequence">The string to search for.</param>
        /// <param name="requireBoundary">If true, there must be whitespace or the end of the line after the string for the search to succeed.</param>
        /// <returns>A Boolean value indicating whether or not the search succeeded.</returns>
        private static bool sequenceCheck(ParseState state, string sequence, bool requireBoundary) {
            if (state.x > state.line.Count - sequence.Length) return false;
            for (int j = 0; j < sequence.Length; ++j) {
                char c1 = sequence[j]; char c2;
                // Set c2 to the opposite case version of c1.
                if (c1 >= 'A' && c1 <= 'Z') c2 = (char) (c1 + 0x20);
                else if (c1 >= 'a' && c1 <= 'z') c2 = (char) (c1 - 0x20);
                else c2 = c1;

                // Check each character.
                char c = state.line[state.x + j].c;
                if (c != c1 && c != c2) return false;
            }
            // Check that this is a whitespace boundary.
            return (!requireBoundary || state.x == state.line.Count - sequence.Length || state.line[state.x + sequence.Length].c == ' ');
        }
        /// <summary>Checks whether any string in a list is present at a given position in the text.</summary>
        /// <param name="i">The position to search.</param>
        /// <param name="sequences">The list of strings to search for. The list must be in lexicographical order, and all in lowercase.</param>
        /// <param name="state">Not used.</param>
        /// <returns>The string that was matched, or null if none were matched.</returns>
        private static string sequenceCheck(ParseState state, IEnumerable<string> sequences, char boundary = ' ') {
            int j; int pos; IEnumerator<string> enumerator = sequences.GetEnumerator();
            enumerator.MoveNext();

            for (j = 0; (pos = state.x + j) < state.line.Count; ++j) {
                // Set c to lower case.
                char c = state.line[pos].c;
                if (c == boundary || c == ' ' || c == ')' || c == ',') break;
                if (c >= 'A' && c <= 'Z') c += (char) 0x20;

                while (j >= enumerator.Current.Length || enumerator.Current[j] != c) {
                    // No match for this entry.
                    if (enumerator.MoveNext() == false) return null;
                    if (enumerator.Current.Length <= j) return null;
                    for (int k = j - 1; k >= 0; --k) {
                        char c2 = state.line[state.x + k].c;
                        if (c2 >= 'A' && c2 <= 'Z') c2 += (char) 0x20;
                        if (c2 != enumerator.Current[k]) return null;
                    }
                }
            }
            if (j == enumerator.Current.Length) return enumerator.Current;
            return null;
        }

        /// <summary>Advances the parse state to the next occurrence of one of the target characters.</summary>
        /// <returns>The character that was matched, or '\n' if the end of the line was reached.</returns>
        private static char skipTo(ParseState state, params char[] targets) {
            return skipTo(state, state.line.Count, targets);
        }
        private static char skipTo(ParseState state, int end, params char[] targets) {
            if (end == int.MaxValue) end = state.line.Count;
            if (end > state.line.Count) throw new ArgumentOutOfRangeException("end");

            for (; state.x < end; ++state.x) {
                foreach (char c2 in targets)
                    if (state.line[state.x].c == c2) return c2;
            }
            return '\n';
        }

        /// <summary>
        /// Advances the parse state to the next non-whitespace character, optionally following "$&amp;" continuations, and optionally giving an error message
        /// if the end of the line is reached.
        /// </summary>
        /// <returns>True if a character was found; false if the end of the line was reached.</returns>
        private static bool nextToken(ParseState state, bool handleContinuations, string errorMessage = null) {
            while (state.x < state.line.Count) {
                if (state.c != ' ') {
                    if (handleContinuations && sequenceCheck(state, "$&", true)) {
                        // Continuation operator.
                        var start = state.x;
                        if (state.textBox is ConsoleTextBox) {
                            addError(state, state.x, state.x + 2, ErrorType.Error, "The continuation operator is not valid here.");
                            state.x = state.line.Count;
                            return false;
                        }
                        state.x += 2;
                        if (nextToken(state, false)) {
                            state.x = start;
                            addError(state, state.x, state.x + 2, ErrorType.Error, "The continuation operator must be the last thing on the line.");
                            return true;
                        }
                        state.x = start;

                        setStyle(state, state.x, state.x + 2, KeywordStyle);

                        ++state.lineIndex;
                        state.x = 0;
                        if (state.lineIndex == state.textBox.LinesCount) {
                            // The last line has a continuation operator.
                            --state.lineIndex;
                            state.x = state.line.Count;
                            if (errorMessage != null)
                                addError(state, state.line.Count - 1, state.line.Count, ErrorType.Error, "Unexpected end of file: " + errorMessage);
                            return false;
                        }

                        new Range(state.textBox, state.lineIndex).ClearStyle(StyleIndex.All ^ (StyleIndex) (1 << state.textBox.GetStyleIndex(ControlCharStyle.Instance)));

                        // Mark the line as a continuation.
                        var lineInfoTable = ((MslDocument) state.textBox.Tag).lineInfoTable;
                        var info = new LineInfo() { bookmark = state.bookmark, BraceLevel = (sbyte) state.braceLevel, errors = state.errors.ToArray(), State = (byte) ParseStateIndex.Continuation, tags = state.tags };
                        lineInfoTable[state.textBox[state.lineIndex - 1].UniqueId] = info;
                    } else
                        return true;
                } else
                    ++state.x;
            }
            if (errorMessage != null) {
                if (state.line.Count == 0) {
                    --state.lineIndex;
                    addError(state, state.line.Count - 1, state.line.Count, ErrorType.Error, "Unexpected end of line: " + errorMessage);
                    ++state.lineIndex;
                } else
                    addError(state, state.line.Count - 1, state.line.Count, ErrorType.Error, "Unexpected end of line: " + errorMessage);
            }
            return false;
        }

        private static void HighlightVariables(int lineIndex, DocumentType syntaxType, FastColoredTextBox textBox) {
            throw new NotImplementedException();
        }

        public static LineInfo HighlightIni(FastColoredTextBox textBox, int lineIndex, DocumentType syntaxType) {
            ParseState state = new ParseState() { type = syntaxType, textBox = textBox, lineIndex = lineIndex, x = 0 };

            if (state.c == '[') {
                setStyle(state, 0, state.line.Count, FunctionStyle);

                var end = state.line.Text.LastIndexOf(']');
                if (end == -1) end = state.line.Count;
                state.SetBookmark(BookmarkType.IniSection, GetLineSubstring(state.line, 1, end));
            } else {
                if (skipTo(state, '=') == '=')
                    setStyle(state, 0, state.x, VariableStyle);
            }

            return null;
        }

        internal class Event {
            public string BookmarkFormat { get; }
            public string Description { get; }
            public ParseParameterDelegate[] Parameters { get; }

            internal Event(string bookmarkFormat, string description, ParseParameterDelegate[] parameters) {
                this.BookmarkFormat = bookmarkFormat;
                this.Description = description;
                this.Parameters = parameters;
            }
        }

        internal class ParseState {
            /// <summary>The backing field for lineIndex.</summary>
            private int _lineIndex;

            /// <summary>The text box whose contents are currently being parsed.</summary>
            public FastColoredTextBox textBox;
            /// <summary>The type of syntax currently being parsed.</summary>
            public DocumentType type;

            /// <summary>The zero-based index of the line currently being parsed.</summary>
            public int lineIndex {
                get { return this._lineIndex; }
                set {
                    this._lineIndex = value;
                    if (value < 0 || value >= this.textBox.LinesCount) this.line = null;
                    else this.line = this.textBox[value];
                }
            }
            /// <summary>The current position of the parser on the line.</summary>
            public int x;
            /// <summary>Returns the line currently being parsed, or null if the value of lineIndex is invalid.</summary>
            public Line line { get; private set; }
            /// <summary>A value describing what the parser is currently seeing.</summary>
            public ParseStateIndex _index = ParseStateIndex.Root;
            public ParseStateIndex index { get { return _index; } set { if (index == ParseStateIndex.Continuation) Program.impossible(8); _index = value; } }
            /// <summary>The depth of nested braces the parser is currently in.</summary>
            public int braceLevel;
            /// <summary>The depth of nested parentheses the parser is currently in.</summary>
            public int brackets;
            /// <summary>The bookmark of the current line, or null if there is none.</summary>
            public MslBookmark bookmark;
            /// <summary>A list of syntax errors seen on the current line.</summary>
            public List<Error> errors = new List<Error>();
            /// <summary>A list of tags on the current line.</summary>
            public List<Tag> tags = new List<Tag>();
            /// <summary>The statement currently being parsed.</summary>
            public Tag statement;

            /// <summary>The character at the current position of the parser, or '\n' if the end of the line was reached.</summary>
            public char c {
                get {
                    if (this.x >= this.line.Count) return '\n';
                    return this.line[this.x].c;
                }
            }

            public void SetBookmark(BookmarkType type, string title) {
                this.SetBookmark(type, title, false);
            }
            public void SetBookmark(BookmarkType type, string title, bool hidden) {
                if (this.bookmark != null && this.bookmark.type == type && this.bookmark.position == this.lineIndex) {
                    if (this.bookmark.title != title)
                        this.bookmark.title = title;
                    this.bookmark.hidden = hidden;
                } else
                    this.bookmark = new MslBookmark(type, title, this.lineIndex) { hidden = hidden };
            }
        }

        public enum ParseStateIndex {
            /// <status>A comment at root level.</status>
            CommentRoot = 1,
            /// <status>A comment inside a menu block.</status>
            CommentMenu,
            /// <status>A comment inside a dialog block.</status>
            CommentDialog,
            /// <status>A comment inside a function.</status>
            CommentFunction,
            /// <summary>Root level code (not inside a function, menu or dialog block).</summary>
            Root,
            /// <summary>Content of a menu block.</summary>
            MenuBody,
            /// <summary>Content of a dialog block.</summary>
            DialogBody,
            /// <summary>Content of a function (event, alias or menu code block).</summary>
            FunctionBody,
            // ----- Lines should not be marked with values below this line, except Continuation. -----
            /// <summary>A menu item command.</summary>
            MenuCommand,
            /// <summary>The condition of an if or while command.</summary>
            Condition,
            /// <summary>The parameters of a 'bare' command.</summary>
            ParameterList,
            /// <summary>The paramaters of a function, with brackets.</summary>
            ParameterListBracketed,
            /// <summary>The parameters of a dynamic pattern in an event.</summary>
            EventParameterList,
            /// <summary>The parameters of a /var command.</summary>
            VarParameterList,
            /// <summary>The parameters of a /if command.</summary>
            IfParameterList,
            /// <summary>An expression in the console.</summary>
            ConsoleExpression,

            /// <summary>The line is continued by a terminating `$&amp;`.</summary>
            Continuation
        }

        private static class ParameterParser {
            internal static void WindowType(ParseState state) {
                var start = state.x;
                var c = state.c;
                if (skipTo(state, ':') != ':') {
                    addError(state, state.x - 1, state.x, ErrorType.Error, "Event missing a command.");
                    return;
                }
                if (state.x - start == 1) {
                    if (c == '*' || c == '#' || c == '?' || c == '=' || c == '!' || c == '@')
                        return;
                }
                addError(state, start, state.x, ErrorType.Error, "Window type parameter must be one of the following: * # ? = ! @");
            }

            internal static void BanTarget(ParseState state) {
                while (true) {
                    var start = state.x;
                    var c = state.c;
                    if (c == '%') {
                        // A variable can be used in place of a scope, but it has to be the entire parameter.
                        switch (skipTo(state, ':', ' ')) {
                            case ' ':
                                skipTo(state, ':');
                                addError(state, start, state.x, ErrorType.Warning, "A variable used as a scope must be the entire parameter.");
                                return;
                            case ':':
                                setStyle(state, start, state.x, VariableStyle);
                                return;
                            case '\n':
                                addError(state, start, state.x, ErrorType.Warning, "Unexpected end of line.");
                                return;
                        }
                        Program.impossible(1);
                    }

                    skipTo(state, ',', ':');
                    
                    // Check that the channel name is valid.
                    if (state.x != start) {
                        if ((c == '*' && state.x - start == 1) || c == '#' || c == '&' || c == '+' || c == '!') continue;
                        addError(state, start, state.x, ErrorType.Error, "Invalid channel name.");
                    } else
                        addError(state, state.x, state.x + 1, ErrorType.Error, "Invalid channel name.");

                    if (state.c == ',') {
                        ++state.x;
                        continue;
                    } else
                        return;
                }
            }

            internal static void DCCType(ParseState state) {
                if (sequenceCheck(state, new string[] { "chat", "fserve", "send" }, ':') != null) {
                    skipTo(state, ':');
                    return;
                }
                var start = state.x;
                skipTo(state, ':');
                addError(state, start, state.x, ErrorType.Error, "DCC type parameter must be one of the following: chat send fserve");
            }

            internal static void DialogEvent(ParseState state) {
                if (sequenceCheck(state, new string[] { "*", "close", "dclick", "drop", "edit", "init", "menu", "mouse", "rclick", "sclick", "scroll", "uclick" }, ':') != null) {
                    skipTo(state, ':');
                    return;
                }
                var start = state.x;
                skipTo(state, ':');
                addError(state, start, state.x, ErrorType.Error, "Dialog event parameter must be one of the following: * init close edit menu scroll mouse sclick uclick dclick rclick drop");
            }
            internal static void DialogID(ParseState state) {
                if (sequenceCheck(state, "*:", false)) {
                    ++state.x;
                    return;
                }

                while (state.c != '\n') {
                    var start = state.x;
                    var c = state.c;
                    var valid = true;

                    for (; state.c != ',' && state.c != '-' && state.c != ':' && state.c != '\n'; ++state.x) {
                        if (state.c < '0' || state.c > '9') {
                            valid = false;
                            skipTo(state, ',', '-', ':');
                            break;
                        }
                    }
                    if (!valid) addError(state, start, state.x, ErrorType.Error, "Invalid control ID.");

                    // Check for a range.
                    if (state.c == '-') {
                        valid = true;
                        ++state.x;
                        start = state.x;
                        for (; state.c != ',' && state.c != ':' && state.c != '\n'; ++state.x) {
                            if (state.c < '0' || state.c > '9') {
                                valid = false;
                                skipTo(state, ',', ':');
                                break;
                            }
                        }
                        if (!valid) addError(state, start, state.x, ErrorType.Error, "Invalid control ID.");
                    }

                    if (state.c == '\n') addError(state, state.x - 1, state.x, ErrorType.Error, "Event missing a command.");
                    if (state.c == ':') return;
                    ++state.x;
                }
            }

            internal static void CustomWindow(ParseState state) {
                var start = state.x;
                var c = state.c;

                skipTo(state, ':');

                if (!(c == '@' || (c == '*' && state.x - start == 1)))
                    addError(state, start, state.x, ErrorType.Error, "Invalid custom window name.");
            }

            internal static void ParselineType(ParseState state) {
                if (sequenceCheck(state, new string[] { "*", "in", "out" }, ':') != null) {
                    skipTo(state, ':');
                    return;
                }
                var start = state.x;
                skipTo(state, ':');
                addError(state, start, state.x, ErrorType.Error, "Parseline type parameter must be one of the following: * in out");
            }

            internal static void TextPattern(ParseState state) {
                var start = state.x;
                if (state.c == '%') {
                    // A variable can be used in place of a pattern, but it has to be the entire parameter.
                    switch (skipTo(state, ':', ' ')) {
                        case ' ':
                            skipTo(state, ':');
                            addError(state, start, state.x, ErrorType.Warning, "A variable used as a text pattern must be the entire parameter. Did you mean to use '$('?");
                            return;
                        case ':':
                            setStyle(state, start, state.x, VariableStyle);
                            return;
                        case '\n':
                            addError(state, start, state.x, ErrorType.Warning, "Unexpected end of line.");
                            return;
                    }
                    Program.impossible(1);
                } else if (sequenceCheck(state, "$(", false)) {
                    setStyleAndAdvance(state, 2, KeywordStyle);
                    start = state.x;
                    skipTo(state, ':');                    
                    var end = state.x;  // The parameter ends at any ':'. It can't even be used inside a dynamic pattern.
                    state.x = start;
                    ParseParameters(state, end);

                    if (state.c == ')') {
                        setStyleAndAdvance(state, 1, KeywordStyle);
                        if (state.c == ':') {
                            return;
                        } else {
                            state.x = end;
                            addError(state, start, state.x, ErrorType.Error, "A dynamic pattern must be the entire parameter.");
                        }
                    } else if (state.c == ':') {
                        addError(state, state.x, state.x + 1, ErrorType.Error, "Unexpected end of parameter inside dynamic pattern.");
                    } else if (state.c == '\n') {
                        addError(state, state.x - 1, state.x, ErrorType.Error, "Unexpected end of line inside dynamic pattern.");
                    }
#if (DEBUG)
                    else
                        Program.impossible(2);
#endif
                } else
                    skipTo(state, ':');
            }
            internal static void TextScope(ParseState state) {
                var start = state.x;
                var c = state.c;

                if (c == '%') {
                    // A variable can be used in place of a scope, but it has to be the entire parameter.
                    switch (skipTo(state, ':', ' ')) {
                        case ' ':
                            skipTo(state, ':');
                            addError(state, start, state.x, ErrorType.Warning, "A variable used as a scope must be the entire parameter.");
                            return;
                        case ':':
                            setStyle(state, start, state.x, VariableStyle);
                            return;
                        case '\n':
                            addError(state, start, state.x, ErrorType.Warning, "Unexpected end of line.");
                            return;
                    }
                    Program.impossible(1);
                }

                skipTo(state, ':');

                if (c == '#' || c == '&' || c == '+' || c == '!') return;
                if (state.x - start == 1 && (c == '*' || c == '?')) return;
                addError(state, start, state.x, ErrorType.Error, "The scope must be a channel name or one of the following: # ? *");
            }

            internal static void Anything(ParseState state) {
                if (skipTo(state, ':') == ':') return;
                addError(state, state.x - 1, state.x, ErrorType.Error, "Unexpected end of line.");
            }

        }

    }
}

