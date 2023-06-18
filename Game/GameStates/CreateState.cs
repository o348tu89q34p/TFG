using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Gui;
using Domain;

namespace Game;

class CreateState : GameState
{
    private Sprite Background { get; }

    private Label StateTitle { get; }
    private RegularButton PlayButton { get; }
    private RegularButton CancelButton { get; }
    private Label Message { get; set; }
    private StateManager GSManager { get; set; }

    private NewNextOption<Nextable<string>, string> OptDeck { get; }
    private NewNextOption<Nextable<int>, int> OptNumPlayers { get; }
    private NewNextOption<Nextable<int>, int> OptNumDecks { get; }
    private NewNextOption<Nextable<int>, int> OptNumWilds { get; }
    private NewNextOption<Nextable<int>, int> OptNumCards { get; }
    private NewNextOption<Nextable<bool>, bool> OptMultWc { get; }
    private NewNextOption<Nextable<bool>, bool> OptConsecWc { get; }
    private NewNextOption<Nextable<bool>, bool> OptCanWrap { get; }
    private NewNextOption<Nextable<bool>, bool> OptNeedsOut { get; }
    private NewNextOption<Nextable<bool>, bool> OptEndDiscard { get; }

    public CreateState(StateManager gsManager, RenderWindow window) {
        this.GSManager = gsManager;

        this.Background = new Sprite(TextureUtils.BackgroundTexture);

        float tipWidth = 600.0f;
        float xLeft = window.Size.X/4.0f - tipWidth/2.0f;
        float xRight = window.Size.X/2.0f + window.Size.X/4.0f - tipWidth/2.0f;
        float lTop = 150.0f;
        float rTop = 150.0f;
        float space = 70.0f;

        Vector2f dims = new Vector2f(tipWidth, 40.0f);
        var optDeck = new NextableStrings(new string[]{"Spanish", "French"}, 0);
        this.OptDeck = new NewNextOption<Nextable<string>, string>("Deck type", optDeck, new Vector2f(xLeft, lTop + space), dims);
        lTop += space;

        var optNumPlayers = new NextableInt(2, 8, 4);
        this.OptNumPlayers = new NewNextOption<Nextable<int>, int>("Number of players", optNumPlayers, new Vector2f(xLeft, lTop + space), dims);
        lTop += space;

        var optNumDecks = new NextableInt(1, 4, 2);
        this.OptNumDecks = new NewNextOption<Nextable<int>, int>("Number of decks", optNumDecks, new Vector2f(xLeft, lTop + space), dims);
        lTop += space;

        var optNumWilds = new NextableInt(0, 10, 4);
        this.OptNumWilds = new NewNextOption<Nextable<int>, int>("Number of wild cards", optNumWilds, new Vector2f(xLeft, lTop + space), dims);
        lTop += space;

        var optNumCards = new NextableInt(1, 109, 10);
        this.OptNumCards = new NewNextOption<Nextable<int>, int>("Cards per player", optNumCards, new Vector2f(xLeft, lTop + space), dims);
        lTop += space;

        var optCanWrap = new NextableBool(false);
        this.OptCanWrap = new NewNextOption<Nextable<bool>, bool>("Open ended runs", optCanWrap, new Vector2f(xRight, rTop + space), dims);
        rTop += space;

        var optMultWc = new NextableBool(true);
        this.OptMultWc = new NewNextOption<Nextable<bool>, bool>("Multiple wild cards per meld", optMultWc, new Vector2f(xRight, rTop + space), dims);
        rTop += space;

        var optConsecWc = new NextableBool(true);
        this.OptConsecWc = new NewNextOption<Nextable<bool>, bool>("Consecutive wild cards in melds", optConsecWc, new Vector2f(xRight, rTop + space), dims);
        rTop += space;

        var optNeedsOut = new NextableBool(false);
        this.OptNeedsOut = new NewNextOption<Nextable<bool>, bool>("Needs coming out", optNeedsOut, new Vector2f(xRight, rTop + space), dims);
        rTop += space;

        var optEndDiscard = new NextableBool(false);
        this.OptEndDiscard = new NewNextOption<Nextable<bool>, bool>("Last card must be discarded", optEndDiscard, new Vector2f(xRight, rTop + space), dims);
        rTop += space;

        // Title.
        Font titleFont = new Font(FontUtils.TitleFont);
        if (titleFont == null) {
            throw new Exception($"Failed to load font: {titleFont}");
        }
        Vector2f titlePos = new Vector2f(window.Size.X / 2, 100);
        Color titleColor = new Color(225, 100, 20);

        this.StateTitle = new Label("Set the rules", titlePos, Origin.CENTER, 60, titleFont, titleColor, titleColor, titleColor);

        Vector2f playPos = new Vector2f(window.Size.X - TextureUtils.IdleTexture.Size.X - 20.0f,
                                        window.Size.Y - TextureUtils.IdleTexture.Size.Y - 20.0f);
        this.PlayButton = new RegularButton(TextureUtils.IdleTexture, TextureUtils.HoverTexture, "play", playPos, 30);

        Vector2f cancelPos = new Vector2f(20.0f, window.Size.Y - TextureUtils.RegularTexture.Size.Y - 20.0f);
        this.CancelButton = new RegularButton(TextureUtils.RegularTexture, TextureUtils.RegularHoverTexture, "cancel", cancelPos, 20);

        // Message.
        Color messageColor = new Color(200, 50, 0);
        this.Message = new Label("", new Vector2f(window.Size.X/2, 100), Origin.CENTER, 15, FontUtils.StatusFont, messageColor, messageColor, messageColor);

        this.BindEvents(window);
    }

    private Label NewMessage(RenderWindow window, string newMessage) {
        Color messageColor = new Color(255, 0, 0);
        return new Label(newMessage, new Vector2f(window.Size.X/2, window.Size.Y - 150.0f), Origin.CENTER, 30, FontUtils.StatusFont, messageColor, messageColor, messageColor);
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
            if (this.CancelButton.IsHovered) {
                this.GSManager.ChangeState(window, new MenuState(this.GSManager, window));
            }
            if (this.PlayButton.IsHovered) {
                try {
                    if (this.OptDeck.Setting.GetIt().CompareTo("Spanish") == 0) {
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
                    } else if (this.OptDeck.Setting.GetIt().CompareTo("French") == 0) {
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
                    }
                } catch (Exception ex) {
                    this.Message = this.NewMessage(window, ex.Message);
                }
            }
            this.OptDeck.ChangeValue(window);
            this.OptNumPlayers.ChangeValue(window);
            this.OptNumDecks.ChangeValue(window);
            this.OptNumWilds.ChangeValue(window);
            this.OptNumCards.ChangeValue(window);
            this.OptMultWc.ChangeValue(window);
            this.OptConsecWc.ChangeValue(window);
            this.OptCanWrap.ChangeValue(window);
            this.OptNeedsOut.ChangeValue(window);
            this.OptEndDiscard.ChangeValue(window);
        }
    }

    public override void Update(RenderWindow window) {
        this.StateTitle.Update(window);
        this.PlayButton.Update(window);
        this.CancelButton.Update(window);

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
        window.Draw(this.Background);

        this.StateTitle.Render(window);
        this.PlayButton.Render(window);
        this.CancelButton.Render(window);

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
