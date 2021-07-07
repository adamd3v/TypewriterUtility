using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TypewriterAudio : MonoBehaviour
{
    /// <summary>
    /// List of sounds that will play when a letter is added.
    /// </summary>
    public AudioClip[] letterSounds;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        TypewriterManager.OnLetterAdded += new System.Action<char>((chr) => UpdateAudio(chr));
    }

    private void OnDisable()
    {
        TypewriterManager.OnLetterAdded -= new System.Action<char>((chr) => UpdateAudio(chr));
    }

    private void UpdateAudio(char character)
    {
        if(character == TypewriterCharacters.letters.FirstOrDefault((chr) => character.ToString().ToLower() == chr.ToString()))
        {
            audioSource.clip = letterSounds.FirstOrDefault((clip) => clip.name.StartsWith($"speech_letter_{character}"));
        }

        if (character == TypewriterCharacters.punctuation.FirstOrDefault((punc) => character == punc) || char.IsWhiteSpace(character))
        {
            audioSource.clip = null;
        }

        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}
