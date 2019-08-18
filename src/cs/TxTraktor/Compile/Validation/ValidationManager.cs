using System.Collections.Generic;
using System.Linq;
using TxTraktor.Source.Model;

namespace TxTraktor.Compile.Validation
{
    internal class ValidationManager
    {
        public IValidator GetValidator(Rule rule)
        {

            int i = 0;
            var soglDic = new Dictionary<string, List<int>>();
            var gramKeyDic = new Dictionary<string, string>();
            foreach (var item in rule.Items)
            {
                foreach (var condition in item.Conditions.ToArray())
                {
                    if (condition.Key == "согл" || condition.Key == "sogl")
                    {
                        var condMas = condition.Values.ToArray();
                        if (condMas.Length != 2)
                            throw new CfgCompileException($"Wrong keys count for validation '{condition.Key}' in item '{item.Key}' in rule '{rule.Name}'");

                        var groupKey = condMas[0];
                        var gramKey = condMas[1];

                        if (gramKeyDic.ContainsKey(groupKey) && gramKeyDic[groupKey] != gramKey)
                            throw new CfgCompileException($"Sogl items in group '{groupKey}' in rule '{rule.Name}' has different grammeme keys");

                        if (!gramKeyDic.ContainsKey(groupKey))
                            gramKeyDic[groupKey] = gramKey;
                        
                        if (!soglDic.ContainsKey(groupKey))
                            soglDic[groupKey] = new List<int>();
                        
                        soglDic[groupKey].Add(i);
                        
                        item.RemoveCondition(condition);
                    }
                }
                i++;
            }

            if (soglDic.Count == 0)
                return null;

            var list = new List<IValidator>();
            foreach (var kp in soglDic)
            {
                if (kp.Value.Count <= 1)
                    throw new CfgCompileException($"Sogl group '{kp.Key}' in rule '{rule.Name}' has {kp.Value.Count} items");
                
                list.Add(new SoglValidator(gramKeyDic[kp.Key], kp.Value));
            }


            return list.Count == 1 ? list[0] : new AndJoinValidator(list);
        }
    }
}