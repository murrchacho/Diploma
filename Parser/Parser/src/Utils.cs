using System;
using TitaniumAS.Opc.Client.Da;
using TitaniumAS.Opc.Client.Da.Browsing;

namespace Parser
{
    public class Utils
    {
        public OpcDaBrowseElement[] BrowseAllElements(IOpcDaBrowser browser, string itemId = null)
        {
            OpcDaBrowseElement[] elements = browser.GetElements(itemId);

            foreach (OpcDaBrowseElement element in elements)
            {
                if (!element.HasChildren)
                    continue;

                BrowseAllElements(browser, element.ItemId);
            }

            return elements;
        }
        public (string, int) FormatGroupName(string name)
        {
            int firstIndex = name.IndexOf("[");
            int lastIndex = name.IndexOf("]");

            if (firstIndex > -1 && lastIndex > -1)
            {
                string groupNumber = name.Substring(firstIndex + 1, lastIndex - firstIndex - 1);
                int number;
                int pointPosition = lastIndex + 1;
                if (int.TryParse(groupNumber, out number) && name.Length > pointPosition && name[pointPosition] == '.')
                {
                    return ($"contour_{number}", number);
                }
            }

            return (null, -1);
        }
    }
}
