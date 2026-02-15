using System;

namespace Scripts.DialogSystem;

public class DialogLine
{
    /// <summary>
    /// Localization key for replica
    /// </summary>
    public readonly string TextLineId;
    
    /// <summary>
    /// Needed to enable correct portrait and name display
    /// </summary>
    public readonly string SpeakerId;
    public readonly string SpeakerNameOverride;
    
    /// <summary>
    /// Needed to choose the right portrait option
    /// </summary>
    public readonly EmotionTag EmotionTag;

    public DialogLine(string textLineId, string speakerId, string speakerNameOverride, EmotionTag emotionTag)
    {
        TextLineId = textLineId;
        SpeakerId = speakerId;
        SpeakerNameOverride = speakerNameOverride;
        EmotionTag = emotionTag;
    }

    public DialogLine(string textLineId, string speakerId, EmotionTag emotionTag)
    {
        TextLineId = textLineId;
        SpeakerId = speakerId;
        SpeakerNameOverride = string.Empty;
        EmotionTag = emotionTag;
    }

    public DialogLine(string textLineId, string speakerId)
    {
        TextLineId = textLineId;
        SpeakerId = speakerId;
        SpeakerNameOverride = string.Empty;
        EmotionTag = EmotionTag.Neutral;
    }
}