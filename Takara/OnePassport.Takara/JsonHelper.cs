using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

using OnePassport.Takara.Model;
using OnePassport.Takara.Json;
using OnePassport.Takara.Xml;

namespace OnePassport.Takara
{
    /// <summary>
    /// JsonHelper to deserialize to model
    /// </summary>
    public static class JsonHelper
    {
        public static TakaraChest DeserializeFromJson(TextReader input)
        {
            JsonReaderSettings settings = new JsonReaderSettings();
            settings.AllowNullValueTypes = true;
            
            JsonDataReader jdr = new JsonDataReader(settings);

            object takaraObject = jdr.Deserialize(input, typeof(OnePassport.Takara.Model.TakaraChest));

            return (TakaraChest) takaraObject;
        }

        public static string SerializeToJson(TakaraChest takaraObject)
        {
            JsonWriterSettings settings = new JsonWriterSettings();
            settings.MaxDepth = 10;

            JsonDataWriter jdw = new JsonDataWriter(settings);

            StringWriter data = new StringWriter();

            jdw.Serialize(data, takaraObject);

            return data.ToString();
        }


        //public static string SerializeToJSON(this JsonValue jv)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    SerializeToJSON(sb, jv, 0, true);
        //    return sb.ToString();
        //}

        ///// <summary>
        ///// This is a recursive funtion to write into JSON format.
        ///// </summary>
        ///// <param name="sb"></param>
        ///// <param name="jv"></param>
        ///// <param name="indent"></param>
        ///// <param name="first"></param>
        //static void SerializeToJSON(StringBuilder sb, JsonValue jv, int indent, bool first)
        //{
        //    if (jv == null)
        //    {
        //        AddIndent(sb, indent);
        //        sb.Append("null");
        //    }
        //    else
        //    {
        //        int i;
        //        switch (jv.JsonType)
        //        {
        //            case JsonType.Array:
        //                if (!first)
        //                {
        //                    AddIndent(sb, indent);
        //                }

        //                sb.AppendLine("[");
        //                for (i = 0; i < jv.Count; i++)
        //                {
        //                    SerializeToJSON(sb, jv[i], indent + 1, false);
        //                    if (i < jv.Count - 1)
        //                    {
        //                        sb.AppendLine(",");
        //                    }
        //                    else
        //                    {
        //                        sb.AppendLine();
        //                    }
        //                }
        //                AddIndent(sb, indent);
        //                sb.Append(']');
        //                break;

        //            case JsonType.Object:
        //                if (!first)
        //                {
        //                    AddIndent(sb, indent);
        //                }
        //                else
        //                {
        //                    first = false;
        //                }

        //                sb.AppendLine("{");
        //                i = 0;

        //                foreach (string key in ((JsonObject)jv).Keys)
        //                {
        //                    SerializeToJSON(sb, new JsonPrimitive(key), indent + 1, true);
        //                    sb.Append(": ");
        //                    JsonValue value = jv[key];

        //                    if (value == null || value.JsonType == JsonType.Boolean || value.JsonType == JsonType.Number || value.JsonType == JsonType.String)
        //                    {
        //                        SerializeToJSON(sb, value, 0, true);
        //                    }
        //                    else
        //                    {
        //                        SerializeToJSON(sb, value, indent + 1, true);
        //                    }

        //                    i++;

        //                    if (i < jv.Count)
        //                    {
        //                        sb.AppendLine(",");
        //                    }
        //                    else
        //                    {
        //                        sb.AppendLine();
        //                    }
        //                }

        //                AddIndent(sb, indent);
        //                sb.Append('}');

        //                break;

        //            case JsonType.Boolean:
        //            case JsonType.Number:
        //            case JsonType.String:
        //                AddIndent(sb, indent);
        //                sb.Append(jv.ToString());
        //                break;
        //        }
        //    }
        //}

        //static void AddIndent(StringBuilder sb, int indent)
        //{
        //    for (int i = 0; i < indent * 2; i++)
        //    {
        //        sb.Append(" ");
        //    }
        //}

    }
}