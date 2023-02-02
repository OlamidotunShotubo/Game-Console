using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

var connection = new HubConnectionBuilder()
                .WithUrl("http://192.168.100.98:5238/puzzle")
                //             .ConfigureLogging(logging =>
                // {
                //     // Log to the Console
                //     logging.AddConsole();

                //     // This will set ALL logging to Debug level
                //     logging.SetMinimumLevel(LogLevel.Debug);
                // })
                .Build();
connection.Closed += async (error) =>
{
    Console.WriteLine("Closed");
    await Task.Delay(new Random().Next(0, 5) * 1000);
    await connection.StartAsync();

};
await connection.StartAsync();
Console.WriteLine("CONNECTED TO GAME ROOM");
Console.Write("Pls Input a USERNAME : ");
bool status = false; var username = "";
Timer ctDwn = null; int count = 0;
Session session = null;
connection.On<Session>("GetReady", (Session game) =>
{
    session = game;
    ctDwn = new Timer(c =>
    {
        Console.Clear();
        GetReady(game);
    }, null, 0, 1000);
});

connection.On<Session>("SendSession", (Session game) =>
{
    session = game;
    if (status == true)
    {
        Console.ForegroundColor = (ConsoleColor.Gray);
        Console.Clear();
        Console.WriteLine("Online Users :-");
        Display(game);
        Console.WriteLine(" ");
        Console.Write("Input Game Dimension : ");
    }
    if (status != true)
    {
        Console.Clear();
        Console.WriteLine("Online Users :-");
        Display(game);
        Console.WriteLine(" ");
        Console.Write("Pls Input a USERNAME : ");
        status = true;
    }
});
connection.On<Session>("GameOver", (Session game) =>
{
    foreach (var player in game.Players)
    {
        if (player.Winner)
        {
            Console.Clear();
            Console.ForegroundColor = (ConsoleColor.Gray);
            Console.WriteLine($"{player.Name} is the Winner of the Game");
            count = 0;
            session = game;
            ctDwn = new Timer(c =>
            {
                Console.Clear();
                GameEnded(game);
            }, null, 0, 1000);
        }
    }
});
connection.On<Session>("Play", (Session game) =>
{
    Console.Clear();
    Display(game);
});

int input = 0;

username = Console.ReadLine();
if (username != null)
{
    status = true;
    Console.Clear();
    try
    {
        await connection.InvokeAsync("Join",
            username);
        input = int.Parse(Console.ReadLine());
        Console.Clear();
        Console.ForegroundColor = (ConsoleColor.Gray);
        Console.WriteLine($"Dimension : {input}");
        Console.WriteLine("Type (v) to Choose another Dimension");
        Console.Write("Type (b) to Start the Game : ");
        var start = Console.ReadLine();

        if (start == "v")
        {
            Console.ForegroundColor = (ConsoleColor.Gray);
            Console.Clear();
            Console.Write("Input Game Dimension : ");
            input = int.Parse(Console.ReadLine());
            Console.Clear();
            Console.WriteLine($"Dimension : {input}");
            Console.WriteLine("Type (v) to Choose another Dimension");
            Console.Write("Type (b) to Start the Game : ");
            start = Console.ReadLine();
        }
        if (start == "b")
        {
            await connection.InvokeAsync("GetDimension", input);
        }
    }
    catch (System.Exception ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.StackTrace);
    }
}
Console.Read();




void Display(Session output)
{
    try
    {
        foreach (var player in output.Players)
        {
            Console.ForegroundColor = (ConsoleColor)player.Color;
            Console.WriteLine(player.Name);
            if (player.Game != null)
            {
                Console.WriteLine(player.Game.Display());

            }
        }
    }
    catch (System.Exception ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.StackTrace);
    }
}

async void GetReady(Session game)
{
    Console.ForegroundColor = (ConsoleColor.Gray);
    Console.WriteLine("The game will Start soon");
    count++;
    Console.WriteLine($"{count} Second");
    if (count == 5)
    {
        ctDwn.Change(Timeout.Infinite, Timeout.Infinite);
        Console.Clear();
        Display(game);
        var direction = new ConsoleKeyInfo();
        do
        {
            Console.WriteLine("PLAY");
            direction = Console.ReadKey();
            Console.Clear();
            foreach (var player in session.Players)
            {
                if (player.Name == username)
                {

                    player.Game.Play(GetDirection(direction));
                    await connection.InvokeAsync("SendPlay", player);
                }
            }
        } while (direction.Key != ConsoleKey.X);
    }
}

void GameEnded(Session game)
{
    count++;
    if (count == 5)
    {
        ctDwn.Change(Timeout.Infinite, Timeout.Infinite);
        Console.Clear();
        Console.WriteLine("end");
    }
}

Direction GetDirection(ConsoleKeyInfo input)
{
    if (input.Key == ConsoleKey.RightArrow || input.Key == ConsoleKey.R)
    {
        return Direction.Right;
    }
    if (input.Key == ConsoleKey.LeftArrow || input.Key == ConsoleKey.A)
    {
        return Direction.Left;
    }
    if (input.Key == ConsoleKey.UpArrow || input.Key == ConsoleKey.W)
    {
        return Direction.Upwards;
    }
    if (input.Key == ConsoleKey.DownArrow || input.Key == ConsoleKey.S)
    {
        return Direction.Downwards;
    }
    return Direction.Right;
}
