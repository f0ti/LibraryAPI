namespace LibraryAPI.IServices
{
    public interface IQRServices
    {
        void GenerateQRCode(string textToEncode);
        string DecodeQRCode(string filename);
    }
}
