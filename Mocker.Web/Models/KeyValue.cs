namespace Mocker.Web.Models
{
    public class KeyValue
    {
        public KeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public KeyValue(string key)
        {
            Key = key;
            Value = string.Empty;
        }
        public KeyValue()
        {
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
