using UnityEngine;

public static class Tag
{
    public const string Player = "Player";
    public const string Block = "Block";
    public const string Ground = "Ground";
}

namespace Layer
{
    public static class Mask
    {
        public static int Player = LayerMask.GetMask(Tag.Player);
        public static int Block = LayerMask.GetMask(Tag.Block);
        public static int Ground = LayerMask.GetMask(Tag.Ground);
    }

    public static class Name
    {
        public static int Player = LayerMask.NameToLayer(Tag.Player);
        public static int Block = LayerMask.NameToLayer(Tag.Block);
        public static int Ground = LayerMask.NameToLayer(Tag.Ground);
    }
}
