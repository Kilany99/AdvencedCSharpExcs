using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AdvencedCSharpExcs
{

    //    Requirements:
    //- Create a PhotoProcessor class that uses delegates to apply multiple filters to a photo
    //- Implement at least 3 different filter methods(Brightness, Contrast, Resize)
    //- Allow filters to be chained together
    //- Each filter should modify a Photo object with properties like Brightness, Contrast, Size
    //-used operators overloading as a plus from my side.
    internal class Delegates
    {
    }
    public delegate Photo PhotoFilterHandler(Photo photo);
    public delegate Func<Photo, Photo> PPP(Photo photo,int factor);

    public class PhotoProcessor
    {
        private const int BrightnessFactor = 100;
        private const int ContrastFactor = 125;
        private const int SizeFactor = 1750;

        public Photo Brightness(Photo photo) { photo.Brightness *= BrightnessFactor; return photo; }
        public Photo Contrast(Photo photo) { photo.Contrast *= ContrastFactor; return photo; }
        public Photo Size(Photo photo) { photo.Size *= SizeFactor; return photo; }

        public Func<Photo, Photo> Facoraization(Photo photo, int factor)
        {
            return X => 
            { 
                return photo*factor; 
            };
        }


    }
    public struct Photo
    {
        public Photo(int brigh, int cont, double size) 
        { 
            Size = size;
            Brightness = brigh;
            Contrast = cont;
        }
        public int Brightness { get; set; }
        public int Contrast { get; set; }
        public double Size { get; set; }
        public static Photo operator *(Photo photo,int factor)
        {
            photo.Size *= factor;
            return photo;
        }
    }

    public class MainClass
    {
        public static PhotoProcessor processor = new PhotoProcessor();
        public Photo photo = new Photo(300, 1200, 15254);
        public void Main(string[] args)
        {
            Func<Photo, Photo> p = processor.Facoraization(photo, 1500);
            Console.WriteLine(photo.Brightness.ToString() + "  " + photo.Contrast.ToString() + "" + photo.Size.ToString());
            PhotoFilterHandler pfh = processor.Brightness;
            pfh(photo);
            Console.WriteLine(photo.Brightness.ToString());
            pfh += processor.Contrast;
            pfh(photo);
            Console.WriteLine(photo.Brightness.ToString()+"  "+photo.Contrast.ToString());
            pfh += processor.Size;
            pfh(photo);
            Console.WriteLine(photo.Brightness.ToString() + "  " + photo.Contrast.ToString() + "" + photo.Size.ToString());

        }
    }
}
