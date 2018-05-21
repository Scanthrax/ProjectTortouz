using UnityEngine;

namespace Utility
{
    public enum Tentacle { Left, Right}
    public enum Grounding { Bottom, Right, Left, Top, BottomLeft, BottomRight, TopRight, TopLeft, None}
    public static class Functions
    {
        public static bool GripButton()
        {
            if (Input.GetKey(KeyCode.F))
            {
                return true;
            }
            else return false;
        }

        public static bool AnchorTentacle()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                return true;
            }
            else return false;
        }
    }
}
