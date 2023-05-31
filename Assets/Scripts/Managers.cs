
public class Managers
{
    public static GameManager Game { get { return GameManager.Instance; } }
    public static PrefabManager Prefab { get { return PrefabManager.Instance; } }
    public static ReferenceManager Reference { get { return ReferenceManager.Instance; } }
    public static UIManager UI { get { return UIManager.Instance; } }
    public static SoundManager Sound { get { return SoundManager.Instance; } }
    public static AdManager Ad { get { return AdManager.Instance; } }
    public static SkinManager Skin { get { return SkinManager.Instance; } }
    public static IAPManager IAP { get { return IAPManager.Instance; } }
}
