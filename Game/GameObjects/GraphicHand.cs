using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Gui;
using Game;
using Domain;

namespace GameObjects;

public class GraphicHand {
    private List<GraphicCard> Cards { get; }
    private float MaxPeek = 40.0f;
    private float PeekWidth { get; set; }
    private HoverHand Hover { get; }
    private RegularButton? Button { get; set; }
    private int NumTaken { get; set; }
    private int LastFree { get; set; }

    public GraphicHand(RenderWindow window, List<Sprite> cards, int cardWidth, int cardHeight) {
        this.Cards = new List<GraphicCard>(cards.Count);
        float handWidth = ((float)window.Size.X)*0.68f;
        // The amount of space available to each peeking card.
        float peekAvailable = (handWidth - (float)cardWidth)/((float)(cards.Count - 1));
        if (peekAvailable > this.MaxPeek) {
            this.PeekWidth = this.MaxPeek;
        } else {
            this.PeekWidth = peekAvailable;
        }
        float actualWidth = this.PeekWidth*((float)(cards.Count - 1)) + (float)cardWidth;
        float firstPos = window.Size.X/2.0f - actualWidth/2.0f;
        float height = (float)window.Size.Y - (float)cardHeight;
        for (int i = 0; i < cards.Count - 1; i++) {
            this.Cards.Add(new GraphicCard(cards[i], new Vector2f(firstPos + this.PeekWidth*i, height)));
        }
        this.Cards.Add(new GraphicCard(cards[cards.Count - 1], new Vector2f(firstPos + this.PeekWidth*(cards.Count - 1), height)));
        this.Hover = new HoverHand(new Vector2f(window.Size.X, window.Size.Y), new Vector2f(TextureUtils.CardWidth, TextureUtils.CardHeight));
        this.Button = null;
        this.NumTaken = 0;
        this.LastFree = this.Cards.Count - 1;
    }

    // Re-compute the positions of every card based on the hand's width.
    public void UpdatePositions(RenderWindow window) {
        float cardWidth = (float)TextureUtils.CardWidth;
        float cardHeight = (float)TextureUtils.CardHeight;
        float winWidth = (float) window.Size.X;
        float winHeight = (float) window.Size.Y;
        float handWidth = ((float)window.Size.X)*0.68f;
        // The amount of space available to each peeking card.
        int n = this.Cards.Count - this.NumTaken;
        float peekAvailable = (handWidth - cardWidth)/((float)(n - 1));
        if (peekAvailable > this.MaxPeek) {
            this.PeekWidth = this.MaxPeek;
        } else {
            this.PeekWidth = peekAvailable;
        }
        float actualWidth = this.PeekWidth*((float)(n - 1)) + cardWidth;
        float firstPos = winWidth/2.0f - actualWidth/2.0f;
        float height = winHeight - cardHeight;
        int t = 0;
        this.LastFree = -1;
        for (int i = 0; i < this.Cards.Count; i++) {
            if (!this.Cards[i].IsTaken()) {
                this.Cards[i].UpdatePosition(new Vector2f(firstPos + this.PeekWidth*(i - t), height));
                this.LastFree = i;
            } else {
                t++;
            }
        }
    }

    // Get the coordinates of the right-most card.
    public Vector2f GetInsertPoint() {
        return this.Cards[this.Cards.Count - 1].GetPosition();
    }

    public Vector2f GetDiscardPoint(int pos) {
        return this.Cards[pos].GetPosition();
    }

    // Add a sprite to the hand.
    public void Add(Sprite sprite) {
        this.Cards.Add(new GraphicCard(sprite, this.PeekWidth));
    }

    // Remove a sprite from the hand.
    public void Shed(int pos) {
        this.Cards.RemoveAt(pos);
    }

    public void ResetHand(RenderWindow window) {
        foreach (GraphicCard c in this.Cards) {
            c.UnTake();
        }

        this.Hover.Clear();
        this.UpdatePositions(window);
        this.Button = null;
        this.NumTaken = 0;
    }

    public void RemoveCards() {
        foreach (GraphicCard h in this.Hover.GetCards()) {
            this.Cards.Remove(h);
        }
    }

    public List<int>? ReadToHover() {
        int pos = 0;
        foreach (GraphicCard c in this.Cards) {
            if (c.IsHovered) {
                this.Hover.Add(c, pos);
                c.Take();
                this.NumTaken++;
                //this.Cards.RemoveAt(pos);
                return null;
            }
            pos++;
        }

        if (this.Button != null && this.Button.IsHovered) {
            return this.Hover.GetPositions();
        }

        return null;
    }

    // On click action for discard.
    public ResultMove<int> ReadCard(ResultMove<int> res) {
        for (int i = 0; i < this.Cards.Count; i++) {
            if (this.Cards[i].IsHovered) {
                res.CardsMoved.Add(i);
                return new ResultMove<int>(res.Move, res.CardsMoved, res.MeldAffected, res.CardAffected);
            }
        }

        return res;
    }

    public bool MeldFormed() {
        return this.Button != null && this.Button.IsHovered;
    }

    public Sprite GetSprite(int i) {
        return this.Cards[i].GetSprite();
    }

    public void Update(RenderWindow window, Step step) {
        int n = this.Cards.Count;
        this.UpdatePositions(window);
        for (int i = 0; i < n; i++) {
            this.Cards[i].Update(window, this.PeekWidth, step, i == this.LastFree);
        }
        if (this.Hover.IsActive() && this.Button == null) {
            Vector2f pos = new Vector2f(window.Size.X/2.0f - 35.0f, window.Size.Y - TextureUtils.CardHeight*2.0f - 4.0f);
            this.Button = new RegularButton("accept", pos);
            //this.Button = new RegularButton(TextureUtils.RegularTexture, TextureUtils.RegularHoverTexture,"accept", pos);
        } else if (this.Button != null) {
            this.Button.Update(window);
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

