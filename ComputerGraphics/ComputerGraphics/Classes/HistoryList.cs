using System.Collections.Generic;
using System.Linq;

namespace ComputerGraphics.Classes
{
    public class HistoryList<T> : List<T>
    {
        public int CurrentPosition { get; set; }
        public HistoryList() : base()
        {
            this.CurrentPosition = -1;
        }
        /// <summary>
        /// Adds new checkpoint, if CurrentPosition is not pointing to last element it'll remove all undone elements
        /// </summary>
        /// <param name="element">Element to add</param>
        /// <returns>Lost data</returns>
        public IEnumerable<T> NewCheckpoint(T element, bool increaseCurrentPosition = false)
        {
            if (increaseCurrentPosition) this.CurrentPosition++;

            List<T> lostData = new List<T>();

            while (this.CurrentPosition < this.Count)
            {
                lostData.Add(this.ElementAt(this.CurrentPosition));
                this.RemoveAt(this.CurrentPosition);
            }

            this.Insert(this.CurrentPosition, element);
            return lostData;
        }
        public T GetUndoData()
        {
            if (this.CurrentPosition != 0)
                this.CurrentPosition--;
            return this.ElementAt(this.CurrentPosition);
        }
        public T GetRedoData()
        {
            if (this.CurrentPosition < this.Count - 1)
                this.CurrentPosition++;
            return this.ElementAt(this.CurrentPosition);
        }
    }
}
