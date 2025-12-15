using UnityEngine;

public static class Tag
{
    public const string Player = "Player";
    public const string Block = "Block";
}

namespace Layer
{
    public static class Mask
    {
        public static int Player = LayerMask.GetMask(Tag.Player);
        public static int Block = LayerMask.GetMask(Tag.Block);
    }

    public static class Name
    {
        public static int Player = LayerMask.NameToLayer(Tag.Player);
        public static int Block = LayerMask.NameToLayer(Tag.Block);
    }
}
