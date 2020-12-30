using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace External_Crosshair_Overlay.Config
{
    public class ConfigSaver
    {
        public string DefaultSavePath { get; private set; } = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\CrosshairOverlay";

        public ConfigSaver()
        {
            if (!Directory.Exists(DefaultSavePath))
            {
                Directory.CreateDirectory(DefaultSavePath);
            }
        }

        public void SaveConfig(OverlayConfig overlayConfig)
        {
            //var fileName = (from x in overlayConfig.ProcessFilePath.Split('\\')
            //                where !string.IsNullOrWhiteSpace(x)
            //                select x).LastOrDefault();
            string[] s = overlayConfig.FileName.Split('(');
            string fileName = s[0];
            for (int i = 1; i < s.Length - 1; i++)
            {
                fileName = fileName + '(' + s[i];
            }

            //var fileSize = new FileInfo(overlayConfig.ProcessFilePath).Length;
            //fileName += fileSize;

            var targetFile = DefaultSavePath + "\\" + fileName + ".eoc";

            if (File.Exists(targetFile))
            {
                var asdasd = new FileInfo(targetFile).FullName;
                File.Delete(targetFile);
            }

            using (var fStream = File.Create(targetFile))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fStream, overlayConfig);
                fStream.Close();
            }
        }

        public OverlayConfig GetConfig(string processName, string processFilePath)
        {
            //var fileName = (from x in processName.Split('(')
            //                where !string.IsNullOrWhiteSpace(x)
            //                select x).LastOrDefault();
            string[] s = processName.Split('(');
            string fileName = s[0];
            for (int i = 1; i < s.Length - 1; i++) {
                fileName = fileName + '(' + s[i];
            }
            //var fileSize = new FileInfo(processFilePath).Length;
            //fileName += fileSize;

            var retThis = new OverlayConfig();
            var targetFile = DefaultSavePath + "\\" + fileName + ".eoc";
            if (!File.Exists(targetFile))
            {
                retThis.ProcessFilePath = processFilePath;
                return retThis;
            }

            try
            {
                using (var fStream = File.OpenRead(targetFile))
                {
                    var formatter = new BinaryFormatter();
                    retThis = (OverlayConfig)formatter.Deserialize(fStream);
                    fStream.Close();
                }
            }
            catch
            {
                retThis.ProcessFilePath = processFilePath;
            }

            return retThis;
        }
    }
}
