using SFML.Graphics;
using SFML.System;

namespace Gui {
    public abstract class GuiComponent {
        public Vector2f Size { get; protected set; }
        public Vector2f Position { get; protected set; }
        public Color FillColor { get; protected set; }
        public Color HoverColor { get; protected set; }
        public Color OutlineColor { get; protected set; }
        public Color PressedColor { get; protected set; }
        public bool MouseOn { get; protected set; }

        protected bool CallUpdate { get; set; }

        public int Id = -1;

        // Methods.
        public abstract FloatRect GetBounds();
        public abstract void Update(RenderWindow window);
        public abstract void Render(RenderTarget window);
    }
}
