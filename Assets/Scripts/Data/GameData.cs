using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    /// <summary>
    /// Tag Types
    /// </summary>
    public const string PLAYER_TAG = "Player";
    public const string GHOST_TAG = "Ghost";
    public const string INTERACTIVE_OBJECT_TAG = "InteractiveObject";

    /// <summary>
    /// Layer Types
    /// </summary>
    public static LayerMask PLAYER_LAYER = 1 << LayerMask.NameToLayer("Player");
    public static LayerMask GHOST_LAYER = 1 << LayerMask.NameToLayer("Ghost");
    public static LayerMask IN_HAND_LAYER = 1 << LayerMask.NameToLayer("InHand");


    /// <summary>
    /// ETC...
    /// </summary>
    public const float SCREEN_CHANGE_FADE_TIME = 2;

    
}
