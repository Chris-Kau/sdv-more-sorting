using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Menus;
using Microsoft.Xna.Framework.Graphics;
using System.Security.Cryptography.X509Certificates;
namespace more_sorting
{
    internal sealed class ModEntry : Mod
    {
        Farmer person = Game1.player;
        Vector2 AlphaSortIconPos;
        Rectangle AlphaSortIconArea;
        Vector2 PriceSortIconPos;
        Rectangle PriceSortIconArea;
        public override void Entry(IModHelper helper)
        {
            //O keybind should sort items alphabetically
            Helper.Events.Input.ButtonPressed += this.Keybinds;
            //Adds Button for the user to "click"
            Helper.Events.Display.RenderedActiveMenu += this.AddAlphaSortButton;
            //Gives the Illusion of the user "clicking" on the buttons
            Helper.Events.Input.ButtonPressed += this.ClickedAlphaSort;
        }

        private void Keybinds(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
            //tests to see if the player clicked O while in a clickable menu
            if((Game1.activeClickableMenu is StardewValley.Menus.ItemGrabMenu menu))
            {
                //checks to see if that clickable menu is a chest
                if (menu.sourceItem is Chest chest)
                {
                    //sorts alphabetically if user clicks O
                    if ((e.Button is SButton.O))
                    {
                        SortOptions.AlphaSort(sender, chest, menu);

                    }
                    //sorts by price if user clicks I
                    else if (e.Button is SButton.I)
                    {
                        SortOptions.PriceSort(sender, chest, menu);
                    }
                }

            }
        }

        private void AddAlphaSortButton(object? sender, RenderedActiveMenuEventArgs e)
        {
            if(Game1.activeClickableMenu is ItemGrabMenu menu)
            {
                //Responsible for creating the button and drawing it to the screen 
                Texture2D AlphaSortIcon = Helper.ModContent.Load<Texture2D>("./assets/AlphaSortIcon.png");
                //Sets the Icon to be in the position to the right of the organize button
                AlphaSortIconPos = new Vector2(menu.organizeButton.getVector2().X + 16 + 64, menu.organizeButton.getVector2().Y);
                //Makes a new rectangle which is then used to check if the mouse is in that area in ClickedAlphaSort()
                AlphaSortIconArea = new Rectangle((int)AlphaSortIconPos.X, (int)AlphaSortIconPos.Y, 64, 64);
                //Draws the Icon
                e.SpriteBatch.Draw(AlphaSortIcon, AlphaSortIconPos, Color.White);

                //Repeated process but for PriceSortIcon, this time to the right of the Quick Stack to Existing Slots button
                Texture2D PriceSortIcon = Helper.ModContent.Load<Texture2D>("./assets/PriceSortIcon.png");
                PriceSortIconPos = new Vector2(menu.fillStacksButton.getVector2().X + 16 + 64, menu.fillStacksButton.getVector2().Y);
                PriceSortIconArea = new Rectangle((int)PriceSortIconPos.X, (int)PriceSortIconPos.Y, 64, 64);
                e.SpriteBatch.Draw(PriceSortIcon, PriceSortIconPos, Color.White);

                //Draws the mouse so the cursor is not under the button
                menu.drawMouse(Game1.spriteBatch);

            }
        }


        private void ClickedAlphaSort(object? sender, ButtonPressedEventArgs e)
        {
            if(e.Button is SButton.MouseLeft && Game1.activeClickableMenu is ItemGrabMenu menu)
            {
                               
                Vector2 mousePosition = new Vector2(Game1.getMouseXRaw(), Game1.getMouseYRaw());
                if(menu.sourceItem is Chest chest)
                {
                    //Checks to see if the player clicks on the AlphaSortIcon, and if the player does, sort it Alphabetically
                    if (AlphaSortIconArea.Contains(mousePosition.X, mousePosition.Y))
                    {
                        SortOptions.AlphaSort(sender, chest, menu);
                    }
                    //Checks to see if the player clicks on the PriceSortIcon
                    else if (PriceSortIconArea.Contains(mousePosition.X, mousePosition.Y))
                    {
                        SortOptions.PriceSort(sender, chest, menu);
                    }
                }

            }
        }



    }
}
