using SFML.Graphics;
using SFML.Window;
using SFML.System;

using System;

using System.Threading;

using Game;
using Domain;
using Gui;

using GameObjects;
using Animation;

namespace Game;

public enum Step {
    START,
    BOT_PICK,
    BOT_PLAY,
    BOT_DISC,
    BOT_END,
    HUM_PICK,
    HUM_PLAY,
    HUM_RUN,
    HUM_SET,
    HUM_LAYOFF,
    HUM_REPLACE,
    HUM_DISC,
    END
}

public enum PileType {
    STOCK,
    DISCARD,
    NONE
}

class MatchState<T, U> : GameState where T : Scale, new() where U : Scale, new()
{
    private StateManager GSManager;
    private GameState<T, U> GameData { get; }

    private Sprite Background { get; }

    private ActionButtons Buttons { get; }
    private GraphicHand Hand { get; }
    private OpponentsInfo Opponents { get; }
    private StatusBar Bar { get; }
    private GraphicPiles Piles { get; }
    private GraphicMelds Melds { get; }

    // State.
    private Texture CardFaces { get; }
    private Texture CardBack { get; }
    private Texture WildFace { get; }

    private string PrevError { get; set; }
    private int Pointer { get; set; }
    private Step Moment { get; set; }
    private bool IsAnimating { get; set; }
    private ResultMove<int> MoveAsked { get; set; }
    private ResultMove<ICard<T, U>>? MoveAnswered { get; set; }
    private string NewStatus { get; set; }
    private IAnimation2? Animation { get; set; }

    // UI Buttons.
    private DoubleImageButton SortRun { get; }
    private DoubleImageButton SortSet { get; }
    private RegularButton QuitButton { get; }

    // Splash screens.
    private StartScreen? Start { get; set; }
    private EndScreen? Ending { get; set; }

    public MatchState(StateManager gsManager, RenderWindow window, Rules<T, U> rules) {
        // Graphic hand.
        switch (rules.Kind) {
            case DeckType.SPANISH_DECK:
                this.CardFaces = TextureUtils.SpanishFaceTexture;
                this.CardBack = TextureUtils.SpanishBackTexture;
                this.WildFace = TextureUtils.SpanishJokerTexture;
                break;
            case DeckType.FRENCH_DECK:
                this.CardFaces = TextureUtils.FrenchFaceTexture;
                this.CardBack = TextureUtils.FrenchBackTexture;
                this.WildFace = TextureUtils.FrenchJokerTexture;
                break;
            default:
                throw new Exception("Invalid deck kind.");
        }

        // Game data.
        this.GameData = new GameState<T, U>(rules);
        // End game data.

        // Action buttons.
        this.Buttons = new ActionButtons(window, this.GenerateActions(rules));
        this.GSManager = gsManager;
        // End action buttons.

        // Background.
        this.Background = new Sprite(TextureUtils.BackgroundTexture);
        // End background.

        // Graphic hand.
        //this.Hand = new GraphicHand(window, this.CardsToSprites(GameData.HumanHand()), TextureUtils.CardWidth, TextureUtils.CardHeight);
        this.Hand = new GraphicHand(window, this.CardsToSprites(this.GameData.HumanHand()));
        // End graphic hand.

        // Opponents.
        this.Opponents = new OpponentsInfo(window, this.CardBack, this.GameData.GetOpponents());
        // End opponents.

        // Status bar.
        this.Bar = new StatusBar(window, "Welcome to Remigio Challenge");
        // End status bar.

        // Piles.
        this.Piles = new GraphicPiles(window, (this.CardBack, TextureUtils.EmptyTexture));
        this.UpdatePiles();
        // End piles.

        // Melds.
        this.Melds = new GraphicMelds(window);
        // End melds.

        this.PrevError = "";

        if (this.GameData.HumanPos() == 0) {
            this.Pointer = this.GameData.NumPlayers() - 2;
        } else {
            this.Pointer = (this.GameData.GetOpponents().Count - this.GameData.HumanPos())%(this.GameData.NumPlayers() - 1);
        }

        this.Moment = Step.START;
        this.IsAnimating = false;

        this.MoveAsked = new ResultMove<int>(MoveKind.EMPTY, new List<int>(), null, null);
        this.MoveAnswered = null;

        this.NewStatus = "";
        this.Animation = null;

        this.Start = new StartScreen(window, this.GameData.CurrentProfile().Name);

        // Quit button.
        float bottom = window.Size.Y - TextureUtils.SquareHoverTexture.Size.Y - 5.0f;
        float leftLeft = 60.0f; //- TextureUtils.SquareHoverTexture.Size.Y - 5.0f;
        float leftRigth = leftLeft + 40.0f;
        this.SortRun = new DoubleImageButton(TextureUtils.SquareTexture,
                                             TextureUtils.SquareHoverTexture,
                                             TextureUtils.SortRunTexture,
                                             new Vector2f(leftLeft, bottom));
        this.SortSet = new DoubleImageButton(TextureUtils.SquareTexture,
                                             TextureUtils.SquareHoverTexture,
                                             TextureUtils.SortSetTexture,
                                             new Vector2f(leftRigth, bottom));
        this.QuitButton = new RegularButton(TextureUtils.SquareTexture, TextureUtils.SquareHoverTexture, "abandon", new Vector2f(10.0f, 10.0f));

        window.SetMouseCursorVisible(true);

        this.BindEvents(window);
    }

