using UnityEngine;

public static class DataManager
{
    // 🔧 Keys
    private const string KEY_MOVE_CONTROL = "MoveControlType";
    private const string KEY_BOMB_CONTROL = "PlaceBombControlType";
    private const string KEY_MUSIC_VOLUME = "MusicVolume";
    private const string KEY_SFX_VOLUME = "SFXVolume";

    // 📥 Load
    public static MoveInputController.MoveControlType GetMoveControlType()
    {
        if (!PlatformUtils.IsMobilePlatform())
        {
            return (MoveInputController.MoveControlType)PlayerPrefs.GetInt(KEY_MOVE_CONTROL, (int)MoveInputController.MoveControlType.Keyboard);
        }
        return (MoveInputController.MoveControlType)PlayerPrefs.GetInt(KEY_MOVE_CONTROL, (int)MoveInputController.MoveControlType.Joystick);
    }

    public static PlaceBombInputController.PlaceBombControlType GetBombControlType()
    {
        if (!PlatformUtils.IsMobilePlatform())
        {
            return (PlaceBombInputController.PlaceBombControlType)PlayerPrefs.GetInt(KEY_BOMB_CONTROL, (int)PlaceBombInputController.PlaceBombControlType.Keyboard);
        }
        return (PlaceBombInputController.PlaceBombControlType)PlayerPrefs.GetInt(KEY_BOMB_CONTROL, (int)PlaceBombInputController.PlaceBombControlType.Area);
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(KEY_MUSIC_VOLUME, 0.5f); // Default 50%
    }

    public static float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(KEY_SFX_VOLUME, 0.5f);
    }

    // 💾 Save
    public static void SetMoveControlType(MoveInputController.MoveControlType type)
    {
        PlayerPrefs.SetInt(KEY_MOVE_CONTROL, (int)type);
        PlayerPrefs.Save();
    }

    public static void SetBombControlType(PlaceBombInputController.PlaceBombControlType type)
    {
        PlayerPrefs.SetInt(KEY_BOMB_CONTROL, (int)type);
        PlayerPrefs.Save();
    }

    public static void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat(KEY_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public static void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat(KEY_SFX_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public static void SaveAll()
    {
        PlayerPrefs.Save();
    }
}
