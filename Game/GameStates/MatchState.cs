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

/*
  put each state in a folder and their respective classes with them instead of
  having them all mixed up here and in gameobjects.
*/
namespace Game;

public enum Step {
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
    HUM_DISC
}

public enum OpponentAnim {
    PICK,
    MELD,
    DROP
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
    private GraphicHand Hand { get; set; }
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

    public MatchState(StateManager gsManager, RenderWindow window, Rules<T, U> rules) {
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
        this.Hand = new GraphicHand(window, this.HumanSprites(GameData.HumanHand()), TextureUtils.CardWidth, TextureUtils.CardHeight);
        // End graphic hand.

        // Opponents.
        this.Opponents = new OpponentsInfo(window, this.GameData.GetOpponents());
        // End opponents.

        // Status bar.
        this.Bar = new StatusBar(window, "Initial message");
        // End status bar.

        // Piles.
        this.Piles = new GraphicPiles(window, this.TopPiles(), this.GameData.StockDepth());
        // End piles.

        // Melds.
        this.Melds = new GraphicMelds(window);
        // End melds.

        this.PrevError = "";
        this.Pointer = this.GameData.NumPlayers() - 1 - this.GameData.HumanPos();
        if (this.GameData.IsHumanTurn()) {
            this.Moment = Step.HUM_PICK;
        } else {
            this.Moment = Step.BOT_PICK;
        }
        this.IsAnimating = false;

        this.MoveAsked = new ResultMove<int>(MoveKind.EMPTY, new List<int>(), null, null);
        this.MoveAnswered = null;

        this.NewStatus = "";
        this.Animation = null;


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
        this.QuitButton = new RegularButton(TextureUtils.SquareTexture, TextureUtils.SquareHoverTexture, "quit", new Vector2f(10.0f, 10.0f));

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

    private List<Sprite> HumanSprites(List<ICard<T, U>> hand) {
        List<Sprite> sprites = new List<Sprite>(hand.Count);
        for (int i = 0; i < hand.Count; i++) {
            ICard<T, U> c = hand.ElementAt(i);
            sprites.Add(this.CardToSprite(c));
        }

        return sprites;
    }

    public (Texture, Texture, Sprite?) TopPiles() {
        Sprite pick;
        if (this.GameData.StockEmpty()) {
            pick = new Sprite(TextureUtils.EmptyTexture);
        } else {
            pick = new Sprite(TextureUtils.FrenchBackTexture);
        }

        Sprite? disc = null;
        ICard<T, U>? c = this.GameData.TopDiscard();
        if (c != null) {
            disc = this.CardToSprite(c);
        }

        return (this.CardBack, TextureUtils.EmptyTexture, disc);
    }

    private void ResyncHand(RenderWindow window) {
        //this.Hand.RefreshHand(this.HumanSprites(this.GameData.HumanHand()));
        this.Hand = new GraphicHand(window, this.HumanSprites(GameData.HumanHand()), TextureUtils.CardWidth, TextureUtils.CardHeight);
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
        switch (this.Moment) {
            case Step.HUM_PICK:
                PileType pt = this.Piles.OnMouseButtonPress(sender, e);
                if (pt == PileType.STOCK && this.EmptyMove()) {
                    this.MoveAsked = new ResultMove<int>(MoveKind.STOCK, new List<int>(), null, null);
                } else if (pt == PileType.DISCARD && this.EmptyMove()) {
                    this.MoveAsked = new ResultMove<int>(MoveKind.DISCARD, new List<int>(), null, null);
                }
                break;
            case Step.HUM_PLAY:
                if (this.SortRun.IsHovered) {
                    this.GameData.HumanSortRun();
                    this.ResyncHand(window);
                }
                if (this.SortSet.IsHovered) {
                    this.GameData.HumanSortSet();
                    this.ResyncHand(window);
                }
                this.Moment = this.Buttons.OnMouseButtonPress(sender, e);
                this.PrevError = "";
                break;
            case Step.HUM_RUN:
                List<int>? lstRun = this.Hand.ReadToHover();
                if (lstRun != null) {
                    this.MoveAsked = new ResultMove<int>(MoveKind.RUN, lstRun, null, null);
                }
                this.Moment = this.Buttons.OnMouseButtonPress(sender, e);
                break;
            case Step.HUM_SET:
                List<int>? lstSet = this.Hand.ReadToHover();
                if (lstSet != null) {
                    this.MoveAsked = new ResultMove<int>(MoveKind.SET, lstSet, null, null);
                }
                this.Moment = this.Buttons.OnMouseButtonPress(sender, e);
                break;
            case Step.HUM_LAYOFF:
                this.Melds.ToggleMelds(this.Moment);
                (int? nMeld, _) = this.Melds.GetSelected();
                List<int>? lstLay = this.Hand.ReadToHover();
                if (lstLay != null && nMeld != null) {
                    this.MoveAsked = new ResultMove<int>(MoveKind.LAY_OFF, lstLay, nMeld, null);
                }
                this.Moment = this.Buttons.OnMouseButtonPress(sender, e);
                break;
            case Step.HUM_DISC:
                this.Moment = this.Buttons.OnMouseButtonPress(sender, e);
                this.MoveAsked = this.Hand.ReadCard(this.MoveAsked);
                break;
        }
    }

    private bool EmptyMove() {
        return this.MoveAsked.Move == MoveKind.EMPTY;
    }

    public void BotPick() {
        this.MoveAnswered = this.GameData.BotPick();
        PileType pt;
        switch (this.MoveAnswered.Move) {
            case MoveKind.STOCK:
                pt = PileType.STOCK;
                break;
            case MoveKind.DISCARD:
                pt = PileType.DISCARD;
                break;
            default:
                throw new Exception("Invalid pick by a bot.");
        }
        Sprite sprite = this.Piles.GetSprite(pt);
        Vector2f start = this.Piles.GetPilePoint(pt);
        Vector2f end = this.Opponents.GetCardPosition(this.Pointer);
        this.Animation = new SlideAnimation(sprite, start, end, 0678.0f);

        // this.Opponents.UpdateInfo(this.Pointer, this.GameData.CurrentProfile());
        this.Piles.PopCard(pt);
        this.Moment = Step.BOT_PLAY;
    }

    private void BotPlay() {
        this.Opponents.UpdateInfo(this.Pointer, this.GameData.CurrentProfile());

        ResultMove<ICard<T, U>>? play = this.GameData.BotPlay();

        if (play == null) {
            this.Moment = Step.BOT_DISC;
            return;
        }

        List<Sprite> ls = this.HumanSprites(play.CardsMoved);
        switch (play.Move) {
            case MoveKind.RUN:
                // we overwrite the count right here
                // add an animation
                this.Opponents.UpdateInfo(this.Pointer, this.GameData.CurrentProfile());
                this.Melds.AddMeld(ls);
                break;
            default:
                throw new Exception("Action not implemented in match state.");
        }
        Thread.Sleep(1000);
    }

    private void BotDisc() {
        this.MoveAnswered = this.GameData.BotDiscard();
        if (this.MoveAnswered.Move != MoveKind.SHED) {
            throw new Exception("The bot must discard.");
        }
        ICard<T, U> card = this.MoveAnswered.CardsMoved.ElementAt(0);
        Sprite sprite = this.CardToSprite(card);
        Vector2f start = this.Opponents.GetCardPosition(this.Pointer);
        Vector2f end = this.Piles.GetPilePoint(PileType.DISCARD);
        this.Animation = new SlideAnimation(sprite, start, end, 0678.0f);

        this.Opponents.UpdateInfo(this.Pointer, this.GameData.CurrentProfile());
        this.Moment = Step.BOT_END;
    }

    private void BotEnd() {
        if (this.MoveAnswered == null) {
            throw new Exception("Bot should have set this value.");
        }
        this.Piles.DiscardCard(this.CardToSprite(this.MoveAnswered.CardsMoved.ElementAt(0)));
        this.ComputeNextTurn();
        this.GameData.FilpDiscard();
        //this.MoveAnswered = null;
    }

    private void EmptyAskedMove() {
        this.MoveAsked = new ResultMove<int>(MoveKind.EMPTY, new List<int>(), null, null);
    }

    private void HumanPick() {
        this.Piles.Highlight();
        this.NewStatus = "Pick a card.";
        if ((this.MoveAsked.Move == MoveKind.STOCK) ||
            (this.MoveAsked.Move == MoveKind.DISCARD))
        {
            (string? err, this.MoveAnswered) = this.GameData.HumanPlay(this.MoveAsked);

            if (err != null) {
                this.PrevError = err;
                this.EmptyAskedMove();;
                return;
            } else if (this.MoveAnswered == null) {
                throw new Exception("Error while picking a card.");
            }
            PileType pt;
            switch (this.MoveAnswered.Move) {
                case MoveKind.STOCK:
                    pt = PileType.STOCK;
                    break;
                case MoveKind.DISCARD:
                    pt = PileType.DISCARD;
                    break;
                default:
                    throw new Exception("Invalid value for the move type.");
            }
            ICard<T, U> card = this.MoveAnswered.CardsMoved.ElementAt(0);
            Sprite sprite = this.Piles.GetSprite(pt);
            //Sprite sprite = this.CardToSprite(card);
            Vector2f start = this.Piles.GetPilePoint(pt);
            Vector2f end = this.Hand.GetInsertPoint();
            this.Animation = new SlideAnimation(sprite, start, end, 0678.0f);
            this.Piles.PopCard(pt);
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
            this.Hand.Add(this.CardToSprite(this.MoveAnswered.CardsMoved.ElementAt(0)));
            this.MoveAnswered = null;
            // maybe not
            this.PrevError = "";
        }
        this.NewStatus = "Make a play or shed a card.";
        this.EmptyAskedMove();
        //this.Melds.Deselect();
    }

    private void HumanMeld() {
        if (this.Moment == Step.HUM_RUN) {
            this.NewStatus = "Select the cards that form the run.";
        } else if (this.Moment == Step.HUM_SET) {
            this.NewStatus = "Select the cards that form the set.";
        }
        if (!this.EmptyMove()) {
            (string? err, this.MoveAnswered) = this.GameData.HumanPlay(this.MoveAsked);
            if (err != null) {
                this.PrevError = err;
            } else if (this.MoveAnswered == null) {
                throw new Exception("Wild error building a meld.");
            } else {
                List<Sprite> ls = this.HumanSprites(this.MoveAnswered.CardsMoved);
                this.Melds.AddMeld(ls);
                this.Hand.RemoveCards();
            }
            this.MoveAnswered = null;
            this.EmptyAskedMove();
            this.NewStatus = "Make a play or shed a card.";
            this.Moment = Step.HUM_PLAY;
        }
    }

    private void HumanLay() {
        this.NewStatus = "Select the cards to lay off and the meld.";
        /*
        if (!this.EmptyMove()) {
            (string? err, this.MoveAnswered) = this.GameData.HumanPlay(this.MoveAsked);
            if (err != null) {
                this.PrevError = err;
            } else if (this.MoveAnswered == null) {
                throw new Exception("Wild error laying off the cards.");
            } else {
                List<Sprite> ls = this.HumanSprites(this.MoveAnswered.CardsMoved);
                this.Melds.AddMeld(ls);
                this.Hand.RemoveCards();
            }
            this.MoveAnswered = null;
            this.EmptyAskedMove();
            this.NewStatus = "Make a play or shed a card.";
            this.Moment = Step.HUM_PLAY;
        }
        */
    }

    private void HumanDiscard() {
        this.NewStatus = "Select the card to shed.";
        this.PrevError = "";
        if (this.EmptyMove()) {
            this.MoveAsked = new ResultMove<int>(MoveKind.SHED, new List<int>(), null, null);
        }
        //this.MoveAsked = new ResultMove<int>(MoveKind.DISCARD, new List<int>(), null, null);
        if (this.MoveAsked.CardsMoved.Count == 1) {
            (string? err, this.MoveAnswered) = this.GameData.HumanPlay(this.MoveAsked);
            if (err != null) {
                this.PrevError = err;
                return;
            } else if (this.MoveAnswered == null) {
                throw new Exception("I don't know if this should be an excpetion");
            }
            ICard<T, U> card = this.MoveAnswered.CardsMoved.ElementAt(0);
            Sprite sprite = this.CardToSprite(card);
            Vector2f start = this.Hand.GetDiscardPoint(this.MoveAsked.CardsMoved.ElementAt(0));
            Vector2f end = this.Piles.GetPilePoint(PileType.DISCARD);
            this.Animation = new SlideAnimation(sprite, start, end, 0678.0f);
            this.Hand.Shed(this.MoveAsked.CardsMoved.ElementAt(0));
            this.EmptyAskedMove();
            this.Moment = Step.BOT_END;
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

    private void CheckWinner(RenderWindow window) {
        if (this.GameData.CurrentHasWon()) {
            string name = this.GameData.CurrentProfile().Name;
            Console.WriteLine(name + " has won the game.");
            this.GSManager.ChangeState(window, new MenuState(this.GSManager, window));
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
                    //Console.WriteLine("Bot pick");
                    this.BotPick();
                    break;
                case Step.BOT_PLAY:
                    //Console.WriteLine("Bot play");
                    this.BotPlay();
                    this.CheckWinner(window);
                    break;
                case Step.BOT_DISC:
                    //Console.WriteLine("Bot disc");
                    this.BotDisc();
                    this.CheckWinner(window);
                    break;
                case Step.BOT_END:
                    //Console.WriteLine("Bot end;");
                    this.BotEnd();
                    break;
                case Step.HUM_PICK:
                    //Console.WriteLine("Human pick");
                    this.HumanPick();
                    break;
                case Step.HUM_PLAY:
                    //Console.WriteLine("Human play");
                    this.Hand.ResetHand(window);
                    this.HumanPlay();
                    this.CheckWinner(window);
                    break;
                case Step.HUM_RUN:
                case Step.HUM_SET:
                    this.HumanMeld();
                    //Console.WriteLine("Running");
                    break;
                case Step.HUM_LAYOFF:
                    this.HumanLay();
                    break;
                case Step.HUM_DISC:
                    //Console.WriteLine("Human discard");
                    this.HumanDiscard();
                    this.CheckWinner(window);
                    break;
                default:
                    //Console.WriteLine("Undefined");
                    break;
            }
        }

        this.Melds.Update(window, this.Moment);
        this.Hand.Update(window, this.Moment);
        this.Piles.Update(window);
        this.ComputeNewBar(window);
        this.SortRun.Update(window);
        this.SortSet.Update(window);
        this.QuitButton.Update(window);
    }

    public override void Draw(RenderWindow window) {
        window.Draw(this.Background);
        this.Buttons.Render(window);
        this.Melds.Render(window);
        this.Hand.Render(window);
        this.Bar.Render(window);
        this.Piles.Render(window);
        this.Opponents.Render(window);

        if (this.Animation != null) {
            this.Animation.Render(window);
        }

        if (this.Moment == Step.HUM_PLAY) {
            this.SortRun.Render(window);
            this.SortSet.Render(window);
        }
        this.QuitButton.Render(window);
    }
}
