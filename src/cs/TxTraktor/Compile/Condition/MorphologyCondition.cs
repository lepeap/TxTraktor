using System.Linq;

namespace TxTraktor.Compile.Condition
{
    internal class MorphologyCondition : ConditionBase
    {
        public MorphologyCondition()
        {
            
        }

        public MorphologyCondition(string[] keys)
        {
            _init(keys);
        }
        public string[] Keys { get; set; }
        
        public override void  Init(string[] args)
        {
            _init(args);
        }

        private void _init(string[] args)
        {
            Keys = args;
        }
        public override bool IsValid(Token token)
        {
            if (token.Morphs == null)
                return false;
            
            var m = token.Morphs.First();
            return Keys.All(k =>  m.Grams.Contains(k));
        }
        
        public class Provider : ConditionProvider<MorphologyCondition>
        {
            public override ConditionArgType ArgType => ConditionArgType.Array;
            public override string[] Keys => new[] {"морф", "morph"};
        }
        
    }
}