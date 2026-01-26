using System;

namespace Escalon.Traits
{
    /// <summary>
    /// Used to refer to a modifiable value
    /// </summary>
    [Serializable]
    public struct ActionValue 
    {
        public string Name;

        public ActionValue(string name)
        {
            Name = name;
        }
    
        public static implicit operator string(ActionValue actionValue)
        {
            return actionValue.Name;
        }
    
        public static implicit operator ActionValue(string s)
        {
            return new ActionValue(s);
        }
    }
}
