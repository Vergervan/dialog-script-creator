using System.Collections.Generic;

namespace DialogScriptCreator
{
    public class Dialog
    {
        private DialogType _type;
        private string _name;
        private List<Route> _routes;
        private List<string> _conditions;
        private string _dialogValue;
        private bool _switchable;
        private Route _parentRoute;

        public bool IsDialog { get => _type == DialogType.Dialog; }
        public bool IsMonolog { get => _type == DialogType.Monolog; }
        public bool IsAnswer { get => _type == DialogType.Answer; }
        public bool Switchable { get => _switchable; }
        public ICollection<Route> Routes { get => _routes; }
        public ICollection<string> Conditions { get => _conditions; }
        public int RoutesCount { get => _routes == null ? 0 : _routes.Count; }
        public Route ParentRoute => _parentRoute;
        public bool HasAvailableRoutes 
        { 
            get
            {
                if (!IsDialog) return false;
                foreach (var item in Routes)
                {
                    if (item.Available)
                        return true;
                }
                return false;
            } 
        }
        public int AvailableRoutesCount
        {
            get
            {
                int counter = 0;
                foreach (var item in Routes)
                {
                    if (item.Available)
                        ++counter;
                }
                return counter;
            }
        }
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
            switch (type)
            {
                case DialogType.Dialog:
                    _routes = new List<Route>();
                    break;
                case DialogType.Answer:
                    _conditions = new List<string>();
                    break;
            }
        }
        public int GetAvailableRoutesCount(bool withConditionsMet = false)
        {
            int counter = 0;
            foreach (var item in Routes)
            {
                if (item.Available && (!withConditionsMet || item.ConditionsMet))
                    ++counter;
            }
            return counter;
        }
        public void AddRoute(Route route)
        {
            _routes.Add(route);
        }
        public void AddCondition(string name)
        {
            _conditions.Add(name);
        }
        public void AddConditions(IEnumerable<string> conditions)
        {
            _conditions.AddRange(conditions);
        }
        public Dialog Clone(Route parentRoute = null)
        {
            var newDialog = new Dialog(_name, _dialogValue, _type, _switchable);
            newDialog._parentRoute = parentRoute;
            if (newDialog.IsDialog)
            {
                foreach (var item in _routes)
                {
                    newDialog.AddRoute(item.Clone(newDialog));
                }
            }
            else if (newDialog.IsAnswer)
            {
                newDialog.AddConditions(Conditions);
            }
            return newDialog;
        }
    }
}
