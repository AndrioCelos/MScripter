using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using FastColoredTextBoxNS;
using System.IO;
using static System.Environment;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MScripter {
#if (!MONO)
    public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);
#endif

    public static class Program {
        public static Encoding encoding;

        public static string[] commands = new string[] {
            "abook", "ajinvite", "alias", "aline", "ame", "amsg", "anick", "aop", "auser",
            "autojoin", "avoice", "away", "background", "ban", "bcopy", "beep", "break",
            "breplace", "bset", "btrunc", "bunset", "bwrite", "channel", "clear", "clearall",
            "cline", "clipboard", "close", "cnick", "color", "comclose", "comopen", "comreg",
            "continue", "copy", "crew", "ctcpreply", "ctcps", "dcc", "dccserver", "dde",
            "ddeserver", "debug", "dec", "describe", "dialog", "did", "didtok", "disable",
            "disconnect", "dlevel", "dline", "dll", "dns", "dqwindow", "drawcopy", "drawdot",
            "drawfill", "drawline", "drawpic", "drawrect", "drawreplace", "drawrot", "drawsave",
            "drawscroll", "drawtext", "ebeeps", "echo", "editbox", "else", "elseif", "emailaddr",
            "enable", "events", "exit", "fclose", "filter", "findtext", "finger", "firewall",
            "flash", "flist", "flood", "flush", "flushini", "font", "fopen", "fseek", "fsend",
            "fserve", "fullname", "fwrite", "ghide", "gload", "gmove", "gopts", "goto", "gplay",
            "gpoint", "gqreq", "groups", "gshow", "gsize", "gstop", "gtalk", "gunload", "guser",
            "hadd", "halt", "haltdef", "hdec", "hdel", "help", "hfree", "hinc", "hload", "hmake",
            "hop", "hsave", "ial", "ialclear", "ialmark", "identd", "if", "ignore", "iline", "inc",
            "invite", "iuser", "join", "kick", "linesep", "links", "list", "load", "loadbuf",
            "localinfo", "log", "mdi", "me", "menubar", "mkdir", "mnick", "mode", "msg", "nick",
            "noop", "notice", "omsg", "onotice", "part", "partall", "pdcc", "perform", "play",
            "playctrl", "pop", "protect", "pvoice", "qme", "qmsg", "query", "queryrn", "quit",
            "raw", "reload", "remini", "remote", "remove", "rename", "renwin", "reseterror",
            "resetidle", "return", "rlevel", "rline", "rmdir", "run", "ruser", "save", "savebuf",
            "saveini", "say", "scid", "scon", "server", "set", "showmirc", "signal", "sline",
            "sockaccept", "sockclose", "socklist", "socklisten", "sockmark", "sockopen",
            "sockpause", "sockread", "sockrename", "sockudp", "sockwrite", "sound", "speak",
            "splay", "sreq", "strip", "switchbar", "timer", "timestamp", "titlebar", "tnick",
            "tokenize", "toolbar", "topic", "tray", "treebar", "ulist", "unload", "unset",
            "unsetall", "updatenl", "url", "uwho", "var", "vcadd", "vcmd", "vcrem", "vol", "while",
            "whois", "window", "winhelp", "write", "writeini"
        };
        public static string[] functions = new string[] {
            "$abook", "$abs", "$active", "$activecid", "$activewid", "$address", "$addtok",
            "$agent", "$agentname", "$agentstat", "$agentver", "$alias", "$and", "$anick",
            "$ansi2mirc", "$aop", "$appactive", "$appstate", "$asc", "$asctime", "$asin", "$atan",
            "$avoice", "$away", "$awaymsg", "$awaytime", "$banmask", "$base", "$bfind", "$bitoff",
            "$biton", "$bnick", "$bvar", "$bytes", "$calc", "$cb", "$cd", "$ceil", "$chan",
            "$chanmodes", "$chantypes", "$chat", "$chr", "$cid", "$clevel", "$click", "$cmdbox",
            "$cmdline", "$cnick", "$color", "$com", "$comcall", "$comchan", "$comerr", "$compact",
            "$compress", "$comval", "$cos", "$count", "$cr", "$crc", "$crew", "$crlf", "$ctime",
            "$ctimer", "$ctrlenter", "$date", "$day", "$daylight", "$dbuh", "$dbuw", "$dccignore",
            "$dccport", "$dde", "$ddename", "$debug", "$decode", "$decompress", "$deltok",
            "$devent", "$dialog", "$did", "$didreg", "$didtok", "$didwm", "$disk", "$dlevel",
            "$dll", "$dllcall", "$dname", "$dns", "$duration", "$ebeeps", "$editbox", "$emailaddr",
            "$encode", "$error", "$eval", "$event", "$exists", "$false", "$feof", "$ferr",
            "$fgetc", "$file", "$filename", "$filtered", "$finddir", "$finddirn", "$findfile",
            "$findfilen", "$findtok", "$fline", "$floor", "$fopen", "$fread", "$fserve",
            "$fulladdress", "$fulldate", "$fullname", "$fullscreen", "$get", "$getdir", "$getdot",
            "$gettok", "$gmt", "$group", "$halted", "$hash", "$height", "$hfind", "$hget",
            "$highlight", "$hnick", "$host", "$hotline", "$hotlinepos", "$ial", "$ialchan", "$ibl",
            "$idle", "$iel", "$ifmatch", "$ignore", "$iif", "$iil", "$inellipse", "$ini",
            "$inmidi", "$inpaste", "$inpoly", "$input", "$inrect", "$inroundrect", "$insong",
            "$instok", "$int", "$inwave", "$ip", "$isalias", "$isbit", "$isdde", "$isdir",
            "$isfile", "$isid", "$islower", "$istok", "$isupper", "$keychar", "$keyrpt", "$keyval",
            "$knick", "$lactive", "$lactivecid", "$left", "$len", "$level", "$lf", "$line",
            "$lines", "$link", "$lock", "$locked", "$log", "$logdir", "$logstamp", "$logstampfmt",
            "$longfn", "$longip", "$lower", "$ltimer", "$maddress", "$mask", "$matchkey",
            "$matchtok", "$md5", "$me", "$menu", "$menubar", "$menucontext", "$menutype", "$mid",
            "$middir", "$mircdir", "$mircexe", "$mircini", "$mklogfn", "$mnick", "$mode",
            "$modefirst", "$modelast", "$modespl", "$mouse", "$msfile", "$network", "$newnick",
            "$nick", "$nofile", "$nopath", "$noqt", "$not", "$notags", "$notify", "$null",
            "$numeric", "$numtok", "$online", "$onpoly", "$opnick", "$or", "$ord", "$os",
            "$passivedcc", "$pic", "$play", "$pnick", "$port", "$portable", "$portfree", "$pos",
            "$prefix", "$prop", "$protect", "$puttok", "$qt", "$query", "$rand", "$rawmsg",
            "$read", "$readini", "$readn", "$regex", "$regml", "$regsub", "$regsubex", "$remove",
            "$remtok", "$replace", "$replacex", "$reptok", "$result", "$rgb", "$right", "$round",
            "$scid", "$scon", "$script", "$scriptdir", "$scriptline", "$sdir", "$send", "$server",
            "$serverip", "$sfile", "$sha1", "$shortfn", "$show", "$signal", "$sin", "$site",
            "$sline", "$snick", "$snicks", "$snotify", "$sock", "$sockbr", "$sockerr", "$sockname",
            "$sorttok", "$sound", "$sqrt", "$sreg", "$ssl", "$sslready", "$status", "$str",
            "$strip", "$stripped", "$style", "$submenu", "$switchbar", "$tan", "$target", "$ticks",
            "$time", "$timer", "$timestamp", "$timestampfmt", "$timezone", "$tip", "$titlebar",
            "$toolbar", "$treebar", "$true", "$trust", "$ulevel", "$ulist", "$upper", "$uptime",
            "$url", "$usermode", "$v1", "$v2", "$var", "$vcmd", "$vcmdstat", "$vcmdver",
            "$version", "$vnick", "$vol", "$wid", "$width", "$wildsite", "$wildtok", "$window",
            "$wrap", "$xor"
        };
        // These always have two operands.
        public static string[] operators1 = new string[] {
            "&", "//", "<", "<=", "=", "==", "===", ">", ">=", @"\\",
            "isban", "ishop", "isignore", "isin", "isincs",
            "ison", "isop", "isreg",
            "isupper", "isvoice", "iswm", "iswmcs"
        };
        // These can have one or two operands.
        public static string[] operators2 = new string[] {
           "isaop", "isavoice", "isignore", "isletter", "isnum", "isprotect"
        };
        // These always have one operand.
        public static string[] operators3 = new string[] {
            "isalnum", "isalpha", "ischan", "islower", "isnotify", "isupper"
        };
        // These are used to combine conditions.
        public static string[] operators4 = new string[] {
            "&&", "||"
        };

        internal static Config Config = Config.Default;

        internal static void impossible(int number) {
#if (DEBUG)
            System.Diagnostics.Debugger.Break();
#endif
        }

        internal static MainForm mainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        internal static int Main(string[] args) {
            encoding = Encoding.GetEncoding("UTF-8", new EncoderReplacementFallback(), new DecoderExceptionFallback());

            MslSyntaxHighlighter.CommentStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
            MslSyntaxHighlighter.KeywordStyle = new TextStyle(Brushes.RoyalBlue, null, FontStyle.Regular);
            MslSyntaxHighlighter.CommandStyle = new TextStyle(Brushes.Magenta, null, FontStyle.Regular);
            MslSyntaxHighlighter.CustomCommandStyle = new TextStyle(Brushes.Purple, null, FontStyle.Regular);
            MslSyntaxHighlighter.FunctionStyle = new TextStyle(Brushes.Olive, null, FontStyle.Regular); ;
            MslSyntaxHighlighter.CustomFunctionStyle = new TextStyle(Brushes.Olive, null, FontStyle.Regular);
            MslSyntaxHighlighter.FunctionPropertyStyle = new TextStyle(Brushes.GreenYellow, null, FontStyle.Regular);
            MslSyntaxHighlighter.VariableStyle = new TextStyle(Brushes.Maroon, null, FontStyle.Regular);
            MslSyntaxHighlighter.AliasStyle = new TextStyle(null, null, FontStyle.Bold);
            MslSyntaxHighlighter.ErrorStyle = new WavyLineStyle(255, Color.Red);
            MslSyntaxHighlighter.WarningStyle = new WavyLineStyle(255, Color.Green);
            MslSyntaxHighlighter.NoticeStyle = new WavyLineStyle(255, Color.Blue);

            Exception configFailure = null;

            try {
                Program.LoadConfig();
            } catch (Exception ex) {
                configFailure = ex;
            }

            int i = 0;
            // Parse switches.
            if (args != null) {
                for (i = 0; i < args.Length; ++i) {
                    if (args[i] == "--") { ++i; break; }
                    if (!args[i].StartsWith("-") && !args[i].StartsWith("/")) break;

                    switch (args[i].TrimStart('-', '/')) {
                        case "h":
                        case "help":
                        case "?":
                            Console.WriteLine("Usage: " + Path.GetFileName(Application.ExecutablePath) + " [--nosync] [--] [filename ...]");
                            Console.WriteLine("    --nosync: surpresses search for mIRC instances on startup.");
                            return 0;
                        case "n":
                        case "nosync":
                            Config.NoSync = true;
                            break;
                        default:
                            Console.Error.WriteLine("Unknown switch " + args[i]);
                            return 1;
                    }

                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            mainForm = new MainForm(getPaths(args, i), configFailure);
            Application.Run(mainForm);
            return 0;
        }

        private static IEnumerable<string> getPaths(string[] args, int i) {
            for (; i < args.Length; ++i)
                yield return args[i];
        }

        public static string ConfigFileDefault
            => Path.Combine(Environment.GetFolderPath(SpecialFolder.ApplicationData), "MScripter", "config.json");

        public static void LoadConfig() {
            string filePath = "config.json";
            if (!File.Exists(filePath)) {
                filePath = ConfigFileDefault;
                if (!File.Exists(filePath)) filePath = null;
            }

            if (filePath != null) {
                string json = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(json))
                    Config = JsonConvert.DeserializeObject<Config>(json);
            }

            if (Config == null) {
                // Create the default config file.
                Config = Config.Default;
                SaveConfig();
                return;
            }
        }

        public static void SaveConfig() {
            string filePath = "config.json";
            if (!File.Exists(filePath)) {
                filePath = Path.Combine(Environment.GetFolderPath(SpecialFolder.ApplicationData), "MScripter");
                Directory.CreateDirectory(filePath);
                filePath = Path.Combine(filePath, "config.json");
            }

            string json = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static void ApplyConfig(Config config) {
            ((TextStyle) MslSyntaxHighlighter.VariableStyle).ForeBrush = new SolidBrush(config.SyntaxColours.VariableStyle.TrueTextColour);
            ((TextStyle) MslSyntaxHighlighter.VariableStyle).BackgroundBrush = new SolidBrush(config.SyntaxColours.VariableStyle.TrueBackgroundColour);
            ((TextStyle) MslSyntaxHighlighter.VariableStyle).FontStyle = config.SyntaxColours.VariableStyle.Style;

            mainForm.applyConfig(config);
        }

#if (!MONO)
        [Flags]
        public enum FileMapProtection : uint {
            PageReadonly = 0x02,
            PageReadWrite = 0x04,
            PageWriteCopy = 0x08,
            PageExecuteRead = 0x20,
            PageExecuteReadWrite = 0x40,
            SectionCommit = 0x8000000,
            SectionImage = 0x1000000,
            SectionNoCache = 0x10000000,
            SectionReserve = 0x4000000,
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFileMapping(
            IntPtr hFile,
            IntPtr lpFileMappingAttributes,
            FileMapProtection flProtect,
            uint dwMaximumSizeHigh,
            uint dwMaximumSizeLow,
            string lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr MapViewOfFile(
            IntPtr hFileMappingObject,
            uint dwDesiredAccess,
            uint dwFileOffsetHigh,
            uint dwFileOffsetLow,
            uint dwNumberOfBytesToMap);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(
            IntPtr hWnd,
            uint msg,
            IntPtr wParam,
            IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SendMessageCallbackW(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam, SendMessageDelegate lpCallBack, IntPtr dwData);

        public delegate void SendMessageDelegate(IntPtr hWnd, uint uMsg, IntPtr dwData, IntPtr lResult);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(
            string lpcClassName,
            string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")] [Obsolete("This function doesn't take into account what font the form actually uses.")]
        public static extern int GetDialogBaseUnits();
#endif

        [Obsolete("This function doesn't take into account what font the form actually uses.")]
        public static Size GetDialogBaseUnitSize() {
#if (!MONO)
            int result = GetDialogBaseUnits();
            return new Size((result & 0xFFFF) / 4, (result / 0x10000) / 8);
#else
            return new Size(2, 2);
#endif
        }

        public static string GetTypeString(DocumentType type) {
            switch (type) {
                case DocumentType.RemoteScript: return "remote script";
                case DocumentType.AliasScript: return "alias script";
                case DocumentType.Popup: return "popup script";
                case DocumentType.Users: return "user list";
                case DocumentType.Variables: return "variable list";
                case DocumentType.INI: return "INI file";
                case DocumentType.Text: return "text file";
                default: throw new ArgumentException("type is not a valid value.");
            }
        }

        public static string GetLineSubstring(Line line, int start, int end) {
            StringBuilder builder = new StringBuilder(end - start);
            for (int i = start; i < end; ++i)
                builder.Append(line[i].c);
            return builder.ToString();
        }
    }

    public struct RECT {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}
