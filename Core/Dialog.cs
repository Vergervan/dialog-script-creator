using System.Collections.Generic;

namespace DialogScriptCreator
{
    public class Dialog
    {
        private DialogType _type;
        private string _name;
        private List<Route> _routes;
        private string _dialogValue;
        private bool _switchable;

        public bool IsDialog { get => _type == DialogType.Dialog; }
        public bool IsMonolog { get => _type == DialogType.Monolog; }
        public bool IsAnswer { get => _type == DialogType.Answer; }
        public bool Switchable { get => _switchable; }
        public IEnumerable<Route> Routes { get => _routes; }
        public int RoutesCount { get => _routes == null ? 0 : _routes.Count; }
        public DialogType Type { get => _type; }
        public string Name { get => _name; }
        public string Value { get => _dialogValue; }
        public Dialog() { }
        public Dialog(string dialogName, string dialogValue, DialogType type, bool switchable = false)
        {
            _name = dialogName;
            _type = type;
            _switchable = switchable;
            _dialogValue = dialogValue;
            if(type == DialogType.Dialog)
            {
                _routes = new List<Route>();
            }
        }
        public void AddRoute(Dialog from, Dialog to)
        {
            _routes.Add(new Route(from, to));
        }
        public Dialog Clone()
        {
            var newDialog = new Dialog(_name, _dialogValue, _type, _switchable);
            if(newDialog.IsDialog)
            {
                foreach(var item in _routes)
                {
                    newDialog.AddRoute(item.From.Clone(), item.To.Clone());
                }
            }
            return newDialog;
        }
    }
}
