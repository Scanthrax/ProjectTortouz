using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachBothCharacters : MonoBehaviour
{
    /// <summary>
    /// Layer for Akkoro
    /// </summary>
    int akkoro;

    #region Delegates
    public delegate void AttachCharacters();
    public static event AttachCharacters attachCharacters;
    #endregion

    private void Start()
    {
        akkoro = LayerMask.NameToLayer("Akkoro");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == akkoro)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                attachCharacters();
            }
        }
    }
}
