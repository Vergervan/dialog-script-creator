namespace DialogScriptCreator
{
    public class Route
    {
        private Dialog _from, _to;
        private bool _switchable = false;
        private bool _available = true;
        public bool Switchable { get => _switchable; }
        public bool Available { get => _available; }
        public Dialog From { get => _from; }
        public Dialog To { get => _to; }
        public Route(Dialog from, Dialog to)
        {
            _from = from;
            _to = to;
            _switchable = from.Switchable;
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
    }
}
