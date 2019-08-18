namespace TxTraktor.Parse.Forest
{
    internal abstract class NodeBase
    {
        public NodeBase(NodeType type, 
                        int index, 
                        string localName,
                        bool isHead,
                        string semanticId)
        {
            Type = type;
            Index = index;
            LocalName = localName;
            IsHead = isHead;
            SemanticId = semanticId;
        }
        
        public int Index { get; }
        
        public string LocalName { get; set; }

        public bool HasLocalName => LocalName != null;
        
        public NodeType Type { get; }
        
        public bool IsHead { get; }
        
        public string SemanticId { get; }

        public T As<T>() where T : NodeBase
        {
            return (T) this;
        }

        public abstract Token Token { get; }
        
        public abstract NodeBase Copy();

        public abstract void GetPositions(out int startPosition, out int endPosition);
    }
}