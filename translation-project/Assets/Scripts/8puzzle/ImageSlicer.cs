using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on https://github.com/SebLague/Programming-Practice
 * Adapted for accessibility purposes
 */

public static class ImageSlicer {

    public static Texture2D[,] GetSlices(Texture2D image, int blocksPerLine)
    {
        int imageSize = Mathf.Min(image.width, image.height);
        int blockSize = imageSize / blocksPerLine;

        int imageShift = 50;

        Texture2D[,] blocks = new Texture2D[blocksPerLine, blocksPerLine];

        for(int i=0; i<blocksPerLine; i++)
        {
            for(int j=0; j<blocksPerLine; j++)
            {
                Texture2D block = new Texture2D(blockSize, blockSize)
                {
                    wrapMode = TextureWrapMode.Clamp
                };

                block.SetPixels(image.GetPixels(j * blockSize + imageShift, i* blockSize, blockSize, blockSize));
                block.Apply();
                blocks[j,i] = block;
            }
        }
        return blocks;
    }

}
