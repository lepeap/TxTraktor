namespace TxTraktor.Compile.Condition
{
    internal class LemmaCondition : ConditionBase
    {
        public LemmaCondition()
        {
            
        }

        public LemmaCondition(string lemma)
        {
            Lemma = lemma;
        }
        public string Lemma { get; private set; }

        public override void  Init(string[] args)
        {
            if (args.Length > 1)
            {
                throw new WrongConditionArgsException("Lemma condition cant have multiple args");
            }
            if (args.Length == 0)
            {
                throw new WrongConditionArgsException("Lemma condition cant have zero args");
            }

            Lemma = args[0];
        }
        
        public override bool IsValid(Token token)
        {
            return token.HasLemma(Lemma);
        }

        public override string ToString()
        {
            return $"l:{Lemma}";
        }
        
        
        public class Provider : ConditionProvider<LemmaCondition>
        {
            public override ConditionArgType ArgType => ConditionArgType.None;
            public override string[] Keys => new[] {"лемма", "lemma"};
        }
    }
}