    private List<(string, PlayerAction, Step)> GenerateActions(Rules<T, U> rules) {
        var actions = new List<(string, PlayerAction, Step)>();
        actions.Add(("Make run", PlayerAction.MeldRun, Step.HUM_RUN));
        actions.Add(("Make set", PlayerAction.MeldSet, Step.HUM_SET));
        actions.Add(("Lay off", PlayerAction.LayOff, Step.HUM_LAYOFF));
        if (rules.NumWc > 0) {
            actions.Add(("Replace", PlayerAction.Replace, Step.HUM_REPLACE));
        }
        actions.Add(("Discard", PlayerAction.Discard, Step.HUM_DISC));

        return actions;
    }

    private Sprite CardToSprite(ICard<T, U> c) {
        int cardWidth = TextureUtils.CardWidth;
        int cardHeight = TextureUtils.CardHeight;

        if (c.IsNatural()) {
            (int suit, int rank) = c.Coords();
            return new Sprite(this.CardFaces, new IntRect(cardWidth*rank, cardHeight*suit, cardWidth, cardHeight));
        } else if (c.IsWild()) {
            return new Sprite(this.WildFace);
        } else {
            throw new Exception("Undefined type of card.");
        }
    }

    private List<Sprite> CardsToSprites(List<ICard<T, U>> hand) {
        List<Sprite> sprites = new List<Sprite>(hand.Count);
        for (int i = 0; i < hand.Count; i++) {
            ICard<T, U> c = hand.ElementAt(i);
            sprites.Add(this.CardToSprite(c));
        }

        return sprites;
    }

    private List<List<Sprite>> AllMeldSprites() {
        List<List<ICard<T, U>>> cards = this.GameData.CurrentMelds();
        List<List<Sprite>> res = new List<List<Sprite>>(cards.Count);

        foreach (List<ICard<T, U>> meld in cards) {
            res.Add(this.CardsToSprites(meld));
        }

        return res;
    }

    private void ShowSortRank(RenderWindow window) {
        if (this.SortRun.IsHovered && this.Animation == null) {
            this.GameData.HumanSortRun();
            this.UpdateHand();
        }
    }

    private void ShowSortSuit(RenderWindow window) {
        if (this.SortSet.IsHovered && this.Animation == null) {
            this.GameData.HumanSortSet();
            this.UpdateHand();
        }
    }

