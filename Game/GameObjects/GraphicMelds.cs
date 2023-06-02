using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Domain;

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

        for (int i = 0; i < this.Melds.Count; i++) {
            this.Melds[i].UpdatePositions(this.Middle, this.Melds.Count, i);
        }
    }

    public void Render(RenderWindow window) {
        foreach (var m in this.Melds) {
            m.Render(window);
        }
    }
}
