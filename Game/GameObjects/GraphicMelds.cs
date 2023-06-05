using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Domain;
using Game;

namespace GameObjects;

public class GraphicMelds {
    private List<GraphicMeld> Melds { get; }
    private float Middle { get; }

    public GraphicMelds(RenderWindow window) {
        this.Melds = new List<GraphicMeld>();
        this.Middle = (float)window.Size.X/2.0f;
    }

    public void AddMeld(List<Sprite> cards) {
        this.Melds.Add(new GraphicMeld(cards, this.Middle, this.Melds.Count + 1, this.Melds.Count));

        /*
        for (int i = 0; i < this.Melds.Count; i++) {
            this.Melds[i].UpdatePositions(this.Middle, this.Melds.Count, i);
        }
        */
    }

    // Return the selected meld and specific card in it;
    public (int?, int?) GetSelected() {
        int i = 0;
        foreach (GraphicMeld m in this.Melds) {
            (int? col, int? item) = m.SelectedParts();
            if (col != null || item != null) {
                return (col, item);
            }
            i++;
        }
        return (null, null);
    }

    public void ToggleMelds(Step step) {
        bool anyHover = false;
        int i = 0;
        while (!anyHover && i < this.Melds.Count) {
            anyHover = this.Melds[i].IsHovered();
            i++;
        }

        if (anyHover) {
            foreach (GraphicMeld m in this.Melds) {
                m.ToggleBigSelector(step);
            }
        }
    }

    public void Update(RenderWindow window, Step step) {
        for (int i = 0; i < this.Melds.Count; i++) {
            this.Melds[i].UpdatePositions(this.Middle, this.Melds.Count, i);
            this.Melds[i].Update(window, step);
        }
    }

    public void Render(RenderWindow window) {
        foreach (var m in this.Melds) {
            m.Render(window);
        }
    }
}
