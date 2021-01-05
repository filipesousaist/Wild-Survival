using UnityEngine;

public static class Colors
{
    public static readonly Color TRANSPARENT = new Color(0, 0, 0, 0);
    public static readonly Color OPAQUE = new Color(1, 1, 1);

    public static readonly Color GREEN = new Color(0, 1, 0);
    public static readonly Color GREENYELLOW = new Color(0.5f, 1, 0);
    public static readonly Color YELLOW = new Color(1, 1, 0);
    public static readonly Color ORANGE = new Color(1, 0.7f, 0);
    public static readonly Color RED = new Color(1, 0, 0);

    public static readonly Color[] HP = { RED, ORANGE, YELLOW, GREENYELLOW, GREEN };
}
