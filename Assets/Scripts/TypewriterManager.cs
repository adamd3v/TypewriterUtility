using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterManager : MonoBehaviour
{
    public static TypewriterManager instance;

    /// <summary>
    /// Output from the typewriter routine.
    /// </summary>
    [TextArea(0, 250)] public string typewriterOutput;

    /// <summary>
    /// Final output on the current text object.
    /// </summary>
    [TextArea(0, 250)] public string typewriterFinalOutput;

    /// <summary>
    /// Queued messages given by a text holder.
    /// </summary>
    public List<TypewriterObject> messageQueue;

    /// <summary>
    /// The time it takes to go to the next letter.
    /// </summary>
    public float letterTime;

    /// <summary>
    /// Action for when a letter gets added.
    /// </summary>
    public static System.Action<char> OnLetterAdded;

    /// <summary>
    /// Action for when the current message has finished.
    /// </summary>
    public static System.Action OnMessageComplete;

    /// <summary>
    /// Action for when all of the messages have went through.
    /// </summary>
    public static System.Action OnDialogueComplete;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(CoTypewriterRoutine(messageQueue, 3f));
    }

    private void OnDisable()
    {
        StopCoroutine(CoTypewriterRoutine(messageQueue, 3f));
        typewriterOutput = string.Empty;
        typewriterFinalOutput = string.Empty;
    }

    private IEnumerator CoTypewriterRoutine(List<TypewriterObject> list, float delay)
    {
        for(int i = 0; i < list.Count; i++)
        {
            typewriterFinalOutput = list[i].message;
            typewriterOutput = string.Empty;

            for(int y = 0; y < list[i].message.Length; y++)
            {
                yield return new WaitForSeconds(letterTime);

                typewriterOutput += list[i].message[y];

                if (OnLetterAdded != null)
                {
                    OnLetterAdded(list[i].message[y]);
                }
            }

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            if (OnMessageComplete != null)
            {
                OnMessageComplete();
            }
        }

        if(OnDialogueComplete != null)
        {
            OnDialogueComplete();
        }

        yield return null;
    }
}
