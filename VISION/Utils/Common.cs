using MvUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace VISION.Utils
{
    //public enum EXECUTION_STATE : uint
    //{
    //    ES_AWAYMODE_REQUIRED = 0x00000040,
    //    ES_CONTINUOUS = 0x80000000,
    //    ES_DISPLAY_REQUIRED = 0x00000002,
    //}

    public static class Common
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(int hWnd);

        //[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        //public static void Prevent_Screensaver(bool enable)
        //{
        //    if (enable) SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
        //    else SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
        //}

        public static Double UseMemory()
        {
            return Process.GetCurrentProcess().WorkingSet64;
        }

        public static Boolean GarbageCollection(Int32 MBytes)
        {
            Double usage = UseMemory() / 1000000;
            Debug.WriteLine(usage, "메모리 사용량");
            if (usage < MBytes) return false;
            GC.Collect();
            return true;
        }

        public static String GetImageFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.Filter = "Image Files(*.bmp; *.jpg; *.png;)| *.bmp; *.jpg; *.png; | All files(*.*) | *.*";
                openFileDialog.Filter = "Image Files(*.bmp; *.png;)| *.bmp; *.png;";
                if (openFileDialog.ShowDialog() != DialogResult.OK) return String.Empty;
                return openFileDialog.FileName;
            }
        }

        public static String[] GetImageFiles()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.Filter = "Image Files(*.bmp; *.jpg; *.png;)| *.bmp; *.jpg; *.png; | All files(*.*) | *.*";
                openFileDialog.Filter = "Image Files(*.bmp; *.png;)| *.bmp; *.png;";
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() != DialogResult.OK) return new string[] { };
                return openFileDialog.FileNames;
            }
        }

        //public static Bitmap LoadImage()
        //{
        //    String FileName = GetImageFile();
        //    if (String.IsNullOrEmpty(FileName)) return null;
        //    return LoadImage(FileName);
        //}

        //public static Bitmap LoadImage(string FileName)
        //{
        //    return LoadImage(new FileInfo(FileName));
        //}
        //public static Bitmap LoadImage(FileInfo FI)
        //{
        //    if (!FI.Exists) return null;
        //    try
        //    {
        //        Bitmap bitmap = new Bitmap(FI.FullName);
        //        bitmap.Tag = FI.FullName;
        //        return bitmap;
        //        //using (Bitmap bitmap = new Bitmap(FI.FullName))
        //        //    return bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), Schemas.환경설정.pixelFormat);
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.DebugException(ex, 2);
        //        Global.오류로그("일반", "이미지 불러오기", ex.Message, true);
        //        return null;
        //    }
        //}

        public static void ImageSaveAs(Bitmap bitmap)
        {
            ImageSaveAs(bitmap, String.Empty);
        }

        public static void ImageSaveAs(Bitmap bitmap, String FileName)
        {
            if (bitmap == null) return;
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                //saveFileDialog.Filter = "Bitmap File(*.bmp) | *.bmp |PNG File(*.png) | *.png |JPG File(*.jpg)| *.jpg";
                saveFileDialog.Filter = "PNG File(*.png) | *.png";
                saveFileDialog.DefaultExt = "png";
                saveFileDialog.FileName = FileName;
                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
                String error = String.Empty;
                ImageSave(bitmap, saveFileDialog.FileName, out error);
            }
        }

        public static Boolean ImageSave(Bitmap bitmap, String FileName, out String error)
        {
            error = String.Empty;
            if (bitmap == null) return false;
            try
            {
                using (Bitmap bmp = bitmap.Clone() as Bitmap)
                    bmp.Save(FileName, ImageFormat.Png);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                Debug.WriteLine(FileName, "저장경로");
                MvUtils.Utils.DebugException(ex, 2);
            }
            return false;
        }

        //public static Boolean ImageSave(CogImage8Grey image, String FileName, out String Error)
        //{
        //    Error = String.Empty;
        //    if (image == null) return false;
        //    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
        //    {
        //        //saveFileDialog.Filter = "Bitmap File(*.bmp) | *.bmp |PNG File(*.png) | *.png |JPG File(*.jpg)| *.jpg";
        //        saveFileDialog.Filter = "PNG File(*.png) | *.png";
        //        saveFileDialog.DefaultExt = "png";
        //        saveFileDialog.FileName = FileName;
        //        if (saveFileDialog.ShowDialog() != DialogResult.OK) return false;
        //        return ImageSavePng(image, saveFileDialog.FileName, out Error);
        //    }
        //}

        //public static Boolean ImageSavePng(CogImage8Grey image, String FileName, out String Error)
        //{
        //    if (image == null)
        //    {
        //        Error = "저장할 이미지가 없습니다.";
        //        return false;
        //    }
        //    try
        //    {
        //        using (CogImageFilePNG PngImage = new CogImageFilePNG())
        //        {
        //            PngImage.Open(FileName, CogImageFileModeConstants.Write);
        //            PngImage.Append(image);
        //            PngImage.Close();
        //        }
        //        Error = String.Empty;
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Error = ex.Message;
        //        return false;
        //    }
        //}

        //public static Boolean ImageSaveBmp(CogImage8Grey image, String FileName, out String Error)
        //{
        //    if (image == null)
        //    {
        //        Error = "저장할 이미지가 없습니다.";
        //        return false;
        //    }
        //    try
        //    {
        //        using (CogImageFileBMP BmpImage = new CogImageFileBMP())
        //        {
        //            BmpImage.Open(FileName, CogImageFileModeConstants.Write);
        //            BmpImage.Append(image);
        //            BmpImage.Close();
        //        }
        //        Error = String.Empty;
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Error = ex.Message;
        //        return false;
        //    }
        //}

        public static String CreateDirectory(List<String> dirs)
        {
            try
            {
                String path = String.Empty;
                foreach (String dir in dirs)
                {
                    if (String.IsNullOrEmpty(path)) path = dir;
                    else path = Path.Combine(path, dir);
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                }
                return path;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return String.Empty;
        }

        public static bool DirectoryExists(string Path)
        {
            return DirectoryExists(Path, false);
        }

        public static bool DirectoryExists(string Path, bool Create)
        {
            try
            {
                if (Directory.Exists(Path))
                    return true;

                Directory.CreateDirectory(Path);
                return Directory.Exists(Path);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "DirectoryExists");
                return false;
            }
        }

        public static Boolean Ping(String Host)
        {
            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions() { DontFragment = true };
            try
            {
                // Create a buffer of 32 bytes of data to be transmitted.
                PingReply reply = pingSender.Send(Host, 1000, Encoding.ASCII.GetBytes("TEST"), options);
                Debug.WriteLine($"PingTest {Host}[{reply.Status}]");
                return reply.Status == IPStatus.Success;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }
    }

    public class MyWatch : Stopwatch
    {
        private string Title = String.Empty;
        public MyWatch(String title)
        {
            this.Title = title;
            this.Start();
        }

        public void Stop(String Tag, Boolean Restart = true)
        {
            this.Stop();
            Debug.WriteLine($"{this.Title}: {Tag} => {this.ElapsedMilliseconds}ms");
            if (Restart)
            {
                this.Reset();
                this.Start();
            }
        }

        public void Print(String contents)
        {
            Debug.WriteLine($"{this.Title}: {contents}");
        }
    }

    public static class StringCipher
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}

