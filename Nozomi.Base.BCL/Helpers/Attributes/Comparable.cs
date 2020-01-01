namespace Nozomi.Base.BCL.Helpers.Attributes
{
    /// <summary>
    /// https://msisgreat.net/2009/06/13/c-enum-to-string-using-attributes/
    /// </summary>
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