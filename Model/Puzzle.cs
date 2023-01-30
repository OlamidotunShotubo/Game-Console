public class Puzzle
{
    public int Dimension { get; set; } = 0;
    public int i { get; set; } = 0;
    public int r { get; set; } = 0;
    public List<List<int>> input { get; set; } = new List<List<int>>();
    public Puzzle()
    {

    }
    // public void Scramble()
    // {
    //     Random rand = new Random();
    //     var lastno = new int[Dimension * Dimension];
    //     int indx = 0;
    //     for (int i = 0; i < input.GetLength(0); i++)
    //     {
    //         for (int r = 0; r < input.GetLength(1); r++)
    //         {
    //             var value = 0;
    //             while (lastno.Contains(value))
    //             {
    //                 value = rand.Next(1, lastno.Length + 1);
    //             }
    //             this.i = i;
    //             this.r = r;
    //             lastno[indx++] = value;
    //             input[i, r] = value;
    //         }
    //     }
    // }
    internal string Display()
    {
        var output = "";
        for (int i = 0; i < Dimension; i++)
        {
            var current = input[i];
            for (int r = 0; r < Dimension; r++)
            {
                if (current[r] != Dimension * Dimension)
                {
                    output += current[r] + "|";
                }
                else
                {
                    output += " |";
                }
            }
            output += "\n";
        }
        return output;
    }
    public bool Check()
    {
        int prev = 0;
        for (int i = 0; i < Dimension; i++)
        {
            var current = input[i];
            for (int r = 0; r < Dimension; r++)
            {

                if (current[r] != prev + 1)
                {
                    return false;
                }
                prev = current[r];
            }
        }
        return true;
    }
    internal Location Checklast()
    {
        Console.WriteLine(Dimension);
        for (int i = 0; i < Dimension; i++)
        {
            Console.WriteLine(i);
            var current = input[i];
            for (int r = 0; r < Dimension; r++)
            {
                Console.WriteLine(r);
                Console.WriteLine(current[r]);
                if (current[r] == Dimension * Dimension)
                {
                    return new Location() { Row = i, Column = r };
                }
            }
        }
        return new Location() { Row = -1, Column = -1 };
    }

    internal void Clear()
    {
        Console.Clear();
    }

    public void Play(Direction directions)
    {
        var g = Checklast();
        Console.WriteLine(g.Row);
        Console.WriteLine(g.Column);
        i = g.Row;
        r = g.Column;
        int replace = 0;
        switch (directions)
        {
            case Direction.Upwards:
                if (i < Dimension - 1)
                {
                    replace = input[i][r];
                    input[i][r] = input[i + 1][r];
                    input[i + 1][r] = replace;
                }
                break;
            case Direction.Left:
                if (r < Dimension - 1)
                {
                    replace = input[i][r];
                    input[i][r] = input[i][r + 1];
                    input[i][r + 1] = replace;
                }
                break;
            case Direction.Right:
                if (r != 0)
                {
                    replace = input[i][r];
                    input[i][r] = input[i][r - 1];
                    input[i][r - 1] = replace;
                }
                break;
            case Direction.Downwards:
                if (i != 0)
                {
                    replace = input[i][r];
                    input[i][r] = input[i - 1][r];
                    input[i - 1][r] = replace;
                }
                break;
        }
    }
}