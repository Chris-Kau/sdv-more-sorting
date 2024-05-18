using StardewValley.Objects;
using StardewValley;

namespace more_sorting
{
    static class ChestMethods
    {
        internal static List<Item> getItems(object? sender, StardewValley.Menus.ItemGrabMenu menu)
        {
            List<Item> containerContents = new List<Item>();
            foreach (Item i in menu.ItemsToGrabMenu.actualInventory)
            {
                containerContents.Add(i);
            }
            return containerContents;
        }

        internal static void ReplaceContents(object? sender, List<Item> ItemsList, StardewValley.Menus.ItemGrabMenu menu)
        {
            menu.ItemsToGrabMenu.actualInventory.Clear();
            foreach (Item i in ItemsList)
            {
                menu.ItemsToGrabMenu.actualInventory.Insert(0, i);
            }
        }
    }
}
