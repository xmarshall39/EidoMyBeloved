using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using Febucci.UI.Core;
using TMPro;

public class Dialogue
{
    public List<string> textChunks = new List<string>();

    public Dialogue(string text, int charCountLimit)
    {
        StringBuilder buf = new StringBuilder(charCountLimit + 50);
        string[] words = text.Split(" ");
        int charCount = 0;
        int i = 0;
        foreach(var word in words)
        {
            if (charCount + word.Length <= charCountLimit)
            {
                buf.Append(word);
                if (i != words.Length - 1) buf.Append(" ");
            }
            else
            {
                textChunks.Add(buf.ToString());
                buf.Clear();
                buf.Append(word);
                if (i != words.Length - 1) buf.Append(" ");
            }

            ++i;
        }

        textChunks.Add(buf.ToString());
    }


}

public class DialogueNode
{
    public int timestamp = 0;
    public string text = "";
    public int questionCode = 0;
    public int orderCode = 0;
    public GhostEmote baseEmote;
    public bool triggerTextEntry = false;
    public string textEntryInput;
    public string textEntryPrompt;
    public string correctAnswerResponse;
    public GhostEmote correctEmote;
    public string wrongAnswerResponse;
    public GhostEmote wrongEmote;
}
public enum GhostEmote
{
Wave, Happy, Sad, Wallowing, Goofy, Baffled, Confused, Crying
}

public class DialogueManager : MonoBehaviour
{
    public int charCountLimit = 300;
    public string dialoguFileName;
    public LinkedList<Dialogue> DialogueQueue = new LinkedList<Dialogue>();
    public Queue<DialogueNode> DialogueNodes = new Queue<DialogueNode>();

    public TypewriterCore typewriter;
    public TextMeshProUGUI displayText;

    public bool DisplayingText = false;
    
    void Start()
    {
        displayText.text = string.Empty;
        string[] dialogue = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, dialoguFileName));

        int i = 0;
        //[
        //0: Starting Timestamp,
        //1: Text,
        //2: Code,
        //3: Ghost Emote,
        //4: Trigger Text Entry,
        //5: Desired Input,
        //6: Text Entry Prompt,
        //7: Correct Answer Response,
        //8: Correct Emote,
        //9: Wrong Answer Response,
        //10: Wrong Emote
        //]
        foreach (var line in dialogue)
        {
            if (i > 0)
            {
                try
                {
                    string[] splitLine = line.Split("\t");
                    var node = new DialogueNode();
                    node.timestamp = int.Parse(splitLine[0]);
                    node.text = splitLine[1];
                    string[] splitCode = splitLine[2].Split("-");
                    if (splitCode.Length >= 2)
                    {
                        node.questionCode = int.Parse(splitCode[0]);
                        node.orderCode = int.Parse(splitCode[1][0].ToString());
                    }
                    else
                    {
                        node.questionCode = 0;
                        node.orderCode = 0;
                    }

                    node.baseEmote = Enum.Parse<GhostEmote>(splitLine[3]);
                    node.triggerTextEntry = splitLine[4] == "1";
                    node.textEntryInput = splitLine[5];
                    node.textEntryPrompt = splitLine[6];
                    node.correctAnswerResponse = splitLine[7];
                    node.correctEmote = string.IsNullOrEmpty(splitLine[8]) ? GhostEmote.Wave : Enum.Parse<GhostEmote>(splitLine[8]);
                    node.wrongAnswerResponse = splitLine[9];
                    node.wrongEmote = string.IsNullOrEmpty(splitLine[10]) ? GhostEmote.Wave : Enum.Parse<GhostEmote>(splitLine[10]);

                    DialogueNodes.Enqueue(node);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Failed on row {i} with exception {ex}");
                }

            }
            ++i;
        }

        GameStateManager.OnTimerUpdate += GameStateManager_OnTimerUpdate;

        StartCoroutine(ProcessDialogues());
    }

    private void GameStateManager_OnTimerUpdate(int time)
    {
        if(DialogueNodes.TryPeek(out DialogueNode node) && node.timestamp <= time)
        {
            DialogueQueue.AddLast(new Dialogue(node.text, charCountLimit));
            DialogueNodes.Dequeue();
        }
    }


    private IEnumerator ProcessDialogues()
    {
        while (true)
        {
            while (typewriter.isShowingText)
            {
                yield return null;
            }
            yield return new WaitForSeconds(.65f);

            if (DialogueQueue != null && DialogueQueue.Count > 0)
            {
                foreach (string line in DialogueQueue.First.Value.textChunks)
                {
                    displayText.text = line;
                    while (typewriter.isShowingText)
                    {
                        yield return null;
                    }
                    yield return new WaitForSeconds(.65f);
                }
                DialogueQueue.RemoveFirst();
            }


        }
    }
}
