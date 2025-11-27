using UnityEngine;

public class StoryDialogues : MonoBehaviour
{
    [Header("Story Dialogue Messages")]
    [TextArea(3, 10)]
    public string[] storyDialogues = new string[]
    {
        "Ya es tarde... Todos se fueron. Pero olvide mis libros en el salon.",
        "Tengo un mal presentimiento sobre esto. La escuela se siente... diferente de noche.",
        "Debo apresurarme. Cuanto antes encuentre mis libros, antes podre salir de aqui.",
        "Por que siento que alguien me esta observando?",
        "Estos pasillos parecen mas largos de noche. O sera mi imaginacion?",
        "No puedo dejar de pensar en las historias que contaban sobre esta escuela...",
        "Concentremonos. Solo necesito mis libros y salir de aqui."
    };

    public void TriggerStoryDialogue(int index)
    {
        if (DialogueSystem.Instance != null && index >= 0 && index < storyDialogues.Length)
        {
            DialogueSystem.Instance.ShowDialogue(storyDialogues[index], 5f);
        }
    }

    public void TriggerRandomStoryDialogue()
    {
        if (DialogueSystem.Instance != null && storyDialogues.Length > 0)
        {
            int randomIndex = Random.Range(0, storyDialogues.Length);
            DialogueSystem.Instance.ShowDialogue(storyDialogues[randomIndex], 5f);
        }
    }
}
