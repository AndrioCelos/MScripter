using System.Drawing;
using System.Linq;
using FastColoredTextBoxNS;

namespace MScripter {
    public class ControlCharStyle : TextStyle {
        public static ControlCharStyle Instance { get; }

        // This class is a singleton.
        static ControlCharStyle() {
            Instance = new ControlCharStyle();
        }
        private ControlCharStyle() : base(null, null, FontStyle.Regular) { }
        

        public override void Draw(Graphics gr, Point position, Range range) {
            float x = position.X;
            foreach (var c in range.Chars) {
                char displayChar = c.c;
                switch (displayChar) {
                    case '\u0001': displayChar = 'A'; break;
                    case '\u0002': displayChar = 'B'; break;
                    case '\u0003': displayChar = 'K'; break;
                    case '\u000F': displayChar = 'O'; break;
                    case '\u0013': displayChar = 'S'; break;
                    case '\u0016': displayChar = 'R'; break;
                    case '\u001C': displayChar = 'I'; break;
                    case '\u001F': displayChar = 'U'; break;
                }

                float y = position.Y;
                gr.DrawString(displayChar.ToString(), range.tb.Font, new SolidBrush(range.tb.ForeColor), x - range.tb.CharWidth / 3, y + range.tb.LineInterval / 2);
                gr.DrawRectangle(new Pen(range.tb.ForeColor), x, y, range.tb.CharWidth, range.tb.CharHeight);
                x += range.tb.CharWidth;
            }
        }
    }
}
