using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Game;

namespace GameObjects;

class GraphicMeld {
    private List<Sprite> Meld { get; }
    private float Middle { get; }
    private int X { get; set; }
    private int N { get; set; }
    private int? HoveredCard { get; set; }
    private int? SelectedCard { get; set; }
    private RectangleShape? BigSelector { get; set; }
    private RectangleShape? BigHover { get; set; }

    private static float Height = 170.0f;
    private static float HorSpacing = 10.0f;
    private static float VertSpacing = 30.0f;

    // n is the number of melds. x is the position of this meld in the list.
    public GraphicMeld(List<Sprite> meld, float middle, int n, int x) {
        this.Meld = meld;
        this.Middle = middle;
        this.X = x;
        this.N = n;
        this.HoveredCard = null;
        this.SelectedCard = null;

        this.UpdatePositions(middle, n, x);
    }

    public void UpdatePositions(float middle, int n, int x) {
        float allWidth = (TextureUtils.CardWidth*n + HorSpacing*(n - 1));
        float singleWidth = (TextureUtils.CardWidth + HorSpacing);
        float start = middle - (allWidth/2.0f);

        for (int i = 0; i < this.Meld.Count; i++) {
            this.Meld[i].Position = new Vector2f(start + singleWidth*x,
                                                 Height + VertSpacing*(this.Meld.Count - i - 1));
        }
    }

    public void Replace(Sprite newCard, float middle, int n, int x, int i) {
        this.Meld[i] = newCard;
        this.UpdatePositions(middle, n, x);
    }

    public void Prepend(List<Sprite> newCards, float middle, int n, int x) {
        for (int i = 0; i < newCards.Count; i++) {
            this.Meld.Insert(i, newCards.ElementAt(i));
        }
        this.UpdatePositions(middle, n, x);
    }

    public void Append(List<Sprite> newCards, float middle, int n, int x) {
        for (int i = 0; i < newCards.Count; i++) {
            this.Meld.Add(newCards.ElementAt(i));
        }
        this.UpdatePositions(middle, n, x);
    }

    private FloatRect Hitbox(int i, Vector2f pos) {
        float height;
        if (i == this.N - 1) {
            height = TextureUtils.CardHeight;
        } else {
            height = VertSpacing;
        }

        return new FloatRect(pos, new Vector2f(TextureUtils.CardWidth, height));
    }

    public bool IsHovered() {
        return this.BigHover != null;
    }

    public void ToggleBigSelector(Step step) {
        if (step == Step.HUM_LAYOFF) {
            if (this.BigHover != null) {
                if (this.BigSelector == null) {
                    this.BigSelector = new RectangleShape(this.BigHover);
                    this.BigSelector.OutlineColor = Color.Red;
                    this.SelectedCard = this.HoveredCard;
                } else {
                    this.BigSelector = null;
                    this.SelectedCard = null;
                }
            } else {
                this.BigSelector = null;
            }
        }
    }

    public (int?, int?) SelectedParts() {
        if (this.BigSelector == null) {
            return (null, this.SelectedCard);
        }

        return (this.X, this.SelectedCard);
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

    public void Update(RenderWindow window, Step step) {
        Vector2f mouse = window.MapPixelToCoords(Mouse.GetPosition(window));
        if (step != Step.HUM_LAYOFF && step != Step.HUM_REPLACE) {
            this.SelectedCard = null;
            this.BigHover = null;
            this.BigSelector = null;
            return;
        } else if (step == Step.HUM_LAYOFF) {
            int i = 0;
            bool hovering = false;
            while (i < this.Meld.Count && !hovering) {
                hovering = this.Hitbox(i, this.Meld[i].Position).Contains(mouse.X, mouse.Y);
                i++;
            }

            if (hovering) {
                this.BigHover = this.BuildBigSelector(Color.Yellow);
                this.HoveredCard = i;
            } else {
                this.BigHover = null;
                this.HoveredCard = null;
            }
            /*
            for (int i = 0; i < this.Meld.Count; i++) {
                hovering = this.Hitbox(i, this.Meld[i].Position).Contains(mouse.X, mouse.Y);
                if (hovering && step == Step.HUM_LAYOFF) {
                    this.BigHover = this.BuildBigSelector(Color.Yellow);
                    this.SelectedCard = i;
                }
            }
            */
        }

        if (step == Step.HUM_REPLACE) {
        }
    }

    public void Render(RenderWindow window) {
        for (int i = this.Meld.Count - 1; i >= 0; i--) {
            window.Draw(this.Meld[i]);
        }

        if (this.BigHover != null) {
            window.Draw(this.BigHover);
        }

        if (this.BigSelector != null) {
            window.Draw(this.BigSelector);
        }
    }
}
