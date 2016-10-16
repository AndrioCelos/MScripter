using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MIDE {
    public class Config : ICloneable {
        private Theme theme;
        [JsonConverter(typeof(StringEnumConverter))]
        public Theme Theme {
            get { return this.theme; }
            set {
                this.theme = value;
                this.SetThemeDefaults();
            }
        }

        public bool DefaultIniFormat = false;
        public bool AssumeMrcFilesAreRemote = false;
        public string PreferredAliasExtension = "mrc";

        // Code editor
        public bool ShowLineNumbers = true;
        public int PreferredIndentationCount = 2;
        public Font EditorFont;

        public SyntaxColourTable SyntaxColours = SyntaxColourTable.Default;
        
        // Synchroniser
        public string mIRCInstanceLabel = "mIRC $titlebar (PID: $pid)";

        // Dialog designer
        [JsonConverter(typeof(StringEnumConverter))]
        public DialogSizeUnit DefaultDialogSizeUnit = DialogSizeUnit.Pixel;

        // Switches
        public bool NoSync = false;

        public static Config Default => new Config();

        public Config() {
            var fonts = new InstalledFontCollection();
            FontFamily family = fonts.Families.FirstOrDefault(f => f.Name == "Consolas");
            if (family == null) family = FontFamily.GenericMonospace;

            this.EditorFont = new Font(family, 9.75f, FontStyle.Regular);
        }

        public Config Clone() {
            var clone = (Config) this.MemberwiseClone();
            clone.SyntaxColours = this.SyntaxColours.Clone();
            return clone;
        }
        object ICloneable.Clone() => this.Clone();

        public void ApplyTheme(FastColoredTextBoxNS.FastColoredTextBox textBox) {
            textBox.ForeColor = this.SyntaxColours.TextStyle.TrueTextColour;
            textBox.BackColor = this.SyntaxColours.TextStyle.TrueBackgroundColour;
            switch (this.Theme) {
                case Theme.Default:
                    textBox.CaretColor = Color.Black;
                    textBox.FoldingIndicatorColor = Color.Green;
                    textBox.IndentBackColor = Color.WhiteSmoke;
                    textBox.LineNumberColor = Color.Teal;
                    textBox.PaddingBackColor = Color.Transparent;
                    textBox.SelectionColor = Color.FromArgb(60, 0, 0);
                    textBox.ServiceLinesColor = Color.Silver;
                    break;
                case Theme.Dark:
                    textBox.CaretColor = Color.White;
                    textBox.FoldingIndicatorColor = Color.Gold;
                    textBox.IndentBackColor = Color.FromArgb(32, 32, 32);
                    textBox.LineNumberColor = Color.Teal;
                    textBox.PaddingBackColor = Color.FromArgb(32, 32, 32);
                    textBox.SelectionColor = Color.FromArgb(150, Color.White);
                    textBox.ServiceLinesColor = Color.DimGray;
                    break;
            }
        }

        public void SetThemeDefaults() {
            this.SyntaxColours.SetThemeDefaults(this.Theme);
        }

        public class SyntaxColourTable : ICloneable {
            public TextStyle TextStyle;
            public TextStyle CommentStyle;
            public TextStyle KeywordStyle;
            public TextStyle CommandStyle;
            public TextStyle CustomCommandStyle;
            public TextStyle FunctionStyle;
            public TextStyle CustomFunctionStyle;
            public TextStyle FunctionPropertyStyle;
            public TextStyle VariableStyle;
            public TextStyle AliasStyle;
            public UnderlineStyle ErrorStyle;
            public UnderlineStyle WarningStyle;
            public UnderlineStyle NoticeStyle;

            public static SyntaxColourTable Default => new SyntaxColourTable() {
                TextStyle = new TextStyle(Color.White, Color.Black),
                CommentStyle = new TextStyle(Color.Green, FontStyle.Italic),
                KeywordStyle = new TextStyle(Color.RoyalBlue),
                CommandStyle = new TextStyle(Color.Magenta),
                CustomCommandStyle = new TextStyle(Color.Purple),
                FunctionStyle = new TextStyle(Color.Olive),
                CustomFunctionStyle = new TextStyle(Color.Yellow),
                FunctionPropertyStyle = new TextStyle(Color.GreenYellow),
                VariableStyle = new TextStyle(Color.Maroon),
                AliasStyle = new TextStyle(Color.Empty, FontStyle.Bold),
                ErrorStyle = new UnderlineStyle(Color.Red),
                WarningStyle = new UnderlineStyle(Color.Green),
                NoticeStyle = new UnderlineStyle(Color.Blue)
            };

            public SyntaxColourTable Clone() {
                return new SyntaxColourTable() {
                    TextStyle = this.TextStyle.Clone(),
                    CommentStyle = this.CommentStyle.Clone(),
                    KeywordStyle = this.KeywordStyle.Clone(),
                    CommandStyle = this.CommandStyle.Clone(),
                    CustomCommandStyle = this.CustomCommandStyle.Clone(),
                    FunctionStyle = this.FunctionStyle.Clone(),
                    CustomFunctionStyle = this.CustomFunctionStyle.Clone(),
                    FunctionPropertyStyle = this.FunctionPropertyStyle.Clone(),
                    VariableStyle = this.VariableStyle.Clone(),
                    AliasStyle = this.AliasStyle.Clone(),
                    ErrorStyle = this.ErrorStyle.Clone(),
                    WarningStyle = this.WarningStyle.Clone(),
                    NoticeStyle = this.NoticeStyle.Clone()
                };
            }
            object ICloneable.Clone() => this.Clone();

            public void SetThemeDefaults(Theme theme) {
                switch (theme) {
                    case Theme.Default:
                        this.TextStyle.SetDefaults(SystemColors.WindowText, SystemColors.Window);
                        this.CommentStyle.SetDefaults(Color.Green, Color.Empty);
                        this.KeywordStyle.SetDefaults(Color.RoyalBlue, Color.Empty);
                        this.CommandStyle.SetDefaults(Color.Purple, Color.Empty);
                        this.CustomCommandStyle.SetDefaults(Color.MediumPurple, Color.Empty);
                        this.FunctionStyle.SetDefaults(Color.Olive, Color.Empty);
                        this.CustomFunctionStyle.SetDefaults(Color.Olive, Color.Empty);
                        this.FunctionPropertyStyle.SetDefaults(Color.Olive, Color.Empty);
                        this.VariableStyle.SetDefaults(Color.Maroon, Color.Empty);
                        this.AliasStyle.SetDefaults(Color.Empty, Color.Empty);
                        break;
                    case Theme.Dark:
                        this.TextStyle.SetDefaults(Color.White, Color.FromArgb(32, 32, 32));
                        this.CommentStyle.SetDefaults(Color.LimeGreen, Color.Empty);
                        this.KeywordStyle.SetDefaults(Color.RoyalBlue, Color.Empty);
                        this.CommandStyle.SetDefaults(Color.MediumPurple, Color.Empty);
                        this.CustomCommandStyle.SetDefaults(Color.Magenta, Color.Empty);
                        this.FunctionStyle.SetDefaults(Color.Olive, Color.Empty);
                        this.CustomFunctionStyle.SetDefaults(Color.Yellow, Color.Empty);
                        this.FunctionPropertyStyle.SetDefaults(Color.Olive, Color.Empty);
                        this.VariableStyle.SetDefaults(Color.Red, Color.Empty);
                        this.AliasStyle.SetDefaults(Color.Empty, Color.Empty);
                        break;
                    default:
                        throw new ArgumentException("theme was not an existing theme.", "theme");
                }
            }
        }

        public class TextStyle : ICloneable {
            [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
            public Color? TextColour;
            [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
            public Color? BackgroundColour;

            [JsonIgnore]
            public Color DefaultTextColour { get; set; }
            [JsonIgnore]
            public Color DefaultBackgroundColour { get; set; }

            [JsonIgnore]
            public Color TrueTextColour => TextColour ?? DefaultTextColour;
            [JsonIgnore]
            public Color TrueBackgroundColour => BackgroundColour ?? DefaultBackgroundColour;

            [JsonConverter(typeof(StringEnumConverter))] [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
            public FontStyle Style;

            public TextStyle() : this(Color.Empty, Color.Empty) { }
            public TextStyle(Color textColour) : this(textColour, Color.Empty) { }
            public TextStyle(Color textColour, FontStyle style) : this(textColour, Color.Empty, style) { }
            public TextStyle(Color textColour, Color backgroundColour) : this(textColour, backgroundColour, 0) { }
            public TextStyle(Color textColour, Color backgroundColour, FontStyle style) {
                this.DefaultTextColour = textColour;
                this.DefaultBackgroundColour = backgroundColour;
                this.Style = style;
            }

            public void SetDefaults(Color textColour, Color backgroundColour) {
                this.DefaultTextColour = textColour;
                this.DefaultBackgroundColour = backgroundColour;
            }

            public TextStyle Clone() {
                return new TextStyle(this.DefaultTextColour, this.DefaultBackgroundColour) { TextColour = this.TextColour, BackgroundColour = this.BackgroundColour, Style = this.Style };
            }
            object ICloneable.Clone() => this.Clone();
        }

        public class UnderlineStyle : ICloneable {
            [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
            public Color? UnderlineColour;
            [JsonIgnore]
            public Color DefaultUnderlineColour { get; set; }

            [JsonIgnore]
            public Color TrueUnderlineColour => UnderlineColour ?? DefaultUnderlineColour;

            public UnderlineStyle() : this(Color.Empty) { }
            public UnderlineStyle(Color underlineColour) {
                this.DefaultUnderlineColour = underlineColour;
            }

            public UnderlineStyle Clone() {
                return new UnderlineStyle(this.DefaultUnderlineColour) { UnderlineColour = this.UnderlineColour };
            }
            object ICloneable.Clone() => this.Clone();
        }
    }

    public enum Theme {
        Default,
        Dark
    }
}
