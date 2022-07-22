using System;

public static class GlobalEvents
{
    public static Action<Core.EGameState> OnGameStateChanged;
    public static Action OnNewSceneLoaded;
    public static Action OnTapToPlay;
    public static Action<Core.EGameOverReason> OnGameOver;
    public static Action<Character> OnCharacterSpawned;
}
