using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdvencedCSharpExcs
{
    internal class Unsafe
    {
    }

    //Unsafe Code and Pointers Exercise: Image Processing System


    public class ImageProcessor
    {
        public struct Pixel
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;
        }

        public unsafe class FastImageProcessor
        {
            private readonly int _width;
            private readonly int _height;
            private readonly Pixel* _pixels;

            public FastImageProcessor(int width, int height)
            {
                _width = width;
                _height = height;
                _pixels = (Pixel*)Marshal.AllocHGlobal(width * height * sizeof(Pixel));
            }

            public delegate void ProcessPixelDelegate(Pixel* pixel);

            public void ProcessImage(ProcessPixelDelegate processAction)
            {
                Pixel* current = _pixels;
                for (int i = 0; i < _width * _height; i++)
                {
                    processAction(current);
                    current++;
                }
            }

            public void ApplyBrightnessAdjustment(int adjustment)
            {
                unsafe
                {
                    ProcessImage(pixel =>
                    {
                        pixel->Red = ClampToByte(pixel->Red + adjustment);
                        pixel->Green = ClampToByte(pixel->Green + adjustment);
                        pixel->Blue = ClampToByte(pixel->Blue + adjustment);
                    });
                }
            }

            public void ApplyGrayscale()
            {
                unsafe
                {
                    ProcessImage(pixel =>
                    {
                        byte gray = (byte)((pixel->Red * 0.3) +
                                         (pixel->Green * 0.59) +
                                         (pixel->Blue * 0.11));
                        pixel->Red = pixel->Green = pixel->Blue = gray;
                    });
                }
            }

            private byte ClampToByte(int value)
            {
                return (byte)Math.Clamp(value, 0, 255);
            }

            public void Dispose()
            {
                Marshal.FreeHGlobal((IntPtr)_pixels);
            }

        }
    }
}
