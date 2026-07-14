public static class TransitionState
{
    public static BiomeData CurrentBiome;
    public static BiomeData NextFloorBiome;
    public static bool HasPendingTransition;

    public static void FloorTransition(BiomeData current, BiomeData next, string scene)
    {
        CurrentBiome = current;
        NextFloorBiome = next;
        HasPendingTransition = true;

        ScreenFader.Instance.TransitionToScene(scene);
    }

    public static bool ConsumePendingTransition(out BiomeData biome)
    {
        biome = NextFloorBiome;
        bool hadTransition = HasPendingTransition;
        HasPendingTransition = false;
        return hadTransition;
    }
}