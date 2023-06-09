using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Game;

namespace GameObjects;

public class CardHolder {
    private Text Name { get; }
    private RectangleShape Bg { get; }
    private RectangleShape Bottom { get; }
    private List<Sprite> Cards { get; }

    public CardHolder(Vector2f pos, Vector2f size, string name, List<Sprite> cards) {
        float leftWidth = 0.1f;
        float rightWidth = 1.0f - leftWidth;
        // 10% of the width, to the left.
        this.Name = new Text(name, FontUtils.ButtonFont, 15);
        Vector2f nameSize = new Vector2f(this.Name.GetGlobalBounds().Width, this.Name.GetGlobalBounds().Height);
        this.Name.Position = new Vector2f(pos.X + size.X*leftWidth/2.0f - nameSize.X/2.0f, pos.Y + size.Y/2.0f - nameSize.Y/2.0f);

        // 90% of the width, to the right.
        this.Bg = new RectangleShape(new Vector2f(size.X*rightWidth, size.Y)) {
            Position = new Vector2f(pos.X + size.X*0.1f, pos.Y),
            FillColor = new Color(0, 0, 0, 100)
        };

        this.Bottom = new RectangleShape(new Vector2f(size.X, size.Y*0.05f)) {
            Position = new Vector2f(pos.X, pos.Y + size.Y - size.Y*0.05f),
            FillColor = new Color(0, 0, 0, 0)
        };

        Vector2f cardZone = new Vector2f(this.Bg.GetGlobalBounds().Width - 10.0f, this.Bg.GetGlobalBounds().Height - 5.0f);
        float scale = cardZone.Y*0.95f/TextureUtils.CardHeight;
        float firstCardY = this.Bg.Position.Y + this.Bg.GetGlobalBounds().Height/2.0f - TextureUtils.CardHeight*scale/2.0f;
        Vector2f firstCard = new Vector2f(this.Bg.Position.X + 5.0f, firstCardY);
        float newWidth = TextureUtils.CardWidth*scale;
        float newHeight = TextureUtils.CardHeight*scale;
        float cardSeparation = (cardZone.X - newWidth)/(cards.Count - 1);
        if (cardSeparation > newWidth + 5.0f) {
            cardSeparation = newWidth + 5.0f;
        }

        this.Cards = new List<Sprite>(cards.Count);
        for (int i = 0; i < cards.Count; i++) {
            this.Cards.Add(cards[i]);
            cards[i].Scale = new Vector2f(scale, scale);
            cards[i].Position = new Vector2f(firstCard.X + cardSeparation*i, firstCard.Y);
        }
    }

    public void Render(RenderWindow window) {
        window.Draw(this.Name);
        window.Draw(this.Bg);
        for (int i = 0; i < this.Cards.Count; i++) {
            window.Draw(this.Cards[i]);
        }
        window.Draw(this.Bottom);
    }
}
