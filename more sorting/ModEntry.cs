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
using System.Reflection.Emit;
using System.Xml.Linq;
namespace more_sorting
{
    internal sealed class ModEntry : Mod
    {
        Farmer person = Game1.player;

        public override void Entry(IModHelper helper)
        {
            //Adds Button for the user to "click"
            Helper.Events.Display.RenderedActiveMenu += this.HoverEffect;
            //Gives the Illusion of the user "clicking" on the buttons
            Helper.Events.Input.ButtonPressed += this.ClickedSortButtons;
            //Updates the button everytime the player opens a chest
            Helper.Events.Display.MenuChanged += this.CreateButtons;
            //Updates the button pos evertime the player resizes the window, so the buttons do not go off screen.
            Helper.Events.Display.WindowResized += this.RecreateButtons;
        }

        private void RecreateButtons(object? sender, WindowResizedEventArgs e)
        {
            if(Game1.activeClickableMenu is ItemGrabMenu menu)
            {
                if(menu.sourceItem is Chest chest)
                {
                    SortButtonMethods.MakeAlphaIcon(menu, Helper.ModContent.Load<Texture2D>("./assets/AlphaSortIcon.png"));
                    SortButtonMethods.MakePriceIcon(menu, Helper.ModContent.Load<Texture2D>("./assets/PriceSortIcon.png"));
                }
            }

        }

        private void CreateButtons(object? sender, MenuChangedEventArgs e)
        {
            if(e.NewMenu is ItemGrabMenu menu)
            {
                if(menu.sourceItem is Chest chest)
                {
                    SortButtonMethods.MakeAlphaIcon(menu, Helper.ModContent.Load<Texture2D>("./assets/AlphaSortIcon.png"));
                    SortButtonMethods.MakePriceIcon(menu, Helper.ModContent.Load<Texture2D>("./assets/PriceSortIcon.png"));
                }
            }
        }
        private void HoverEffect(object? sender, RenderedActiveMenuEventArgs e)
        {
            if(Game1.activeClickableMenu is ItemGrabMenu menu)
            {
                //get mouse position for later uses
                Vector2 mousePosition = new Vector2(Game1.getMouseXRaw(), Game1.getMouseYRaw());
                //these are in charge of drawing the icons as well as making a scale up/down effect when hovering.
                if (SortButtonMethods.PriceSortIcon is not null && SortButtonMethods.AlphaSortIcon is not null)
                {
                    SortButtonMethods.HoverEffect(SortButtonMethods.PriceSortIcon, (int)mousePosition.X, (int)mousePosition.Y, 1f, 1.1f, 0.02f);
                    SortButtonMethods.HoverEffect(SortButtonMethods.AlphaSortIcon, (int)mousePosition.X, (int)mousePosition.Y, 1f, 1.1f, 0.02f);
                }
                //Draws the mouse so the cursor is not under the button
 
                menu.drawMouse(Game1.spriteBatch);
            }
        }
        private void ClickedSortButtons(object? sender, ButtonPressedEventArgs e)
        {
            bool reverse = false;
            //if the user right clicks button, reverse the sorted contents.
            if (e.Button is SButton.MouseRight)
                reverse = true;
            if((e.Button is SButton.MouseLeft || e.Button is SButton.MouseRight) && Game1.activeClickableMenu is ItemGrabMenu menu)
            {
                               
                Vector2 mousePosition = new Vector2(Game1.getMouseXRaw(), Game1.getMouseYRaw());
                if(menu.sourceItem is Chest chest)
                {
                    //Checks to see if the player clicks on the AlphaSortIcon, and if the player does, sort it Alphabetically
                    if (SortButtonMethods.AlphaSortIconArea.Contains(mousePosition.X, mousePosition.Y))
                    {
                        SortOptions.AlphaSort(sender, chest, menu, reverse);
                        SortButtonMethods.Clicked(SortButtonMethods.AlphaSortIcon, 1f, 1.1f);
                        Game1.playSound("Ship");
                    }
                    //Checks to see if the player clicks on the PriceSortIcon
                    else if (SortButtonMethods.PriceSortIconArea.Contains(mousePosition.X, mousePosition.Y))
                    {
                        SortOptions.PriceSort(sender, chest, menu, reverse);
                        SortButtonMethods.Clicked(SortButtonMethods.PriceSortIcon, 1f, 1.1f);
                        Game1.playSound("Ship");
                    }
                }

            }
        }



    }
}
