using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Game;

namespace GameObjects;

public class GraphicMelds {
    private List<GraphicMeld> Melds { get; }
    private Vector2f Canvas { get; }

    public GraphicMelds(RenderWindow window) {
        this.Melds = new List<GraphicMeld>();
        this.Canvas = new Vector2f(window.Size.X, window.Size.Y);
    }

    // Done only after a change in the melds representation has occured.
    public void UpdateMelds(List<List<Sprite>> melds) {
        this.Melds.Clear();
        for (int i = 0; i < melds.Count; i++) {
            this.Melds.Add(new GraphicMeld(melds[i], this.Canvas, melds.Count, i));
        }
    }

    // Returns the meld and card in it that are currently selected.
    public (int?, int?) GetSelected() {
        for (int i = 0; i < this.Melds.Count; i++) {
            (int? col, int? item) = this.Melds[i].SelectedParts();
            if (col != null || item != null) {
                return (col, item);
            }
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
                m.ToggleSelector();
            }
        }
    }

    public void Update(RenderWindow window, Step step) {
        for (int i = 0; i < this.Melds.Count; i++) {
            // Maybe we don't need to update positionsn on every frame.
            //this.Melds[i].UpdatePositions();
            this.Melds[i].Update(window, step);
        }
    }

    public void Render(RenderWindow window) {
        foreach (GraphicMeld meld in this.Melds) {
            meld.Render(window);
        }
    }
}
