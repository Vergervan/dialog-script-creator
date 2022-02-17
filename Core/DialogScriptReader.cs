using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DialogScriptCreator
{
    public class DialogScriptReader
    {
        private Regex dialogRegex = new Regex("^\\[[M|D](!)?\\]");
        private string _scriptname;
        private List<Dialog> _dialogs = new List<Dialog>();
        private string[] lines;
        public string ScriptName { get => _scriptname; }
        public int ScriptStringLength { get => lines.Length; }
        public int DialogsCount { get => _dialogs.Count; }
        public bool ReadScript(string scriptname)
        {
            if (!File.Exists(scriptname))
            {
                return false;
            }
            _scriptname = scriptname;
            ReadLines();
            ReadDialogs();

            return true;
        }

        private void ReadLines()
        {
            byte[] buffer = { };
            using (FileStream ifs = new FileStream(_scriptname, FileMode.Open, FileAccess.Read))
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
        }

        private void ReadDialogs()
        {
            Match match;
            for(int i = 0; i < lines.Length; i++)
            {
                match = dialogRegex.Match(lines[i]);
                if (!match.Success) continue;
                _dialogs.Add(new Dialog());
            }
        }
    }
}
