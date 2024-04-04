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
        Farmer person = Game1.player;
        public override void Entry(IModHelper help)
        {

            //Helper.Events.Display.MenuChanged += this.SortItems;
            Helper.Events.Input.ButtonPressed += this.PressO;
        }

        private void SortItems(object? sender, Chest chest, StardewValley.Menus.ItemGrabMenu menu)
        {
            //gets the items in the chest the player is currently in
            List<Item> containerContents = accessChest.getItems(sender, chest);
            //prints all the items into the console
            foreach (Item i in containerContents)
            {
                this.Monitor.Log($"{i.Name}", LogLevel.Debug);
            }

            //clears the chest (just for testing purposes)
            for(int i = 0; i < containerContents.Count; i++)
            {
                menu.ItemsToGrabMenu.actualInventory.RemoveAt(0);
            }
        }

        private void PressO(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
            //tests to see if the player clicked O while in a clickable menu
            if ((e.Button is SButton.O) && (Game1.activeClickableMenu is StardewValley.Menus.ItemGrabMenu menu))
            { 
                //checks to see if that clickable menu is a chest
                if(menu.sourceItem is Chest chest)
                {
                    this.SortItems(sender, chest, menu);
                }

            }

        }

    }
}
