/*

ref:
https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/indexers/
  
You define indexers when instances of a class or struct can be indexed like an array or other collection. The indexed value can be set or retrieved without explicitly specifying a type or instance member. Indexers resemble properties except that their accessors take parameters.

The following example defines a generic class with get and set accessor methods to assign and retrieve values.
namespace Indexers;

public class SampleCollection<T>
{
   // Declare an array to store the data elements.
   private T[] arr = new T[100];

   // Define the indexer to allow client code to use [] notation.
   public T this[int i]
   {
      get => arr[i];
      set => arr[i] = value;
   }
}

The preceding example shows a read / write indexer. It contains both the get and set accessors. You can define read only indexers as an expression bodied member, as shown in the following examples:
namespace Indexers;

public class ReadOnlySampleCollection<T>(params IEnumerable<T> items)
{
   // Declare an array to store the data elements.
   private T[] arr = [.. items];

   public T this[int i] => arr[i];

}

The get keyword isn't used; => introduces the expression body.

Indexers enable indexed properties: properties referenced using one or more arguments. Those arguments provide an index into some collection of values.

- Indexers enable objects to be indexed similar to arrays.
- A get accessor returns a value. A set accessor assigns a value.
- The this keyword defines the indexer.
- The value keyword is the argument to the set accessor.
- Indexers don't require an integer index value; it's up to you how to define the specific look-up mechanism.
- Indexers can be overloaded.
- Indexers can have one or more formal parameters, for example, when accessing a two-dimensional array.
- You can declare partial indexers in partial types.
- You can apply almost everything you learned from working with properties to indexers. The only exception to that rule is automatically implemented properties. The compiler can't always generate the correct storage for an indexer. You can define multiple indexers on a type, as long as the argument lists for each indexer is unique.


Uses of indexers
You define indexers in your type when its API models some collection. Your indexer isn't required to map directly to the collection types that are part of the .NET core framework. Indexers enable you to provide the API that matches your type's abstraction without exposing the inner details of how the values for that abstraction are stored or computed.

Arrays and Vectors
Your type might model an array or a vector. The advantage of creating your own indexer is that you can define the storage for that collection to suit your needs. Imagine a scenario where your type models historical data that is too large to load into memory at once. You need to load and unload sections of the collection based on usage. The example following models this behavior. It reports on how many data points exist. It creates pages to hold sections of the data on demand. It removes pages from memory to make room for pages needed by more recent requests.

namespace Indexers;

public record Measurements(double HiTemp, double LoTemp, double AirPressure);

public class DataSamples
{
    private class Page
    {
        private readonly List<Measurements> pageData = new ();
        private readonly int _startingIndex;
        private readonly int _length;

        public Page(int startingIndex, int length)
        {
            _startingIndex = startingIndex;
            _length = length;

            // This stays as random stuff:
            var generator = new Random();
            for (int i = 0; i < length; i++)
            {
                var m = new Measurements(HiTemp: generator.Next(50, 95),
                    LoTemp: generator.Next(12, 49),
                    AirPressure: 28.0 + generator.NextDouble() * 4
                );
                pageData.Add(m);
            }
        }
        public bool HasItem(int index) =>
            ((index >= _startingIndex) &&
            (index < _startingIndex + _length));

        public Measurements this[int index]
        {
            get
            {
                LastAccess = DateTime.Now;
                return pageData[index - _startingIndex];
            }
            set
            {
                pageData[index - _startingIndex] = value;
                Dirty = true;
                LastAccess = DateTime.Now;
            }
        }

        public bool Dirty { get; private set; } = false;
        public DateTime LastAccess { get; set; } = DateTime.Now;
    }

    private readonly int _totalSize;
    private readonly List<Page> pagesInMemory = new ();

    public DataSamples(int totalSize)
    {
        this._totalSize = totalSize;
    }

    public Measurements this[int index]
    {
        get
        {
            if (index < 0) throw new IndexOutOfRangeException("Cannot index less than 0");
            if (index >= _totalSize) throw new IndexOutOfRangeException("Cannot index past the end of storage");

            var page = updateCachedPagesForAccess(index);
            return page[index];
        }
        set
        {
            if (index < 0) throw new IndexOutOfRangeException("Cannot index less than 0");
            if (index >= _totalSize) throw new IndexOutOfRangeException("Cannot index past the end of storage");
            var page = updateCachedPagesForAccess(index);

            page[index] = value;
        }
    }

    private Page updateCachedPagesForAccess(int index)
    {
        foreach (var p in pagesInMemory)
        {
            if (p.HasItem(index))
            {
                return p;
            }
        }
        var startingIndex = (index / 1000) * 1000;
        var newPage = new Page(startingIndex, 1000);
        addPageToCache(newPage);
        return newPage;
    }

    private void addPageToCache(Page p)
    {
        if (pagesInMemory.Count > 4)
        {
            // remove oldest non-dirty page:
            var oldest = pagesInMemory
                .Where(page => !page.Dirty)
                .OrderBy(page => page.LastAccess)
                .FirstOrDefault();
            // Note that this may keep more than 5 pages in memory
            // if too much is dirty
            if (oldest != null)
                pagesInMemory.Remove(oldest);
        }
        pagesInMemory.Add(p);
    }
}
You can follow this design idiom to model any sort of collection where there are good reasons not to load the entire set of data into an in-memory collection. Notice that the Page class is a private nested class that isn't part of the public interface. Those details are hidden from users of this class.


Dictionaries
Another common scenario is when you need to model a dictionary or a map. This scenario is when your type stores values based on key, possibly text keys. This example creates a dictionary that maps command line arguments to lambda expressions that manage those options. The following example shows two classes: an ArgsActions class that maps a command line option to an System.Action delegate, and an ArgsProcessor that uses the ArgsActions to execute each Action when it encounters that option.

namespace Indexers;
public class ArgsProcessor
{
    private readonly ArgsActions _actions;

    public ArgsProcessor(ArgsActions actions)
    {
        _actions = actions;
    }

    public void Process(string[] args)
    {
        foreach (var arg in args)
        {
            _actions[arg]?.Invoke();
        }
    }

}
public class ArgsActions
{
    readonly private Dictionary<string, Action> _argsActions = new();

    public Action this[string s]
    {
        get
        {
            Action? action;
            Action defaultAction = () => { };
            return _argsActions.TryGetValue(s, out action) ? action : defaultAction;
        }
    }

    public void SetOption(string s, Action a)
    {
        _argsActions[s] = a;
    }
}

In this example, the ArgsAction collection maps closely to the underlying collection. The get determines if a given option is configured. If so, it returns the Action associated with that option. If not, it returns an Action that does nothing. The public accessor doesn't include a set accessor. Rather, the design is using a public method for setting options.



In this example, the ArgsAction collection maps closely to the underlying collection. The get determines if a given option is configured. If so, it returns the Action associated with that option. If not, it returns an Action that does nothing. The public accessor doesn't include a set accessor. Rather, the design is using a public method for setting options.
namespace Indexers;
public class Mandelbrot(int maxIterations)
{

    public int this[double x, double y]
    {
        get
        {
            var iterations = 0;
            var x0 = x;
            var y0 = y;

            while ((x * x + y * y < 4) &&
                (iterations < maxIterations))
            { 
                (x, y) = (x * x - y * y + x0, 2 * x * y + y0);
                iterations++;
            }
            return iterations;
        }
    }
}

The Mandelbrot Set defines values at every (x,y) coordinate for real number values. That defines a dictionary that could contain an infinite number of values. Therefore, there's no storage behind the set. Instead, this class computes the value for each point when code calls the get accessor. There's no underlying storage used.

Summing Up
You create indexers anytime you have a property-like element in your class where that property represents not a single value, but rather a set of values. One or more arguments identify each individual item. Those arguments can uniquely identify which item in the set should be referenced. Indexers extend the concept of properties, where a member is treated like a data item from outside the class, but like a method on the inside. Indexers allow arguments to find a single item in a property that represents a set of items.




 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormBase.DSample
{
    internal class IndexerSample
    {

        class SampleCollection1<T>
        {
            // Declare an array to store the data elements.
            private T[] arr = new T[100];

            // Define the indexer to allow client code to use [] notation.
            public T this[int i]
            {
                get { return arr[i]; }
                set { arr[i] = value; }
            }
        }

        class SampleCollection2<T>
        {
            // Declare an array to store the data elements.
            private T[] arr = new T[100];
            int nextIndex = 0;

            // Define the indexer to allow client code to use [] notation.
            public T this[int i] => arr[i];

            public void Add(T value)
            {
                if (nextIndex >= arr.Length)
                    throw new IndexOutOfRangeException($"The collection can hold only {arr.Length} elements.");
                arr[nextIndex++] = value;
            }
        }

        class SampleCollection3<T>
        {
            // Declare an array to store the data elements.
            private T[] arr = new T[100];

            // Define the indexer to allow client code to use [] notation.
            public T this[int i]
            {
                get => arr[i];
                set => arr[i] = value;
            }
        }
    }
}
