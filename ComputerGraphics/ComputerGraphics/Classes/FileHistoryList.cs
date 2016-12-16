using System;
using System.IO;

namespace ComputerGraphics.Classes
{
    public class FileHistoryList : IDisposable
    {
        private string HistoryDirectory;
        private HistoryList<string> HistoryList;
        public string FileExtension { get; set; }
        public FileHistoryList()
            : base()
        {
            this.HistoryList = new HistoryList<string>();
            this.HistoryDirectory = Path.Combine(Path.GetTempPath(), "_tmp_cg_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.HistoryDirectory);
            this.FileExtension = string.Empty;
        }
        public string NewCheckpoint()
        {
            this.CurrentPosition++;

            string fileName =
                Path.ChangeExtension(
                    Path.Combine(
                        this.HistoryDirectory,
                        this.CurrentPosition.ToString()),
                    this.FileExtension);

            // release file handles
            GC.Collect();
            GC.WaitForPendingFinalizers();

            var deletedFiles = this.HistoryList.NewCheckpoint(fileName);
            foreach (string item in deletedFiles)
            {
                try
                {
                    File.Delete(item);
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine($"Cannot delete file \"{item}\"");
                }
            }

            return fileName;
        }
        public string GetUndoData() => this.HistoryList.GetUndoData();
        public string GetRedoData() => this.HistoryList.GetRedoData();
        public int CurrentPosition
        {
            get
            {
                return this.HistoryList.CurrentPosition;
            }
            set
            {
                this.HistoryList.CurrentPosition = value;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    this.HistoryList = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                // release file handles
                GC.Collect();
                GC.WaitForPendingFinalizers();

                try
                {
                    Directory.Delete(
                        this.HistoryDirectory,
                        recursive: true);
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Cannot delete directory");
                }

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~FileHistoryList()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
