using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Domain;
using Gui;

namespace GameObjects;

class SpecificAction {
    private ImageButton Button { get; }
    private PlayerAction AssociatedAction { get; }

    public SpecificAction(string text, PlayerAction pa, Vector2f pos) {
        this.Button = new ImageButton(text, pos);
        this.AssociatedAction = pa;
    }

    public void Render(RenderWindow window) {
        this.Button.Render(window);
    }
}

public class ActionButtons {
    private List<SpecificAction> Elems { get; }

    public ActionButtons(RenderWindow window, List<(string, PlayerAction)> actions) {
        float leftDist = 20.0f;
        float botDist = 20.0f;
        float botInc = 60.0f;

        this.Elems = new List<SpecificAction>(actions.Count);
        for (int i = 0; i < actions.Count; i++) {
            string text = actions[actions.Count - i - 1].Item1;
            PlayerAction pa = actions[actions.Count - i - 1].Item2;
            var pos = new Vector2f(window.Size.X - leftDist, window.Size.Y - botDist);
            this.Elems.Add(new SpecificAction(text, pa, pos));
            botDist += botInc;
        }
    }

    public void Render(RenderWindow window) {
        foreach (var e in this.Elems) {
            e.Render(window);
        }
    }
}
