using SFML.System;
using SFML.Graphics;
using SFML.Window;
using SelfDrivingCar;

//Init
RenderWindow window = new RenderWindow(new VideoMode(800, 600), "My window");
Inputs.Window = window;
View view = new View() { Center = new Vector2f(0,0), Size = (Vector2f)window.Size};
GameTime.StartClock();
GameTime.SetFrameRate(144);
GameTime.SetUpdateRate(200);
int fps = 0;
int ups = 0;

RoadHandler roadHandler = new RoadHandler();
Car[] cars = new Car[1];
cars[0] = new Car(new Vector2f(0,0), Car.CarType.AI);

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

        //Update view
        view.Center = cars[0].Position;
        if (Inputs.IsClicked(Keyboard.Key.C))
        {
            view.Size = GameMath.ScaleVector(view.Size, new Vector2f(2, 2));
        }
        if (Inputs.IsClicked(Keyboard.Key.V))
        {
            view.Size = GameMath.ScaleVector(view.Size, new Vector2f(2, 2),true);
        }

        //Update roads
        roadHandler.Update(cars[0]);

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
        roadHandler.Draw(window);
        foreach (Car car in cars) { car.Draw(window); }
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
        Console.WriteLine($"[CAR POS]{cars[0].Position}                         ");
        fps = 0;
        ups = 0;
    }
}

