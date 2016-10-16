namespace MIDE {
    public class MslBookmark {
        public BookmarkType type;
        public string title;
        public int position;
        public int endLine;
        internal bool hidden;

        public MslBookmark(BookmarkType type, int position) : this(type, "?", position) { }
        public MslBookmark(BookmarkType type, string title, int position) {
            this.type = type;
            this.title = title;
            this.position = position;
            this.endLine = -1;
        }

        public override string ToString() {
            return this.title;
        }
    }

    public enum BookmarkType {
        Event = 1,
        Alias,
        CtcpEvent,
        RawEvent,
        Dialog,
        Menu,
        IniSection
    }
}
