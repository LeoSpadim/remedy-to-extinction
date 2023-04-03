using UnityEngine;

[CreateAssetMenu(fileName = "Novo MenuSave", menuName = "Objects/MenuSave")]
public class MenuSaves : ScriptableObject
{
    public bool FullScreen;
    public float MusicVolume;
    public bool MusicMute;
    public float SoundVolume;
    public bool SoundMute;
    public bool Vsync;
    public int ResolutionIndex;
}
