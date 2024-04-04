using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Objects;

namespace more_sorting
{
    internal sealed class ModEntry : Mod
    {
        //Farmer person = Game1.player;
        public override void Entry(IModHelper help)
        {

            Helper.Events.Display.MenuChanged += this.sortItems;
        }

        private void sortItems(object? sender, MenuChangedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
            List<Item> containerContents = accessChest.getItems(sender, e);
            foreach(Item i in containerContents)
            {
                this.Monitor.Log($"{i.Name}", LogLevel.Debug);
            }

        }

    }
}
