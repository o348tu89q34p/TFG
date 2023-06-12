using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Game;

namespace Gui;

public class NewNextOption<T1, T2> : GuiComponent where T1 : Nextable<T2>
{
    private RectangleShape Bg { get; }
    private Label Name { get; }
    private RegularButton Left { get; }
    private Label TheValue { get; set; }
    private RegularButton Right { get; }
    private Vector2f Pos { get; }
    private float RightSide { get; }
    public T1 Setting { get; set; }

    public NewNextOption(string name, T1 setting, Vector2f pos, Vector2f size) {
        this.Setting = setting;

        this.Bg = new RectangleShape(size) {
            Position = pos,
            FillColor = new Color(0, 200, 0, 200),
            OutlineColor = new Color(0, 120, 0, 200),
            OutlineThickness = 3.0f
        };

        this.Name = new Label(name + ":", pos, Origin.TOPLEFT, 20, FontUtils.StatusFont, Color.Black, Color.Black, Color.Black);
        float namePosX = pos.X + 10.0f;
        float namePosY = pos.Y + size.Y/2.0f - this.Name.GetBounds().Height/1.6f;
        this.Name.SetPosition(new Vector2f(namePosX, namePosY));

        float right = pos.X + size.X;
        this.RightSide = right;
        Texture leftIdle = new Texture(TextureUtils.NextButtonsImage, new IntRect(new Vector2i(0, 0), new Vector2i(20, 20)));
        Texture leftHover = new Texture(TextureUtils.NextButtonsImage, new IntRect(new Vector2i(0, 20), new Vector2i(20, 20)));
        Vector2f leftPos = new Vector2f(right - 40.0f - 100.0f, pos.Y + size.Y/2.0f - 10.0f);
        this.Left = new RegularButton(leftIdle, leftHover, "", leftPos, 10);

        Texture rightIdle = new Texture(TextureUtils.NextButtonsImage, new IntRect(new Vector2i(20, 0), new Vector2i(20, 20)));
        Texture rightHover = new Texture(TextureUtils.NextButtonsImage, new IntRect(new Vector2i(20, 20), new Vector2i(20, 20)));
        Vector2f rightPos = new Vector2f(right - 25.0f, pos.Y + size.Y/2.0f - 10.0f);
        this.Right = new RegularButton(rightIdle, rightHover, "", rightPos, 10);

        this.TheValue = this.UpdateTheValue();
    }

    private Label UpdateTheValue() {
        Label aux = new Label(this.Setting.ToString(), new Vector2f(0.0f, 0.0f), Origin.TOPLEFT, 22, FontUtils.StatusFont, Color.Black, Color.Black, Color.Black);
        Vector2f pos = new Vector2f(this.Left.GetPosition().X + (this.Right.GetPosition().X - this.Left.GetPosition().X + 10.0f)/2.0f - aux.GetBounds().Width/2.0f, this.Bg.Position.Y + this.Bg.GetGlobalBounds().Height/2.0f - aux.GetBounds().Height/1f);
        aux.SetPosition(pos);

        return aux;
    }

    public void ChangeValue(RenderWindow window) {
        Font font = new Font(FontUtils.TextFont1);
        if (font == null) {
            throw new Exception($"Failed to load font: {font}");
        }

        if (this.Left.IsHovered) {
            this.Setting.Prev();
            this.TheValue = this.UpdateTheValue();
        } else if (this.Right.IsHovered) {
            this.Setting.Next();
            this.TheValue = this.UpdateTheValue();
        }
    }

    public override FloatRect GetBounds() {
        // Make this be the way to get the size of the element by others.
        // random value
        return new FloatRect(new Vector2f(0.0f, 0.0f), new Vector2f(0.0f, 0.0f));
    }

    public override void Update(RenderWindow window) {
        this.Left.Update(window);
        this.Right.Update(window);
    }

    public override void Render(RenderTarget window) {
        window.Draw(this.Bg);
        this.Name.Render(window);
        this.Left.Render(window);
        this.TheValue.Render(window);
        this.Right.Render(window);
    }
}
