using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Game;

namespace GameObjects;

public class GraphicMeld {
    private List<Sprite> Meld { get; }
    private Vector2f Canvas { get; }
    private int X { get; set; }
    private int N { get; set; }

    private int? HoveredCard { get; set; }
    private int? SelectedCard { get; set; }
    private RectangleShape? Selector { get; set; }
    private RectangleShape? Hover { get; set; }

    private static float Height = 170.0f;
    private static float HorSpacing = 10.0f;
    private static float VertSpacing = 30.0f;

    public GraphicMeld(List<Sprite> meld, Vector2f canvas, int n, int x) {
        this.Meld = meld;
        this.Canvas = canvas;
        this.X = x;
        this.N = n;

        this.UpdatePositions();
    }

    // Place the melds in columns in the middle of the screen.
    public void UpdatePositions() {
        float allWidth = (TextureUtils.CardWidth*this.N + HorSpacing*(this.N - 1));
        float singleWidth = (TextureUtils.CardWidth + HorSpacing);
        float start = this.Canvas.X/2.0f - (allWidth/2.0f);

        for (int i = 0; i < this.Meld.Count; i++) {
            this.Meld[i].Position = new Vector2f(start + singleWidth*this.X,
                                                 Height + VertSpacing*(this.Meld.Count - i - 1));
        }
    }

    public (int?, int?) SelectedParts() {
        if (this.Selector == null) {
            return (null, this.SelectedCard);
        }

        return (this.X, this.SelectedCard);
    }

    public void ToggleSelector() {
        if (this.Hover == null) {
            this.Selector = null;
            this.SelectedCard = null;
        } else {
            if (this.Selector == null) {
                this.Selector = new RectangleShape(this.Hover);
                this.Selector.OutlineColor = Color.Red;
                this.SelectedCard = this.HoveredCard;
            } else {
                this.Selector = null;
                this.SelectedCard = null;
            }
        }
    }

    public bool IsHovered() {
        return this.Hover != null;
    }

    // Given the order of a card in a meld and its position on the plane, return its visible area.
    private FloatRect Hitbox(Vector2f pos, int i) {
        float height;
        if (i == 0) {
            height = TextureUtils.CardHeight;
        } else {
            height = VertSpacing;
        }

        return new FloatRect(pos, new Vector2f(TextureUtils.CardWidth, height));
    }

    private RectangleShape BuildBigSelector(Color color) {
        float width = TextureUtils.CardWidth;
        float height = VertSpacing*(this.Meld.Count - 1) + TextureUtils.CardHeight;
        Vector2f newPos = new Vector2f(this.Meld[0].Position.X,
                                       this.Meld[0].Position.Y - VertSpacing*(this.Meld.Count - 1));
        RectangleShape rs = new RectangleShape(new Vector2f(width, height)) {
            Position = newPos,
            FillColor = new Color(0, 0, 0, 0),
            OutlineColor = color,
            OutlineThickness = 2.0f
        };

        return rs;
    }

    private RectangleShape BuildSmallSelector(int i, Color color) {
        float width = TextureUtils.CardWidth;
        float height;
        if (i == 0) {
            height = TextureUtils.CardHeight;
        } else {
            height = VertSpacing;
        }
        // Consider the fact that we draw bottom to top on the screen.
        Vector2f newPos = new Vector2f(this.Meld[0].Position.X,
                                       this.Meld[0].Position.Y - VertSpacing*(i));
        RectangleShape rs = new RectangleShape(new Vector2f(width, height)) {
            Position = newPos,
            FillColor = new Color(0, 0, 0, 0),
            OutlineColor = color,
            OutlineThickness = 2.0f
        };

        return rs;
    }

    public void Update(RenderWindow window, Step step) {
        Vector2f mouse = window.MapPixelToCoords(Mouse.GetPosition(window));

        if (step != Step.HUM_LAYOFF && step != Step.HUM_REPLACE) {
            this.HoveredCard = null;
            this.Hover = null;
            this.Selector = null;
            return;
        }

        int i = 0;
        bool hovering = false;
        while (i < this.Meld.Count && !hovering) {
            hovering = this.Hitbox(this.Meld[i].Position, i).Contains(mouse.X, mouse.Y);
            if (!hovering) {
                i++;
            }
        }

        if (step == Step.HUM_LAYOFF) {
            if (hovering) {
                this.Hover = this.BuildBigSelector(Color.Yellow);
            } else {
                this.Hover = null;
            }
        } else if (step == Step.HUM_REPLACE) {
            if (hovering) {
                this.Hover = this.BuildSmallSelector(i, Color.Yellow);
                this.HoveredCard = i;
            } else {
                this.HoveredCard = null;
                this.Hover = null;
            }
        }
    }

    public void Render(RenderWindow window) {
        for (int i = this.Meld.Count - 1; i >= 0; i--) {
            window.Draw(this.Meld[i]);
        }

        if (this.Hover != null) {
            window.Draw(this.Hover);
        }

        if (this.Selector != null) {
            window.Draw(this.Selector);
        }
    }
}
