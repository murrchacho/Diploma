using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using TitaniumAS.Opc.Client.Da;

namespace Parser.parser
{
    internal class Parser
    {
        public void Parse(IEnumerable<Condition> conditions, OpcDaItemValue[] values)
        {
            foreach (var condition in conditions)
            {
                foreach (OpcDaItemValue value in values)
                {
                    if (value.Value.GetType().ToString() != "System.Single[*]" && condition.Var == value.Item.ItemId)
                    {
                        foreach (Rules rule in condition.Rules)
                        {
                            string conditionInRule = rule.Condtion;
                            string conditionToCheck = conditionInRule.Replace("{self}", value.Value.ToString());

                            using (V8ScriptEngine scriptEngine = new V8ScriptEngine("jscript"))
                            {
                                string conditionToCheckResult = scriptEngine.Evaluate(conditionToCheck).ToString();
                                bool dependsOnResult = (conditionToCheckResult == rule.DependsOn);

                                if (dependsOnResult)
                                {
                                    Console.WriteLine($"Переменная {condition.Var}, произошло событие: {rule.Action}, сработало условие: {rule.Condtion}");
                                    Console.WriteLine(new string('\n', 2));
                                }
                            }
                        }  
                    }
                }
            }
        }
    }
}
