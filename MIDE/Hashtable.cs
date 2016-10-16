using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MIDE {
    public class Hashtable {
        public string Name { get; set; }
        public string Path { get; set; }
        public HashtableFormat Format { get; set; }
        public string Section { get; set; }
        public bool ReadOnly { get; set; }
        public IntPtr SyncTarget { get; set; }

        public Dictionary<string, byte[]> content;

        public Hashtable() : this(100) { }
        public Hashtable(int capacity) {
            this.content = new Dictionary<string, byte[]>(capacity);
        }
        public Hashtable(string name, Dictionary<string, byte[]> content, string path, HashtableFormat format, bool readOnly, IntPtr syncTarget) {
            this.Name = name;
            this.content = content;
            this.Path = path;
            this.Format = format;
            this.ReadOnly = readOnly;
            this.SyncTarget = syncTarget;
        }

        public static Hashtable FromFile(string hashtableName, string path, HashtableFormat format, string section, bool readOnly = false, IntPtr syncTarget = default(IntPtr)) {
            if (format == HashtableFormat.Binary)
                return Hashtable.FromBinaryFile(hashtableName, path, format, readOnly, syncTarget);
            else
                return Hashtable.FromTextFile(hashtableName, path, format, section, readOnly, syncTarget);
        }

        private static Hashtable FromTextFile(string hashtableName, string path, HashtableFormat format, string section, bool readOnly = false, IntPtr syncTarget = default(IntPtr)) {
            if (format == HashtableFormat.INI && section == null)
                throw new ArgumentException("A section must be specified for INI files.");

            Dictionary<string, byte[]> content = null;
            if (format == HashtableFormat.Text) content = new Dictionary<string, byte[]>();

            // Open the file.
            StreamReader reader = new StreamReader(File.Open(path, FileMode.Open, FileAccess.Read));
            string line; string key = null;

            while (!reader.EndOfStream) {
                line = reader.ReadLine();
                if (format == HashtableFormat.Text) {
                    if (key == null) {
                        if (!string.IsNullOrWhiteSpace(line)) key = line;
                    } else {
                        content.Add(key, Encoding.UTF8.GetBytes(line));
                        key = null;
                    }
                } else {
                    if (line.StartsWith("[") && line.EndsWith("]")) {
                        if (content != null) break;

                        key = line.Substring(1, line.Length - 2);
                        if (key.Equals(section, StringComparison.InvariantCultureIgnoreCase))
                            content = new Dictionary<string, byte[]>();
                    } else {
                        string[] fields = line.Split(new char[] { '=' }, 2);
                        if (fields.Length == 2)
                            content.Add(fields[0], Encoding.UTF8.GetBytes(fields[1]));
                    }
                }
            }

            // Finish up.
            reader.Close();
            if (content == null) throw new KeyNotFoundException("No section '" + section + "' is in the file.");
            return new Hashtable(hashtableName, content, path, format, readOnly, syncTarget);
        }
         private static Hashtable FromBinaryFile(string hashtableName, string path, HashtableFormat format, bool readOnly = false, IntPtr syncTarget = default(IntPtr)) {
             Dictionary<string, byte[]> content = new Dictionary<string, byte[]>();

            // Open the file.
            using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read)) {
                BinaryReader reader = new BinaryReader(stream);
                ushort count; string key; byte[] value;

                try {
                    while (true) {
                        count = reader.ReadUInt16();
                        key = Encoding.UTF8.GetString(reader.ReadBytes(count));

                        count = reader.ReadUInt16();
                        value = reader.ReadBytes(count);

                        content.Add(key, value);
                    }
                } catch (EndOfStreamException) { }
                // The stream will automatically be closed when the using block ends.
            }

            return new Hashtable(hashtableName, content, path, format, readOnly, syncTarget);
        }
        
        public void Save() {
            if (this.Format == HashtableFormat.Binary)
                this.SaveToBinaryFile();
            else
                this.SaveToTextFile();
        }
        public void Save(string path) {
            this.Path = path;
            this.Save();
        }
        public void Save(string path, HashtableFormat format) {
            this.Path = path;
            this.Format = format;
            this.Save();
        }

        private void SaveToTextFile() {
            using (FileStream stream = File.Open(this.Path, FileMode.Create, FileAccess.Write)) {
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

                if (this.Format == HashtableFormat.INI) {
                    writer.WriteLine("[" + this.Section + "]");
                    foreach (var entry in this.content)
                        writer.WriteLine(entry.Key + "=" + entry.Value);
                } else {
                    foreach (var entry in this.content) {
                        writer.WriteLine(entry.Key);
                        string value;
                        
                            value = Encoding.UTF8.GetString(entry.Value);
                                                writer.WriteLine(value.Replace("\r", "").Replace("\n", ""));
                    }
                }
                writer.Close();
            }
        }
        private void SaveToBinaryFile() {
            using (FileStream stream = File.Open(this.Path, FileMode.Create, FileAccess.Write)) {
                BinaryWriter writer = new BinaryWriter(stream);
                foreach (var entry in this.content) {
                    byte[] data;

                    data = Encoding.UTF8.GetBytes(entry.Key);
                    writer.Write((ushort) data.Length);
                    writer.Write(data);

                    data = entry.Value;
                    writer.Write((ushort) data.Length);
                    writer.Write(data);
                }
                writer.Close();
            }
        }
    }

    public enum HashtableFormat {
        Text,
        Binary,
        INI
    }
}
