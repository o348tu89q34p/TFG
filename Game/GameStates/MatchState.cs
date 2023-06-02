using SFML.Graphics;
using SFML.Window;
using SFML.System;

using System;

using System.Threading;

using Game;
using Domain;
using Gui;

using GameObjects;

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
    HUM_DISC,
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

    private ActionButtons Buttons { get; }
    // Both sort buttons. Make them actions if they have to be disabled for the duration of other's turn. SEPARATED!!
    private GraphicHand Hand { get; }
    private OpponentsInfo Opponents { get; }
    private StatusBar Bar { get; }
    private GraphicPiles Piles { get; }
    // private Melds.
    private GraphicMelds Melds { get; }

    // State.
    private Texture CardFaces { get; }
    private Texture CardBack { get; }
    private Texture WildFace { get; }

    private string PrevError { get; set; }
    private int Pointer { get; set; }
    private Step Moment { get; set; }
    private bool IsAnimating { get; set; }
    private ResultMove<int> HumanMove { get; set; }
    private string NewStatus { get; set; }

    // Rework this.
    private Label AbandonButton { get; }

    public MatchState(StateManager gsManager, RenderWindow window, Rules<T, U> rules) {
        // Game data.
        this.GameData = new GameState<T, U>(rules);
        // End game data.

        // Action buttons.
        this.Buttons = new ActionButtons(window, this.GenerateActions());
        this.GSManager = gsManager;
        // End action buttons.

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

        this.HumanMove = new ResultMove<int>(MoveKind.EMPTY, new List<int>(), null, null);

        this.NewStatus = "";


        // Abandon button.
        Font buttonsFont = new Font(FontUtils.TextFont1);
        if (buttonsFont == null) {
            throw new Exception($"Failed to load font: {buttonsFont}");
        }
        Vector2f buttonPos = new Vector2f(0, 0);
        Color buttonColor = new Color(0, 70, 255);
        Color hoverColor = new Color(0, 200, 50);
        this.AbandonButton = new Label("Abandona", buttonPos, Origin.TOPLEFT, 30, buttonsFont, buttonColor, hoverColor, buttonColor);

        window.SetMouseCursorVisible(true);

        this.BindEvents(window);
    }

    private List<(string, PlayerAction)> GenerateActions() {
        var actions = new List<(string, PlayerAction)>();
        actions.Add(("Make run", PlayerAction.MeldRun));
        actions.Add(("Make set", PlayerAction.MeldSet));
        actions.Add(("Lay off", PlayerAction.LayOff));
        // if rules.NumWc > 0:
        actions.Add(("Replace", PlayerAction.Replace));
        actions.Add(("Discard", PlayerAction.Discard));

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

    public (Sprite, Sprite, Sprite) TopPiles() {
        Sprite pick;
        if (this.GameData.StockEmpty()) {
            pick = new Sprite(TextureUtils.EmptyTexture);
        } else {
            pick = new Sprite(TextureUtils.FrenchBackTexture);
        }

        Sprite disc;
        ICard<T, U>? c = this.GameData.TopDiscard();
        if (c == null) {
            disc = new Sprite(TextureUtils.EmptyTexture);
        } else {
            disc = this.CardToSprite(c);
        }

        return (pick, disc, new Sprite(TextureUtils.EmptyTexture));
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
            if (this.AbandonButton.MouseOn) {
                this.GSManager.ChangeState(window, new MenuState(this.GSManager, window));
            }
        }
        switch (this.Moment) {
            case Step.HUM_PICK:
                PileType pt = this.Piles.OnMouseButtonPress(sender, e);
                if (pt == PileType.STOCK) {
                    this.HumanMove = new ResultMove<int>(MoveKind.STOCK, new List<int>(), null, null);
                } else if (pt == PileType.DISCARD) {
                    this.HumanMove = new ResultMove<int>(MoveKind.SHED, new List<int>(), null, null);
                }
                break;
        }
    }

    public void BotPick() {
        int opponent = this.Pointer;
        Sprite sprite;
        Vector2f start;
        OpponentAnim animation = OpponentAnim.PICK;

        ResultMove<ICard<T, U>> play = this.GameData.BotPick();
        ICard<T, U> card = play.CardsMoved.ElementAt(0);
        // Get the profile after picking the card.
        PlayerProfile profile = this.GameData.CurrentProfile();
        switch (play.Move) {
            case MoveKind.STOCK:
                sprite = new Sprite(this.CardBack);
                start = this.Piles.StockPos();
                this.Piles.PopPick();
                break;
            case MoveKind.DISCARD:
                sprite = this.CardToSprite(card);
                start = this.Piles.DiscardPos();
                this.Piles.PopDisc();
                break;
            default:
                throw new Exception("Bad pick type.");
        }

        this.Opponents.StartAnimation(opponent, sprite, start, profile, animation);
        this.IsAnimating = true;
        this.Moment = Step.BOT_PLAY;
    }

    private void BotPlay() {
        ResultMove<ICard<T, U>>? play = this.GameData.BotPlay();

        if (play == null) {
            this.Moment = Step.BOT_DISC;
            return;
        }

        List<Sprite> ls = this.HumanSprites(play.CardsMoved);
        switch (play.Move) {
            case MoveKind.RUN:
                this.Opponents.UpdateInfo(this.Pointer, this.GameData.CurrentProfile());
                this.Melds.AddMeld(ls);
                break;
            default:
                throw new Exception("Action not implemented in match state.");
        }
        Thread.Sleep(3000);
    }

    private void BotDisc() {
        ResultMove<ICard<T, U>> play = this.GameData.BotDiscard();
        int opponent = this.Pointer;
        Sprite sprite = this.CardToSprite(play.CardsMoved.ElementAt(0));
        Vector2f end = this.Piles.DiscardPos();
        PlayerProfile profile = this.GameData.CurrentProfile();
        OpponentAnim animation = OpponentAnim.DROP;

        this.Opponents.StartAnimation(opponent, sprite, end, profile, animation);
        this.Piles.SetNextDisc(sprite);

        this.IsAnimating = true;
        this.Moment = Step.BOT_END;
    }

    private void BotEnd() {
        this.Piles.ChangeNextDisc();
        this.ComputeNextTurn();
    }

    private void HumanPick() {
        this.Piles.Highlight();
        switch (this.HumanMove.Move) {
            case MoveKind.STOCK:
                var (_, res) = this.GameData.HumanPlay(this.HumanMove);
                this.Hand.StartAnimation(res);
                this.NewStatus = "Make a play or shed a card.";
                this.IsAnimating = true;
                this.Moment = Step.HUM_PLAY;
                break;
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

    public override void Update(RenderWindow window) {
        if (this.IsAnimating) {
            /*
              Keep IsAnimating in and add an enum 
              Whenever PlayAnimation returns the ANIMATING value
              skip call return so the animation keeps going the next update call.
              Otherwise return another avlue like END_DISC to signal to update
              the discard pile, change the turn value and set IsAnimating to false.
              Actually:
              maybe something like this.
              this.IsAnimating = ((this.OpponentsAnimations(this.Pointer) ||
              (this.PlayerAnimations()));
            */
            this.IsAnimating =
                this.Opponents.PlayAnimation(this.Pointer);
        } else {
            switch (this.Moment) {
                case Step.BOT_PICK:
                    this.BotPick();
                    break;
                case Step.BOT_PLAY:
                    this.BotPlay();
                    break;
                case Step.BOT_DISC:
                    this.BotDisc();
                    break;
                case Step.BOT_END:
                    this.BotEnd();
                    break;
                case Step.HUM_PICK:
                    this.HumanPick();
                    break;
            }
            // Increase this.Pointer at the end of HUMAN turn.
        }

        /*
          if (this.Moment == Step.PLAY) {
          case Phase.OPPONENT:

          } else if (this.Moment == Step.DISPLAY) {
                
          } else {
          throw new Exception("Bad moment.");
          }
          string newStatus;
          switch (this.GameData.GamePhase) {
          case Phase.OPPONENT:
          newStatus = this.OpponentStatus();
          this.GameData.MakeMove();
          //this.Opponents.Update(window, this.GameData.CurrentProfile(), this.Pointer);
          Sleep(500);
          this.Pointer = (this.Pointer + 1)%this.Opponents.Count;
          break;
          case Phase.HUMAN_INIT:
          newStatus = this.PrevError + "Pick a card.";
          break;
          case Phase.HUMAN_INIT:
          newStatus = this.PrevError + "Pick a card.";
          }
          this.Bar.Update(window, this.PrevError, this.GameData.GetPhaseStatus());
        */
        this.Bar.Update(window, this.GameData.TurnStatus());
        this.AbandonButton.Update(window);
    }

    public override void Draw(RenderWindow window) {
        this.Buttons.Render(window);
        this.Hand.Render(window);
        this.Bar.Render(window);
        this.Piles.Render(window);
        this.Melds.Render(window);
        this.Opponents.Render(window);

        this.AbandonButton.Render(window);
    }
}