    // The events that can occur on a main menu.
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
            if (this.QuitButton.IsHovered) {
                this.GSManager.ChangeState(window, new MenuState(this.GSManager, window));
            }
        }
        bool cancel = false;
        bool press = false;
        switch (this.Moment) {
            case Step.START:
                if (this.GameData.IsHumanTurn()) {
                    this.Moment = Step.HUM_PICK;
                } else {
                    this.Moment = Step.BOT_PICK;
                }
                break;
            case Step.HUM_PICK:
                this.ShowSortRank(window);
                this.ShowSortSuit(window);
                this.MoveAsked = this.Piles.PileClicked();
                break;
            case Step.HUM_PLAY:
                this.ShowSortRank(window);
                this.ShowSortSuit(window);
                (cancel, this.Moment) = this.Buttons.OnMouseButtonPress(sender, e);
                this.PrevError = "";
                break;
            case Step.HUM_RUN:
                (List<int>? lstRun, press) = this.Hand.ReadToHover();
                if (lstRun != null) {
                    this.MoveAsked = new ResultMove<int>(MoveKind.RUN, lstRun, null, null);
                }
                (cancel, this.Moment) = this.Buttons.OnMouseButtonPress(sender, e);
                break;
            case Step.HUM_SET:
                (List<int>? lstSet, press) = this.Hand.ReadToHover();
                if (lstSet != null) {
                    this.MoveAsked = new ResultMove<int>(MoveKind.SET, lstSet, null, null);
                }
                (cancel, this.Moment) = this.Buttons.OnMouseButtonPress(sender, e);
                break;
            case Step.HUM_LAYOFF:
                (List<int>? lstLay, press) = this.Hand.ReadToHover();
                if (!press) {
                    this.Melds.ToggleMelds(this.Moment);
                }
                (int? nMeld, _) = this.Melds.GetSelected();
                if (lstLay != null && nMeld != null) {
                    this.MoveAsked = new ResultMove<int>(MoveKind.LAY_OFF, lstLay, nMeld, null);
                }
                (cancel, this.Moment) = this.Buttons.OnMouseButtonPress(sender, e);
                break;
            case Step.HUM_REPLACE:
                (List<int>? lstReplace, press) = this.Hand.ReadToHover();
                if (!press) {
                    this.Melds.ToggleMelds(this.Moment);
                }
                (int? nMeldRep, int? nCard) = this.Melds.GetSelected();
                if (lstReplace != null && nMeldRep != null && nCard != null) {
                    this.MoveAsked = new ResultMove<int>(MoveKind.REPLACE, lstReplace, nMeldRep, nCard);
                }
                (cancel, this.Moment) = this.Buttons.OnMouseButtonPress(sender, e);
                break;
            case Step.HUM_DISC:
                (cancel, this.Moment) = this.Buttons.OnMouseButtonPress(sender, e);
                List<int>? lstDisc = this.Hand.ReadShed();
                if (lstDisc != null) {
                    this.MoveAsked = new ResultMove<int>(MoveKind.SHED, lstDisc, null, null);
                }
                break;
            case Step.END:
                if (this.Ending != null) {
                    if (this.Ending.Close()) {
                        this.GSManager.ChangeState(window, new MenuState(this.GSManager, window));
                    }
                    this.Ending.ToggleTop();
                }
                break;
        }
        if (cancel) {
            this.Hand.ResetHand();
        }
    }

    private bool EmptyMove() {
        return this.MoveAsked.Move == MoveKind.EMPTY;
    }

    private void EmptyAskedMove() {
        this.MoveAsked = new ResultMove<int>(MoveKind.EMPTY, new List<int>(), null, null);
    }

    private Sprite SpritePicked() {
        if (this.MoveAnswered == null) {
            throw new Exception("Answer is null");
        }
        switch (this.MoveAnswered.Move) {
            case MoveKind.STOCK:
                return new Sprite(this.CardBack);
            case MoveKind.DISCARD:
                return this.CardToSprite(this.MoveAnswered.CardsMoved.ElementAt(0));
            default:
                throw new Exception("Invalid answer type.");
        }
    }

    private void UpdatePiles() {
        (bool notEmpty, ICard<T, U>? topDiscard) = this.GameData.PilesStatus();
        Sprite? sprite = null;

        if (topDiscard != null) {
            sprite = this.CardToSprite(topDiscard);
        }

        this.Piles.UpdateChanges(notEmpty, sprite);
    }

    private void UpdateHand() {
        this.Hand.UpdateHand(this.CardsToSprites(this.GameData.HumanHand()));
    }

    private void BotPick() {
        this.MoveAnswered = this.GameData.BotPick();
        Vector2f start;
        switch (this.MoveAnswered.Move) {
            case MoveKind.STOCK:
                start = this.Piles.StockPos();
                break;
            case MoveKind.DISCARD:
                start = this.Piles.DiscardPos();
                break;
            default:
                throw new Exception("Invalid pick by a bot.");
        }
        Sprite sprite = this.SpritePicked();
        Vector2f end = this.Opponents.GetCardPosition(this.Pointer);
        this.Animation = new SlideAnimation(sprite, start, end, 0678.0f); // SPEED

        this.UpdatePiles();
        this.Moment = Step.BOT_PLAY;
        this.MoveAnswered = null;
    }

    private void BotPlay() {
        this.Opponents.UpdateInfo(this.Pointer, this.GameData.CurrentProfile());

        ResultMove<ICard<T, U>>? play = this.GameData.BotPlay();

        if (play == null) {
            this.Moment = Step.BOT_DISC;
            return;
        }

        List<Sprite> ls = this.CardsToSprites(play.CardsMoved);
        switch (play.Move) {
            case MoveKind.RUN:
                // we overwrite the count right here
                // add an animation
                this.Opponents.UpdateInfo(this.Pointer, this.GameData.CurrentProfile());
                //this.Melds.AddMeld(ls);
                break;
            default:
                throw new Exception("Action not implemented in match state.");
        }
        this.Melds.UpdateMelds(this.AllMeldSprites());
        Thread.Sleep(1000);
    }

    private void BotDisc() {
        this.MoveAnswered = this.GameData.BotDiscard();
        if (this.MoveAnswered.Move != MoveKind.SHED) {
            throw new Exception("The bot must discard.");
        }
        Sprite sprite = this.CardToSprite(this.MoveAnswered.CardsMoved.ElementAt(0));
        Vector2f start = this.Opponents.GetCardPosition(this.Pointer);
        Vector2f end = this.Piles.DiscardPos();
        this.Animation = new SlideAnimation(sprite, start, end, 0678.0f);

        this.Opponents.UpdateInfo(this.Pointer, this.GameData.CurrentProfile());
        this.Moment = Step.BOT_END;
        this.MoveAnswered = null;
    }

    private void BotEnd() {
        this.UpdatePiles();
        this.ComputeNextTurn();
        this.GameData.FlipDiscard();
        this.UpdatePiles();
    }

    private void HumanPick() {
        this.Piles.Highlight();
        this.NewStatus = "Pick a card.";
        if ((this.MoveAsked.Move == MoveKind.STOCK) || (this.MoveAsked.Move == MoveKind.DISCARD)) {
            (string? err, this.MoveAnswered) = this.GameData.HumanPlay(this.MoveAsked);
            if (err != null) {
                this.PrevError = err;
                this.EmptyAskedMove();;
                return;
            } else if (this.MoveAnswered == null) {
                throw new Exception("Error while picking a card.");
            }
            Vector2f start;
            switch (this.MoveAnswered.Move) {
                case MoveKind.STOCK:
                    start = this.Piles.StockPos();
                    break;
                case MoveKind.DISCARD:
                    start = this.Piles.DiscardPos();
                    break;
                default:
                    throw new Exception("Invalid value for the move type.");
            }
            Vector2f end = this.Hand.GetInsertPoint();
            Sprite sprite = this.SpritePicked();
            this.Animation = new SlideAnimation(sprite, start, end, 0678.0f);
            this.UpdatePiles();
            this.Moment = Step.HUM_PLAY;
            this.Piles.Lowlight();
            this.EmptyAskedMove();
        } else if (this.MoveAsked.Move == MoveKind.EMPTY) {
            return;
        } else {
            throw new Exception("Invalid move during the pick phase.");
        }
    }

    private void HumanPlay() {
        if (this.MoveAnswered != null) {
            //this.Hand.Add(this.CardToSprite(this.MoveAnswered.CardsMoved.ElementAt(0)));
            this.MoveAnswered = null;
            // maybe not
            this.PrevError = "";
            this.UpdateHand();
        }
        this.NewStatus = "Make a play or shed a card.";
        this.EmptyAskedMove();
        //this.Melds.Deselect();
    }

    private void HumanMeld() {
        if (this.Moment == Step.HUM_RUN) {
            this.NewStatus = "Select the cards that form the run in ascending order.";
        } else if (this.Moment == Step.HUM_SET) {
            this.NewStatus = "Select the cards that form the set.";
        }
        if (!this.EmptyMove()) {
            (string? err, this.MoveAnswered) = this.GameData.HumanPlay(this.MoveAsked);
            if (err != null) {
                this.PrevError = err;
                this.Hand.ResetHand();
            } else if (this.MoveAnswered == null) {
                throw new Exception("Wild error building a meld.");
            } else {
                List<Sprite> ls = this.CardsToSprites(this.MoveAnswered.CardsMoved);
                this.Melds.UpdateMelds(this.AllMeldSprites());
                this.UpdateHand();
            }
            this.EmptyAskedMove();
            this.MoveAnswered = null;
            this.NewStatus = "Make a play or shed a card.";
            this.Moment = Step.HUM_PLAY;
        }
    }

    private void HumanLay() {
        this.NewStatus = "Select the cards to lay off and the meld.";
        if (!this.EmptyMove()) {
            (string? err, this.MoveAnswered) = this.GameData.HumanPlay(this.MoveAsked);
            if (err != null) {
                this.PrevError = err;
                this.Hand.ResetHand();
            } else if (this.MoveAnswered == null) {
                throw new Exception("Wild error laying off the cards.");
            } else {
                List<Sprite> ls = this.CardsToSprites(this.MoveAnswered.CardsMoved);
                this.Melds.UpdateMelds(this.AllMeldSprites());
                this.UpdateHand();
            }
            this.MoveAnswered = null;
            this.EmptyAskedMove();
            this.NewStatus = "Make a play or shed a card.";
            this.Moment = Step.HUM_PLAY;
        }
    }

    private void HumanReplace() {
        this.NewStatus = "Select the wildcard you want an the card to replace it with.";
        if (!this.EmptyMove()) {
            (string? err, this.MoveAnswered) = this.GameData.HumanPlay(this.MoveAsked);
            if (err != null) {
                this.PrevError = err;
                this.Hand.ResetHand();
            } else if (this.MoveAnswered == null) {
                throw new Exception("Wild error while replacing a wildcard.");
            } else {
                List<Sprite> ls = this.CardsToSprites(this.MoveAnswered.CardsMoved);
                this.Melds.UpdateMelds(this.AllMeldSprites());
                this.UpdateHand();
            }
            this.MoveAnswered = null;
            this.EmptyAskedMove();
            this.NewStatus = "Make a play or shed a card.";
            this.Moment = Step.HUM_PLAY;
        }
    }

    private void HumanDiscard() {
        this.NewStatus = "Select the card to shed.";
        this.PrevError = "";

        if (!this.EmptyMove()) {
            (string? err, this.MoveAnswered) = this.GameData.HumanPlay(this.MoveAsked);
            if (err != null) {
                this.PrevError = err;
                return;
            } else if (this.MoveAnswered == null) {
                // This check removest the warning when reading the card for the sprite.
                throw new Exception("This should never happen.");
            }
            Sprite sprite = this.CardToSprite(this.MoveAnswered.CardsMoved.ElementAt(0));
            Vector2f start = this.Hand.GetDiscardPoint(this.MoveAsked.CardsMoved.ElementAt(0));
            Vector2f end = this.Piles.DiscardPos();
            this.Animation = new SlideAnimation(sprite, start, end, 0678.0f);

            this.UpdateHand();
            this.Moment = Step.BOT_END;
            this.EmptyAskedMove();
            this.MoveAnswered = null;
        }
    }

    private void ComputeNextTurn() {
        this.GameData.NextTurn();
        if (this.GameData.IsHumanTurn()) {
            this.Moment = Step.HUM_PICK;
        } else {
            this.Moment = Step.BOT_PICK;
            this.Pointer = (this.Pointer + 1)%(this.GameData.NumPlayers() - 1);
        }
    }

    private void ComputeNewBar(RenderWindow window) {
        if (this.GameData.IsHumanTurn()) {
            string message;
            if (this.PrevError.CompareTo("") == 0) {
                message = this.NewStatus;
            } else {
                message = this.PrevError + " " + this.NewStatus;
            }
            this.Bar.Update(window, message);
        } else {
            this.Bar.Update(window, this.GameData.TurnStatus());
        }
    }

    private List<(string, List<Sprite>)> EndData() {
        List<PlayerProfile> profiles = this.GameData.PlayerProfiles();
        List<List<ICard<T, U>>> cards = this.GameData.PlayerHands();

        List<(string, List<Sprite>)> res = new List<(string, List<Sprite>)>(profiles.Count);
        for (int i = 0; i < profiles.Count; i++) {
            cards[i].Sort((x, y) => x.CompareTo(y));
            res.Add((profiles[i].Name, this.CardsToSprites(cards[i])));
        }

        return res;
    }

    private void CheckWinner(RenderWindow window) {
        if (this.GameData.CurrentHasWon()) {
            string name = this.GameData.CurrentProfile().Name;
            //Console.WriteLine(name + " has won the game.");
            this.Moment = Step.END;
            this.Ending = new EndScreen(window, this.GameData.CurrentProfile().Name, this.EndData());
            this.NewStatus = "The game is over!";
        }
    }

    public override void Update(RenderWindow window) {
        if (this.Animation != null) {
            if (!this.Animation.RunAnimation()) {
                this.Animation = null;
            }
        } else {
            this.Buttons.Update(window, this.Moment);
            switch (this.Moment) {
                case Step.BOT_PICK:
                    this.BotPick();
                    break;
                case Step.BOT_PLAY:
                    this.BotPlay();
                    this.CheckWinner(window);
                    break;
                case Step.BOT_DISC:
                    this.BotDisc();
                    this.CheckWinner(window);
                    break;
                case Step.BOT_END:
                    this.BotEnd();
                    break;
                case Step.HUM_PICK:
                    this.HumanPick();
                    break;
                case Step.HUM_PLAY:
                    this.HumanPlay();
                    this.CheckWinner(window);
                    break;
                case Step.HUM_RUN:
                case Step.HUM_SET:
                    this.HumanMeld();
                    break;
                case Step.HUM_LAYOFF:
                    this.HumanLay();
                    break;
                case Step.HUM_REPLACE:
                    this.HumanReplace();
                    break;
                case Step.HUM_DISC:
                    this.HumanDiscard();
                    this.CheckWinner(window);
                    break;
                default:
                    // Undefined
                    break;
            }
        }

        if (this.Ending != null) {
            this.Ending.Update(window);
        }
        this.Melds.Update(window, this.Moment);
        this.Hand.Update(window, this.Moment);
        this.Piles.Update(window, this.Moment);
        this.ComputeNewBar(window);
        this.SortRun.Update(window);
        this.SortSet.Update(window);
        this.QuitButton.Update(window);
    }

    public override void Draw(RenderWindow window) {
        window.Draw(this.Background);
        this.Opponents.Render(window);
        this.Melds.Render(window);
        this.Hand.Render(window);
        this.Bar.Render(window);
        this.Piles.Render(window);
        this.Buttons.Render(window);

        if (this.Animation != null) {
            this.Animation.Render(window);
        }

        if (this.Moment == Step.HUM_PICK ||
            this.Moment == Step.HUM_PLAY) {
            this.SortRun.Render(window);
            this.SortSet.Render(window);
        }
        if (this.Moment == Step.START && this.Start != null) {
            this.Start.Render(window);
        }
        if (this.Moment == Step.END && this.Ending != null) {
            this.Ending.Render(window);
        }
        this.QuitButton.Render(window);
    }
}
