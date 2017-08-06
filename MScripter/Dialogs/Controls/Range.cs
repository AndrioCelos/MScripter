using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace MScripter {
    [TypeConverter(typeof(RangeConverter))]
    public struct ScrollRange {
        private int minimum;
        private int maximum;

        public int Minimum {
            get { return this.minimum; }
            set {
                if (value < 0) throw new ArgumentOutOfRangeException("value", "Value cannot be negative.");
                this.minimum = value;
                if (value > this.maximum) this.maximum = value;
            }
        }
        public int Maximum {
            get { return this.maximum; }
            set {
                if (value < 0) throw new ArgumentOutOfRangeException("value", "Value cannot be negative.");
                this.maximum = value;
                if (value < this.minimum) this.minimum = value;
            }
        }

        public static ScrollRange Empty => default(ScrollRange);
        [Browsable(false)]
        public bool IsEmpty => this.Maximum == 0 && this.Minimum == 0;

        public ScrollRange(int mininum, int maximum) {
            if (mininum < 0) throw new ArgumentOutOfRangeException("mininum", "Value cannot be negative.");
            if (maximum < 0) throw new ArgumentOutOfRangeException("maximum", "Value cannot be negative.");
            this.minimum = mininum;
            this.maximum = maximum;
        }

        public override string ToString() {
            return this.Minimum.ToString() + " " + this.Maximum.ToString();
        }
    }

    public class RangeConverter : TypeConverter {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            string text = value as string;
            if (text == null)
                return base.ConvertFrom(context, culture, value);
            
            text = text.Trim();
            if (text.Length == 0) return null;

            var fields = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (fields.Length != 2) throw new FormatException();

            var minimum = int.Parse(fields[0]);
            var maximum = int.Parse(fields[1]);
            if (maximum < minimum) throw new ArgumentException("maximum cannot be less than minimum.");

            return new ScrollRange(minimum, maximum);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context) => true;

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            => TypeDescriptor.GetProperties(typeof(ScrollRange), attributes)
                .Sort(new string[] { "Minimum", "Maximum" });
    }
}
