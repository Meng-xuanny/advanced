

var converter = new ObjectToTextConverter();

//Console.WriteLine(converter.Convert(
//    new House("123 Maple Road, Berrytown", 170.6, 2)));

//Console.WriteLine(converter.Convert(
//    new Pet("Blue", 4, 37)));
//var validPerson = new Person("john");
//var invalidDog = new Dog("r");
//var validator = new Validator();



//Console.WriteLine(validator.Validate(validPerson)? "person is valid":"person isn't valid");
//Console.WriteLine(validator.Validate(invalidDog)? "dog is valid":"dog isn't valid");
var tuple1 = Tuple.Create(3, true);
var tuple2= new Tuple<int, bool>(3, true);

var valueTuple1 = (Text:"hello", Value:35);
var valueTuple2 = new ValueTuple<string, int>("aaa", 6);
valueTuple1.Value = 3;



Console.WriteLine(tuple1==tuple2);
Console.WriteLine(tuple1.Equals(tuple2));
Console.WriteLine(tuple1.GetHashCode());
Console.WriteLine(tuple2.GetHashCode());

var point = new Point(1, 3);
var point1 = new Point(1, 3);

var tuple = Tuple.Create(-1, -3);
Point point3 = tuple;
//Console.WriteLine(point.GetHashCode());
//Console.WriteLine(point1.GetHashCode());
//Console.WriteLine(point3.GetHashCode());


//Console.WriteLine(point + point1);
//Console.WriteLine(point==point1);

//var newPoint = point;
//newPoint.Y = 100;

//Console.WriteLine(point.Equals(point1));
//var pointMoved = point with { X = 2, Y = 9 };

//Console.WriteLine(point);
//Console.WriteLine(pointMoved);

//someMethod(4);
someMethod(new Dog("lulu"));

var jay = new Person("jay", 23);
var xuan = new Person("xuan", 23);

//Console.WriteLine(jay.GetHashCode());
//Console.WriteLine(xuan.GetHashCode());

//Console.WriteLine(jay.Equals(xuan));



void someMethod<T> (T param) where T:class
{

}

internal class ObjectToTextConverter
{
   
    public string Convert(object obj)
    {
        var type = obj.GetType();
        var properties = type.GetProperties();

        return string.Join(", ",properties.Select(p=>$"{p.Name} is {p.GetValue(obj)}"));
    }
}

public class House
{
    public string Address { get; }
    public double Area { get; }
    public int Floor { get; }

    public House(string address, double area, int floor)
    {
        //if (address == null)
        //{
        //    throw new ArgumentNullException(nameof(address));
        //}
        //if (area == null)
        //{
        //    throw new ArgumentNullException(nameof(address));
        //}
       
        Address = address;
        Area = area;
        Floor = floor;
    }
}


public class Pet
{
    public string Name { get;}
    public int Age { get; }
    public double Weight { get;  }

    public Pet(string name, int age, double weight)
    {
        Name = name;
        Age = age;
        Weight = weight;
    }
}

public class Person
{
    [StringLengthValidate(2,10)]
    public string Name { get; init; }
    public int Id { get; init; }

    public Person(string name,int id)

    {
        Name = name;
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        return obj is Person other && Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id;
    }
}

public class Dog
{
    [StringLengthValidate(2, 25)]
    public string Name { get; }

    public Dog(string name)

    {
        Name = name;
    }
}


[AttributeUsage(AttributeTargets.Property)]
internal class StringLengthValidateAttribute : Attribute
{
    public int Min { get; } 
    public int Max { get; }

    public StringLengthValidateAttribute(int min, int max)
    {
        Min = min;
        Max = max;
    }
}


class Validator
{

    public bool Validate(object obj)
    {
        var type = obj.GetType();
        var properties = type.GetProperties().Where(p=>Attribute.IsDefined(p,typeof(StringLengthValidateAttribute)));

        foreach(var property in properties)
        {
            var propertyValue = property.GetValue(obj);
            if(propertyValue is not string)
            {
                throw new InvalidOperationException();
            }

            var value = (string)propertyValue;
            var attribute = (StringLengthValidateAttribute)property.GetCustomAttributes(true).First();

            if(value.Length < attribute.Min || value.Length > attribute.Max)
            {
                Console.WriteLine($"{property.Name} is invalid.");
                return false;
            }
        }

        return true;
    }
}

struct Point : IEquatable<Point>
{
    public int X { get; init; }
    public int Y { get; init; }

    public Point(int x,int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"X: {X}, Y: {Y}.";
    }

    public override bool Equals(object? obj)
    {
        return obj is Point point && Equals(point);
               
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public readonly bool Equals(Point other)
    {
        return X == other.X &&
               Y == other.Y;
    }

    public static Point operator +(Point point1, Point point2)
    {
        return new Point(point1.X + point2.X, point1.Y + point2.Y);
    }

    public static bool operator ==(Point point1, Point point2) => point1.Equals(point2);

    public static bool operator !=(Point point1, Point point2) => !point1.Equals(point2);

    public static implicit operator Point(Tuple<int, int> tuple)
    {
        return new Point(tuple.Item1, tuple.Item2);
    }
}