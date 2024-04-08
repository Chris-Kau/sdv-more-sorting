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
    //I eat bleh
    static class ChestMethods
    {
        internal static List<Item> getItems(object? sender, Chest chest)
        {
            List<Item> containerContents = new List<Item>();
            foreach (Item i in chest.Items)
                containerContents.Add(i);
            return containerContents;
        }

        internal static void ReplaceContents(object? sender, List<Item> ItemsList, StardewValley.Menus.ItemGrabMenu menu)
        {
            menu.ItemsToGrabMenu.actualInventory.Clear();
            foreach(Item i in ItemsList)
            {
                menu.ItemsToGrabMenu.actualInventory.Insert(0, i);
            }
        }
    }
}
