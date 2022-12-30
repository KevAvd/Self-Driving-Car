using SFML.System;
using SFML.Graphics;
using SFML.Window;
using SelfDrivingCar;

//Init
RenderWindow window = new RenderWindow(new VideoMode(800, 600), "My window", Styles.Default, new ContextSettings() { AntialiasingLevel = 4});
Inputs.Window = window;
View view = new View() { Center = new Vector2f(0,0), Size = (Vector2f)window.Size};
GameTime.StartClock();
GameTime.SetFrameRate(144);
GameTime.SetUpdateRate(200);
int fps = 0;
int ups = 0;

//Init car handler
CarHandler carHndlr = new CarHandler();
carHndlr.Creat_AI_Car(new Vector2f(0, 0));
carHndlr.GenerateTraffic(8);

//Game loop
while (window.IsOpen)
{
    //Handle time
    GameTime.RestartClock();

    //Handle event
    window.DispatchEvents();

    //Update
    if (GameTime.DeltaTimeU >= GameTime.UpdateRate)
    {
        //Handle input
        Inputs.Update();

        //Close window (F4)
        if (Inputs.IsClicked(Keyboard.Key.F4)) { window.Close(); }

        //Update game
        carHndlr.Update();

        //Update view
        view.Center = carHndlr.Focused_AI.Position;
        if (Inputs.IsClicked(Keyboard.Key.C))
        {
            view.Size = GameMath.ScaleVector(view.Size, new Vector2f(2, 2));
        }
        if (Inputs.IsClicked(Keyboard.Key.V))
        {
            view.Size = GameMath.ScaleVector(view.Size, new Vector2f(2, 2),true);
        }

        //Increment update counter
        ups++;

        //Reset accumulator
        GameTime.ResetUpdateAcc();
    }

    //Render
    if (GameTime.DeltaTimeF >= GameTime.FrameRate)
    {
        window.Clear(new Color(100,100,100));
        window.SetView(view);
        carHndlr.Draw(window);
        window.Display();

        //Increment frame counter
        fps++;

        //Reset accumulator
        GameTime.ResetFrameAcc();
    }

    if(GameTime.Accumulator >= 1)
    {
        GameTime.ResetAccumulator();
        Console.SetCursorPosition(0, 0);
        Console.WriteLine($"[FRAME/SECOND] {fps}");
        Console.WriteLine($"[UPDATE/SECOND] {ups}");
        Console.WriteLine($"[CAR POS]{carHndlr.Focused_AI.Position}                         ");
        fps = 0;
        ups = 0;
    }
}


