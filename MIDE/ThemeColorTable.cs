using System.Drawing;
using System.Windows.Forms;

namespace MIDE {
    class DarkThemeColorTable : ProfessionalColorTable {
        public DarkThemeColorTable() {
            this.UseSystemColors = false;
        }

        public override Color ButtonCheckedGradientBegin          => Color.FromArgb(unchecked((int) 0xFF909090));
        public override Color ButtonCheckedGradientEnd            => Color.FromArgb(unchecked((int) 0xFF707070));
        public override Color ButtonCheckedGradientMiddle         => Color.FromArgb(unchecked((int) 0xFF808080));
        public override Color ButtonCheckedHighlight              => Color.FromArgb(unchecked((int) 0xFF808080));
        public override Color ButtonCheckedHighlightBorder        => Color.FromArgb(unchecked((int) 0xFFC0C0C0));
        public override Color ButtonPressedBorder                 => Color.FromArgb(unchecked((int) 0xFF202020));
        public override Color ButtonPressedGradientBegin          => Color.FromArgb(unchecked((int) 0xFF101010));
        public override Color ButtonPressedGradientEnd            => Color.FromArgb(unchecked((int) 0xFF000000));
        public override Color ButtonPressedGradientMiddle         => Color.FromArgb(unchecked((int) 0xFF101010));
        public override Color ButtonPressedHighlight              => Color.FromArgb(unchecked((int) 0xFF000000));
        public override Color ButtonPressedHighlightBorder        => Color.FromArgb(unchecked((int) 0xFF000000));
        public override Color ButtonSelectedBorder                => Color.FromArgb(unchecked((int) 0xFF808080));
        public override Color ButtonSelectedGradientBegin         => Color.FromArgb(unchecked((int) 0xFF707070));
        public override Color ButtonSelectedGradientEnd           => Color.FromArgb(unchecked((int) 0xFF505050));
        public override Color ButtonSelectedGradientMiddle        => Color.FromArgb(unchecked((int) 0xFF606060));
        public override Color ButtonSelectedHighlight             => Color.FromArgb(unchecked((int) 0xFF000000));
        public override Color ButtonSelectedHighlightBorder       => Color.FromArgb(unchecked((int) 0xFF000000));
        public override Color CheckBackground                     => Color.FromArgb(unchecked((int) 0xFF606060));
        public override Color CheckPressedBackground              => Color.FromArgb(unchecked((int) 0xFF404040));
        public override Color CheckSelectedBackground             => Color.FromArgb(unchecked((int) 0xFF808080));
        public override Color GripDark                            => Color.FromArgb(unchecked((int) 0xFF000000));
        public override Color GripLight                           => Color.FromArgb(unchecked((int) 0xFF808080));
        public override Color ImageMarginGradientBegin            => Color.FromArgb(unchecked((int) 0xFF606060));
        public override Color ImageMarginGradientEnd              => Color.FromArgb(unchecked((int) 0xFF202020));
        public override Color ImageMarginGradientMiddle           => Color.FromArgb(unchecked((int) 0xFF404040));
        public override Color ImageMarginRevealedGradientBegin    => Color.FromArgb(unchecked((int) 0xFF000000));
        public override Color ImageMarginRevealedGradientEnd      => Color.FromArgb(unchecked((int) 0xFF000000));
        public override Color ImageMarginRevealedGradientMiddle   => Color.FromArgb(unchecked((int) 0xFF000000));
        public override Color MenuBorder                          => Color.FromArgb(unchecked((int) 0xFF808080));
        public override Color MenuItemBorder                      => Color.FromArgb(unchecked((int) 0xFF606060));
        public override Color MenuItemPressedGradientBegin        => Color.FromArgb(unchecked((int) 0xFF202020));
        public override Color MenuItemPressedGradientEnd          => Color.FromArgb(unchecked((int) 0xFF000000));
        public override Color MenuItemPressedGradientMiddle       => Color.FromArgb(unchecked((int) 0xFF101010));
        public override Color MenuItemSelected                    => Color.FromArgb(unchecked((int) 0xFF606060));
        public override Color MenuItemSelectedGradientBegin       => Color.FromArgb(unchecked((int) 0xFF707070));
        public override Color MenuItemSelectedGradientEnd         => Color.FromArgb(unchecked((int) 0xFF505050));
        public override Color MenuStripGradientBegin              => Color.FromArgb(unchecked((int) 0xFF404040));
        public override Color MenuStripGradientEnd                => Color.FromArgb(unchecked((int) 0xFF404040));
        public override Color OverflowButtonGradientBegin         => Color.FromArgb(unchecked((int) 0xFF303030));
        public override Color OverflowButtonGradientEnd           => Color.FromArgb(unchecked((int) 0xFF101010));
        public override Color OverflowButtonGradientMiddle        => Color.FromArgb(unchecked((int) 0xFF202020));
        public override Color RaftingContainerGradientBegin       => Color.FromArgb(unchecked((int) 0xFF505050));
        public override Color RaftingContainerGradientEnd         => Color.FromArgb(unchecked((int) 0xFF303030));
        public override Color SeparatorDark                       => Color.FromArgb(unchecked((int) 0xFF000000));
        public override Color SeparatorLight                      => Color.FromArgb(unchecked((int) 0xFF808080));
        public override Color StatusStripGradientBegin            => Color.FromArgb(unchecked((int) 0xFF707070));
        public override Color StatusStripGradientEnd              => Color.FromArgb(unchecked((int) 0xFF505050));
        public override Color ToolStripBorder                     => Color.FromArgb(unchecked((int) 0xFF808080));
        public override Color ToolStripContentPanelGradientBegin  => Color.FromArgb(unchecked((int) 0xFF505050));
        public override Color ToolStripContentPanelGradientEnd    => Color.FromArgb(unchecked((int) 0xFF303030));
        public override Color ToolStripDropDownBackground         => Color.FromArgb(unchecked((int) 0xFF404040));
        public override Color ToolStripGradientBegin              => Color.FromArgb(unchecked((int) 0xFF505050));
        public override Color ToolStripGradientEnd                => Color.FromArgb(unchecked((int) 0xFF303030));
        public override Color ToolStripGradientMiddle             => Color.FromArgb(unchecked((int) 0xFF404040));
        public override Color ToolStripPanelGradientBegin         => Color.FromArgb(unchecked((int) 0xFF505050));
        public override Color ToolStripPanelGradientEnd           => Color.FromArgb(unchecked((int) 0xFF303030));
    }
}
