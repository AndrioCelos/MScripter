using System.Linq;
using System.Threading;
using System.Windows.Forms;

using FastColoredTextBoxNS;

namespace MScripter {
    /// <summary>
    /// Console emulator.
    /// </summary>
    public class ConsoleTextBox : FastColoredTextBox {
        private volatile bool isReadLineMode;
        private volatile bool isUpdating;
        private Place StartReadPlace { get; set; }

        [System.ComponentModel.Browsable(false)]
        public string ReadText {
            get {
                return new Range(this, StartReadPlace, this.Range.End).Text.TrimEnd('\r', '\n');
            }
        }

        /// <summary>
        /// Control is waiting for line entering. 
        /// </summary>
        public bool IsReadLineMode {
            get { return isReadLineMode; }
            set { isReadLineMode = value; }
        }

        /// <summary>
        /// Append line to end of text.
        /// </summary>
        /// <param name="text"></param>
        public void Write(string text) {
            IsReadLineMode = false;
            isUpdating = true;
            try {
                AppendText(text);
                GoEnd();
            } finally {
                isUpdating = false;
                ClearUndo();
            }
        }
        public void Write(string text, Style style) {
            IsReadLineMode = false;
            isUpdating = true;
            try {
                AppendText(text, style);
                GoEnd();
            } finally {
                isUpdating = false;
                ClearUndo();
            }
        }

        /// <summary>
        /// Wait for line entering.
        /// Set IsReadLineMode to false for break of waiting.
        /// </summary>
        /// <returns></returns>
        public string ReadLine() {
            GoEnd();
            StartReadPlace = Range.End;
            IsReadLineMode = true;
            try {
                while (IsReadLineMode) {
                    Application.DoEvents();
                    Thread.Sleep(5);
                }
            } finally {
                IsReadLineMode = false;
                ClearUndo();
            }

            return new Range(this, StartReadPlace, Range.End).Text.TrimEnd('\r', '\n');
        }

        public void AcceptLine() {
            GoEnd();
            StartReadPlace = Range.End;
            IsReadLineMode = true;
        }

        private int lineEnd;
        public override void OnTextChanging(ref string text) {
            if (lineEnd != 0) return;

            if (!IsReadLineMode && !isUpdating) {
                text = ""; //cancel changing
                return;
            }

            if (IsReadLineMode) {
                if (Selection.Start < StartReadPlace) {
                    if (Selection.End < StartReadPlace) {
                        Selection = new Range(this, StartReadPlace, StartReadPlace);
                    }
                    Selection = new Range(this, StartReadPlace, Selection.End);
                }

                if (Selection.Start == StartReadPlace && Selection.End == StartReadPlace)
                    if (text == "\b") {  //backspace
                        text = ""; //cancel deleting of last char of readonly text
                        return;
                    }

                if (text != null && text.Contains('\n')) {
                    if (text == "\n") {
                        GoEnd();
                    } else {
                        text = text.Substring(0, text.IndexOf('\n')).TrimEnd('\r');
                        base.OnTextChanging(ref text);
                        lineEnd = 2;
                    }
                    IsReadLineMode = false;
                } else {
                    base.OnTextChanging(ref text);
                }
            }
             
        }

        protected override void OnTextChanged(TextChangedEventArgs args) {
            if (lineEnd == 2) {
                lineEnd = 1;
                this.AppendText("\n");
                lineEnd = 0;
                GoEnd();
            }
            base.OnTextChanged(args);
        }

        public override void Clear() {
            var oldIsReadMode = isReadLineMode;

            isReadLineMode = false;
            isUpdating = true;

            base.Clear();

            isUpdating = false;
            isReadLineMode = oldIsReadMode;

            StartReadPlace = Place.Empty;
        }
    }
}
