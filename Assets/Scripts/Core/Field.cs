using System;
using System.Collections.Generic;

namespace Core
{
    public class Field
    {
        private const int Side = 9;

        private Cell[,] _grid = new Cell[Side, Side];
        private Dictionary<Cell, int> _countMineNeighbors = new Dictionary<Cell, int>();
        private int _countMinesOnField;
        private int _countClosedTiles = Side * Side;

        private Random _random = new Random();

        public Action<Cell[,]> InitializeCompleted;

        public void Initialize()
        {
            for (int x = 0; x < _grid.GetLength(0); x++)
            {
                for (int y = 0; y < _grid.GetLength(1); y++)
                {
                    bool isMine = _random.Next(0, 10) < 1;

                    Cell cell = new Cell(x, y, isMine);

                    _grid[x, y] = cell;

                    cell.CellOpened += OnCellOpen;
                    cell.MineActivated += OnMineActivated;
                    cell.FlagSet += OnFlagSet;
                    cell.FlagRemoved += OnFlagRemoved;

                    if (isMine) 
                    {
                        _countMinesOnField++;
                    }
                }
            }
        
            CountMineNeighborsForEachCell();
            FieldInformation.CountFlags = _countMinesOnField;
            InitializeCompleted?.Invoke(_grid);
        }
    
        private void CountMineNeighborsForEachCell() 
        {
            for (int x = 0; x < _grid.GetLength(0); x++) 
            {
                for (int y = 0; y < _grid.GetLength(1); y++)
                {
                    Cell cell = _grid[x, y];

                    if (cell.IsMine == false) 
                    {
                        List<Cell> neighbors = GetNeighbors(cell);

                        int countMineNeighbors = 0;

                        foreach (Cell neighbor in neighbors)
                        {
                            if (neighbor.IsMine) 
                            {
                                countMineNeighbors++;
                            }
                        }

                        _countMineNeighbors.Add(cell, countMineNeighbors);
                    }
                }
            }
        }

        private List<Cell> GetNeighbors(Cell cell) 
        {
            List<Cell> result = new List<Cell>();
        
            for (int y = cell.Y - 1; y <= cell.Y + 1; y++) 
            {
                for (int x = cell.X - 1; x <= cell.X + 1; x++) 
                {
                    if ((x >= 0 && x < Side) && (y >= 0 && y < Side) && (_grid[x, y] != cell))
                    {
                        result.Add(_grid[x, y]);
                    }
                }
            }
        
            return result;
        }

        private void OnMineActivated()
        {
            GameOver();
        }

        private void GameOver()
        {
            FieldInformation.IsGameStopped = true;
        }

        private void OnCellOpen(Cell cell)
        {
            int countMinesNeighbors = _countMineNeighbors.GetValueOrDefault(cell);
        
            if (countMinesNeighbors == 0)
            {
                cell.SendMessage(string.Empty);
                OpenAdjacentCells(cell);
            } 
            else 
            {
                cell.SendMessage(countMinesNeighbors.ToString());
            }
        
            _countClosedTiles--;
        
            if (_countClosedTiles == _countMinesOnField && FieldInformation.IsGameStopped == false) 
            {
                Win();
            }
        }

        private void OpenAdjacentCells(Cell cell)
        {
            List<Cell> neighbors = GetNeighbors(cell);

            foreach (Cell neighbor in neighbors) 
            {
                if (neighbor.IsOpen == false) 
                {
                    neighbor.Open();
                }
            }
        }

        private void Win()
        {
            FieldInformation.IsGameStopped = true;
        }

        private void OnFlagSet()
        {
            FieldInformation.CountFlags--;
        }

        private void OnFlagRemoved()
        {
            FieldInformation.CountFlags++;
        }
    }
}