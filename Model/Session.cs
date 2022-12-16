public class Session
{
    public string PropertyID { get; set; }
    public int Dimension { get; set; }
    public List<Player> Players { get; set; }=new List<Player>();
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}