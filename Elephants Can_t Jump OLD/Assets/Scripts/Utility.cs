﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// The states that the tentacles can be in
    /// </summary>
    public enum Tentacles { Expanding, Retracting, Anchored, None, First, Second}
    /// <summary>
    /// The various ways the characters can be grounded
    /// </summary>
    public enum Grounding { Bottom, Right, Left, Top, BottomLeft, BottomRight, TopRight, TopLeft, Corner, None, Grounded, Both}
    /// <summary>
    /// The different states of launch
    /// </summary>
    public enum Launch { Contracting, Launching, Grounded}

    public enum Sling { Prep, Airborne, Back, None}

    public enum Controlling { Akkoro, Both}

    public enum Movement { Ground, Wallclimb, Airborne}


    public interface LeftTentacle { }
    public interface RightTentacle { }


    public static class Functions
    {
        public static void OrderByDistance(List<Vector3> list, Vector3 origin)
        {
            // we only need to sort the order if there are more than 2 units in the list
            if (list.Count > 1)
            {
                // sorting algorithm
                list.Sort(delegate (Vector3 c1, Vector3 c2)
                {
                    return Vector2.Distance(origin, c1).CompareTo((Vector2.Distance(origin, c2)));
                });
            }
        }
    }

    public static class Variables
    {
        public static float vertExtent = Camera.main.orthographicSize;
        public static float horzExtent = Camera.main.orthographicSize * (16f / 9f);
        public static Transform room = null;
        public static KeyCode detach = KeyCode.T;
        public static KeyCode launch = KeyCode.Space;
        public static KeyCode wallGrip = KeyCode.LeftShift;
        public static Controlling controlling = Controlling.Akkoro;

    }

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