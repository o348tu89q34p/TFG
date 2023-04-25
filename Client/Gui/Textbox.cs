using SFML.Graphics;

/*
 * https://www.youtube.com/watch?v=T31MoLJws4U
 * 5:50
 * Implementing the input logic private method.
 */

namespace Gui.Textbox {
    public class Textbox {
        private Text? _textbox;
        // private BufferedStream? _text;
        private bool _isSelected;
        // private bool _hasLimit;
        // private int? _limit;
        private uint _fontSize;
        private Color _color;

        public Textbox(uint fontSize, Color textColor, bool selected) {
            this._textbox = null;
            // this._text = null;
            this._isSelected = selected;
            // this._hasLimit = false;
            // this._limit = null;
            this._fontSize = fontSize;
            this._color = textColor;
        }

        // Method used to remove warnings.
        private void Filler(Font font) {
            this._textbox = new Text("filler", font);
            // this._hasLimit = "filler".Length%2 == 0;
        }
    }
}
