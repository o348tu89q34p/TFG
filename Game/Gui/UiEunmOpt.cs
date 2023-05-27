namespace Gui {
    public enum ColorType {
        FILL,
        OUTLINE,
        HOVER,
        PRESSED
    }

    public enum Origin {
        TOPLEFT,
        TOPRIGHT,
        BOTTOMLEFT,
        BOTTOMRIGHT,
        CENTER,
        TOPCENTER,
        BOTTOMCENTER,
        LEFTCENTER,
        RIGHTCENTER
    }

    public enum ButtonState {
        BTN_IDLE = 0,
        BTN_HOVER = 2,
        BTN_ACTIVE = 3,
        BTN_DISABLED = 4
    }

    public enum DivType {
        VERTICAL,
        HORIZONTAL
    }

    public enum DivAlign {
        HERE,
        CENTER,
        THERE
    }

    public enum DivPos {
        AFTER,
        FIXED
    }
}
