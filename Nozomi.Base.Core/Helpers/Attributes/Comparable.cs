namespace Nozomi.Base.Core.Helpers.Attributes
{
    public class Comparable : System.Attribute
    {
        private bool _value;
        public Comparable(bool value)
        {
            _value = value;
        }
        public bool Value
        {
            get { return _value; }
        }
    }
}