using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Extensions;
using System.Security.AccessControl;
using StardewValley.Objects;
using GenericModConfigMenu;

namespace more_sorting
{
    internal sealed class ModEntry : Mod
    {
        private ModConfig Config { get; set; } = new ModConfig();
        private bool HasBetterChests;
        public override void Entry(IModHelper helper)
        {
            HasBetterChests = this.Helper.ModRegistry.IsLoaded("furyx639.BetterChests");
            //GameLaunched is for Generic Mod Config Menu
            Helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            //Gives the Illusion of the user "clicking" on the buttons
            Helper.Events.Input.ButtonPressed += this.ClickedSortButtons;
            //Updates the button everytime the player opens a chest
            Helper.Events.Display.MenuChanged += this.CreateButtonsOnMenuChanged;
            //Updates the button everytime the player resizes the window, so the buttons do not go off screen while in the chest.
            Helper.Events.Display.WindowResized += this.RecreateButtonsOnWindowResize;
            //in charge of the buttons' hover effects
            Helper.Events.Display.RenderedActiveMenu += this.HoverEffect;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            // get Generic Mod Config Menu's API (if it's installed)
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            // register mod
            configMenu.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(this.Config)
            );

            // add some config options
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => "Fix Better Chest Offset",
                getValue: () => this.Config.FixOffsetForBCColorPicker,
                setValue: value => this.Config.FixOffsetForBCColorPicker = value
            );

