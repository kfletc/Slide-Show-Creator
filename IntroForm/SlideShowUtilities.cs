using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace IntroForm
{
    public class SlideShowUtilities
    {
        static public void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest, true);
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }

        public static void ConvertAiffToWav(string aiffFile, string wavFile)
        {
            try
            {
                using (AiffFileReader reader = new AiffFileReader(aiffFile))
                {
                    using (WaveFileWriter writer = new WaveFileWriter(wavFile, reader.WaveFormat))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead = 0;
                        do
                        {
                            bytesRead = reader.Read(buffer, 0, buffer.Length);
                            writer.Write(buffer, 0, bytesRead);
                        } while (bytesRead > 0);
                    }
                }
            }
            catch(FormatException e) 
            {
                throw e;
            }
        }
    }
}
