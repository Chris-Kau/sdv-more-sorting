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
        Vector2 AlphaSortIconPos;
        Rectangle AlphaSortIconArea;
        Vector2 PriceSortIconPos;
        Rectangle PriceSortIconArea;
        //the ? makes it so PriceSortIcon is nullable if not assigned to anything
        ClickableTextureComponent? PriceSortIcon;
        ClickableTextureComponent? AlphaSortIcon;
        public override void Entry(IModHelper helper)
        {
            //O keybind should sort items alphabetically
            Helper.Events.Input.ButtonPressed += this.Keybinds;
            //Adds Button for the user to "click"
            Helper.Events.Display.RenderedActiveMenu += this.AddSortButtons;
            //Gives the Illusion of the user "clicking" on the buttons
            Helper.Events.Input.ButtonPressed += this.ClickedSortButtons;
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

        private void AddSortButtons(object? sender, RenderedActiveMenuEventArgs e)
        {
            if(Game1.activeClickableMenu is ItemGrabMenu menu)
            {
                //get mouse position for later uses
                Vector2 mousePosition = new Vector2(Game1.getMouseXRaw(), Game1.getMouseYRaw());

                //Only create one instance so we can edit its .scale property for a smoother transition, otherwise it'll keep creating a ClickableComponentTexture w scale 1 when we 
                //are trying to edit the scale
                if (AlphaSortIcon is null)
                {
                    //Responsible for creating the button and drawing it to the screen 
                    AlphaSortIcon = new ClickableTextureComponent(
                        new Rectangle((int)menu.organizeButton.getVector2().X + 16 + 64, (int)menu.organizeButton.getVector2().Y, 64, 64),
                        Helper.ModContent.Load<Texture2D>("./assets/AlphaSortIcon.png"),
                        new Rectangle(0, 0, 64, 64),
                        1f
                        ); 
                    //Sets the Icon to be in the position to the right of the organize button
                    AlphaSortIconPos = new Vector2(menu.organizeButton.getVector2().X + 16 + 64, menu.organizeButton.getVector2().Y);
                    //Makes a new rectangle which is then used to check if the mouse is in that area in ClickedAlphaSort()
                    AlphaSortIconArea = new Rectangle((int)AlphaSortIconPos.X, (int)AlphaSortIconPos.Y, 64, 64);
                }

                //Repeated process but for PriceSortIcon, this time to the right of the Quick Stack to Existing Slots button
                
                if (PriceSortIcon is null)
                {
                    PriceSortIcon = new ClickableTextureComponent(
                         new Rectangle((int)menu.fillStacksButton.getVector2().X + 16 + 64, (int)menu.fillStacksButton.getVector2().Y, 64, 64),
                         Helper.ModContent.Load<Texture2D>("./assets/PriceSortIcon.png"),
                         new Rectangle(0, 0, 64, 64),
                         1f
                         );
                    PriceSortIconPos = new Vector2(menu.fillStacksButton.getVector2().X + 16 + 64, menu.fillStacksButton.getVector2().Y);
                    PriceSortIconArea = new Rectangle((int)PriceSortIconPos.X, (int)PriceSortIconPos.Y, 64, 64);
                }
 

                //these if statements are in charge of drawing the icons as well as making a scale up/down effect when hovering.
                if(AlphaSortIcon.containsPoint((int)mousePosition.X, (int)mousePosition.Y))
                {
                    ScaleTransition(AlphaSortIcon, 1.1f, 0.02f);
                    AlphaSortIcon.draw(Game1.spriteBatch, Color.White, 0f);
                }
                else//when the user stops hovering the button, scale it to its original size
                {
                    ScaleTransition(AlphaSortIcon, 1f, -0.02f);
                    AlphaSortIcon.draw(Game1.spriteBatch, Color.White, 0f);
                }


                if(PriceSortIcon.containsPoint((int)mousePosition.X, (int)mousePosition.Y))
                {
                    ScaleTransition(PriceSortIcon, 1.1f, 0.02f);
                    PriceSortIcon.draw(Game1.spriteBatch, Color.White, 0f);
                }
                else
                {
                    ScaleTransition(PriceSortIcon, 1f, -0.02f);
                    PriceSortIcon.draw(Game1.spriteBatch, Color.White, 0f);
                }

                //Draws the mouse so the cursor is not under the button
                menu.drawMouse(Game1.spriteBatch);

            }
        }

        private void ScaleTransition(ClickableTextureComponent icon, float scaleResult, float speed)
        {
            //if speed > 0, we want to scale up, else scale down
            if(speed > 0)
            {
                if (icon.scale < scaleResult)
                {
                    icon.scale += speed;
                }
                else
                {
                    icon.scale = scaleResult;
                }
            }
            else
            {
                if (icon.scale > scaleResult)
                {
                    icon.scale += speed;
                }
                else
                {
                    icon.scale = scaleResult;
                }
            }

        }
        private void ClickedSortButtons(object? sender, ButtonPressedEventArgs e)
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
