using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Gui;
using Game;
using Domain;

namespace GameObjects;

public class GraphicHand {
    private List<GraphicCard> Cards { get; }
    private Vector2f Canvas { get; }
    private float HandWidth { get; }
    private HoverHand Hover { get; }
    private RegularButton? Button { get; set; }
    private int LastFree { get; set; }

    private static float MaxPeek = 40.0f;
    private static float CardWidth = (float)TextureUtils.CardWidth;
    private static float CardHeight = (float)TextureUtils.CardHeight;

    public GraphicHand(RenderWindow window, List<Sprite> cards) {
        this.Cards = new List<GraphicCard>(cards.Count);
        for (int i = 0; i < cards.Count; i++) {
            this.Cards.Add(new GraphicCard(cards[i], i));
        }
        this.Canvas = new Vector2f(window.Size.X, window.Size.Y);
        this.HandWidth = (this.Canvas.X)*0.68f;
        this.Hover = new HoverHand(new Vector2f(this.Canvas.X, this.Canvas.Y), new Vector2f(TextureUtils.CardWidth, TextureUtils.CardHeight));
        this.Button = null;
        this.LastFree = 0;
        this.UpdatePositions();
    }

    // Re-compute the positions of every card based on the hand's width.
    public void UpdatePositions() {
        float winWidth = this.Canvas.X;
        float winHeight = this.Canvas.Y;

        // The amount of space available to each peeking card.
        int n = this.Cards.Count - this.Hover.Size();

        float peekWidth = this.PeekWidth();
        float actualWidth = peekWidth*((float)(n - 1)) + CardWidth;
        float firstPos = winWidth/2.0f - actualWidth/2.0f;
        float height = winHeight - CardHeight;
        int t = 0;
        this.LastFree = -1;

        for (int i = 0; i < this.Cards.Count; i++) {
            if (this.Cards[i].IsTaken()) {
                t++;
            } else {
                this.Cards[i].UpdatePosition(new Vector2f(firstPos + peekWidth*(i - t), height));
                this.LastFree = i;
            }
        }
    }

    private float PeekWidth() {
        int n = this.Cards.Count - this.Hover.Size();
        float peekAvailable = (this.HandWidth - CardWidth)/((float)(n - 1));
        float peekWidth;
        if (peekAvailable > MaxPeek) {
            peekWidth = MaxPeek;
        } else {
            peekWidth = peekAvailable;
        }

        return peekWidth;
    }

    public void UpdateHand(List<Sprite> cards) {
        this.Cards.Clear();
        for (int i = 0; i < cards.Count; i++) {
            this.Cards.Add(new GraphicCard(cards[i], i));
        }
        this.Hover.Clear();
        this.Button = null;
        this.LastFree = 0;
        this.UpdatePositions();
    }

    // Get the coordinates of the right-most card.
    public Vector2f GetInsertPoint() {
        return this.Cards[this.Cards.Count - 1].GetPosition();
    }

    public Vector2f GetDiscardPoint(int pos) {
        return this.Cards[pos].GetPosition();
    }

    public void ResetHand() {
        if (this.Hover.Size() > 0) {
            foreach (GraphicCard c in this.Cards) {
                c.UnTake();
            }

            this.Hover.Clear();
            this.UpdatePositions();
        }

        this.Button = null;
    }


    public (List<int>?, bool) ReadToHover() {
        if (this.Button != null && this.Button.IsHovered) {
            return (this.Hover.GetPositions(), true);
        }

        int pos = 0;
        foreach (GraphicCard c in this.Cards) {
            if (c.IsTaken() && c.HoveredCross) {
                this.Hover.Demote(pos);
                return (null, false);
            } else if (c.IsHovered) {
                this.Hover.Add(c, pos);
                return (null, false);
            }
            pos++;
        }

        return (null, false);
    }

    // On click action for discard.
    public List<int>? ReadShed() {
        for (int i = 0; i < this.Cards.Count; i++) {
            if (this.Cards[i].IsHovered) {
                List<int> lst = new List<int>();
                lst.Add(i);
                return lst;
            }
        }

        return null;
    }

    public bool MeldFormed() {
        return this.Button != null && this.Button.IsHovered;
    }

    public Sprite GetSprite(int i) {
        return this.Cards[i].GetSprite();
    }

    public void Update(RenderWindow window, Step step) {
        int n = this.Cards.Count;
        this.UpdatePositions();
        for (int i = 0; i < n; i++) {
            this.Cards[i].Update(window, this.PeekWidth(), step, i == this.LastFree);
        }
        if (this.Hover.IsActive() && this.Button == null) {
            Vector2f pos = new Vector2f(window.Size.X/2.0f - 35.0f, window.Size.Y - TextureUtils.CardHeight*2.0f - 4.0f);
            this.Button = new RegularButton("accept", pos);
        } else if (this.Hover.IsActive() && this.Button != null) {
            this.Button.Update(window);
        } else {
            this.Button = null;
        }
    }

    public void Render(RenderWindow window) {
        this.Hover.Render(window);

        if (this.Button != null) {
            this.Button.Render(window);
        }

        foreach (GraphicCard gc in this.Cards) {
            if (!gc.IsTaken()) {
                gc.Render(window);
            }
        }
    }
}
