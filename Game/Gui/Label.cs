using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Gui {
    public class Label : GuiComponent {
        private Text _text;

        public Label(string text, Vector2f pos, Origin origin,
                     uint charSize, Font font, Color fillColor,
                     Color hoverColor, Color pressedColor)
        {
            this._text = new Text(text, font) {
                CharacterSize = charSize,
                FillColor = fillColor,
                OutlineColor = fillColor,
                OutlineThickness = 0.0f,
                Position = pos
            };

            this.SetOrigin(origin);
            this.FillColor = fillColor;
            this.OutlineColor = fillColor;
            this.HoverColor = hoverColor;
            this.PressedColor = pressedColor;
            this.CallUpdate = true;
            this.MouseOn = false;
        }

        public void SetColor(Color newColor, ColorType name) {
            switch (name) {
                case ColorType.FILL:
                    this.FillColor = newColor;
                    this._text.FillColor = this.FillColor;
                    break;
                case ColorType.HOVER:
                    this.HoverColor = newColor;
                    this.CallUpdate = true;
                    break;
                case ColorType.OUTLINE:
                    this.OutlineColor = newColor;
                    this._text.OutlineColor = newColor;
                    break;
                default:
                    break;
            }
        }

        private void SetOrigin(Origin name) {
            float x;
            float y;
            switch (name) {
                case Origin.TOPLEFT:
                    x = 0.0f;
                    y = 0.0f;
                    break;
                case Origin.TOPRIGHT:
                    x = this._text.GetGlobalBounds().Width;
                    y = 0.0f;
                    break;
                case Origin.BOTTOMLEFT:
                    x = 0.0f;
                    y = this._text.GetGlobalBounds().Height;
                    break;
                case Origin.BOTTOMRIGHT:
                    x = this._text.GetGlobalBounds().Width;
                    y = this._text.GetGlobalBounds().Height;
                    break;
                case Origin.CENTER:
                    x = this._text.GetGlobalBounds().Width/2;
                    y = this._text.GetGlobalBounds().Height/2;
                    break;
                default:
                    x = 0.0f;
                    y = 0.0f;
                    break;
            }
            this._text.Origin = new Vector2f(x, y);
        }

        public float Width() {
            return this._text.GetGlobalBounds().Width;
        }

        public override FloatRect GetBounds() {
            return this._text.GetGlobalBounds();
        }

        public void SetPosition(Vector2f position) {
            this._text.Position = position;
        }

        public override void Update(RenderWindow window) {
            /*
            if (!this.CallUpdate) {
                return;
            }
            */
            Vector2f mouse = window.MapPixelToCoords(Mouse.GetPosition(window));
            FloatRect bounds = this._text.GetGlobalBounds();
            this.MouseOn = bounds.Contains(mouse.X, mouse.Y);

            if (this.MouseOn) {
                this._text.FillColor = this.HoverColor;
            } else {
                this._text.FillColor = this.FillColor;
            }
        }

        public override void Render(RenderTarget window) {
            window.Draw(this._text);
        }
    }

}
