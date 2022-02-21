using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DialogScriptCreator
{
    public enum DialogType
    {
        Dialog, Monolog, Answer
    }
    internal struct RouteNames
    {
        public Dialog dialog;
        public string from, to;
        public RouteNames(Dialog dialog, string from, string to)
        {
            this.from = from;
            this.to = to;
            this.dialog = dialog;
        }
    } 
    public class DialogScriptReader
    {
        private static Dictionary<char, DialogType> dialogChars = new Dictionary<char, DialogType>{ {'D', DialogType.Dialog}, { 'M', DialogType.Monolog }, { 'A', DialogType.Answer } };
        private Regex dialogRegex = new Regex("^\\[([M|D|A])(!)?\\]([a-zA-Z0-9]+)=([a-zA-Z0-9]+)");
        private string _scriptname;
        private Dictionary<string, Dialog> _dialogNames = new();
        private List<RouteNames> _dialogRoutes = new();
        public string ScriptName { get => _scriptname; }
        public int DialogsCount { get => _dialogNames.Count; }
        public bool ReadScript(string scriptname)
        {
            if (!File.Exists(scriptname))
            {
                return false;
            }
            _scriptname = scriptname;
            _dialogNames.Clear();
            _dialogRoutes.Clear();
            ReadDialogs(ReadLines());

            return true;
        }

        private string[] ReadLines()
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
            return System.Text.Encoding.UTF8.GetString(buffer).Replace(" ", "").Split('\n');
        }

        private void ReadDialogs(string[] lines)
        {
            bool fillDialog = false;
            Dialog prevDialog = null;
            for(int i = 0; i < lines.Length; i++)
            {
                if(prevDialog != null && fillDialog)
                {
                    if(lines[i][0] == '\t')
                    {
                        var op = lines[i].Trim('\t');
                        var splt = op.Split('>');
                        if (splt.Length != 2)
                            throw new ScriptSyntaxException();
                        _dialogRoutes.Add(new RouteNames(prevDialog, splt[0].Trim(), splt[1].Trim()));
                    }
                    else
                    {
                        fillDialog = false;
                    }
                    continue;
                }
                if (string.IsNullOrWhiteSpace(lines[i]) || lines[i][0] == '#') continue;
                Match match = dialogRegex.Match(lines[i]);
                if (!match.Success) continue;
                prevDialog = new Dialog(match.Groups[3].Value, match.Groups[4].Value, dialogChars[match.Groups[1].Value[0]], match.Groups[2].Success);
                _dialogNames.Add(prevDialog.Name, prevDialog);
                if(prevDialog.Type == DialogType.Dialog)
                {
                    fillDialog = true;
                }
            }
            foreach(var item in _dialogRoutes)
            {
                item.dialog.AddRoute(_dialogNames[item.from], _dialogNames[item.to]);
            }
        }
        public Dialog GetDialogByName(string name) => _dialogNames[name].Clone();
    }
}
