using System;
using System.IO;
using System.Linq;

namespace DialogScriptCreator
{
    public class DialogScriptReader
    {
        private string _scriptname;
        private string[] lines;
        public string ScriptName { get => _scriptname; }
        public int ScriptStringLength { get => lines.Length; }
        public bool ReadScript(string scriptname)
        {
            if (!File.Exists(scriptname))
            {
                return false;
            }
            _scriptname = scriptname;
            byte[] buffer = { };
            using (FileStream ifs = new FileStream(scriptname, FileMode.Open, FileAccess.Read))
            {
                int numBytesToRead = (int)ifs.Length, numBytesRead = 0;
                buffer = new byte[numBytesToRead];
                while (numBytesToRead > 0)
                {
                    int n = ifs.Read(buffer, numBytesRead, numBytesToRead);
                    numBytesRead += n;
                    numBytesToRead -= n;
                }
            }
            string file = System.Text.Encoding.UTF8.GetString(buffer).Replace(" ", "");
            lines = file.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();


            return true;
        }
    }
}
