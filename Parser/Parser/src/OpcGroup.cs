using s = Parser.parser;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TitaniumAS.Opc.Client.Da;

namespace Parser
{
    /// <summary>
    /// Список элементов OpcDaGroup
    /// </summary>
    internal class OpcGroup
    {
        private List<CustomOpcDaGroup> groups = new List<CustomOpcDaGroup>();
        private Utils utils = new Utils();
        public string GroupName { get; set; }
        public string Conditions { get; set; }
        public List<CustomOpcDaGroup> GetGroups { get { return groups; } }
        public void CreateGroup(int groupId, string groupName, List<OpcDaItemDefinition> items, OpcDaServer server)
        {
            CustomOpcDaGroup group = new CustomOpcDaGroup(groupId, server.AddGroup(groupName));
            group.AddItems(items);
            groups.Add(group);
        }
        public void AddToGroup(List<OpcDaItemDefinition> items, int groupIndex)
        {
            groups[groupIndex].AddItems(items);
        }
        public (int, int) IsGroupExist(string name)
        {   
            (string groupName, int groupId) = utils.FormatGroupName(name);
            return (groups.FindIndex(group => group.GroupName == groupName), groupId);
        }
        public void SortList()
        {
            groups.Sort();
        }
        public void SetConditions(string conditions)
        {
            this.Conditions = conditions;
        }
        async public Task<List<String>> ReadAll()
        {
            List<String> results = new List<String>();
            //ToDo: сделать что-нибудь с наименованиями, ужас же
            s.Parser parser = new s.Parser();

            foreach (CustomOpcDaGroup group in groups)
            {
                if (this.Conditions != null)
                {
                    OpcDaItemValue[] values = await group.PrintValues();

                    List<s.Condition> resultObjects = JsonConvert.DeserializeObject<List<s.Condition>>(Conditions);
                    IEnumerable<s.Condition> contourConditions = resultObjects.Where(p => p.Contour == group.GroupName);

                    parser.Parse(contourConditions, values);
                }
                else 
                {
                    Console.WriteLine("Нет условий, первичная настройка..");
                    Console.WriteLine("\n");
                    return null;
                }
            }

            Console.WriteLine("\n");

            return results;
        }
    }
}
