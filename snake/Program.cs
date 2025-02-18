namespace Snake;

class Program
{
    static int screenWidth = 32;
    static int screenHeight = 16;
    static Random random = new Random();
    static int score = 5;
    static int gameOver = 0;
    static Pixel head = new Pixel();
    static List<int> bodyX = new List<int>();
    static List<int> bodyY = new List<int>();
    static int berryX = random.Next(0, screenWidth);
    static int berryY = random.Next(0, screenHeight);
    static string movement = "RIGHT";
    static DateTime lastUpdate = DateTime.Now;
    static string buttonPressed = "no";

    static void Main(string[] args)
    {
        Console.WindowHeight = screenHeight;
        Console.WindowWidth = screenWidth;
        head.xpos = screenWidth / 2;
        head.ypos = screenHeight / 2;
        head.color = ConsoleColor.Red;

        while (true)
        {
            Console.Clear();
            DrawBorders();
            DrawBerry();
            HandleCollisions();

            if (gameOver == 1)
            {
                break;
            }

            DrawSnake();
            UpdateSnakePosition();
            HandleInput();

            lastUpdate = DateTime.Now;
            buttonPressed = "no";
            WaitForNextFrame();
        }

        ShowGameOver();
    }

    static void DrawBorders()
    {
        for (int i = 0; i < screenWidth; i++)
        {
            Console.SetCursorPosition(i, 0);
            Console.Write("■");
            Console.SetCursorPosition(i, screenHeight - 1);
            Console.Write("■");
        }

        for (int i = 0; i < screenHeight; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write("■");
            Console.SetCursorPosition(screenWidth - 1, i);
            Console.Write("■");
        }
    }

    static void DrawBerry()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.SetCursorPosition(berryX, berryY);
        Console.Write("■");
    }

    static void HandleCollisions()
    {
        if (head.xpos == screenWidth - 1 || head.xpos == 0 || head.ypos == screenHeight - 1 || head.ypos == 0)
        {
            gameOver = 1;
        }

        for (int i = 0; i < bodyX.Count; i++)
        {
            if (bodyX[i] == head.xpos && bodyY[i] == head.ypos)
            {
                gameOver = 1;
            }
        }

        if (head.xpos == berryX && head.ypos == berryY)
        {
            score++;
            berryX = random.Next(1, screenWidth - 2);
            berryY = random.Next(1, screenHeight - 2);
        }
    }

    static void DrawSnake()
    {
        Console.ForegroundColor = head.color;
        Console.SetCursorPosition(head.xpos, head.ypos);
        Console.Write("■");

        for (int i = 0; i < bodyX.Count; i++)
        {
            Console.SetCursorPosition(bodyX[i], bodyY[i]);
            Console.Write("■");
        }
    }

    static void UpdateSnakePosition()
    {
        bodyX.Add(head.xpos);
        bodyY.Add(head.ypos);

        switch (movement)
        {
            case "UP":
                head.ypos--;
                break;
            case "DOWN":
                head.ypos++;
                break;
            case "LEFT":
                head.xpos--;
                break;
            case "RIGHT":
                head.xpos++;
                break;
        }

        if (bodyX.Count > score)
        {
            bodyX.RemoveAt(0);
            bodyY.RemoveAt(0);
        }
    }

    static void HandleInput()
    {
        while (true)
        {
            if (DateTime.Now.Subtract(lastUpdate).TotalMilliseconds > 500) { break; }
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key.Equals(ConsoleKey.UpArrow) && movement != "DOWN" && buttonPressed == "no")
                {
                    movement = "UP";
                    buttonPressed = "yes";
                }
                if (key.Key.Equals(ConsoleKey.DownArrow) && movement != "UP" && buttonPressed == "no")
                {
                    movement = "DOWN";
                    buttonPressed = "yes";
                }
                if (key.Key.Equals(ConsoleKey.LeftArrow) && movement != "RIGHT" && buttonPressed == "no")
                {
                    movement = "LEFT";
                    buttonPressed = "yes";
                }
                if (key.Key.Equals(ConsoleKey.RightArrow) && movement != "LEFT" && buttonPressed == "no")
                {
                    movement = "RIGHT";
                    buttonPressed = "yes";
                }
            }
        }
    }

    static void WaitForNextFrame()
    {
        Thread.Sleep(100);
    }

    static void ShowGameOver()
    {
        Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
        Console.WriteLine($"Game over, Score: {score}");
        Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 1);
    }

    class Pixel
    {
        public int xpos { get; set; }
        public int ypos { get; set; }
        public ConsoleColor color { get; set; }
    }
}
