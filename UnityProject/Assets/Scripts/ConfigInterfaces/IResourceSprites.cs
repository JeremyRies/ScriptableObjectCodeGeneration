using UnityEngine;

public interface IResourceSprites
{
    Sprite[] GetSprites();
    int GetNumber();
    Texture GetTexture();
    
    Sprite GetSpriteByInt(int spriteId);
    
    Sprite GetResourceSprite(MineResource resource);
}
