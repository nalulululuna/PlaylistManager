using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace PlaylistManager.Utilities
{
    internal static class ImageUtils
    {
        internal static Sprite FindSpriteInAssembly(string path)
        {
            try
            {
                var assemblyPath = path.Split(':');
                var assembly = Assembly.GetExecutingAssembly();
                var resourcePath = path;
                if (assemblyPath.Length == 2)
                {
                    assembly = Assembly.Load(assemblyPath[0]);
                    resourcePath = assemblyPath[1];
                }

                if (!assembly.GetManifestResourceNames().Contains(resourcePath))
                    return null;

                using (var stream = assembly.GetManifestResourceStream(resourcePath))
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return LoadSpriteRaw(memoryStream.ToArray());
                }
            }
            catch (Exception e)
            {
                Plugin.Log.Error($"Unable to find sprite in assembly: {path}\n{e}");
                return null;
            }
        }

        internal static Sprite LoadSpriteRaw(byte[] image, float pixelsPerUnit = 100f)
        {
            if (image == null || image.Length == 0)
                return null;

            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
            if (!ImageConversion.LoadImage(texture, image))
                return null;

            return BeatSaberMarkupLanguage.Utilities.LoadSpriteFromTexture(texture, pixelsPerUnit);
        }
    }
}
