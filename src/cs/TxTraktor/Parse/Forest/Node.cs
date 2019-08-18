using System.Collections.Generic;
using System.Linq;
using TxTraktor.Compile.Model;

namespace TxTraktor.Parse.Forest
{
    internal class Node : NodeBase
    {
        private List<NodeBase> _children;
        public Node(Rule rule, int index, string localName=null, bool isHead=false, string semanticId=null) : base(NodeType.Node, index, localName, isHead, semanticId)
        {
            Rule = rule;
            _children = new List<NodeBase>();
        }

        private Node(Rule rule, List<NodeBase> children, int index, string localName, bool isHead, string semanticId)
            : base(NodeType.Node, index, localName, isHead, semanticId)
        {
            Rule = rule;
            _children = children;
        }
        public Rule Rule { get; }

        public bool IsPossibleList => Rule.IsPossibleList;

        public Token Head
        {
            get
            {
                var head = Children.FirstOrDefault(n => n.IsHead);
                if (head == null)
                    return FirstLeaf?.Token;

                return head.Type == NodeType.Leaf ? head.As<Leaf>().Token : head.As<Node>().Head;
            }
        }

        public Leaf FirstLeaf
        {
            get
            {
                if (!_children.Any())
                    return null;
                var node = _children[0];
                return node.Type == NodeType.Leaf ? node.As<Leaf>() : node.As<Node>().FirstLeaf;
            }
        }

        public bool IsValid
        {
            get
            {
                if (!Rule.HasValidator) 
                    return true;

                return Rule.Validator.Validate(Children);
            }
        }

        public override Token Token => Head;

        public IEnumerable<NodeBase> Children => _children;

        private void AddChild(NodeBase node)
        {
            _children.Add(node);
        }
        
        public Node Attache(NodeBase newItem)
        {
            var node = new Node(Rule, _children.ToList(), Index, LocalName, IsHead, SemanticId);
            node.AddChild(newItem);
            return node;
        }

        public Node DeepCopy()
        {
            return new Node(Rule, Children.Select(c=>c.Copy()).ToList(), Index, LocalName, IsHead, SemanticId);
        }
        
        public override NodeBase Copy()
        {
             return new Node(Rule, _children, Index, LocalName, IsHead, SemanticId);
        }

        public override string ToString()
        {
            return $"\n{Rule.Name} ({LocalName}, {Index}) -> [{string.Join(",", Children)}]";
        }

        public override void GetPositions(out int startPosition, out int endPosition)
        {
            Children.First().GetPositions(out startPosition, out int _);
            Children.Last().GetPositions(out _, out endPosition);
        }
    }
}