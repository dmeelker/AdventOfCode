using System.Text;

namespace Solution;

public class MirrorField
{
    public char[,] Cells;
    public int Width;
    public int Height;

    public MirrorField(char[,] cells)
    {
        Cells = cells;
        Width = cells.GetLength(1);
        Height = cells.GetLength(0);
    }

    public void RotateRight()
    {
        var temp = new char[Width, Height];
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                temp[Width - 1 - y, x] = Cells[x, y];
            }
        }

        Cells = temp;
    }

    public void RollUp()
    {
        for (var i = 0; i < Width; i++)
        {
            RollColumn(i);
        }
    }

    public void RollColumn(int columnIndex)
    {
        for (var i = 0; i < Height; i++)
        {
            if (Cells[columnIndex, i] == 'O')
            {
                var j = i - 1;
                while (j >= 0 && Cells[columnIndex, j] != '#' && Cells[columnIndex, j] != 'O')
                {
                    Cells[columnIndex, j + 1] = '.';
                    Cells[columnIndex, j] = 'O';

                    j--;
                }
            }
        }
    }

    public MirrorField Clone()
    {
        return new MirrorField((char[,])Cells.Clone());
    }

    public string StateKey()
    {
        var key = new StringBuilder();

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                key.Append(Cells[x, y]);
            }

            key.AppendLine();
        }

        return key.ToString();
    }

    public int CalculateScore()
    {
        var score = 0;

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                if (Cells[x, y] == 'O')
                {
                    score += Height - y;
                }
            }
        }

        return score;
    }
}