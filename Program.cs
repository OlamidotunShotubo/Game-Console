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
bool status = false;
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

connection.On<Session>("Play", (Session game) =>
{
    Console.Clear();
    Display(game);
});

int input = 0;
void Display(Session output)
{
    try
    {
        foreach (var Player in output.Players)
        {
            Console.WriteLine(Player.Name);
            if (Player.Game != null)
            {
                Console.WriteLine(Player.Game.Display());
            }
        }
    }
    catch (System.Exception ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.StackTrace);
    }
}

bool show = false;
void GetReady(Session game)
{
    Console.WriteLine("The game will Start in 5 Seconds");
    count++;
    Console.WriteLine($"{count} Second");
    if (count == 5)
    {
        ctDwn.Change(Timeout.Infinite, Timeout.Infinite);
        Console.Clear();
        Display(game);
    }
}
// async void Program()
// {
var username = "";
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
        Console.WriteLine($"Dimension : {input}");
        Console.Write("Type (b) to Start the Game : ");
        var start = Console.ReadLine();
        if (start == "b")
        {
            await connection.InvokeAsync("GetDimension", input);
        }
        var direction = new ConsoleKeyInfo();
        do
        {
            direction = Console.ReadKey();
            Console.Clear();
            foreach (var player in session.Players)
            {
                if (player.Name == username)
                {
                    player.Game.Play(GetDirection(direction));
                    await connection.InvokeAsync("SendPlay", player);
                    //                   if ()
                    //                 {
                    //                     Console.Clear();
                    //                   Console.WriteLine("Weldone You did it.");
                    //                  Console.WriteLine("Type (r) to replay");
                    //                var restart = Console.ReadLine();
                    //          }
                }
            }
        } while (direction.Key != ConsoleKey.X);

    }
    catch (System.Exception ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.StackTrace);
    }
}
Console.Read();


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
