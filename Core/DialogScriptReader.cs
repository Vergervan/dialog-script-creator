using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

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
        public bool hasTriggers;
        public string[] triggers;
        public RouteNames(Dialog dialog, string from, string to, params string[] triggers)
        {
            this.from = from;
            this.to = to;
            this.dialog = dialog;
            this.triggers = triggers;
            hasTriggers = triggers != null && triggers.Length > 0;
        }
    } 
    public class DialogScriptReader
    {
        private static Dictionary<char, DialogType> dialogChars = new Dictionary<char, DialogType>{ {'D', DialogType.Dialog}, { 'M', DialogType.Monolog }, { 'A', DialogType.Answer } };
        private Regex dialogRegex = new Regex("^\\[([M|D|A])(!)?(\\([\\w\\,]+\\))?\\](\\w+)=(\\w+)");
        private string _scriptname;
        private ConditionKeeper _keeper;
        private Dictionary<string, Dialog> _dialogNames = new Dictionary<string, Dialog>();
        private List<RouteNames> _dialogRoutes = new List<RouteNames>();
        public ICollection<Dialog> GetDialogs() => _dialogNames.Select(item => item.Value).ToArray();
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
            _keeper = new ConditionKeeper();
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
                        int triggerStart = splt[1].IndexOf('[');
                        int triggerEnd = splt[1].IndexOf(']');
                        string[] triggers = null;
                        if (triggerStart != -1)
                        {
                            triggers = splt[1].Substring(triggerStart + 1, triggerEnd - triggerStart - 1).Split(',').Where(str => !string.IsNullOrWhiteSpace(str)).Select(str => str.Trim()).ToArray();
                        }
                        _dialogRoutes.Add(new RouteNames(prevDialog, splt[0].Trim(), triggerStart == -1 ? splt[1].Trim() : splt[1].Substring(0, triggerStart).Trim(), triggers));
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
                prevDialog = new Dialog(match.Groups[4].Value, match.Groups[5].Value, dialogChars[match.Groups[1].Value[0]], match.Groups[2].Success);
                if (match.Groups[3].Success)
                {
                    if (!prevDialog.IsAnswer)
                        throw new Exception("Bad type of dialog for used arguments");
                    string[] conditions = match.Groups[3].Value.Substring(1, match.Groups[3].Value.Length-2).Split(',');
                    prevDialog.AddConditions(conditions);
                    _keeper.AddConditions(conditions);
                }
                _dialogNames.Add(prevDialog.Name, prevDialog);
                if(prevDialog.Type == DialogType.Dialog)
                {
                    fillDialog = true;
                }
            }
            foreach(var item in _dialogRoutes)
            {
                item.dialog.AddRoute(new Route(item.dialog, _dialogNames[item.from], _dialogNames[item.to], _keeper, item.triggers));
            }
        }
        public Dialog GetDialogByName(string name) => _dialogNames[name].Clone();
        public ConditionKeeper GetConditionKeeper() => _keeper;
    }
}
