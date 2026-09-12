using BeatSaberPlaylistsLib.Types;
using HarmonyLib;
using PlaylistManager.Configuration;
using PlaylistManager.Utilities;

/*
 * This patch removes the download icon for empty beatmaplevelcollections
 * Introduced since 1.18.0
 */

namespace PlaylistManager.HarmonyPatches
{
    [HarmonyPatch(typeof(AnnotatedBeatmapLevelCollectionCell), "SetDownloadIconVisible")]
    internal class AnnotatedBeatmapLevelCollectionCell_RefreshAvailabilityAsync
    {
        private static void Prefix(AnnotatedBeatmapLevelCollectionCell __instance, ref bool visible)
        {
            if (__instance._beatmapLevelPack is PlaylistLevelPack playlistLevelPack)
            {
                visible = PluginConfig.Instance.ShowDownloadIcon && PlaylistLibUtils.GetMissingSongs(playlistLevelPack.playlist).Count > 0;
            }
        }
    }
}
