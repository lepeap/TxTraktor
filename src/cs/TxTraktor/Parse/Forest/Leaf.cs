using TxTraktor.Compile.Model;

namespace TxTraktor.Parse.Forest
{
    internal class Leaf : NodeBase
    {
        public Leaf(TermBase term, Token token, int index, string localName, bool isHead, string semanticId) 
            : base(NodeType.Leaf, index, localName, isHead, semanticId)
        {
            Term = term;
            Token = token;
        }
        public TermBase Term { get; }
        public override Token Token { get; }

        public override NodeBase Copy()
        {
            return new Leaf(Term, Token, Index, LocalName, IsHead, SemanticId);
        }

        public override string ToString()
        {
            return $"{Token.Text} ({LocalName}, {Index})";
        }

        public override void GetPositions(out int startPosition, out int endPosition)
        {
            startPosition = Token.StartPosition;
            endPosition = Token.EndPosition;
        }
    }
}