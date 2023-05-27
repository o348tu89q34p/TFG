using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Domain;
using Game;

namespace GameObjects;

class SingleOpponent {
    private Sprite NameBg;
    private Text Name;
    private Text NumCards;
    private Sprite BackCard;

    public SingleOpponent(PlayerProfile profile, Vector2f pos) {
        Texture NameTex = TextureUtils.NamePlateTexture;
        this.NameBg = new Sprite(NameTex) {
            Position = new Vector2f(pos.X, pos.Y)
        };

        uint fontSize = 20;
        float vertOff = 0.0f;

        this.Name = new Text(profile.Name, FontUtils.StatusFont, fontSize) {
            FillColor = Color.Black,
            Position = new Vector2f(pos.X + 5.0f, pos.Y + vertOff)
        };

        this.NumCards = new Text(profile.NumCards.ToString(), FontUtils.StatusFont, fontSize) {
            FillColor = Color.Blue
            //Position = new Vector2f(pos.X + 93.0f, pos.Y + vertOff)
        };
        this.NumCards.Position = new Vector2f(pos.X + NameTex.Size.X - 14.0f - this.NumCards.GetGlobalBounds().Width/2.0f, pos.Y + vertOff);

        Texture BackTex = TextureUtils.FrenchBackTexture;
        this.BackCard = new Sprite(BackTex) {
            Position = new Vector2f(pos.X, pos.Y + (float)NameTex.Size.Y)
        };
    }

    public void Render(RenderWindow window) {
        window.Draw(this.NameBg);
        window.Draw(this.Name);
        window.Draw(this.NumCards);
        window.Draw(this.BackCard);
    }
}
