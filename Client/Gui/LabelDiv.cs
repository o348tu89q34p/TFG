using SFML.Graphics;
using SFML.System;

namespace Gui {
    class Div : GuiComponent {
        private GuiComponent[] _items;

        public Div(GuiComponent[] items, Vector2f pos, Vector2f size) {
            this._items = items;
            this.Size = new Vector2f(0.0f, 0.0f);
            this.Position = new Vector2f(0.0f, 0.0f);
            this.FillColor = Color.Transparent;
            this.HoverColor = Color.Transparent;
            this.OutlineColor = Color.Transparent;
            this.PressedColor = Color.Transparent;
            this.CallUpdate = false;
            this.Id = -1;
        }

        public override void Update(Vector2f mousePos) {
            
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
                result[i] = new Label(names[i], pos, height, font, fColor);
            }

            return result;
        }

        public override void Update(Vector2f mousePos) {
            foreach (Label l in this._labels) {
                l.Update(mousePos);
            }
        }

        public override void Render(RenderTarget window) {
            foreach (Label l in this._labels) {
                l.Render(window);
            }
        }
    }
}
