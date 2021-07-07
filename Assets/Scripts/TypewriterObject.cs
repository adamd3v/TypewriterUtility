using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Typewriter Object", menuName = "Typewriter Object")]
public class TypewriterObject : ScriptableObject
{
    public enum SpeakerType
    {
        Speaker_Default,
        Speaker_Deep,
        Speaker_High
    }

    public SpeakerType type;
    public float pitch;

    [TextArea(0, 500)]
    public string message;
}
