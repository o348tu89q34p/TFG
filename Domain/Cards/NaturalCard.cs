using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Assets;

namespace Domain {
    public class NaturalCard<T, U> : ICard<T, U>
        where T : Scale, new()
        where U : Scale, new()
        {
            public NaturalField<T> Suit { get; private set; }
            public NaturalField<U> Rank { get; private set; }
            public Texture FrontTexture { get; private set; }
            public Sprite CardSprite { get; private set; }

            /*
              https://www.sfml-dev.org/documentation/2.5.1/annotated.php
              https://www.sfml-dev.org/documentation/2.5.1/classsf_1_1Text.php
              https://documentation.help/SFML.Net/e3f4d85f-f056-fb71-2bb6-5ca172d6601d.htm
              https://oprypin.github.io/crsfml/api/SF/Texture.html
              Tuto:
              https://gamefromscratch.com/sfml-c-tutorial-sprites-and-textures/
             */
            public NaturalCard(NaturalField<T> suit, NaturalField<U> rank) {
                this.Suit = suit;
                this.Rank = rank;

                this.FrontTexture = new Texture("../Assets/Graphics/Cards/ace.png");

                this.CardSprite = new Sprite(this.FrontTexture) {
                    Position = new Vector2f(100.0f, 100.0f)
                };
            }

            public void OnClick(RenderWindow window) {
                Vector2f mouse = window.MapPixelToCoords(Mouse.GetPosition(window));
                this.CardSprite.Position = new Vector2f(mouse.X, mouse.Y);
            }

            public bool IsFirst() {
                return (this.Suit.IsFirst() &&
                        this.Rank.IsFirst());
            }

            public bool IsLast() {
                return (this.Suit.IsLast() &&
                        this.Rank.IsLast());
            }

            public bool IsWithin(NaturalCard<T, U> a, NaturalCard<T, U> b) {
                if ((this.GetSuit().CompareTo(a.GetSuit()) != 0) ||
                    (this.GetSuit().CompareTo(b.GetSuit()) != 0))
                {
                    return false;
                }
                return this.GetRank().IsWithin(a.GetRank(), b.GetRank());
            }

            public bool IsNatural() {
                return true;
            }

            public bool IsWild() {
                return false;
            }

            public NaturalField<T> GetSuit() {
                return this.Suit;
            }

            public NaturalField<U> GetRank() {
                return this.Rank;
            }

            public void Next(bool wrap, bool inSuit) {
                if (this.Rank.IsLast() && !wrap && inSuit) {
                    throw new Exception("No wrap in suit.");
                }
                if (this.IsLast() && !wrap) {
                    throw new Exception("Cannot prev first card without wrap.");
                }

                if (this.Rank.IsLast() && !inSuit) {
                    this.Suit.Next();
                }

                this.Rank.Next();
            }

            public void Prev(bool wrap, bool inSuit) {
                if (this.Rank.IsFirst() && !wrap && inSuit) {
                    throw new Exception("No wrap in suit.");
                }
                if (this.IsFirst() && !wrap) {
                    throw new Exception("Cannot prev first card without wrap.");
                }

                if (this.Rank.IsFirst() && !inSuit) {
                    this.Suit.Prev();
                }

                this.Rank.Prev();
            }

            public int CompareTo(ICard<T, U> c) {
                if (c is WildCard<T, U>) {
                    return 1;
                }

                var nc = (NaturalCard<T, U>)c;
                int res = this.Suit.CompareTo(nc.Suit);
                if (res == 0) {
                    return this.Rank.CompareTo(nc.Rank);
                }

                return res;
            }

            public int CompareRank(ICard<T, U> c) {
                if (c is WildCard<T, U>) {
                    throw new ArgumentException("Cannot compare a natural's rank with a wild card.");
                }

                return c.GetRank().CompareTo(c.GetRank());
            }

            public int CompareSuit(ICard<T, U> c) {
                if (c is WildCard<T, U>) {
                    throw new ArgumentException("Cannot compare a natural's suit with a wild card.");
                }

                return c.GetSuit().CompareTo(c.GetSuit());
            }

            public bool Equals(ICard<T, U> c) {
                if (c is WildCard<T, U>) {
                    return false;
                }

                var nc = (NaturalCard<T, U>)c;
                return (this.Suit.CompareTo(nc.Suit) == 0 &&
                        this.Rank.CompareTo(nc.Rank) == 0);
            }

            public void Print() {
                Console.WriteLine("{0} of {1}",
                                  arg0: this.GetRank().ToString(),
                                  arg1: this.GetSuit().ToString());
            }

            public void Update(RenderWindow window) {
            }

            public void Render(RenderWindow window) {
                window.Draw(this.CardSprite);
            }
        }
}
