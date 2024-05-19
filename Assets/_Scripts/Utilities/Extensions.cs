using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Kingdom.Extensions
{

    public static class KingdomExtensions
    {
        public static Vector2 GetSpriteRelativePivot(this Image img)
        {

            Bounds bounds = img.sprite.bounds;
            var pivotX = -bounds.center.x / bounds.extents.x / 2 + 0.5f;
            var pivotY = -bounds.center.y / bounds.extents.y / 2 + 0.5f;
            Vector2 pivotPixel = new Vector2(pivotX, pivotY);


            return pivotPixel;
        }

        public static Vector2 GetSpritePivotPosition(this Image img)
        {

            Vector2 pivotRelative = img.GetSpriteRelativePivot();

            float xPos = (pivotRelative.x - 0.5f) * img.rectTransform.rect.width;
            float yPos = (pivotRelative.y - 0.5f) * img.rectTransform.rect.height;

            return new Vector2(xPos, yPos);
        }


    }
}