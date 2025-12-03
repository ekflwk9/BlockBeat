using UnityEngine;

public static class Tag
{
    public const string Player = "Player";
}

namespace Layer
{
    public static class Mask
    {
        public static int Player = LayerMask.GetMask(Tag.Player);
    }

    public static class Name
    {
        public static int Player = LayerMask.NameToLayer(Tag.Player);
    }
}
