using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Domain;
using Game;

namespace GameObjects;

public class OpponentsInfo {
    private List<SingleOpponent> Opponents { get; }

    public OpponentsInfo(RenderWindow window, List<PlayerProfile> profiles) {
        float nTop;
        int n = profiles.Count;
        if (n < 3) {
            nTop = (float)n;
        } else {
            nTop = (float)(n - 2);
        }
        float space = ((float)window.Size.X)/(nTop + 1.0f);
        float start = space - ((float)TextureUtils.FrenchBackTexture.Size.X)/2.0f;

        this.Opponents = new List<SingleOpponent>(n);
        if (n < 3) {
            for (int i = 0; i < n; i++) {
                this.Opponents.Add(new SingleOpponent(profiles[i], new Vector2f(start + space*i, 0.0f)));
            }
        } else {
            float fromTop = 80.0f;
            this.Opponents.Add(new SingleOpponent(profiles[0], new Vector2f(0.0f, fromTop)));
            for (int i = 1; i < n - 1; i++) {
                this.Opponents.Add(new SingleOpponent(profiles[i], new Vector2f(start + space*(i - 1), 0.0f)));
            }
            float leftDist = (float)window.Size.X - (float)TextureUtils.NamePlateTexture.Size.X;
            this.Opponents.Add(new SingleOpponent(profiles[n - 1], new Vector2f(leftDist, fromTop)));
        }
    }

    public void StartAnim(int i, Sprite sprite, Vector2f pos, PlayerProfile profile) {
        this.Opponents.ElementAt(i).StartAnimation(sprite, pos, profile.NumCards);
    }

    public bool PlayAnimation() {
        bool res = false;

        foreach (var opp in this.Opponents) {
            if (opp.IsAnimating()) {
                res = res || opp.PlayAnimation();
            }
        }

        return res;
    }

    public void Render(RenderWindow window) {
        foreach (var opponent in this.Opponents) {
            opponent.Render(window);
        }
    }
}
