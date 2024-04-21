using StardewValley;
using StardewValley.Objects;

namespace more_sorting
{
    static class SortOptions
    {
        private static void SelectionSort(List<Item> items, string method)
        {
            int n = items.Count;
            for(int i = 0; i < n - 1; i++)
            {
                int minidx = i;
                for(int j = i + 1; j < n; j++)
                {
                    if(method == "alpha")
                    {
                        int result = string.Compare(items[minidx].Name, items[j].Name);
                        //if result < 0, that means that string j comes before string j+1 alphabetically
                        //if result > 0, then that means string j comes after string j+1 alphabetically
                        //if result == 0, then the strings are the same.
                        if (result < 0)
                        {
                            minidx = j;
                        }
                    }
                    if(method == "price")
                    {
                        if(items[j].sellToStorePrice() < items[minidx].sellToStorePrice())
                        {
                            minidx = j;
                        }
                        //if prices are the same, sort them alphabetically
                        else if(items[j].sellToStorePrice() == items[minidx].sellToStorePrice())
                        {
                            int result = string.Compare(items[minidx].Name, items[j].Name);
                            if(result < 0)
                            {
                                minidx = j;
                            }
                        }
                    }
                }
                Item temp = items[minidx];
                items[minidx] = items[i];
                items[i] = temp;
            }
        }


        internal static void AlphaSort(object? sender, Chest chest, StardewValley.Menus.ItemGrabMenu menu, bool reverse)
        {
            //gets the items in the chest the player is currently in
            List<Item> chestContents = ChestMethods.getItems(sender, chest);
            //bubble sort for test, but we should 100% not use bubble sort 
            SelectionSort(chestContents, "alpha");
            //if user right clicks, we reverse the list
            if (reverse)
                chestContents.Reverse();
            //replaces the contents of the chest with the sorted items:
            ChestMethods.ReplaceContents(sender, chestContents, menu);
        }

        internal static void PriceSort(object? sender, Chest chest, StardewValley.Menus.ItemGrabMenu menu, bool reverse)
        {
            List<Item> chestContents = ChestMethods.getItems(sender, chest);
            SelectionSort(chestContents, "price");
            if (reverse)
                chestContents.Reverse();
            ChestMethods.ReplaceContents(sender, chestContents, menu);
        }


    }
}
