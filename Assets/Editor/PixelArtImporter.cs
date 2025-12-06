using UnityEditor;
using UnityEngine;

public class PixelArtImporter : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        TextureImporter importer = (TextureImporter)assetImporter;

        // Apply only to textures imported as sprites
        if (importer.textureType == TextureImporterType.Sprite)
        {
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
        }
    }
}
