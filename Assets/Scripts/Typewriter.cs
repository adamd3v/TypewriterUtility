using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Typewriter : MonoBehaviour
{
    /// <summary>
    /// Queued messages given by a text holder.
    /// </summary>
    public List<TypewriterObject> messageQueue;

    /// <summary>
    /// The time it takes to go to the next letter.
    /// </summary>
    public float letterTime;

    public Text text;
    public RawImage mouthImg;

    public Texture2D[] speakingMouthTextures;

    public AudioClip[] vowelSounds;
    public AudioClip[] consonantSounds;

    /// <summary>
    /// Action for when a character gets added.
    /// </summary>
    public static System.Action<char> OnLetterAdded;

    internal System.Action<char> A_LetterAdded;

    internal char[] cPunctuation = new char[]
    {
        '.', ',', '!', '?', ';', ':', '-'
    };

    internal char[] cVowels = new char[]
    {
        'a', 'e', 'i', 'o', 'u'
    };

    internal char[] cConsonants = new char[]
    {
        'b', 'c', 'd', 'f', 'g',
        'h', 'j', 'k', 'l', 'm',
        'n', 'p', 'q', 'r', 's',
        't', 'v', 'w', 'x', 'y',
        'z'
    };

    private AudioSource source;

    /// <summary>
    /// Starts the typewriter sequence.
    /// </summary>
    public void Play()
    {
        ResetTypewriter();
        StartCoroutine(CoTypewriterRoutine(messageQueue, 3f));
    }

    private void ResetTypewriter()
    {
        text.material.color = Color.white;
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        A_LetterAdded = new System.Action<char>((character) =>
        {
            if(character == cVowels.FirstOrDefault((vowel) => character == vowel))
            {
                source.clip = vowelSounds.FirstOrDefault((clip) => clip.name.StartsWith($"speech_letter_{character}"));
            }

            if (character == cConsonants.FirstOrDefault((cons) => character == cons))
            {
                source.clip = consonantSounds.FirstOrDefault((clip) => clip.name.StartsWith($"speech_letter_{character}"));
            }

            if (character == cPunctuation.FirstOrDefault((punc) => character == punc) || char.IsWhiteSpace(character))
            {
                source.clip = null;
            }

            if (source.clip != null)
            {
                source.Play();
            }

            mouthImg.texture = speakingMouthTextures[Random.Range(0, speakingMouthTextures.Length)];
        });
    }

    private void OnEnable()
    {
        OnLetterAdded += A_LetterAdded;
        Play();
    }

    private void OnDisable()
    {
        OnLetterAdded -= A_LetterAdded;

        StopCoroutine(CoTypewriterRoutine(messageQueue, 3f));
    }

    private IEnumerator CoTypewriterRoutine(List<TypewriterObject> list, float delay)
    {
        text.text = string.Empty;
        float timer = 0f;
        
        for(int i = 0; i < list.Count; i++)
        {
            text.text = string.Empty;
            timer = 0f;

            for(int y = 0; y < list[i].message.Length; y++)
            {
                yield return new WaitForSeconds(letterTime);

                text.text += list[i].message[y];

                if (OnLetterAdded != null)
                {
                    OnLetterAdded(list[i].message.ToLower()[y]);
                }
            }

            while (timer < delay || Input.GetKey(KeyCode.Space))
            {
                timer += Time.deltaTime;
                yield return null;
            }
        }

        yield return CoFadeRoutine(delay);
    }

    private IEnumerator CoFadeRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        float a = 1;

        while(text.material.color.a > 0)
        {
            text.material.color = new Color(text.material.color.r, text.material.color.g, text.material.color.r, a);

            a -= 0.002f;

            yield return null;
        }

        gameObject.SetActive(false);

        yield return null;
    }
}
