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
RoadHandler roadHandler = new RoadHandler();
List<Car> cars = new List<Car>();
cars.Add(new Car(new Vector2f(0, 0), Car.CarType.AI));
cars.AddRange(GenerateTraffic(roadHandler,8));

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
        for(int i = 0; i < cars.Count; i++)
        {
            if (cars[i].Type == Car.CarType.AI)
            {
                cars[i].Forwards = Keyboard.IsKeyPressed(Keyboard.Key.W);
                cars[i].Backwards = Keyboard.IsKeyPressed(Keyboard.Key.S);
                cars[i].Left = Keyboard.IsKeyPressed(Keyboard.Key.A);
                cars[i].Right = Keyboard.IsKeyPressed(Keyboard.Key.D);
            }
            else
            {
                if (cars[i].Position.Y > roadHandler.Roads[0].Position.Y + roadHandler.Roads[0].Height/2 || cars[i].Position.Y < roadHandler.Roads[2].Position.Y)
                {
                    cars[i].Dead = true;

                    cars.AddRange(GenerateTraffic(roadHandler,1));
                }
            }

            cars[i].Update();

            if (cars[i].Position.X > 300 || cars[i].Position.X < -300) { cars[i].Dead = true; }
        }

        //Remove dead traffic
        for(int i = cars.Count - 1; i >= 0; i--) 
        {
            if (cars[i].Type != Car.CarType.AI && cars[i].Dead)
            {
                cars.Remove(cars[i]);
            }
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

Car[] GenerateTraffic(RoadHandler roadHandler, int nbr)
{
    List<Car> traffic = new List<Car>();
    nbr = Math.Clamp(nbr, 1, 12);
    Vector2f roadPos = roadHandler.Roads[2].Position;
    float carWidth = cars[0].Width;
    float carHeigth = cars[0].Height;

    //Contains all possible position for spawning a traffic car
    List<Vector2f> possiblePosition = new List<Vector2f>()
    {
        //First lane
        new Vector2f(-200,roadPos.Y+(carHeigth+20)*1),
        new Vector2f(-200,roadPos.Y+(carHeigth+20)*3),
        new Vector2f(-200,roadPos.Y+(carHeigth+20)*5),
        new Vector2f(-200,roadPos.Y+(carHeigth+20)*7),

        //Second lane
        new Vector2f(0000,roadPos.Y+(carHeigth+20)*1),
        new Vector2f(0000,roadPos.Y+(carHeigth+20)*3),
        new Vector2f(0000,roadPos.Y+(carHeigth+20)*5),
        new Vector2f(0000,roadPos.Y+(carHeigth+20)*7),

        //Third lane
        new Vector2f(0200,roadPos.Y+(carHeigth+20)*1),
        new Vector2f(0200,roadPos.Y+(carHeigth+20)*3),
        new Vector2f(0200,roadPos.Y+(carHeigth+20)*5),
        new Vector2f(0200,roadPos.Y+(carHeigth+20)*7)
    };

    for(int i = 0; i < nbr; i++)
    {
        int carType = GameMath.Rnd.Next(1, 5);
        int carPos = GameMath.Rnd.Next(0, possiblePosition.Count);
        traffic.Add(new Car(possiblePosition[carPos], (Car.CarType)carType));
        possiblePosition.Remove(possiblePosition[carPos]);
    }

    return traffic.ToArray();
}


