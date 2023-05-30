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

    private Vector2f Velocity { get; set; }
    private int NewNum { get; set; }
    private Sprite? PickAnimation { get; set; }
    private const uint FontSize = 20;
    private const float VertOff = 0.0f;

    public SingleOpponent(PlayerProfile profile, Vector2f pos) {
        Texture NameTex = TextureUtils.NamePlateTexture;
        this.NameBg = new Sprite(NameTex) {
            Position = new Vector2f(pos.X, pos.Y)
        };

        this.Name = new Text(profile.Name, FontUtils.StatusFont, FontSize) {
            FillColor = Color.Black,
            Position = new Vector2f(pos.X + 5.0f, pos.Y + VertOff)
        };

        this.NewNum = profile.NumCards;
        this.NumCards = new Text(profile.NumCards.ToString(), FontUtils.StatusFont, FontSize) {
            FillColor = Color.Blue
            //Position = new Vector2f(pos.X + 93.0f, pos.Y + vertOff)
        };
        float x = this.NameBg.Position.X + this.NameBg.GetGlobalBounds().Width - this.NumCards.GetGlobalBounds().Width/2.0f - 14.0f;
        float y = this.NameBg.Position.Y + this.NumCards.GetGlobalBounds().Height/5.0f;
        this.NumCards.Position = new Vector2f(x, y);

        Texture BackTex = TextureUtils.FrenchBackTexture;
        this.BackCard = new Sprite(BackTex) {
            Position = new Vector2f(pos.X, pos.Y + (float)NameTex.Size.Y)
        };
    }

    private void UpdateState() {
        this.NumCards = new Text(this.NewNum.ToString(), FontUtils.StatusFont, FontSize) {
            FillColor = Color.Blue
        };
        float x = this.NameBg.Position.X + this.NameBg.GetGlobalBounds().Width - this.NumCards.GetGlobalBounds().Width/2.0f - 14.0f;
        float y = this.NameBg.Position.Y + this.NumCards.GetGlobalBounds().Height/5.0f;
        this.NumCards.Position = new Vector2f(x, y);
    }

    private Vector2f GetVelocity(Vector2f start, Vector2f finish, float times) {
        float diffX = finish.X - start.X;
        float diffY = finish.Y - start.Y;
        float m = diffY/diffX;
        float b = start.Y - (start.X*m);

        float sqrX = diffX*diffX;
        float sqrY = diffY*diffY;
        float d = (float)Math.Sqrt((double)(sqrX + sqrY));

        float distanceToMove = d/times;

        float ux = diffX/d;
        float uy = diffY/d;

        return new Vector2f(distanceToMove*ux, distanceToMove*uy);
    }

    private bool AreClose(Vector2f p1, Vector2f p2, Vector2f r) {
        // Only for positive positions.
        return ((Math.Abs(p1.X - p2.X) < r.X) &&
                (Math.Abs(p1.Y - p2.Y) < r.Y));
    }

    public void StartAnimation(Sprite sprite, Vector2f pos, int newNum) {
        this.NewNum = newNum;
        this.PickAnimation = sprite;
        this.PickAnimation.Position = pos;
        this.Velocity = this.GetVelocity(pos, this.BackCard.Position, 500.0f);
    }

    public bool PlayAnimation() {
        if (this.PickAnimation == null ||
            this.AreClose(this.BackCard.Position, this.PickAnimation.Position, new Vector2f(10.0f, 10.0f))) {
            this.UpdateState();
            this.PickAnimation = null;
            return false;
        }

        this.PickAnimation.Position += this.Velocity;
        return true;
    }

    public bool IsAnimating() {
        return this.PickAnimation != null;
    }

    public void Render(RenderWindow window) {
        window.Draw(this.NameBg);
        window.Draw(this.Name);
        window.Draw(this.NumCards);
        window.Draw(this.BackCard);

        if (this.PickAnimation != null) {
            window.Draw(this.PickAnimation);
        }
    }
}
