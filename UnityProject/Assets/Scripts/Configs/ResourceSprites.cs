using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
* This class was generated from "ScriptableObjectsTemplate.cs.txt"
*/

[CreateAssetMenu(order = 1,fileName = "ResourceSprites",menuName = "ScriptableObjects/ResourceSprites")]
public class ResourceSprites : ScriptableObject , IResourceSprites
{
    [SerializeField] Sprite[] _GetSprites;
    public Sprite[] GetSprites() => _GetSprites;

    [SerializeField] Int32 _GetNumber;
    public Int32 GetNumber() => _GetNumber;

    [SerializeField] Texture _GetTexture;
    public Texture GetTexture() => _GetTexture;

    [SerializeField] Sprite[] _GetSpriteByInt;
    public Sprite GetSpriteByInt(int index)
    {
        return _GetSpriteByInt[index];
    }

    [Serializable]
    private class GetResourceSpriteHelper
    {
        public MineResource EnumType;
        public Sprite Sprite;
    }

    [SerializeField] List<GetResourceSpriteHelper> _GetResourceSprite;
    public Sprite GetResourceSprite(MineResource enumType)
    {
        return _GetResourceSprite.First(x => x.EnumType == enumType).Sprite;
    }
}