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
            //O keybind should sort items alphabetically
            Helper.Events.Input.ButtonPressed += this.keyO;
        }

        private void keyO(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
            //tests to see if the player clicked O while in a clickable menu
            if ((e.Button is SButton.O) && (Game1.activeClickableMenu is StardewValley.Menus.ItemGrabMenu menu))
            { 
                //checks to see if that clickable menu is a chest
                if(menu.sourceItem is Chest chest)
                {
                    SortOptions.AlphaSort(sender, chest, menu);
                }

            }

        }

    }
}
