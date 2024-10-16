using System;

namespace Core
{
    public class Cell
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool IsMine { get; private set; }
        public bool IsOpen { get; private set; }
        public bool IsFlag { get; private set; }

        public Action<Cell> CellOpened;
        public Action<string> MessageReceived;
        public Action MineActivated;
        public Action FlagSet;
        public Action FlagRemoved;

        public Cell(int x, int y, bool isMine)
        {
            X = x;
            Y = y;
            IsMine = isMine;
        }
    
        public void Open() 
        {
            if (IsOpen == false && IsFlag == false && FieldInformation.IsGameStopped == false) 
            {
                if (IsMine) 
                {
                    MineActivated?.Invoke();
                } 
                else 
                {
                    IsOpen = true;
                    CellOpened?.Invoke(this);
                }
            }
        }

        public void SetFlag() 
        {
            if (IsOpen == false && FieldInformation.IsGameStopped == false && FieldInformation.CountFlags != 0) 
            {
                if (IsFlag == false) 
                {
                    IsFlag = true;
                    FlagSet?.Invoke();
                } 
                else 
                {
                    IsFlag = false;
                    FlagRemoved?.Invoke();
                }
            }
        }

        public void SendMessage(String message)
        {
            MessageReceived?.Invoke(message);
        }
    }
}
