using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Game;

public class GameLoop {
    private RenderWindow Window { get; }
    private StateManager GSManager { get; set; }

    public GameLoop(uint width, uint height, string name) {
        // Create the window.
        var mode = new VideoMode(width, height);
        this.Window = new RenderWindow(mode, name);
        this.Window.KeyPressed += Window_KeyPressed;
        this.Window.Closed += (s, a) => this.Window.Close();

        // Initial state for the game.
        this.GSManager = new StateManager(this.Window);
    }

    public void Run_MinimumTimeStep(int min_fps) {
        Clock clock = new Clock();
        Time timeSinceLastUpdate;
        Time timePerFrame = Time.FromSeconds(1.0F/min_fps);

        while (this.Window.IsOpen) {
            ProcessEvents();
            timeSinceLastUpdate = clock.Restart();

            while (timeSinceLastUpdate > timePerFrame) {
                timeSinceLastUpdate -= timePerFrame;
                Update(timePerFrame);
            }

            Update(timeSinceLastUpdate);
            Render();
        }
    }

    public void Run(int fps) {
        Run_MinimumTimeStep(fps);
    }

    private void ProcessEvents() {
        this.Window.DispatchEvents();
    }

    private void Update(Time deltaTime) {
        this.GSManager.Update(this.Window);
    }

    private void DebugBackground() {
        uint width = this.Window.Size.X;
        uint height = this.Window.Size.Y;

        float wl = 0.0f;
        float wc = width/2.0f;
        float wr = (float)width;
        float ht = 0.0f;
        float hc = height/2.0f;
        float hb = (float)height;

        var size = new Vector2f(wc, hc);
        var poses = new Vector2f[] {
            new Vector2f(wl, ht),
            new Vector2f(wc, ht),
            new Vector2f(wl, hc),
            new Vector2f(wc, hc),
        };
        byte start = 20;
        byte step = 10;

        for (byte i = 0; i < poses.Length; i++) {
            byte next = (byte)(start + step*i);
            var q = new RectangleShape(size) {
                Position = poses[i],
                FillColor = new Color(next, next, next)
            };
            this.Window.Draw(q);
        }
    }

    private void Render() {
        this.Window.Clear();
        //this.DebugBackground();
        this.GSManager.Draw(this.Window);
        this.Window.Display();
    }

    private void Window_KeyPressed(object? sender, KeyEventArgs e) {
    }
}
