namespace nthLink.Header.Interface
{
    interface IEncodeDecode
    {
        string Encrypt(string text);
        string Decrypt(string text);
    }
}
