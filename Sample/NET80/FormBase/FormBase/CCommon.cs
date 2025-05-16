using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace FormBase
{
    public static class CCommon
    {
        public static byte[] Serialize(object PObject)
            => JsonSerializer.SerializeToUtf8Bytes(PObject, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs), });
        public static void SerializeToFile(object PObject, string PFile_Create)
        {
            using FileStream stream1 = File.Create(PFile_Create);
            byte[] bytes = Serialize(PObject);
            stream1.Write(bytes, 0, bytes.Length);
        }

    }
}
