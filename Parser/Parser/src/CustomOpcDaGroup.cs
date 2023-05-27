using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TitaniumAS.Opc.Client.Da;

namespace Parser
{
    internal class CustomOpcDaGroup : IComparable
    {
        public CustomOpcDaGroup(int groupId, OpcDaGroup group)
        {
            GroupId = groupId;
            Group = group;
        }
        public int GroupId { get; set; }
        public string GroupName {
            get { return Group.Name; } 
            set { Group.Name = value; } 
        }
        public OpcDaGroup Group { get; set; }
        private void printFormatedHeaders()
        {
            Console.WriteLine(" {0, -18}{1}", "Название группы:", GroupName);
            Console.WriteLine(" {0, -18}{1}", "Метка времени:", DateTime.Now);
            Console.WriteLine(" " + new string('-', 75));
            Console.WriteLine(" " + "|{0,34}{1,28} {2, -10}|", "Поле", "|", "Значение");
            Console.WriteLine(" " + new string('-', 75));
        }
        public int CompareTo(object obj)
        {
            CustomOpcDaGroup group = obj as CustomOpcDaGroup;
            if (group == null) return 1;

            return this.GroupId.CompareTo(group.GroupId);
        }
        public void AddItems(List<OpcDaItemDefinition> items)
        {
            Group.AddItems(items);
        }
        async public Task<OpcDaItemValue[]> ReadAsync()
        {
           return await Group.ReadAsync(Group.Items);
        }
        async public Task<OpcDaItemValue[]> PrintValues() 
        {
            printFormatedHeaders();
            OpcDaItemValue[] readedValues = await Group.ReadAsync(Group.Items);

            if (readedValues != null)
            {
                foreach (OpcDaItemValue value in readedValues)
                {
                    if (value.Value.GetType().ToString() != "System.Single[*]")
                    {
                        Console.WriteLine(" | {0, -60}| {1,-10}|", value.Item.ItemId, value.Value);
                    }
                }
                Console.WriteLine(" " + new string('-', 75));
            }
            else
            {
                Console.WriteLine("Не могу получить значения для контура {0}", GroupName);
            }
            Console.WriteLine('\n');

            return readedValues;
        }
    }
}
