using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeCreator
{
    public enum Orientation
    {
        Up, Right, Down, Left
    }
    public enum CellType
    {
        Empty, Wall
    }
    public class Cell
    {


        public bool IsUsed;

        public CellType Value { get; set; } //1 - стена, 0 - проход


        public void SetValues(int _i, int _j, CellType _cellType)
        {
            Value = _cellType;
            i = _i;
            j = _j;
            IsUsed = Value == CellType.Empty ? false : true;
        }

        public int i { get; set; }

        public int j { get; set; }

        public Cell(int _i, int _j, CellType _cellType)
        {
            SetValues(_i, _j, _cellType);
        }


    }

    public class Maze
    {

        public Random rand;

        public bool IsDone
        {
            get
            {
                foreach (var item in MazeMatrix)
                {
                    if (!item.IsUsed)
                        return false;
                }
                return true;
            }
        }

        public Cell[,] MazeMatrix { get; set; }

        public List<Cell> CellsInList
        {
            get
            {
                var t = new List<Cell>();
                foreach (var item in MazeMatrix)
                {
                    t.Add(item);
                }
                return t;
            }
        }

        int RandomSeed;

        int RoomSize;

        public Maze(int Size, int _RandomSeed = 0)
        {
            RoomSize = Size;
            MazeMatrix = new Cell[RoomSize * 2 + 1, RoomSize * 2 + 1];
            RandomSeed = _RandomSeed;
        }

        public void FillMaze()
        {
            for (int i = 0; i < MazeMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < MazeMatrix.GetLength(1); j++)
                {
                    if (i % 2 == 1 && j % 2 == 1)
                        MazeMatrix[i, j] = new Cell(i, j, CellType.Empty);
                    else
                        MazeMatrix[i, j] = new Cell(i, j, CellType.Wall);
                }
            }
        }

        public void ResetMaze()
        {
            for (int i = 0; i < MazeMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < MazeMatrix.GetLength(1); j++)
                {
                    if (i % 2 == 1 && j % 2 == 1)
                        MazeMatrix[i, j].SetValues(i, j, CellType.Empty);
                    else
                        MazeMatrix[i, j].SetValues(i, j, CellType.Wall);
                }
            }
        }

        public List<Orientation> WhereCanIGo(int i, int j)
        {
            List<Orientation> orients = new List<Orientation>();
            if (i - 2 >= 0 && !MazeMatrix[i - 2, j].IsUsed && MazeMatrix[i - 2, j].Value == CellType.Empty)
                orients.Add(Orientation.Up);

            if (j + 2 < MazeMatrix.GetLength(1) && !MazeMatrix[i, j + 2].IsUsed && MazeMatrix[i, j + 2].Value == CellType.Empty)
                orients.Add(Orientation.Right);

            if (i + 2 < MazeMatrix.GetLength(0) && !MazeMatrix[i + 2, j].IsUsed && MazeMatrix[i + 2, j].Value == CellType.Empty)
                orients.Add(Orientation.Down);

            if (j - 2 >= 0 && !MazeMatrix[i, j - 2].IsUsed && MazeMatrix[i, j - 2].Value == CellType.Empty)
                orients.Add(Orientation.Left);


            return orients;
        }

        public Cell GoToNextCell(Cell curCell, Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.Up:
                    MazeMatrix[curCell.i - 1, curCell.j].Value = CellType.Empty;
                    return MazeMatrix[curCell.i - 2, curCell.j];

                case Orientation.Right:
                    MazeMatrix[curCell.i, curCell.j + 1].Value = CellType.Empty;
                    return MazeMatrix[curCell.i, curCell.j + 2];
                case Orientation.Down:
                    MazeMatrix[curCell.i + 1, curCell.j].Value = CellType.Empty;
                    return MazeMatrix[curCell.i + 2, curCell.j];
                case Orientation.Left:
                    MazeMatrix[curCell.i, curCell.j - 1].Value = CellType.Empty;
                    return MazeMatrix[curCell.i, curCell.j - 2];
                default:
                    return null;
            }

        }

        public Cell[,] BuildMaze(int startI = 0, int startJ = 0, bool RandomStart = false)
        {

            if (RandomSeed != 0)
                rand = new Random(RandomSeed);
            else
                rand = new Random();

            Stack<Cell> cells = new Stack<Cell>();
            if (!RandomStart)
            {
                if (startI > RoomSize || startJ > RoomSize)
                    startI = startJ = 0;
            }
            else
            {
                startI = rand.Next(RoomSize);
                startJ = rand.Next(RoomSize);
            }

            startI = startI * 2 + 1;
            startJ = startJ * 2 + 1;

            var CurCell = MazeMatrix[startI, startJ];
            CurCell.IsUsed = true;

            cells.Push(CurCell);

            while (!IsDone)
            {

                var Orients = WhereCanIGo(CurCell.i, CurCell.j);
                if (Orients.Count != 0)
                {

                    CurCell = GoToNextCell(CurCell, Orients[rand.Next(Orients.Count)]);

                    CurCell.IsUsed = true;

                    cells.Push(CurCell);
                }
                else
                    CurCell = cells.Pop();
            }
            return MazeMatrix;
        }



    }
}
