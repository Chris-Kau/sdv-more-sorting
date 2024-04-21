using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace more_sorting
{
    public class SortButtonMethods
    {
        public static Vector2 AlphaSortIconPos;
        public static Rectangle AlphaSortIconArea;
        public static Vector2 PriceSortIconPos;
        public static Rectangle PriceSortIconArea;
        public static ClickableTextureComponent? AlphaSortIcon;
        public static ClickableTextureComponent? PriceSortIcon;

        internal static void MakeAlphaIcon(ItemGrabMenu menu, Texture2D img)
        {
            //Responsible for creating the button and drawing it to the screen 
            AlphaSortIcon = new ClickableTextureComponent(
                new Rectangle((int)menu.organizeButton.getVector2().X + 16 + 64, (int)menu.organizeButton.getVector2().Y, 64, 64),
                img,
                new Rectangle(0, 0, 64, 64),
                1f
                );
            AlphaSortIcon.hoverText = "Sorts A->Z";
            //Gets position of the button as a vector
            AlphaSortIconPos = new Vector2(menu.organizeButton.getVector2().X + 16 + 64, menu.organizeButton.getVector2().Y);
            //uses that position to create a rectangle around that button, used for hover animations and detecting clicks
            AlphaSortIconArea = new Rectangle((int)AlphaSortIconPos.X, (int)AlphaSortIconPos.Y, 64, 64);
        }

        internal static void MakePriceIcon(ItemGrabMenu menu, Texture2D img)
        {
            PriceSortIcon = new ClickableTextureComponent(
                    new Rectangle((int)menu.fillStacksButton.getVector2().X + 16 + 64, (int)menu.fillStacksButton.getVector2().Y, 64, 64),
                    img,
                    new Rectangle(0, 0, 64, 64),
                    1f
                    );
            PriceSortIcon.hoverText = "Sorts price high->low";
            PriceSortIconPos = new Vector2(menu.fillStacksButton.getVector2().X + 16 + 64, menu.fillStacksButton.getVector2().Y);
            PriceSortIconArea = new Rectangle((int)PriceSortIconPos.X, (int)PriceSortIconPos.Y, 64, 64);
        }
        internal static void HoverEffect(ClickableTextureComponent icon, int x, int y, float originalScale, float scaleResult, float delta)
        {
            //Checks to see if the mouse cursor is hovering the button, if it is, scale the button up, but if it isnt, scale the button to its original size
            if (icon.containsPoint(x,y))
            {
                ScaleTransition(icon, scaleResult, delta);
                DrawButton(icon);
                //draws the hover text
                IClickableMenu.drawHoverText(
                 Game1.spriteBatch,
                 icon.hoverText,
                 Game1.smallFont
                 );
            }
            else
            {
                ScaleTransition(icon, originalScale, -delta);
                DrawButton(icon);
            }
        }

        internal static void ClickAnimation(ClickableTextureComponent icon, float originalScale, float scaleResult)
        {
            ScaleTransition(icon, originalScale, 0.01f);
            ScaleTransition(icon, scaleResult, 0.01f);
        }

        private static void DrawButton(ClickableTextureComponent icon)
        {
            icon.draw(Game1.spriteBatch, Color.White, 0.99f);
        }

        private static void ScaleTransition(ClickableTextureComponent icon, float scaleResult, float delta)
        {
            //if delta > 0, that means we want to scale up, otherwise scale down
            if(delta > 0)
            {
                if(icon.scale < scaleResult)
                {
                    icon.scale += delta;
                }
                else
                {
                    icon.scale = scaleResult;
                }
            }
            else
            {
                if(icon.scale > scaleResult)
                {
                    icon.scale += delta;
                }
                else
                {
                    icon.scale = scaleResult;
                }
            }
        }
    }
}
