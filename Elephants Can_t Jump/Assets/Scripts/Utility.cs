using UnityEngine;

namespace Utility
{
    public enum Tentacle { Left, Right}

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
    }
}
