using Parser.connection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TitaniumAS.Opc.Client.Da;
using TitaniumAS.Opc.Client.Da.Browsing;

namespace Parser
{
    internal class Initialize
    {
        public async Task Start(Uri url, SocketConnection server)
        {
            using (OpcDaServer opcServer = new OpcDaServer(url))
            {
                Utils utils = new Utils();
                opcServer.Connect();

                OpcDaBrowserAuto browser = new OpcDaBrowserAuto(opcServer);
                OpcDaBrowseElement[] browseElements = utils.BrowseAllElements(browser);
                OpcGroup groups = new OpcGroup();
                List<OpcDaItemsDefinitionsGroup> OpcDaItemsDefinitions = new List<OpcDaItemsDefinitionsGroup>();

                foreach (OpcDaBrowseElement browseElement in browseElements)
                {
                    OpcDaItemDefinition item = new OpcDaItemDefinition { ItemId = browseElement.ItemId };

                    (string groupName, int groupId) = utils.FormatGroupName(item.ItemId);

                    int itemDefinitionsIndex = OpcDaItemsDefinitions.FindIndex(element => element.GroupName == groupName);
                    if (itemDefinitionsIndex > 0)
                    {
                        OpcDaItemsDefinitions[itemDefinitionsIndex].AddElement(item);
                    }
                    else
                    {
                        OpcDaItemsDefinitions.Add(new OpcDaItemsDefinitionsGroup(groupId, groupName, item));
                    }
                }

                foreach (OpcDaItemsDefinitionsGroup element in OpcDaItemsDefinitions)
                {
                    string groupName = element.GroupName;

                    if (groupName != null)
                    {
                        (int groupIndex, _) = groups.IsGroupExist(groupName);

                        if (groupIndex > 0)
                        {
                            groups.AddToGroup(element.items, groupIndex);
                        }
                        else
                        {
                            groups.CreateGroup(element.GroupId, groupName, element.items, opcServer);
                        }
                    }
                }

                groups.SortList();

                while (true)
                {
                    server.SendData("getConditions");
                    server.ReceiveData(groups);
                    await groups.ReadAll();

                    Thread.Sleep(1000);
                }
            }
        }
    }
}
