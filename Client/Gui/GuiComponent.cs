using SFML.Graphics;
using SFML.System;

namespace Gui {
    public abstract class GuiComponent {
        protected Vector2f Size {get; set;}
        protected Vector2f Position {get; set;}
        protected Color FillColor {get; set;}
        protected Color HoverColor {get; set;}
        protected Color OutlineColor {get; set;}
        protected Color PressedColor {get; set;}

        protected bool CallUpdate {get; set;}

        public int Id = -1;

        // Methods.
        public abstract void Update(Vector2f mousePos);
        public abstract void Render(RenderTarget window);
    }
}
