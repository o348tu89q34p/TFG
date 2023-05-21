using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Gui {
    public class SettingsBox : GuiComponent {

        public override FloatRect GetBounds() {
            // random value
            return new FloatRect(new Vector2f(0.0f, 0.0f), new Vector2f(0.0f, 0.0f));
        }

        public override void Update(RenderWindow window) {
            
        }

        public override void Render(RenderTarget window) {
            
        }
    }
}
