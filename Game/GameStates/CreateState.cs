using SFML.Graphics;
using SFML.Window;
using SFML.System;

using Gui;
using Domain;

namespace Game {
    class CreateState : GameState {
        private Label StateTitle { get; }
        private Label BackButton { get; }
        private Label StartButton { get; }
        private Label Message { get; set; }
        private StateManager GSManager { get; set; }

        private NextOption<Nextable<string>, string> OptDeck { get; }
        private NextOption<Nextable<int>, int> OptNumPlayers { get; }
        private NextOption<Nextable<int>, int> OptNumDecks { get; }
        private NextOption<Nextable<int>, int> OptNumWilds { get; }
        private NextOption<Nextable<int>, int> OptNumCards { get; }
        private NextOption<Nextable<bool>, bool> OptMultWc { get; }
        private NextOption<Nextable<bool>, bool> OptConsecWc { get; }
        private NextOption<Nextable<bool>, bool> OptCanWrap { get; }
        private NextOption<Nextable<bool>, bool> OptNeedsOut { get; }
        private NextOption<Nextable<bool>, bool> OptEndDiscard { get; }

        public CreateState(StateManager gsManager, RenderWindow window) {
            this.GSManager = gsManager;

            float xLeft = 50.0f;
            float xRight = 480.0f;
            float lTop = 150.0f;
            float rTop = 150.0f;
            float space = 70.0f;

            var optDeck = new NextableStrings(new string[]{"Espanyola", "Francesa"}, 0);
            this.OptDeck = new NextOption<Nextable<string>, string>("Tipus de baralla", optDeck, new Vector2f(xLeft, lTop + space));
            lTop += space;

            var optNumPlayers = new NextableInt(2, 8, 2);
            this.OptNumPlayers = new NextOption<Nextable<int>, int>("Nombre de jugadors", optNumPlayers, new Vector2f(xLeft, lTop + space));
            lTop += space;

            var optNumDecks = new NextableInt(1, 4, 1);
            this.OptNumDecks = new NextOption<Nextable<int>, int>("Nombre de baralles", optNumDecks, new Vector2f(xLeft, lTop + space));
            lTop += space;

            var optNumWilds = new NextableInt(0, 10, 0);
            this.OptNumWilds = new NextOption<Nextable<int>, int>("Nombre de comodins", optNumWilds, new Vector2f(xLeft, lTop + space));
            lTop += space;

            var optNumCards = new NextableInt(1, 109, 7);
            this.OptNumCards = new NextOption<Nextable<int>, int>("Cartes per jugador", optNumCards, new Vector2f(xLeft, lTop + space));
            lTop += space;

            var optCanWrap = new NextableBool(false);
            this.OptCanWrap = new NextOption<Nextable<bool>, bool>("Les escales no es tanquen", optCanWrap, new Vector2f(xRight, rTop + space));
            rTop += space;

            var optMultWc = new NextableBool(false);
            this.OptMultWc = new NextOption<Nextable<bool>, bool>("Multiples comodins per grup", optMultWc, new Vector2f(xRight, rTop + space));
            rTop += space;

            var optConsecWc = new NextableBool(false);
            this.OptConsecWc = new NextOption<Nextable<bool>, bool>("Comodins consecutius en un grup", optConsecWc, new Vector2f(xRight, rTop + space));
            rTop += space;

            var optNeedsOut = new NextableBool(false);
            this.OptNeedsOut = new NextOption<Nextable<bool>, bool>("S'ha de sortir", optNeedsOut, new Vector2f(xRight, rTop + space));
            rTop += space;

            var optEndDiscard = new NextableBool(false);
            this.OptEndDiscard = new NextOption<Nextable<bool>, bool>("S'ha de descartar la ultima carta", optEndDiscard, new Vector2f(xRight, rTop + space));
            rTop += space;

            // Title.
            Font titleFont = new Font(FontUtils.TitleFont);
            if (titleFont == null) {
                throw new Exception($"Failed to load font: {titleFont}");
            }
            Vector2f titlePos = new Vector2f(window.Size.X / 2, 100);
            Color titleColor = new Color(0, 70, 255);

            this.StateTitle = new Label("Crea la partida", titlePos, Origin.CENTER, 60, titleFont, titleColor, titleColor, titleColor);

            // Buttons.
            Font buttonsFont = new Font(FontUtils.TextFont1);
            if (buttonsFont == null) {
                throw new Exception($"Failed to load font: {buttonsFont}");
            }

            Vector2f buttonPos = new Vector2f(50, window.Size.Y - 20);
            Color buttonColor = titleColor;
            Color hoverColor = new Color(200, 0, 12);
            this.BackButton = new Label("Cancelar", buttonPos, Origin.BOTTOMLEFT, 30, buttonsFont, buttonColor, hoverColor, buttonColor);

            buttonPos = new Vector2f(window.Size.X - 50, window.Size.Y - 20);
            this.StartButton = new Label("Jugar", buttonPos, Origin.BOTTOMRIGHT, 30, buttonsFont, buttonColor, hoverColor, buttonColor);

            // Message.
            Color messageColor = new Color(200, 50, 0);
            this.Message = new Label("", new Vector2f(window.Size.X/2, 100), Origin.CENTER, 15, buttonsFont, messageColor, messageColor, messageColor);

            this.BindEvents(window);
        }

