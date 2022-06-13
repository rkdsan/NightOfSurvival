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
    public const string UNTAGGED_TAG = "Untagged";
    public const string INTERACTIVE_OBJECT_TAG = "InteractiveObject";
    public const string IN_HAND_STRING = "InHand";

    /// <summary>
    /// Layer
    /// </summary>
    public static LayerMask PLAYER_LAYER = LayerMask.NameToLayer(PLAYER_TAG);
    public static LayerMask GHOST_LAYER = LayerMask.NameToLayer(GHOST_TAG);
    public static LayerMask IN_HAND_LAYER = LayerMask.NameToLayer(IN_HAND_STRING);

    public static LayerMask PLAYER_LAYER_MASK = 1 << PLAYER_LAYER;
    public static LayerMask GHOST_LAYER_MASK = 1 << GHOST_LAYER;
    public static LayerMask IN_HAND_LAYER_MASK = 1 << IN_HAND_LAYER;


    /// <summary>
    /// ETC...
    /// </summary>
    public const float SCREEN_CHANGE_FADE_TIME = 2;

    
}
