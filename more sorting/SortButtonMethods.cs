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
        private static int width = 64;
        private static int height = 64;

        internal static void MakeAlphaIcon(ItemGrabMenu menu, Texture2D img, bool HasBC)
        {
            //Responsible for creating the button and drawing it to the screen 
            AlphaSortIcon = new ClickableTextureComponent(
                new Rectangle((int)menu.organizeButton.getVector2().X + 16 + width, (int)menu.organizeButton.getVector2().Y, width, height),
                img,
                new Rectangle(0, 0, 64, 64),
                1f
                );
            AlphaSortIcon.hoverText = "A->Z";
            if (HasBC)
                AlphaSortIcon.bounds = new Rectangle((int)menu.organizeButton.getVector2().X + 176 + width, (int)menu.organizeButton.getVector2().Y, width, height);
            //Gets position of the button as a vector
            AlphaSortIconPos = new Vector2(AlphaSortIcon.bounds.X, AlphaSortIcon.bounds.Y) * Game1.options.uiScale;
            //uses that position to create a rectangle around that button, used for hover animations and detecting clicks
            AlphaSortIconArea = new Rectangle((int)AlphaSortIconPos.X, (int)AlphaSortIconPos.Y, (int)(width * Game1.options.uiScale), (int)(height * Game1.options.uiScale));
        }

        internal static void MakePriceIcon(ItemGrabMenu menu, Texture2D img, bool HasBC)
        {
            PriceSortIcon = new ClickableTextureComponent(
                    new Rectangle((int)menu.fillStacksButton.getVector2().X + 16 + width, (int)menu.fillStacksButton.getVector2().Y, width, height),
                    img,
                    new Rectangle(0, 0, 64, 64),
                    1f
                    );
            PriceSortIcon.hoverText = "$$$->$";
            if (HasBC)
                PriceSortIcon.bounds = new Rectangle((int)menu.fillStacksButton.getVector2().X + 176 + width, (int)menu.fillStacksButton.getVector2().Y, width, height);
            PriceSortIconPos = new Vector2(PriceSortIcon.bounds.X, PriceSortIcon.bounds.Y) * Game1.options.uiScale;
            PriceSortIconArea = new Rectangle((int)PriceSortIconPos.X, (int)PriceSortIconPos.Y, (int)(width * Game1.options.uiScale), (int)(height * Game1.options.uiScale));
        }
        internal static void HoverEffect(ClickableTextureComponent icon,Rectangle area, int x, int y, float originalScale, float scaleResult, float delta)
        {
            //Checks to see if the mouse cursor is hovering the button, if it is, scale the button up, but if it isnt, scale the button to its original size
            if (area.Contains(x,y))
            {
                ScaleTransition(icon, scaleResult, delta);
                DrawButton(icon);
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
