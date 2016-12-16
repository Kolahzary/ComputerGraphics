namespace ComputerGraphics.Classes
{
    public partial class Bgra32BitmapTool
    {
        private FileHistoryList History = new FileHistoryList() { FileExtension = "png" };

        public void SaveCheckpoint()
        {
            string path = this.History.NewCheckpoint();
            this.SavePng(path);
        }
        public void Undo()
        {
            this.Load(this.History.GetUndoData());
        }
        public void Redo()
        {
            this.Load(this.History.GetRedoData());
        }

    }
}
