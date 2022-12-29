using SFML.System;
using SFML.Graphics;
using SFML.Window;
using SelfDrivingCar;

//Init
RenderWindow window = new RenderWindow(new VideoMode(800, 600), "My window");
Inputs.Window = window;
Renderer.States = new RenderStates(new Texture("..\\..\\..\\..\\SpriteSheet.png"));
Renderer.Target = window;
Renderer.Camera = new Camera(new Vector2f(0,0), (Vector2f)window.Size);
GameTime.StartClock();
GameTime.SetFrameRate(144);
GameTime.SetUpdateRate(200);
int fps = 0;
int ups = 0;

Car[] cars = new Car[1];
cars[0] = new Car(new Vector2f(0, 0), Car.CarType.AI);

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

        //Update camera
        Renderer.Camera.Move_Up = Keyboard.IsKeyPressed(Keyboard.Key.Up);
        Renderer.Camera.Move_Left = Keyboard.IsKeyPressed(Keyboard.Key.Left);
        Renderer.Camera.Move_Down = Keyboard.IsKeyPressed(Keyboard.Key.Down);
        Renderer.Camera.Move_Right = Keyboard.IsKeyPressed(Keyboard.Key.Right);
        if (Inputs.IsClicked(Keyboard.Key.V))
            Renderer.Camera.Zoom(2);
        if (Inputs.IsClicked(Keyboard.Key.C))
            Renderer.Camera.Zoom(2, true);
        Renderer.Camera.UpdateCam();

        //Update cars
        for(int i = 0; i < cars.Length; i++)
        {
            if (cars[i].Type == Car.CarType.AI)
            {
                cars[i].Forwards = Keyboard.IsKeyPressed(Keyboard.Key.W);
                cars[i].Backwards = Keyboard.IsKeyPressed(Keyboard.Key.S);
                cars[i].Left = Keyboard.IsKeyPressed(Keyboard.Key.A);
                cars[i].Right = Keyboard.IsKeyPressed(Keyboard.Key.D);
            }

            cars[i].Update();
        }

        //Increment update counter
        ups++;

        //Reset accumulator
        GameTime.ResetUpdateAcc();
    }

    //Render
    if (GameTime.DeltaTimeF >= GameTime.FrameRate)
    {
        Renderer.LOAD_CAR_VERTICES(cars);
        Renderer.Draw();
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
        fps = 0;
        ups = 0;
    }
}

