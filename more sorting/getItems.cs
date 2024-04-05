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
    static class accessChest
    {
        internal static List<Item> getItems(object? sender, Chest chest)
        {
            List<Item> containerContents = new List<Item>();
            foreach (Item i in chest.Items)
                containerContents.Add(i);
            return containerContents;
        }
    }
}
