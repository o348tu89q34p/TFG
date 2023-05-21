using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Gui {
    public class Ui {
        /*
        private RenderWindow _window;
        private List<GuiComponent> _uiElements;

        public Ui(RenderWindow window) {
            this._window = window;
            this._uiElements = new List<GuiComponent>();
        }

        public void AddElement(GuiComponent element) {
            this._uiElements.Add(element);
        }

        public Label CreateLabel(string text,
                                 Vector2f pos,
                                 uint charSize,
                                 Font font,
                                 Color fillColor)
        {
            Label l = new Label(text, pos, charSize, font, fillColor);
            l.Id = this._uiElements.Count;
            this._uiElements.Add(l);

            return l;
        }

        public void Update(RenderWindow window) {
            if (this._uiElements.Count == 0) {
                return;
            }

            foreach (GuiComponent elem in this._uiElements) {
                elem.Update((Vector2f)Mouse.GetPosition(window));
            }
        }

        public void Render() {
            if (this._uiElements.Count == 0) {
                return;
            }

            foreach (GuiComponent elem in this._uiElements) {
                elem.Render(this._window);
            }
        }
        */
    }
}
