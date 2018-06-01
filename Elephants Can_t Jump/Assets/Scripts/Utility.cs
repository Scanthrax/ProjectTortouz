using UnityEngine;

namespace Utility
{
    public enum TentacleType { Anchor, Aim}
    public enum Tentacles { Expanding, Retracting, Anchored, None}
    public enum Grounding { Bottom, Right, Left, Top, BottomLeft, BottomRight, TopRight, TopLeft, None}
    public enum Launch { Contracting, Launching, Grounded}
}
