using StardewModdingAPI.Events;
using StardewModdingAPI;
using StardewValley.Objects;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace more_sorting
{
    static class accessChest
    {
        internal static List<Item> getItems(object? sender, MenuChangedEventArgs e)
        {
            List<Item> containerContents = new List<Item>();
            if (e.NewMenu is StardewValley.Menus.ItemGrabMenu menu)
            {
                //chest, includes stone chest and big chests
                if (menu.sourceItem is Chest chest)
                {
                    foreach (Item i in chest.Items)
                    {
                        containerContents.Add(i);
                    }
                }
            }
            return containerContents;
        }
    }
}
