using UnityEngine;
public static class PlatformUtils
{
    public static bool IsMobilePlatform()
    {
        return Application.platform == RuntimePlatform.Android
            || Application.platform == RuntimePlatform.IPhonePlayer;
    }
}
