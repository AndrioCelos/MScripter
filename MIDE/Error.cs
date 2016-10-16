﻿
using FastColoredTextBoxNS;

namespace MIDE {
    public class Error {
        public ErrorType Type;
        public Range Location;
        public string Text;
    }

    public enum ErrorType {
        Error,
        Warning,
        Note
    }
}
