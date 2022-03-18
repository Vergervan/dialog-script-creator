using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DialogScriptCreator
{
    public class ConditionKeeper
    {
        private Dictionary<string, bool> _conditions;
        public ConditionKeeper()
        {
            _conditions = new Dictionary<string, bool>();
        }
        public ConditionKeeper(string[] conditions)
        {
            _conditions = new Dictionary<string, bool>();
            foreach(var str in conditions) 
                _conditions.Add(str, false);
        }
        public void SetConditionValue(string name, bool b)
        {
            if (!_conditions.ContainsKey(name))
                throw new KeyNotFoundException();
            _conditions[name] = b;
        }
        public bool GetConditionValue(string name)
        {
            if (!_conditions.ContainsKey(name))
                throw new KeyNotFoundException();
            return _conditions[name];
        }
        public void AddCondition(string name)
        {
            if (_conditions.ContainsKey(name)) return;
            _conditions.Add(name, false);
        }
        public void AddConditions(string[] conditions)
        {
            foreach (var str in conditions)
            {
                if (_conditions.ContainsKey(str)) continue;
                _conditions.Add(str, false);
            }
        }
    }
}
