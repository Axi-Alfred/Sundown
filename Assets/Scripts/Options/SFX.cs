public static class SFX
{
    public static void Play(int index)
    {
        if (SFXLibrary.Instance == null)
        {
            UnityEngine.Debug.LogWarning($"No active SFXLibrary in scene. Could not play SFX {index}.");
            return;
        }

        SFXLibrary.Instance.Play(index);
    }
}
