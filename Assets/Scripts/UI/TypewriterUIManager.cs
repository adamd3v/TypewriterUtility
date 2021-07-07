using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterUIManager : MonoBehaviour
{
    public Text text;

    public RawImage face;

    public Texture2D[] faceTextures;

    private void Awake()
    {
        TypewriterManager.OnDialogueComplete += new System.Action(() => StartCoroutine(CoFadeRoutine(1.75f)));
    }

    private void OnEnable()
    {
        if(text != null)
        {
            text.text = string.Empty;
        }

        TypewriterManager.OnLetterAdded += new System.Action<char>((chr) => CharUpdate(chr));

        TypewriterManager.OnMessageComplete += new System.Action(() => text.text = string.Empty);
    }

    private void OnDisable()
    {
        TypewriterManager.OnLetterAdded -= new System.Action<char>((chr) => CharUpdate(chr));

        TypewriterManager.OnMessageComplete -= new System.Action(() => text.text = string.Empty);

        TypewriterManager.OnDialogueComplete -= new System.Action(() => StartCoroutine(CoFadeRoutine(1.75f)));
    }

    private void CharUpdate(char chr)
    {
        if (text != null)
        {
            text.text += chr;
        }

        if (face != null)
        {
            face.texture = faceTextures[Random.Range(0, faceTextures.Length)];
        }
    }

    private IEnumerator CoFadeRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        float a = 1;

        while (text.material.color.a > 0)
        {
            text.material.color = new Color(text.material.color.r, text.material.color.g, text.material.color.r, a);

            a -= 0.002f;

            yield return null;
        }

        gameObject.SetActive(false);

        yield return null;
    }
}
