using System;
using System.Collections.Generic;
using System.Drawing;
using NBitcoin;
using QRCoder;

namespace PaperWallet
{
    public class PaperWallet
    {
        public static string[] generateKeys()
        {
            Key privKey = new Key();
            BitcoinSecret mainNetPrivateKey = privKey.GetBitcoinSecret(Network.Main);
            PubKey pubKey = privKey.PubKey;
            string[] Keys = new string[2];
            Keys[0] = mainNetPrivateKey.ToString();
            Keys[1] = pubKey.GetAddress(ScriptPubKeyType.Legacy, Network.Main).ToString();
            return Keys;
        }

        public static string generatePaperWallet(string[] keys)
        {
            List<string> QRCodesImages = new List<string>();
            foreach (string key in keys)
            {
                var generator = new PayloadGenerator.BitcoinAddress(key.ToString(), 0);
                string payload = generator.ToString();

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                var qrCodeAsBitmap = qrCode.GetGraphic(8);
                qrCodeAsBitmap.Save(@"C:\PaperWalletImages\QRCode" + key + ".png");

                QRCodesImages.Add(@"C:\PaperWalletImages\QRCode" + key + ".png");
                QRCodesImages.Add(key.ToString());
            }

            Image instanceImg = Bitmap.FromFile(@"C:\PaperWalletInstance.jpg");
            Image privKey = Bitmap.FromFile(QRCodesImages[0]);
            Image pubKey = Bitmap.FromFile(QRCodesImages[2]);
            Graphics i = Graphics.FromImage(instanceImg);

            i.DrawImage(pubKey, 0, 370);
            i.DrawImage(privKey, 2300, 300);
            i.DrawString(QRCodesImages[1], new Font("Arial", (float)13.5),
            new SolidBrush(Color.Black), 1730, 1450);
            i.DrawString(QRCodesImages[3], new Font("Arial", (float)15),
            new SolidBrush(Color.Black), 0, 1450);

            instanceImg.Save(@"C:\PaperWalletImages\outer" + QRCodesImages[1] + ".jpg");

            return @"C:\PaperWalletImages\outer" + QRCodesImages[1] + ".jpg";
        }
    }
}
