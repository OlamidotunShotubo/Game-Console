using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
                .WithUrl("http://192.168.100.98:5238/puzzle")
                .Build();

connection.Closed += async (error) =>
{
    await Task.Delay(new Random().Next(0, 5) * 1000);
    await connection.StartAsync();
};
await connection.StartAsync();
Console.WriteLine("CONNECTED TO GAME ROOM");
Console.WriteLine("Pls Input a USERNAME");
bool status = false;
Timer ctDwn = null;int count = 0;
connection.On<Session>("GetReady", (Session game) =>
{
    Console.WriteLine("Get Ready");
    ctDwn = new Timer(c =>
    {
        Console.Clear();
        GetReady(game);
    }, null, 0, 1000);
});
connection.On<Session>("SendSession", (Session game) =>
{
    if (status == true)
    {
        Console.Clear();
        Console.WriteLine("Online Users :-");
        Display(game);
        Console.WriteLine(" ");
        Console.WriteLine("Input Game Dimension");
    }

    if (status != true)
    {
        Console.Clear();
        Console.WriteLine("Online Users :-");
        Display(game);
        Console.WriteLine(" ");
        Console.WriteLine("Pls Input a USERNAME");
        status = true;
    }

});


int input = 0;

void Display(Session output)
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
var username = Console.ReadLine();
if (username != null)
{
    status = true;
    Console.Clear();
    await connection.InvokeAsync("Join",
            username);
    input = int.Parse(Console.ReadLine());
    Console.Clear();
    Console.WriteLine($"Dimension : {input}");
    Console.WriteLine("Type (b) to Start the Game");
    var start = Console.ReadLine();
    if (start == "b")
    {
        await connection.InvokeAsync("GetDimension", input);
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
