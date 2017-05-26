namespace DotNetBoilerplate.File
{
  public interface IFile
  {
    string FileId { get; }
    string FileName { get; }
    string FileStoreId { get; }
  }
}
