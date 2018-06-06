namespace Utility
{
    /// <summary>
    /// Tentacle types are either Anchor or Aim
    /// </summary>
    public enum TentacleType { Anchor, Aim}
    /// <summary>
    /// The states that the tentacles can be in
    /// </summary>
    public enum Tentacles { Expanding, Retracting, Anchored, None}
    /// <summary>
    /// The various ways the characters can be grounded
    /// </summary>
    public enum Grounding { Bottom, Right, Left, Top, BottomLeft, BottomRight, TopRight, TopLeft, Corner, None}
    /// <summary>
    /// The different states of launch
    /// </summary>
    public enum Launch { Contracting, Launching, Grounded}
}

/// <summary>
/// This object is able to be broken down by Pengin/Akkoro's attacks!
/// </summary>
public interface IBreakable
{
    /// <summary>
    /// This function damages the object
    /// </summary>
    void Break(int damage);
}
