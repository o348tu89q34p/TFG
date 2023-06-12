using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Domain;
using Game;
using Animation;

namespace GameObjects;

class SingleOpponent {
    private Sprite NameBg { get; }
    private int Amount { get; set; }
    private Text Name { get; }
    private Text NumCards { get; set; }
    private Sprite BackCard { get; set; }

    private const uint FontSize = 20;
    private const float VertOff = 0.0f;

    public SingleOpponent(PlayerProfile profile, Texture back, Vector2f pos) {
        this.Amount = profile.NumCards;
        Texture NameTex = TextureUtils.NamePlateTexture;
        this.NameBg = new Sprite(NameTex) {
            Position = new Vector2f(pos.X, pos.Y)
        };

        this.Name = new Text(profile.Name, FontUtils.StatusFont, FontSize) {
            FillColor = Color.Black,
            Position = new Vector2f(pos.X + 5.0f, pos.Y + VertOff)
        };

        this.NumCards = new Text(profile.NumCards.ToString(), FontUtils.StatusFont, FontSize) {
            FillColor = Color.Blue
            //Position = new Vector2f(pos.X + 93.0f, pos.Y + vertOff)
        };
        float x = this.NameBg.Position.X + this.NameBg.GetGlobalBounds().Width - this.NumCards.GetGlobalBounds().Width/2.0f - 14.0f;
        float y = this.NameBg.Position.Y + this.NumCards.GetGlobalBounds().Height/5.0f;
        this.NumCards.Position = new Vector2f(x, y);

        Texture BackTex = back;
        this.BackCard = new Sprite(BackTex) {
            Position = new Vector2f(pos.X, pos.Y + (float)NameTex.Size.Y)
        };
    }

    public void UpdateCount(int num) {
        this.Amount = num;
        this.NumCards = new Text(num.ToString(), FontUtils.StatusFont, FontSize) {
            FillColor = Color.Blue
        };
        float x = this.NameBg.Position.X + this.NameBg.GetGlobalBounds().Width - this.NumCards.GetGlobalBounds().Width/2.0f - 14.0f;
        float y = this.NameBg.Position.Y + this.NumCards.GetGlobalBounds().Height/5.0f;
        this.NumCards.Position = new Vector2f(x, y);
    }

    public Vector2f GetCoords() {
        return this.BackCard.Position;
    }

    // Debug method
    public int GetCards() {
        return this.Amount;
    }

    public void Render(RenderWindow window) {
        window.Draw(this.NameBg);
        window.Draw(this.Name);
        window.Draw(this.NumCards);
        window.Draw(this.BackCard);
    }
}
