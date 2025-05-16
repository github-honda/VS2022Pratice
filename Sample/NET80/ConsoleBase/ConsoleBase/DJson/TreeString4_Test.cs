/*

Test result, 2025-05-02

Run()
ProjectName=專案名稱.
ConnectionString=HostDatabaseUserPassword.
{
  "Item": [
    {
      "Item": [
        {
          "Item": [
            {
              "Item": null,
              "Name": "111N",
              "Text": "111T",
              "Value": "111V",
              "Type": "111F"
            },
            {
              "Item": null,
              "Name": "112N",
              "Text": "112T",
              "Value": null,
              "Type": null
            }
          ],
          "Name": "11N",
          "Text": "11T",
          "Value": "11V",
          "Type": "11F"
        }
      ],
      "Name": "1N",
      "Text": "1T",
      "Value": "1V",
      "Type": "1F"
    },
    {
      "Item": [
        {
          "Item": [
            {
              "Item": null,
              "Name": "211N",
              "Text": "211T",
              "Value": "211V",
              "Type": "211F"
            },
            {
              "Item": null,
              "Name": "212N",
              "Text": "212T",
              "Value": null,
              "Type": null
            }
          ],
          "Name": "21N",
          "Text": "21T",
          "Value": "21V",
          "Type": "21F"
        }
      ],
      "Name": "2N",
      "Text": "2T",
      "Value": "2V",
      "Type": "2F"
    }
  ],
  "Name": "RootN",
  "Text": "RootT",
  "Value": "RootV",
  "Type": "RootF"
}
{
  "Item[0].Item[0].Item[0].Item": null,
  "Item[0].Item[0].Item[0].Name": "111N",
  "Item[0].Item[0].Item[0].Text": "111T",
  "Item[0].Item[0].Item[0].Value": "111V",
  "Item[0].Item[0].Item[0].Type": "111F",
  "Item[0].Item[0].Item[1].Item": null,
  "Item[0].Item[0].Item[1].Name": "112N",
  "Item[0].Item[0].Item[1].Text": "112T",
  "Item[0].Item[0].Item[1].Value": null,
  "Item[0].Item[0].Item[1].Type": null,
  "Item[0].Item[0].Name": "11N",
  "Item[0].Item[0].Text": "11T",
  "Item[0].Item[0].Value": "11V",
  "Item[0].Item[0].Type": "11F",
  "Item[0].Name": "1N",
  "Item[0].Text": "1T",
  "Item[0].Value": "1V",
  "Item[0].Type": "1F",
  "Item[1].Item[0].Item[0].Item": null,
  "Item[1].Item[0].Item[0].Name": "211N",
  "Item[1].Item[0].Item[0].Text": "211T",
  "Item[1].Item[0].Item[0].Value": "211V",
  "Item[1].Item[0].Item[0].Type": "211F",
  "Item[1].Item[0].Item[1].Item": null,
  "Item[1].Item[0].Item[1].Name": "212N",
  "Item[1].Item[0].Item[1].Text": "212T",
  "Item[1].Item[0].Item[1].Value": null,
  "Item[1].Item[0].Item[1].Type": null,
  "Item[1].Item[0].Name": "21N",
  "Item[1].Item[0].Text": "21T",
  "Item[1].Item[0].Value": "21V",
  "Item[1].Item[0].Type": "21F",
  "Item[1].Name": "2N",
  "Item[1].Text": "2T",
  "Item[1].Value": "2V",
  "Item[1].Type": "2F",
  "Name": "RootN",
  "Text": "RootT",
  "Value": "RootV",
  "Type": "RootF"
}

 
 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleBase.DItem;

namespace ConsoleBase.DJson
{
    internal class TreeString4_Test
    {
        public void Run()
        {

            TreeString4  SampleTree1 = new TreeString4()
            {
                Name = "RootN",
                Text = "RootT",
                Value = "RootV",
                Type = "RootF",
                Items = new TreeString4[]
                {
                new TreeString4()
                {
                    Name = "1N", Text = "1T", Value = "1V", Type = "1F",
                    Items = new TreeString4[]
                    {
                        new TreeString4()
                        {
                            Name = "11N", Text = "11T", Value = "11V", Type="11F",
                            Items = new TreeString4[]
                            {
                                new TreeString4()
                                {
                                    Name = "111N", Text = "111T", Value = "111V", Type="111F",
                                },
                                new TreeString4()
                                {
                                    Name = "112N", Text = "112T",
                                },
                            },
                        },
                    },
                },
                new TreeString4()
                {
                    Name = "2N", Text = "2T", Value = "2V", Type = "2F",
                    Items = new TreeString4[]
                    {
                        new TreeString4()
                        {
                            Name = "21N", Text = "21T", Value = "21V", Type="21F",
                            Items = new TreeString4[]
                            {
                                new TreeString4()
                                {
                                    Name = "211N", Text = "211T", Value = "211V", Type="211F",
                                },
                                new TreeString4()
                                {
                                    Name = "212N", Text = "212T",
                                },
                            },
                        },
                    },
                },
                }
            };


            // 1. Serialize
            JsonSerializerOptions option1 = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
            string json = Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes<TreeString4>(SampleTree1, option1));
            Debug.WriteLine(json);
            Console.WriteLine(json);

            // 2. Deserialize
            TreeString4? data = FlatJsonConvert.Deserialize<TreeString4>(json);
            if (data == null) throw new Exception("Deserialize failed");

            string flattenJson = FlatJsonConvert.Serialize<TreeString4>(data);
            Debug.WriteLine(flattenJson);
            Console.WriteLine(flattenJson);
        }


        //public class String_NameTextValue : String4
        //{
        //    /// <summary>
        //    /// nested children
        //    /// </summary>
        //    public String_NameTextValue[]? _C { get; set; }

        //}

        //public class AType2
        //{
        //    /// <summary>
        //    /// nested children
        //    /// </summary>
        //    public String_NameTextValue[]? _C { get; set; }
        //}

    }
}
