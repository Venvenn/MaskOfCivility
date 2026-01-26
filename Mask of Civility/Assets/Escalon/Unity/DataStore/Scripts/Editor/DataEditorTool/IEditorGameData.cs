namespace Escalon
{
    public interface IEditorGameData
    {
        IData GetData();
        void Delete();
        void Duplicate(string folderPath, string newFilePath);
    }
}