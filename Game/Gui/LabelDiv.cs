using SFML.Graphics;
using SFML.System;

namespace Gui {
    class Div : GuiComponent {
        protected GuiComponent[] _items;
        protected DivType _orient;
        protected DivAlign _align;
        protected DivPos _pos;
        protected float _space;

        public Div(GuiComponent[] items, DivAlign align, DivPos space, float s,
                   DivPos p, DivType orient, Vector2f pos, Vector2f size)
        {
            this._orient = orient;
            this._align = align;
            this._pos = p;
            this._space = s;
            this.Size = size;
            this.Position = pos;
            // this._items = this.Align(items);
            this._items = items;
            this.FillColor = Color.Green;
            this.HoverColor = Color.Transparent;
            this.OutlineColor = Color.Transparent;
            this.PressedColor = Color.Transparent;
            this.CallUpdate = false;
            this.Id = -1;
        }

        /*
        public GuiComponent[] Align(GuiComponent[] gc) {
            float x = 0.0f;
            float y = 0.0f;
            switch ((this._orient, this._align)) {
                case (DivType.VERTICAL, DivAlign.HERE):
                    x = 0.0f;
                    break;
                case (DivType.VERTICAL, DivAlign.CENTER):
                    x = this.Size.X/2;
                    break;
                case (DivType.VERTICAL, DivAlign.THERE):
                    x = this.Size.X;
                    break;
                case (DivType.HORIZONTAL, DivAlign.HERE):
                    y = 0.0f;
                    break;
                case (DivType.HORIZONTAL, DivAlign.CENTER):
                    y = this.Size.Y/2;
                    break;
                case (DivType.HORIZONTAL, DivAlign.THERE):
                    y = this.Size.Y;
                    break;
                default:
                    break;
            }

            float start = 0.0f;
            switch (this._orient) {
                case DivType.VERTICAL:
                    start = this.Position.Y;
                    break;
                case DivType.HORIZONTAL:
                    start = this.Position.X;
                    break;
            }

            for (int i = 0; i < gc.Length; i++) {
                switch ((this._orient, this._pos)) {
                    case (DivType.VERTICAL, DivPos.AFTER):
                        start += gc[i].Size.Y + this._space;
                        break;
                    case (DivType.VERTICAL, DivPos.FIXED):
                        start += this._space;
                        break;
                    case (DivType.HORIZONTAL, DivPos.AFTER):
                        start += gc[i].Size.X + this._space;
                        break;
                    case (DivType.HORIZONTAL, DivPos.FIXED):
                        start += this._space;
                        break;
                }
                switch (this._orient) {
                    case DivType.VERTICAL:
                        gc[i].Position.X = this.Position.X;
                        gc[i].Position.Y = start;
                        break;
                    case DivType.HORIZONTAL:
                        gc[i].Position.X = start;
                        gc[i].Position.Y = this.Position.Y;
                        break;
                }
            }

            return gc;
        }
        */

        public override FloatRect GetBounds() {
            // random value
            return new FloatRect(new Vector2f(0.3f, 0.3f), new Vector2f(3.2f, 4.2f));
        }

        public override void Update(RenderWindow window) {
            foreach (GuiComponent gc in this._items) {
                gc.Update(window);
            }
        }

        public override void Render(RenderTarget window) {
            
        }
    }
    class LabelDiv : GuiComponent {
        public uint X {get; set;}
        public uint Y {get; set;}
        public uint Width {get; private set;}
        public uint Height {get; private set;}
        private Label[] _labels;

        /*
         * Improvements:
         * - Make it so it takes any GuiComponent.
         * - Create two types of div: vertical and horizontal.
         * - Vertical divs can have three positions: left, center and right.
         * - Horizontal divs can have three positions: top, center and bottom.
         * - Items can spaced from each other's bottom by a constant or
         *   be placed at constant steps.
         * - The same point for horizontal.
         */
        public LabelDiv(
            string[] names, Font font, uint x, uint y,
            uint w, uint h, Color fColor)
        {
            this.X = x;
            this.Y = y;
            this.Width = w;
            this.Height = h;
            this._labels = MakeLabels(names, font, fColor);
        }

        private Label[] MakeLabels(string[] names, Font font, Color fColor) {
            uint len = (uint)names.Length;
            uint height = this.Height/len;
            Label[] result = new Label[len];

            for (int i = 0; i < len; i++) {
                var pos = new Vector2f(this.X, this.Y + height*i);
                result[i] = new Label(names[i], pos, Origin.CENTER, height, font, fColor, fColor, fColor);
            }

            return result;
        }

        public override FloatRect GetBounds() {
            // random value
            return new FloatRect(new Vector2f(0.3f, 0.3f), new Vector2f(3.2f, 4.2f));
        }

        public override void Update(RenderWindow window) {
            foreach (Label l in this._labels) {
                l.Update(window);
            }
        }

        public override void Render(RenderTarget window) {
            foreach (Label l in this._labels) {
                l.Render(window);
            }
        }
    }
}
