namespace DotNetBoilerplate.File
{
  using System;
  using DotNetBoilerplate.File.Store;

  public interface IFileControl
  {
    string DefaultFileStoreId { get; set; }

    void RegisterFileStore(IFileStore fileStore);

    IFileStore DefaultFileStore { get; }

    IFileStore GetFileStore(string fileStoreId);
  }
}
