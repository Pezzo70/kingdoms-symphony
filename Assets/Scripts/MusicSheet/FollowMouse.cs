using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    // [SerializeField]
    // private Vector2 _offsetPosition;

    // [SerializeField]
    // private Sprite[] notationSprites;

    // private SpriteRenderer spriteRenderer;
    // private int currentSpriteIndex = 0;

    // [SerializeField]
    // private bool isHover = false;

    // void Start()
    // {
    //     spriteRenderer = GetComponent<SpriteRenderer>();
    //     UpdateSprite();
    // }

    // void Update()
    // {
    //    if(isHover) SpriteFollowMouse();
    // }

    // void SpriteFollowMouse()
    // {
    //      Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     transform.position = new Vector2(cursorPos.x + _offsetPosition.x, cursorPos.y + _offsetPosition.y);

    //     float scroll = Input.GetAxis("Mouse ScrollWheel");

    //     if (scroll > 0f)
    //     {
    //         currentSpriteIndex++;
    //         if (currentSpriteIndex >= notationSprites.Length)
    //         {
    //             currentSpriteIndex = 0;
    //         }

    //         UpdateSprite();
    //     }
    //     else if (scroll < 0f)
    //     {
    //         currentSpriteIndex--;
    //         if (currentSpriteIndex < 0)
    //         {
    //             currentSpriteIndex = notationSprites.Length - 1;
    //         }
    //         UpdateSprite();
    //     }
    // }

    // void UpdateSprite()
    // {
    //     if (currentSpriteIndex >= 0 && currentSpriteIndex < notationSprites.Length)
    //     {
    //         spriteRenderer.sprite = notationSprites[currentSpriteIndex];
    //     }
    // }
}
