using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FormBase.DSample
{
    internal class DecoderSample
    {
        /*
          
          
        https://learn.microsoft.com/en-us/dotnet/api/system.text.decoder?view=net-8.0&devlangs=csharp&f1url=%3FappId%3DDev17IDEF1%26l%3DEN-US%26k%3Dk(System.Text.Decoder)%3Bk(SolutionItemsProject)%3Bk(DevLang-csharp)%26rd%3Dtrue
          

        To obtain an instance of an implementation of the Decoder class, call the GetDecoder method of an Encoding implementation.
        The GetCharCount method determines how many characters result in decoding a sequence of bytes, 
        and the GetChars method performs the actual decoding. 
        There are several versions of both of these methods available in the Decoder class. 
        For more information, see Encoding.GetChars. 
        A Decoder object maintains state information between successive calls to GetChars or Convert methods so it can correctly decode byte sequences that span blocks. 
        The Decoder also preserves trailing bytes at the end of data blocks and uses the trailing bytes in the next decoding operation. 
        Therefore, GetDecoder and GetEncoder are useful for network transmission and file operations because those operations often deal with blocks of data instead of a complete data stream.
        若要取得 Decoder 類別的實作的實例，請呼叫 Encoding 實作的 GetDecoder 方法。
        GetCharCount 方法決定解碼字節序列時產生多少個字符，而 GetChars 方法執行實際的解碼。 
        Decoder 類別中提供了這兩種方法的多個版本。 有關詳細信息，請參閱Encoding.GetChars。 
        
        Decoder 物件維護對 GetChars 或 Convert 方法的連續呼叫之間的狀態訊息，以便它可以正確解碼跨區塊的位元組序列。 
        解碼器還在資料塊末尾保留尾隨字節，並在下一個解碼操作中使用尾隨字節。 
        因此，GetDecoder 和 GetEncoder 對於網路傳輸和檔案操作非常有用，因為這些操作通常處理資料區塊而不是完整的資料流。

         */
    }
}
