using System.Collections.Generic;
using TitaniumAS.Opc.Client.Da;

namespace Parser
{
    /// <summary>
    /// Список элементов OpcDaItemDefinition
    /// </summary>
    internal class OpcDaItemsDefinitionsGroup
    {
        public List<OpcDaItemDefinition> items = new List<OpcDaItemDefinition>();
        /// <summary>
        /// GroupName - имя списка, каждый контур имеет свой OpcDaItemsDefinitionsGroup из которого в дальнейшем формируются OpcDaGroup
        /// item - новый объект в списке 
        /// </summary>
        public OpcDaItemsDefinitionsGroup(int groupId, string groupName, OpcDaItemDefinition item)
        {
            GroupId = groupId;
            GroupName = groupName;
            items.Add(item);
        }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public void AddElement(OpcDaItemDefinition item)
        {
            items.Add(item);
        }
    }
}
