namespace Snake;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class Pixel
{
    public int X { get; set; }
    public int Y { get; set; }
    public ConsoleColor Color { get; init; }
}

public class SnakeGame
{
    private const int ScreenWidth = 32;
    private const int ScreenHeight = 16;
    private readonly Random _random = new();
    private int _score = 5;
    private bool _gameOver;
    private readonly Pixel _head;
    private readonly List<Pixel> _body = [];
    private readonly Pixel _berry;
    private Direction _movement = Direction.Right;
    private DateTime _lastUpdate = DateTime.Now;
    private bool _buttonPressed;

    public SnakeGame()
    {
        Console.WindowHeight = ScreenHeight;
        Console.WindowWidth = ScreenWidth;
        _head = new Pixel { X = ScreenWidth / 2, Y = ScreenHeight / 2, Color = ConsoleColor.Red };
        _berry = new Pixel
            { X = _random.Next(1, ScreenWidth - 1), Y = _random.Next(1, ScreenHeight - 1), Color = ConsoleColor.Cyan };
    }

    public void Run()
    {
        while (!_gameOver)
        {
            Console.Clear();
            DrawBorders();
            DrawBerry();
            HandleCollisions();

            if (_gameOver)
            {
                break;
            }

            DrawSnake();
            UpdateSnakePosition();
            HandleInput();

            _lastUpdate = DateTime.Now;
            _buttonPressed = false;
            WaitForNextFrame();
        }

        ShowGameOver();
    }

    private static void DrawBorders()
    {
        for (var i = 0; i < ScreenWidth; i++)
        {
            Console.SetCursorPosition(i, 0);
            DrawFruit();
            Console.SetCursorPosition(i, ScreenHeight - 1);
            DrawFruit();
        }

        for (var i = 0; i < ScreenHeight; i++)
        {
            Console.SetCursorPosition(0, i);
            DrawFruit();
            Console.SetCursorPosition(ScreenWidth - 1, i);
            DrawFruit();
        }
    }

    private static void DrawFruit()
    {
        Console.Write("■");
    }

    private void DrawBerry()
    {
        Console.ForegroundColor = _berry.Color;
        Console.SetCursorPosition(_berry.X, _berry.Y);
        DrawFruit();
    }

    private void HandleCollisions()
    {
        if (_head.X == 0 || _head.X == ScreenWidth - 1 || _head.Y == 0 || _head.Y == ScreenHeight - 1)
        {
            _gameOver = true;
        }

        foreach (var unused in _body.Where(part => part.X == _head.X && part.Y == _head.Y))
        {
            _gameOver = true;
        }

        if (_head.X != _berry.X || _head.Y != _berry.Y) return;
        _score++;
        _berry.X = _random.Next(1, ScreenWidth - 1);
        _berry.Y = _random.Next(1, ScreenHeight - 1);
    }

    private void DrawSnake()
    {
        Console.ForegroundColor = _head.Color;
        Console.SetCursorPosition(_head.X, _head.Y);
        DrawFruit();

        foreach (var part in _body)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(part.X, part.Y);
            DrawFruit();
        }
    }

    private void UpdateSnakePosition()
    {
        _body.Add(new Pixel { X = _head.X, Y = _head.Y, Color = ConsoleColor.Green });

        switch (_movement)
        {
            case Direction.Up:
                _head.Y--;
                break;
            case Direction.Down:
                _head.Y++;
                break;
            case Direction.Left:
                _head.X--;
                break;
            case Direction.Right:
                _head.X++;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (_body.Count > _score)
        {
            _body.RemoveAt(0);
        }
    }

    private void HandleInput()
    {
        while (true)
        {
            if (DateTime.Now.Subtract(_lastUpdate).TotalMilliseconds > 500) break;
            if (!Console.KeyAvailable) continue;
            var key = Console.ReadKey(true);

            _movement = key.Key switch
            {
                ConsoleKey.UpArrow when _movement != Direction.Down && !_buttonPressed => Direction.Up,
                ConsoleKey.DownArrow when _movement != Direction.Up && !_buttonPressed => Direction.Down,
                ConsoleKey.LeftArrow when _movement != Direction.Right && !_buttonPressed => Direction.Left,
                ConsoleKey.RightArrow when _movement != Direction.Left && !_buttonPressed => Direction.Right,
                _ => throw new ArgumentOutOfRangeException()
            };

            _buttonPressed = true;
        }
    }

    private static void WaitForNextFrame()
    {
        Thread.Sleep(100);
    }

    private void ShowGameOver()
    {
        Console.SetCursorPosition(ScreenWidth / 5, ScreenHeight / 2);
        Console.WriteLine($"Game over, Score: {_score}");
        Console.SetCursorPosition(ScreenWidth / 5, ScreenHeight / 2 + 1);
    }
}

internal abstract class Program
{
    private static void Main()
    {
        var game = new SnakeGame();
        game.Run();
    }
}