/*
Test result: 2025-05-02


Run()
{
  "A": [
    1,
    2,
    3
  ],
  "B": {
    "X": 4,
    "Y": 5
  },
  "C": [
    {
      "Z": 6,
      "D": [
        {
          "P": true,
          "Q": false,
          "B": {
            "X": 0,
            "Y": 1
          }
        },
        {
          "P": true,
          "Q": true,
          "B": null
        }
      ]
    },
    {
      "Z": 7,
      "D": null
    }
  ]
}
{
  "A[0]": 1,
  "A[1]": 2,
  "A[2]": 3,
  "B.X": 4,
  "B.Y": 5,
  "C[0].Z": 6,
  "C[0].D[0].P": true,
  "C[0].D[0].Q": false,
  "C[0].D[0].B.X": 0,
  "C[0].D[0].B.Y": 1,
  "C[0].D[1].P": true,
  "C[0].D[1].Q": true,
  "C[0].D[1].B": null,
  "C[1].Z": 7,
  "C[1].D": null
}
Check Result = PASS
*/

using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace ConsoleBase.DJson
{
    internal class FlatJsonConvert_Test
    {
        public void Run()
        {
            AType data = new AType()
            {
                A = new int[] { 1, 2, 3 },
                B = new BType() { X = 4, Y = 5 },
                C = new CType[]
                {
                    new CType() {
                        Z = 6,
                        D = new DType[] {
                            new DType() {
                                P = true,
                                Q = false,
                                B = new BType() { X = 0, Y = 1 }
                            },
                            new DType()
                            {
                                P = true,
                                Q = true                            }
                        }
                    },
                    new CType() { Z = 7}
                }
            };
            var Options1 = new JsonSerializerOptions() { WriteIndented = true };

            var origJson = Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(data, Options1));
            var flattenJson = FlatJsonConvert.Serialize<AType>(data);
            var restored = FlatJsonConvert.Deserialize<AType>(flattenJson);
            var restoredJson = Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(restored, Options1));

            Debug.WriteLine(origJson);
            Debug.WriteLine(flattenJson);
            Debug.WriteLine("Check Result = " + (restoredJson == origJson ? "PASS" : "FAIL"));

            Console.WriteLine(origJson);
            Console.WriteLine(flattenJson);
            Console.WriteLine("Check Result = " + (restoredJson == origJson ? "PASS" : "FAIL"));
        }
    }

    public class AType
    {
        public int[]? A { get; set; }
        public BType? B { get; set; }
        public CType[]? C { get; set; }
    }

    public class BType
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class CType
    {
        public int Z { get; set; }
        public DType[]? D { get; set; }
    }

    public class DType
    {
        public bool P { get; set; }
        public bool Q { get; set; }
        public BType? B { get; set; }
    }
}
