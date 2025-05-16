﻿/*
FlatJsonConvert.cs

CodeHelper\cs\vs2022\VS2022Pratice\Sample\NET80\ConsoleBase\ConsoleBase\DJson\FlatJsonConvert.txt
  
 */

using System.Dynamic;
using System.Text;
using System.Text.Json;

namespace ConsoleBase.DJson
{
    /// <summary>
    /// Flatten nested JSON 
    /// { "A":[1,2,3], "B":{"X":4,"Y":5},"C":[{"Z":6},{"Z":7}]}
    /// to simple key/value dictionary
    /// {"A[0]":1,"A[1]":2,"A[2]": 3,"B.X":4,"B.Y":5,"C[0].Z":6,"C[1].Z":7}
    /// </summary>
    public class FlatJsonConvert
    {
        static void ExploreAddProps(Dictionary<string, object?> props, string name, Type? type, object? value)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (value == null || type.IsPrimitive || type == typeof(DateTime) || type == typeof(string))
                props.Add(name, value);
            else if (type.IsArray)
            {
                var a = (Array)value;
                for (var i = 0; i < a.Length; i++)
                    ExploreAddProps(props, $"{name}[{i}]", type.GetElementType(), a.GetValue(i));
            }
            else
            {
                type.GetProperties().ToList()
                    .ForEach(p =>
                    {
                        var prefix = string.IsNullOrEmpty(name) ? string.Empty : name + ".";
                        ExploreAddProps(props, $"{prefix}{p.Name}", p.PropertyType, p.GetValue(value));
                    });
            }
        }

        public static string Serialize<T>(T data)
        {
            var props = new Dictionary<string, object?>();
            ExploreAddProps(props, string.Empty, typeof(T), data);
            JsonSerializerOptions option1 = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
            return Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(props, option1));
        }

        public static T? Deserialize<T>(string json)
        {
            var props = JsonSerializer.Deserialize<Dictionary<string, object?>>(json);
            if (props == null) return default;
            ExpandoObject data = new ExpandoObject();
            Action<string, object?> SetProp = (propName, propValue) =>
            {
                var seg = propName.Split('.');
                object? curr = data;
                for (var i = 0; i < seg.Length; i++)
                {
                    if (curr == null) continue;
                    var n = seg[i];
                    var isLeaf = i == seg.Length - 1;
                    if (n.Contains("[")) //for array
                    {
                        var pn = n.Split('[').First();
                        var d = curr as IDictionary<string, object?>;
                        if (d == null) continue;

                        if (!d.ContainsKey(pn)) d[pn] = new List<object>();
                        if (isLeaf)
                        {
                            var dpn = d[pn] as List<object?>;
                            if (dpn == null) continue;
                            dpn.Add(propValue);
                        }
                        else
                        {
                            var idx = int.Parse(n.Split('[').Last().TrimEnd(']'));
                            var lst = (d[pn] as List<object>);
                            if (lst == null) continue;
                            if (idx == lst.Count) lst.Add(new ExpandoObject());
                            if (idx < lst.Count)
                                curr = lst[idx];
                            else
                                throw new NotImplementedException("Skiped index is not supported");
                        }
                    }
                    else //for property
                    {
                        if (curr is List<object>)
                            throw new NotImplementedException("Array of array is not supported");
                        else
                        {
                            var d = curr as IDictionary<string, object?>;
                            if (d == null) continue;
                            if (isLeaf)
                                d[n] = propValue;
                            else
                            {
                                if (!d.ContainsKey(n)) d[n] = new ExpandoObject();
                                curr = d[n];
                            }
                        }
                    }
                }
            };

            props.Keys.OrderBy(o => o.Split('.').Length) //upper level first
                .ThenBy(o => //prop first, array elements ordered by index
                    !o.Split('.').Last().Contains(']') ? -1 :
                    int.Parse(o.Split('.').Last().Split('[').Last().TrimEnd(']')))
                .ToList().ForEach(o => SetProp(o, props[o]));

            var unflattenJson = JsonSerializer.SerializeToUtf8Bytes(data);
            return JsonSerializer.Deserialize<T>(unflattenJson);
        }
    }
}

