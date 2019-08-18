namespace TxTraktor.Extract
{
    public class ExtractionValue
    {
        public ExtractionValue(object value, ValueType type, string semanticId=null)
        {
            Value = value;
            Type = type;
            SemanticId = semanticId;
        }
        
        public object Value { get;  }
        
        public ValueType Type { get; }
        
        public string SemanticId { get; }

        public T GetValue<T>()
        {
            return (T) Value;
        }
    }
}