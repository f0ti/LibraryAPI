using LibraryAPI.IServices;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace LibraryAPI.Services
{
    public class QRServices : IQRServices
    {
        public void GenerateQRCode(string textToEncode)
        {
            QRCodeEncoder encoder = new QRCodeEncoder();
            encoder.QRCodeScale = 8;
            Bitmap bmp = encoder.Encode(textToEncode);
            string filename = Guid.NewGuid().ToString("n").Substring(0, 8);
            bmp.Save(filename, ImageFormat.Jpeg);
        }

        public string DecodeQRCode(string filename)
        {
            Bitmap qrbmp = new Bitmap(filename);
            QRCodeDecoder decoder = new QRCodeDecoder();
            QRCodeBitmapImage image = new QRCodeBitmapImage(qrbmp);
            string decodedText = decoder.Decode(image, Encoding.UTF8);

            return decodedText;
        }

    }
}
