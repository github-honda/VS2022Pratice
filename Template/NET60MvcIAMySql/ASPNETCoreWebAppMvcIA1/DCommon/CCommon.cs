using System.Diagnostics.CodeAnalysis;

namespace NET60MvcIAMySql.DCommon
{
#nullable enable
    public static class CCommon
    {
        public static bool ZIsDBNull([NotNullWhen(false)] this object? PObject) 
        {
            return (PObject == DBNull.Value);
        }
        public static bool ZIsNullOrDBNull([NotNullWhen(false)] this object? PObject) 
        {
            if (PObject == null)
                return true;
            return ZIsDBNull(PObject);
        }
        public static string? ZToString(this object? PValue, string? PDefault = null)
        {
            if (PValue.ZIsNullOrDBNull())
                return PDefault;
            return PValue.ToString();
        }

    }
}
