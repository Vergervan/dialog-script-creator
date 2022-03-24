using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DialogScriptCreator
{
    public interface IUpdateConditions
    {
        void OnConditionsUpdate();
    }

    public class ConditionKeeper
    {
        private Dictionary<string, bool> _conditions;
        private List<IUpdateConditions> subscribers = new List<IUpdateConditions>();
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
        public bool HasCondition(string name) => _conditions.ContainsKey(name);
        public void SetConditionValue(string name, bool b)
        {
            if (!_conditions.ContainsKey(name))
                throw new KeyNotFoundException();
            _conditions[name] = b;
            UpdateSubscribers();
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
        private void UpdateSubscribers()
        {
            lock (subscribers)
            {
                foreach (var sub in subscribers)
                {
                    sub.OnConditionsUpdate();
                }
            }
        }
        public void SubscribeOnUpdates(IUpdateConditions subscriber)
        {
            subscribers.Add(subscriber);
        }
        public void UnsubscribeFromUpdates(IUpdateConditions subscriber)
        {
            subscribers.Remove(subscriber);
        }
    }
}
