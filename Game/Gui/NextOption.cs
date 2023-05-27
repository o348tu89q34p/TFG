using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Game;

namespace Gui {
    public class NextOption<T1, T2> : GuiComponent
        where T1 : Nextable<T2>
    {
        private Label? Name { get; set; }
        private Label? Left { get; set; }
        private Label? TheValue { get; set; }
        private Label? Right { get; set; }
        private bool OnLeft { get; set; }
        private bool OnRight { get; set; }
        private string ValueName { get; }
        private Vector2f Pos { get; }
        public T1 Setting { get; set; }

        public NextOption(string name, T1 setting, Vector2f pos) {
            this.MouseOn = false;
            this.Setting = setting;
            this.ValueName = name + ":";
            this.Pos = pos;

            this.BuildOption();
        }

        private void BuildOption() {
            Font font = new Font(FontUtils.TextFont1);
            if (font == null) {
                throw new Exception($"Failed to load font: {font}");
            }

            Color normalColor = new Color(0, 200, 50);
            Color hoverColor = new Color(200, 0, 12);
            Vector2f newPos = this.Pos;
            float hSpace = 10;
            uint fontSize = 25;
            this.Name = new Label(this.ValueName, newPos, Origin.TOPLEFT, fontSize, font, normalColor, normalColor, normalColor);

            newPos = new Vector2f(newPos.X + hSpace + this.Name.Width(), newPos.Y);
            this.Left = new Label("<", newPos, Origin.TOPLEFT, fontSize, font, normalColor, hoverColor, normalColor);

            Color valueColor = new Color(0, 100, 200);
            newPos = new Vector2f(newPos.X + hSpace + this.Left.Width(), newPos.Y);
            this.TheValue = new Label(this.Setting.ToString(), newPos, Origin.TOPLEFT, fontSize, font, valueColor, valueColor, valueColor);

            newPos = new Vector2f(newPos.X + hSpace + this.TheValue.Width(), newPos.Y);
            this.Right = new Label(">", newPos, Origin.TOPLEFT, fontSize, font, normalColor, hoverColor, normalColor);
        }

        public void ChangeValue(RenderWindow window) {
            Font font = new Font(FontUtils.TextFont1);
            if (font == null) {
                throw new Exception($"Failed to load font: {font}");
            }

            if (this.OnLeft) {
                this.Setting.Prev();
                this.BuildOption();
            } else if (this.OnRight) {
                this.Setting.Next();
                this.BuildOption();
            }
        }

        public override FloatRect GetBounds() {
            // Make this be the way to get the size of the element by others.
            // random value
            return new FloatRect(new Vector2f(0.0f, 0.0f), new Vector2f(0.0f, 0.0f));
        }

        private void CheckNulls() {
            if ((this.Name == null) || (this.Left == null) || (this.Right == null) || (this.TheValue == null)) {
                throw new Exception("Corrupted object.");
            }
        }

        public override void Update(RenderWindow window) {
            if ((this.Name == null) || (this.Left == null) || (this.Right == null) || (this.TheValue == null)) {
                throw new Exception("Corrupted object.");
            }
            Vector2f mouse = window.MapPixelToCoords(Mouse.GetPosition(window));
            FloatRect bounds = this.Left.GetBounds();
            this.OnLeft = bounds.Contains(mouse.X, mouse.Y);

            bounds = this.Right.GetBounds();
            this.OnRight = bounds.Contains(mouse.X, mouse.Y);

            this.MouseOn = this.OnLeft || this.OnRight;

            this.Name.Update(window);
            this.Left.Update(window);
            this.TheValue.Update(window);
            this.Right.Update(window);
        }

        public override void Render(RenderTarget window) {
            if ((this.Name == null) || (this.Left == null) || (this.Right == null) || (this.TheValue == null)) {
                throw new Exception("Corrupted object.");
            }
            this.Name.Render(window);
            this.Left.Render(window);
            this.TheValue.Render(window);
            this.Right.Render(window);
        }
    }
}
