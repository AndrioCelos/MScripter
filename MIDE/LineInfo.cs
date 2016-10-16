using System.Collections.Generic;
using FastColoredTextBoxNS;

namespace MIDE {
    public class LineInfo {
        public byte State;
        public sbyte BraceLevel;
        public MslBookmark bookmark;
        public Error[] errors;
        public List<Tag> tags;

        public bool Matches(LineInfo other) => other != null && this.State == other.State && this.BraceLevel == other.BraceLevel;
    }

    public class Tag {
        public Range range;
        public TagType type;
    }

    public enum TagType {
        CommandCall,
        FunctionCall,
        AliasName,
        EventName,
        EventParameter,
        Variable
    }
}