        private void UpdateMessage(RenderWindow window, string newMessage) {
            Font buttonsFont = new Font(FontUtils.TextFont1);
            if (buttonsFont == null) {
                throw new Exception($"Failed to load font: {buttonsFont}");
            }
            Color messageColor = new Color(200, 50, 0);
            this.Message = new Label(newMessage, new Vector2f(window.Size.X/2, window.Size.Y - 100), Origin.CENTER, 15, buttonsFont, messageColor, messageColor, messageColor);
        }

        public override void BindEvents(RenderWindow window) {
            window.Closed += new EventHandler(OnWindowClose);
            window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPress);
            window.MouseButtonPressed += new EventHandler<MouseButtonEventArgs>(OnMouseButtonPress);
        }

        public override void UnbindEvents(RenderWindow window) {
            window.Closed -= new EventHandler(OnWindowClose);
            window.KeyPressed -= new EventHandler<KeyEventArgs>(OnKeyPress);
            window.MouseButtonPressed -= new EventHandler<MouseButtonEventArgs>(OnMouseButtonPress);
        }

        public void OnWindowClose(object? sender, EventArgs e) {
            if (sender == null) {
                return;
            }

            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        public void OnKeyPress(object? sender, KeyEventArgs e) {
        }

        public void OnMouseButtonPress(object? sender, MouseButtonEventArgs e) {
            if (sender == null) {
                return;
            }
            RenderWindow window = (RenderWindow)sender;

            if (e.Button == Mouse.Button.Left) {
                if (this.BackButton.MouseOn) {
                    this.GSManager.ChangeState(window, new MenuState(this.GSManager, window));
                }
                if (this.StartButton.MouseOn) {
                    try {
                        if (this.OptDeck.Setting.GetIt().CompareTo("Espanyola") == 0) {
                            var r = new Rules<SpanishSuit, SpanishRank>(
                                (new SpanishSuit()).Data.Length,
                                (new SpanishRank()).Data.Length,
                                this.OptNumPlayers.Setting.GetIt(),
                                this.OptNumDecks.Setting.GetIt(),
                                this.OptNumWilds.Setting.GetIt(),
                                this.OptNumCards.Setting.GetIt(),
                                this.OptCanWrap.Setting.GetIt(),
                                this.OptMultWc.Setting.GetIt(),
                                this.OptConsecWc.Setting.GetIt(),
                                this.OptNeedsOut.Setting.GetIt(),
                                3, 3,
                                this.OptEndDiscard.Setting.GetIt(),
                                DeckType.SPANISH_DECK);
                            this.GSManager.ChangeState(window, new MatchState<SpanishSuit, SpanishRank>(this.GSManager, window, r));
                        } else if (this.OptDeck.Setting.GetIt().CompareTo("Francesa") == 0) {
                            var r = new Rules<FrenchSuit, FrenchRank>(
                                (new FrenchSuit()).Data.Length,
                                (new FrenchRank()).Data.Length,
                                this.OptNumPlayers.Setting.GetIt(),
                                this.OptNumDecks.Setting.GetIt(),
                                this.OptNumWilds.Setting.GetIt(),
                                this.OptNumCards.Setting.GetIt(),
                                this.OptCanWrap.Setting.GetIt(),
                                this.OptMultWc.Setting.GetIt(),
                                this.OptConsecWc.Setting.GetIt(),
                                this.OptNeedsOut.Setting.GetIt(),
                                3, 3,
                                this.OptEndDiscard.Setting.GetIt(),
                                DeckType.FRENCH_DECK);
                            this.GSManager.ChangeState(window, new MatchState<FrenchSuit, FrenchRank>(this.GSManager, window, r));
                        } else {
                            Console.WriteLine("Good rules, bad comparison.");
                            throw new Exception("Bad deck type");
                        }
                    } catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                        this.UpdateMessage(window, "Combinacio invalida de valors: " + ex.Message);
                    }
                }
                if (this.OptDeck.MouseOn) {
                    this.OptDeck.ChangeValue(window);
                }
                if (this.OptNumPlayers.MouseOn) {
                    this.OptNumPlayers.ChangeValue(window);
                }
                if (this.OptNumDecks.MouseOn) {
                    this.OptNumDecks.ChangeValue(window);
                }
                if (this.OptNumWilds.MouseOn) {
                    this.OptNumWilds.ChangeValue(window);
                }
                if (this.OptNumCards.MouseOn) {
                    this.OptNumCards.ChangeValue(window);
                }
                if (this.OptMultWc.MouseOn) {
                    this.OptMultWc.ChangeValue(window);
                }
                if (this.OptConsecWc.MouseOn) {
                    this.OptConsecWc.ChangeValue(window);
                }
                if (this.OptCanWrap.MouseOn) {
                    this.OptCanWrap.ChangeValue(window);
                }
                if (this.OptNeedsOut.MouseOn) {
                    this.OptNeedsOut.ChangeValue(window);
                }
                if (this.OptEndDiscard.MouseOn) {
                    this.OptEndDiscard.ChangeValue(window);
                }
            }
        }

        public override void Update(RenderWindow window) {
            this.StateTitle.Update(window);
            this.BackButton.Update(window);
            this.StartButton.Update(window);

            this.OptDeck.Update(window);
            this.OptNumPlayers.Update(window);
            this.OptNumDecks.Update(window);
            this.OptNumWilds.Update(window);
            this.OptNumCards.Update(window);
            this.OptMultWc.Update(window);
            this.OptConsecWc.Update(window);
            this.OptCanWrap.Update(window);
            this.OptNeedsOut.Update(window);
            this.OptEndDiscard.Update(window);

            this.Message.Update(window);
        }

        public override void Draw(RenderWindow window) {
            this.StateTitle.Render(window);
            this.BackButton.Render(window);
            this.StartButton.Render(window);

            this.OptDeck.Render(window);
            this.OptNumPlayers.Render(window);
            this.OptNumDecks.Render(window);
            this.OptNumWilds.Render(window);
            this.OptNumCards.Render(window);
            this.OptMultWc.Render(window);
            this.OptConsecWc.Render(window);
            this.OptCanWrap.Render(window);
            this.OptNeedsOut.Render(window);
            this.OptEndDiscard.Render(window);

            this.Message.Render(window);
        }
    }
}
