 public class Puzzle
{
    internal int Dimension = 0;
    internal int i = 0;
    internal int r = 0;
    int[,] input = null;
    internal Puzzle(int dimension)
    {
        input = new int[dimension, dimension];
        this.Dimension = dimension;
        Scramble();
    }
    public void Scramble()
    {
        Random rand = new Random();
        var lastno = new int[Dimension * Dimension];
        int indx = 0;
        for (int i = 0; i < input.GetLength(0); i++)
        {
            for (int r = 0; r < input.GetLength(1); r++)
            {
                var value = 0;
                while (lastno.Contains(value))
                {
                    value = rand.Next(1, lastno.Length + 1);
                }
                this.i = i;
                this.r = r;
                lastno[indx++] = value;
                input[i, r] = value;
            }
        }
    }
    internal string Display()
    {
        var output = "";
        for (int i = 0; i < input.GetLength(0); i++)
        {
            for (int r = 0; r < input.GetLength(1); r++)
            {
                if (input[i, r] != Dimension * Dimension)
                {
                    output += input[i, r] + "|";
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
        for (int i = 0; i < input.GetLength(0); i++)
        {
            for (int r = 0; r < input.GetLength(1); r++)
            {
                if (input[i, r] != prev + 1)
                {
                    return false;
                }
                prev = input[i, r];
            }
        }
        return true;
    }
    internal Location Checklast()
    {
        for (int i = 0; i < input.GetLength(0); i++)
        {
            for (int r = 0; r < input.GetLength(1); r++)
            {
                if (input[i, r] == Dimension * Dimension)
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
        i = g.Row;
        r = g.Column;
        int replace = 0;
        switch (directions)
        {
            case Direction.Upwards:
                if (i < Dimension - 1)
                {
                    replace = input[i, r];
                    input[i, r] = input[i + 1, r];
                    input[i + 1, r] = replace;
                }
                break;
            case Direction.Left:
                if (r < Dimension - 1)
                {
                    replace = input[i, r];
                    input[i, r] = input[i, r + 1];
                    input[i, r + 1] = replace;
                }
                break;
            case Direction.Right:
                if (r != 0)
                {
                    replace = input[i, r];
                    input[i, r] = input[i, r - 1];
                    input[i, r - 1] = replace;
                }
                break;
            case Direction.Downwards:
                if (i != 0)
                {
                    replace = input[i, r];
                    input[i, r] = input[i - 1, r];
                    input[i - 1, r] = replace;
                }
                break;
        }
    }
}