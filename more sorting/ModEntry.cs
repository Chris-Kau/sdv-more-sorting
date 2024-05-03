using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using Microsoft.Xna.Framework.Graphics;

namespace more_sorting
{
    internal sealed class ModEntry : Mod
    {
        private bool HasBetterChests;
        public override void Entry(IModHelper helper)
        {
            HasBetterChests = this.Helper.ModRegistry.IsLoaded("furyx639.BetterChests");
            //Gives the Illusion of the user "clicking" on the buttons
            Helper.Events.Input.ButtonPressed += this.ClickedSortButtons;
            //Updates the button everytime the player opens a chest
            Helper.Events.Display.MenuChanged += this.CreateButtonsOnMenuChanged;
            //Updates the button everytime the player resizes the window, so the buttons do not go off screen while in the chest.
            Helper.Events.Display.WindowResized += this.RecreateButtonsOnWindowResize;
            //in charge of the buttons' hover effects
            Helper.Events.Display.RenderedActiveMenu += this.HoverEffect;
        }

        private void RecreateButtonsOnWindowResize(object? sender, WindowResizedEventArgs e)
        {
            MakeButtons(sender);
        }

        private void MakeButtons(object? sender)
        {
            Console.WriteLine($"Accessed MakeButtons()");
            if (Game1.activeClickableMenu is ItemGrabMenu menu)
            {
                if (menu.source == 1 &&  menu.sourceItem is StardewValley.Objects.Chest chest)
                {
                    Console.WriteLine($"Accessed MakeButtons() else");
                    SortButtonMethods.MakeAlphaIcon(menu, Helper.ModContent.Load<Texture2D>("./assets/AlphaSortIcon.png"), HasBetterChests);
                    SortButtonMethods.MakePriceIcon(menu, Helper.ModContent.Load<Texture2D>("./assets/PriceSortIcon.png"), HasBetterChests);
                }


            }
        }

        private void CreateButtonsOnMenuChanged(object? sender, MenuChangedEventArgs e)
        {
            
            //if player exits a menu, make buttons invisible
            if(e.NewMenu is null && SortButtonMethods.AlphaSortIcon is not null && SortButtonMethods.PriceSortIcon is not null)
            {
                SortButtonMethods.AlphaSortIcon.visible = false;
                SortButtonMethods.PriceSortIcon.visible = false;
            }
            if(e.NewMenu is ItemGrabMenu menu)
            {
                if(menu.source == 1)
                {
                    MakeButtons(sender);
                    SortButtonMethods.AlphaSortIcon.visible = true;
                    SortButtonMethods.PriceSortIcon.visible = true;
                }
                //if the player opened an ItemGrabMenu that is not a chest or fridge
                else
                {
                    if (SortButtonMethods.AlphaSortIcon is not null && SortButtonMethods.PriceSortIcon is not null)
                    {
                        //if the player opens up an ItemGrabMenu that isnt a chest or fridge, hide the buttons.
                        SortButtonMethods.AlphaSortIcon.visible = false;
                        SortButtonMethods.PriceSortIcon.visible = false;
                    }
                }
            }
        }

        private void HoverEffect(object? sender, RenderedActiveMenuEventArgs e)
        {
            if(Game1.activeClickableMenu is ItemGrabMenu menu)
            {
                Vector2 mousePosition = new Vector2(Game1.getMouseXRaw(), Game1.getMouseYRaw());
                //these are in charge of scaling the buttons up/down when hovering.
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
                if(menu.source == 1 && menu.sourceItem is StardewValley.Objects.Chest chest)
                {
                    //Checks to see if the player clicks on the AlphaSortIcon, and if the player does, sort it Alphabetically
                    if (SortButtonMethods.AlphaSortIconArea.Contains(mousePosition.X, mousePosition.Y))
                    {
                        SortOptions.AlphaSort(sender, chest, menu, reverse);
                        SortButtonMethods.ClickAnimation(SortButtonMethods.AlphaSortIcon, 1f, 1.1f);
                        Game1.playSound("Ship");
                    }
                    //Checks to see if the player clicks on the PriceSortIcon
                    else if (SortButtonMethods.PriceSortIconArea.Contains(mousePosition.X, mousePosition.Y))
                    {
                        SortOptions.PriceSort(sender, chest, menu, reverse);
                        SortButtonMethods.ClickAnimation(SortButtonMethods.PriceSortIcon, 1f, 1.1f);
                        Game1.playSound("Ship");
                    }
                }

            }
        }



    }
}
