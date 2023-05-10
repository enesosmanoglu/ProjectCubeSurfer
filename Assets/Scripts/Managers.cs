
public class Managers
{
    public static GameManager Game { get { return GameManager.Instance; } }
    public static PrefabManager Prefab { get { return PrefabManager.Instance; } }
    public static ReferenceManager Reference { get { return ReferenceManager.Instance; } }
    public static UIManager UI { get { return UIManager.Instance; } }
    public static SoundManager Sound { get { return SoundManager.Instance; } }
}
