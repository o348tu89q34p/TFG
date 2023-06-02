using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Domain;
using Game;
using Animation;

namespace GameObjects;

class SingleOpponent {
    private Sprite NameBg { get; }
    private Text Name { get; }
    private Text NumCards { get; set; }
    private Sprite BackCard { get; set; }
    private IAnimation<int> Animation { get; set; }

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

        this.Animation = new EmptyAnimation<int>();
    }

    private void UpdateState(int num) {
        this.NumCards = new Text(num.ToString(), FontUtils.StatusFont, FontSize) {
            FillColor = Color.Blue
        };
        float x = this.NameBg.Position.X + this.NameBg.GetGlobalBounds().Width - this.NumCards.GetGlobalBounds().Width/2.0f - 14.0f;
        float y = this.NameBg.Position.Y + this.NumCards.GetGlobalBounds().Height/5.0f;
        this.NumCards.Position = new Vector2f(x, y);
    }

    public void StartAnimation(OpponentAnim oa, Sprite sprite, Vector2f start, int newNum) {
        switch (oa) {
            case OpponentAnim.PICK:
                this.Animation = new TransCardAnimation<int>(sprite, start, this.BackCard.Position, 1000.0f, newNum);
                break;
            case OpponentAnim.MELD:
                break;
            case OpponentAnim.DROP:
                this.Animation = new TransCardAnimation<int>(sprite, this.BackCard.Position, start, 1000.0f, newNum);
                break;
        }
    }

    public Vector2f GetCoords() {
        return this.BackCard.Position;
    }

    public bool PlayAnimation() {
        bool res = this.Animation.RunAnimation();
        if (!res) {
            this.UpdateState(this.Animation.GetNewState());
            this.Animation = new EmptyAnimation<int>();
        }

        return res;
        /*
        this.Animation.RunAnimation();
        if (this.TransCardAnimation == null ||
            this.AreClose(this.BackCard.Position, this.TransCardAnimation.Position, new Vector2f(10.0f, 10.0f))) {
            this.UpdateState();
            this.TransCardAnimation = null;
            return false;
        }

        this.TransCardAnimation.Position += this.Velocity;
        return true;
        */
    }

    public void UpdateCount(int num) {
        this.UpdateState(num);
    }

    public void Render(RenderWindow window) {
        window.Draw(this.NameBg);
        window.Draw(this.Name);
        window.Draw(this.NumCards);
        window.Draw(this.BackCard);

        this.Animation.Render(window);
    }
}
