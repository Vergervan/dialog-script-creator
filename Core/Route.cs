using System.Collections.Generic;
using System.Linq;

namespace DialogScriptCreator
{
    public class Route : IUpdateConditions
    {
        private Dialog _from, _to, _parent;
        private bool _switchable = false;
        private bool _available = true;
        private ConditionKeeper _keeper;
        private string[] _triggers;
        private bool _conditionsMet = true;
        public bool Switchable { get => _switchable; }
        public bool Available { get => _available; }
        //Checks if all conditions are true
        public bool ConditionsMet => _conditionsMet;
        public int ConditionsCount => _from.Conditions.Count();
        public bool HasConditions => ConditionsCount > 0;
        public bool HasTriggers => _triggers != null && _triggers.Length > 0;
        public ICollection<string> Triggers => _triggers;
        public Dialog From { get => _from; }
        public Dialog To { get => _to; }
        public Dialog Parent => _parent;
        public Route(Dialog parent, Dialog from, Dialog to, ConditionKeeper keeper, params string[] triggers)
        {
            _parent = parent;
            _from = from.Clone(this);
            _to = to.Clone(this);
            _switchable = from.Switchable;
            _keeper = keeper;
            _keeper.SubscribeOnUpdates(this);
            ((IUpdateConditions)this).OnConditionsUpdate();
            _triggers = triggers;
        }
        ~Route()
        {
            _keeper.UnsubscribeFromUpdates(this);
        }
        public void TurnOn()
        {
            if (!_switchable)
                throw new System.Exception();
            _available = true;
        }
        public void TurnOff()
        {
            if (!_switchable)
                throw new System.Exception();
            _available = false;
        }
        public void Switch() => _available = !_available;
        public static bool operator ==(Route obj1, Route obj2)
        {
            return (obj1._from.Name == obj2._from.Name && obj1._to.Name == obj2._to.Name);
        }
        public static bool operator !=(Route obj1, Route obj2)
        {
            return (obj1._from.Name != obj2._from.Name || obj1._to.Name != obj2._to.Name);
        }

        void IUpdateConditions.OnConditionsUpdate()
        {
            foreach(var str in _from.Conditions)
            {
                if (!_keeper.GetConditionValue(str))
                {
                    _conditionsMet = false;
                    return;
                }
            }
            _conditionsMet = true;
        }
        public Route Clone(Dialog parent) => new Route(parent, _from, _to, _keeper, _triggers);
    }
}
