using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypewriterCharacters : MonoBehaviour
{
    public static readonly char[] letters = new char[]
    {
        'a', 'b', 'c', 'd', 
        'e', 'f', 'g', 'h', 
        'i', 'j', 'k', 'l', 
        'm', 'n', 'o', 'p', 
        'q', 'r', 's', 't', 
        'u', 'v', 'w', 'x',
        'y', 'z'
    };

    public static readonly char[] punctuation = new char[]
    {
        '.', ',', ';', ':',
        '-', '"'
    };
}
