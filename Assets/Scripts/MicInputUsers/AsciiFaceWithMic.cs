using TMPro;

public class AsciiFaceWithMic : MicUser
{
    public TMP_Text textFace;
    public string notTalkingFace, talkingFace;

    public override void NewMicLevel(float level)
    {
        if (level > 0)
            textFace.text = talkingFace;
        else
            textFace.text = notTalkingFace;
    }

    private void UpdateMood(AsciiFaceMoodData data)
    {
        talkingFace = data.talkingFace;
        notTalkingFace = data.notTalkingFace;
        NewMicLevel(0f);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        MoodHandler.OnMoodChanged += UpdateMood;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        MoodHandler.OnMoodChanged -= UpdateMood;
    }
}
