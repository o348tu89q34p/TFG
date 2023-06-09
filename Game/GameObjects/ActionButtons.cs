using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Domain;
using Gui;
using Game;

namespace GameObjects;

class SpecificAction {
    private ImageButton Button { get; }
    private PlayerAction AssociatedAction { get; }
    private Step Action { get; }

    public SpecificAction(string text, PlayerAction pa, Vector2f pos, Step step) {
        this.Button = new ImageButton(text, pos);
        this.AssociatedAction = pa;
        this.Action = step;
    }

    public Step OnMouseButtonPress(object? sender, MouseButtonEventArgs e) {
        if (sender == null) {
            return Step.HUM_PLAY;
        }

        RenderWindow window = (RenderWindow)sender;
        if (e.Button == Mouse.Button.Left) {
            if (this.Button.Hovering && this.Button.IsEnabled()) {
                if (this.Button.IsActivated()) {
                    this.Button.Deactivate();
                    return Step.HUM_PLAY;
                } else {
                    this.Button.Activate();
                    return this.Action;
                }
            }
        }

        if (this.Button.IsActivated()) {
            return this.Action;
        }

        return Step.HUM_PLAY;
    }

    public void Update(RenderWindow window, Step step) {
        if (Step.HUM_PLAY == step) {
            this.Button.Enable();
            this.Button.Deactivate();
        } else if (this.Action == step) {
            this.Button.Enable();
        } else {
            this.Button.Disable();
        }
        this.Button.Update(window);
    }

    public void Render(RenderWindow window) {
        this.Button.Render(window);
    }
}

public class ActionButtons {
    private List<SpecificAction> Elems { get; }

    public ActionButtons(RenderWindow window, List<(string, PlayerAction, Step)> actions) {
        float leftDist = 20.0f;
        float botDist = 20.0f;
        float botInc = 60.0f;

        this.Elems = new List<SpecificAction>(actions.Count);
        for (int i = 0; i < actions.Count; i++) {
            int index = actions.Count - i - 1;
            string text = actions[index].Item1;
            PlayerAction pa = actions[index].Item2;
            var pos = new Vector2f(window.Size.X - leftDist, window.Size.Y - botDist);
            this.Elems.Add(new SpecificAction(text, pa, pos, actions[index].Item3));
            botDist += botInc;
        }
    }

    public (bool, Step) OnMouseButtonPress(object? sender, MouseButtonEventArgs e) {
        if (sender == null) {
            return (false, Step.HUM_PLAY);
        }

        for (int i = this.Elems.Count - 1; i >= 0; i--) {
            Step s = this.Elems[i].OnMouseButtonPress(sender, e);
            if (s != Step.HUM_PLAY) {
                return (false, s);
            }
        }

        return (true, Step.HUM_PLAY);
    }

    public void Update(RenderWindow window, Step step) {
        foreach (var e in this.Elems) {
            e.Update(window, step);
        }
    }

    public void Render(RenderWindow window) {
        foreach (var e in this.Elems) {
            e.Render(window);
        }
    }
}
