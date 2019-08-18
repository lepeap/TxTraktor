using System;
using System.Collections.Generic;
using System.Linq;
using TxTraktor.Compile.Condition;
using TxTraktor.Source.Model;

namespace TxTraktor.Compile
{
    internal class ConditionManager
    {
        private static Dictionary<string, IConditionProvider> _condProvDic;

        static ConditionManager()
        {
            _condProvDic = new Dictionary<string, IConditionProvider>();
            var interfType = typeof(IConditionProvider);
            var assembly = interfType.Assembly;
            var condProviders = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(interfType) && !t.IsAbstract);
            foreach (var condProv in condProviders)
            {
                var prov = (IConditionProvider) Activator.CreateInstance(condProv);
                foreach (var key in prov.Keys)
                {
                    _condProvDic[key] = prov;
                }
            }

        }

        public ICondition GetCondition(RuleItem item)
        {
            IEnumerable<Source.Model.Condition> srcConditions = item.Conditions;
            ICondition mainCondition = null;

            if (item.Type == RuleItemType.Regex)
                mainCondition = new RegexCondition(item.Key);

            if (item.Type == RuleItemType.Terminal)
                mainCondition = new TextCondition(item.Key);

            if (item.Type == RuleItemType.Lemma)
                mainCondition = new LemmaCondition(item.Key);

            if (item.Type == RuleItemType.Morphology)
            {
                // главный морфологический ключ объединяется
                // с второстепенными морфологичекими условиями

                var morphKeys = item.Key.Split(',');

                if (item.Conditions != null)
                {
                    morphKeys = morphKeys.Concat(item.Conditions
                            .Where(c => c.Key == "морф")
                            .SelectMany(c => c.Values))
                        .ToArray();
                }

                mainCondition = new MorphologyCondition(morphKeys);
                srcConditions = srcConditions?.Where(x => x.Key != "морф");
            }

            if (item.HasConditions)
            {
                var adConds = _compileConditions(srcConditions).ToArray();

                if (adConds.Length == 1 && mainCondition == null)
                    mainCondition = adConds[0];
                else if (mainCondition == null)
                    mainCondition = new AndCondition(adConds);
                else
                    mainCondition = new AndCondition(new[] {mainCondition}.Concat(adConds));

            }

            return mainCondition;
        }

        private IEnumerable<ICondition> _compileConditions(IEnumerable<Source.Model.Condition> srcConditions)
        {
            foreach (var srcCondition in srcConditions)
            {
                if (_condProvDic.ContainsKey(srcCondition.Key))
                    yield return _condProvDic[srcCondition.Key].Create(srcCondition.Values);
                else
                    throw new NotImplementedException($"Condition key '{srcCondition.Key}' has no implementation");
            }
        }
    }
}
