using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvencedCSharpExcs
{

    //Here is an excercise about overriding Equality, Comparability and overloading operators
    internal class OverRidingEquality
    {
        public void MainMe()
        {
            Area area1 = new(12, 15), area2 = new(0,5);
            Console.WriteLine(area1.Equals(area2)); // True
            Console.WriteLine(area1 == area2); // True
            Console.WriteLine(area2 != area1); //false


            bool after2010 = DateTime.Now > new DateTime(2010, 1, 1);
            Console.Write(after2010.ToString());  //True

            Note n1 = new(125),n2 = new(220);
            Console.WriteLine(n1.CompareTo(n2));
            Console.WriteLine(n1<n2);  // <0



        }
    }

    public struct Area : IEquatable<Area>
    {
        public readonly int Measure1;
        public readonly int Measure2;
        public Area(int m1, int m2)
        {
            Measure1 = Math.Min(m1, m2);
            Measure2 = Math.Max(m1, m2);
        }
        public override bool Equals(object other)
        => other is Area a && Equals(a); // Calls method below
        public bool Equals(Area other) // Implements IEquatable<Area>
        => Measure1 == other.Measure1 && Measure2 == other.Measure2;
        public override int GetHashCode()
        => HashCode.Combine(Measure1, Measure2);
        // Note that we call the static Equals method in the object class: this
        // does null checking before calling our own (instance) Equals method.
        public static bool operator ==(Area a1, Area a2) => Equals(a1, a2);
        public static bool operator !=(Area a1, Area a2) => !(a1 == a2);
    }

    public struct Note : IComparable<Note>, IEquatable<Note>, IComparable
    {
        private int _semitonesFromA;
        public int SemitonesFromA { get { return _semitonesFromA; } }
        public Note(int semitonesFromA)
        {
            _semitonesFromA = semitonesFromA;
        }
        public int CompareTo(Note other) // Generic IComparable<T>
        {
            if (Equals(other)) return 0; // Fail-safe check
            return _semitonesFromA.CompareTo (other._semitonesFromA);
        }
        int IComparable.CompareTo(object other) // Nongeneric IComparable
        {
            if (!(other is Note))
                throw new InvalidOperationException("CompareTo: Not a note");
            return CompareTo((Note)other);
        }
        public static bool operator <(Note n1, Note n2)
        => n1.CompareTo(n2) < 0;
        public static bool operator >(Note n1, Note n2)
        => n1.CompareTo(n2) > 0;
        public bool Equals(Note other) // for IEquatable<Note>
        => _semitonesFromA == other._semitonesFromA;
        public override bool Equals(object other)
        {
            if (!(other is Note)) return false;
            return Equals((Note)other);
        }
        public override int GetHashCode() => _semitonesFromA.GetHashCode();
        // Call the static Equals method to ensure nulls are properly handled:
        public static bool operator ==(Note n1, Note n2) => Equals(n1, n2);
        public static bool operator !=(Note n1, Note n2) => !(n1 == n2);
    }

    //contravariance
    public interface ICalculator<in T>
    {
        int CompareTo(T other);
    }

    public class Calculator : ICalculator<Area>
    {
        private Area _area = new(13, 150);
        public int CompareTo(Area area)
        {
            return (area == _area) ? 1 : 0;
        }

    }

    //covariance
    public interface ICalc<out T>
    {
        T GetValue();
    }

    public class Calc : ICalc<Area>
    {
        private Area _area;
        
        public Area GetValue()
        {
            ImmutableArray<int> array = [1, 2, 3];
            ImmutableArray<Area>.Builder builder = ImmutableArray.CreateBuilder<Area>();
            builder.Add(_area);
            return builder[0];
        }
       
    }


  
}
