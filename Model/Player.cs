public class Player
{
    public Puzzle Game { get; set; }
    public bool Winner { get; set; }
    public ConsoleColor Color { get; set; }
    public string Name { get; set; }
    public DateTime EndTime { get; set; }
    public int Moves { get; set; }
}