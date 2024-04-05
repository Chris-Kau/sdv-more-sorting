using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace more_sorting
{
    static class SortOptions
    {
        //TODO: sorts items alphabetically A -> Z
        internal static void AlphaSort(object? sender, Chest chest, StardewValley.Menus.ItemGrabMenu menu)
        {
            //gets the items in the chest the player is currently in
            List<Item> containerContents = accessChest.getItems(sender, chest);
            //prints all the items into the console
            foreach (Item i in containerContents)
            {
                Console.WriteLine($"{i.Name}");
            }

            //clears the chest (just for testing purposes)
            for (int i = 0; i < containerContents.Count; i++)
            {
                menu.ItemsToGrabMenu.actualInventory.RemoveAt(0);
            }
        }
    }
}
