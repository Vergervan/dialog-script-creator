using System;

namespace DialogScriptCreator
{
    public class ScriptSyntaxException : Exception
    {
        public ScriptSyntaxException() : base() { }
        public ScriptSyntaxException(string message) : base(message) { }
    }
}