            configMenu.AddParagraph(
                mod: this.ModManifest,
                text:() => "If you're using Better Chest mod, and you want the buttons to be in place of the Better Chests's color picker option, change this setting to change the offset of the button. If you're not a user of Better Chests mod, ignore this option."
            );

        }
        private void RecreateButtonsOnWindowResize(object? sender, WindowResizedEventArgs e)
        {
            MakeButtons(sender);
        }

        private void MakeButtons(object? sender)
        {
            if (Game1.activeClickableMenu is ItemGrabMenu menu)
            {
                if ((menu.organizeButton is not null && menu.fillStacksButton is not null) && (menu.source == 1 && menu.sourceItem is StardewValley.Objects.Chest || menu.context is StardewValley.Objects.Chest))
                {
                    SortButtonMethods.MakeAlphaIcon(menu, Helper.ModContent.Load<Texture2D>("./assets/AlphaSortIcon.png"), HasBetterChests, this.Config.FixOffsetForBCColorPicker);
                    SortButtonMethods.MakePriceIcon(menu, Helper.ModContent.Load<Texture2D>("./assets/PriceSortIcon.png"), HasBetterChests, this.Config.FixOffsetForBCColorPicker);
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
                if((menu.source == 1 || menu.context is StardewValley.Objects.Chest))
                {
                    MakeButtons(sender);
                    if (SortButtonMethods.AlphaSortIcon is not null && SortButtonMethods.PriceSortIcon is not null)
                    {
                        SortButtonMethods.AlphaSortIcon.visible = true;
                        SortButtonMethods.PriceSortIcon.visible = true;
                    }

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
                    SortButtonMethods.HoverEffect(SortButtonMethods.PriceSortIcon, SortButtonMethods.PriceSortIconArea, (int)mousePosition.X, (int)mousePosition.Y, 1f, 1.1f, 0.02f);
                    SortButtonMethods.HoverEffect(SortButtonMethods.AlphaSortIcon, SortButtonMethods.AlphaSortIconArea, (int)mousePosition.X, (int)mousePosition.Y, 1f, 1.1f, 0.02f);
                }




                //Draws only if the player is within a chest or fridge to prevent crashing from custom menus by other mods
                if (menu.source == 1 && menu.sourceItem is StardewValley.Objects.Chest || menu.context is StardewValley.Objects.Chest)
                {
                    //Draws color picker toggle button hover text over icon/alpha sort buttons
                    if (!HasBetterChests)
                    {
                        if(menu.colorPickerToggleButton is not null)
                        {
                            Rectangle colorbutton = new Rectangle((int)Math.Ceiling(menu.colorPickerToggleButton.bounds.X * Game1.options.uiScale), (int)Math.Ceiling(menu.colorPickerToggleButton.bounds.Y * Game1.options.uiScale), (int)Math.Ceiling(64 * Game1.options.uiScale), (int)Math.Ceiling(64 * Game1.options.uiScale));
                            if (colorbutton.Contains(mousePosition))
                            {
                                IClickableMenu.drawHoverText(
                                Game1.spriteBatch,
                                menu.colorPickerToggleButton.hoverText,
                                Game1.smallFont);
                            }
                        }
   
                    }
                    //Draws organize button hover text over icon/alpha sort buttons
                    if (menu.organizeButton is not null)
                    {
                        Rectangle orgbutton = new Rectangle((int)Math.Ceiling(menu.organizeButton.bounds.X * Game1.options.uiScale), (int)Math.Ceiling(menu.organizeButton.bounds.Y * Game1.options.uiScale), (int)Math.Ceiling(64 * Game1.options.uiScale), (int)Math.Ceiling(64 * Game1.options.uiScale));
                        if (orgbutton.Contains(mousePosition))
                        {
                            IClickableMenu.drawHoverText(
                            Game1.spriteBatch,
                            menu.organizeButton.hoverText,
                            Game1.smallFont);
                        }
                    }
                    //Draws fill stacks button hover text over icon/alpha sort buttons
                    if(menu.fillStacksButton is not null)
                    {
                        Rectangle fillButton = new Rectangle((int)Math.Ceiling(menu.fillStacksButton.bounds.X * Game1.options.uiScale), (int)Math.Ceiling(menu.fillStacksButton.bounds.Y * Game1.options.uiScale), (int)Math.Ceiling(64 * Game1.options.uiScale), (int)Math.Ceiling(64 * Game1.options.uiScale));
                        if (fillButton.Contains((int)mousePosition.X, (int)mousePosition.Y))
                        {
                            IClickableMenu.drawHoverText(
                            Game1.spriteBatch,
                            menu.fillStacksButton.hoverText,
                            Game1.smallFont);
                        }
                    }

                    //Draws hover text over icons
                    if (SortButtonMethods.PriceSortIconArea.Contains(mousePosition) && SortButtonMethods.PriceSortIcon is not null)
                    {
                        IClickableMenu.drawHoverText(
                        Game1.spriteBatch,
                        SortButtonMethods.PriceSortIcon.hoverText,
                        Game1.smallFont
                        );
                    }
                    //Draws hover text over icons
                    if (SortButtonMethods.AlphaSortIconArea.Contains(mousePosition) && SortButtonMethods.AlphaSortIcon is not null)
                    {
                        IClickableMenu.drawHoverText(
                        Game1.spriteBatch,
                        SortButtonMethods.AlphaSortIcon.hoverText,
                        Game1.smallFont
                        );
                    }

                    //Draws the mouse so the cursor is not under the button
                    menu.drawMouse(Game1.spriteBatch);
                }
                


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

  
                if (menu.source == 1 && menu.sourceItem is StardewValley.Objects.Chest || menu.context is StardewValley.Objects.Chest)
                {
                    //Checks to see if the player clicks on the AlphaSortIcon, and if the player does, sort it Alphabetically
                    if (SortButtonMethods.AlphaSortIconArea.Contains(mousePosition.X, mousePosition.Y))
                    {
                        SortOptions.AlphaSort(sender, menu, reverse);
                        SortButtonMethods.ClickAnimation(SortButtonMethods.AlphaSortIcon, 1f, 1.1f);
                        Game1.playSound("Ship");
                    }
                    //Checks to see if the player clicks on the PriceSortIcon
                    else if (SortButtonMethods.PriceSortIconArea.Contains(mousePosition.X, mousePosition.Y))
                    {
                        SortOptions.PriceSort(sender, menu, reverse);
                        SortButtonMethods.ClickAnimation(SortButtonMethods.PriceSortIcon, 1f, 1.1f);
                        Game1.playSound("Ship");
                    }
                }

            }
        }



    }
}
