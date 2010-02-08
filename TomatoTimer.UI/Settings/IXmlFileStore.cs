namespace TomatoTimer.UI.Settings
{
    public interface IXmlFileStore
    {
        bool Exists(string fileName);
        void SaveXml(string xml);
        string LoadXml(string filePath);
    }
}