using System;
using System.Globalization;
using Newtonsoft.Json;

namespace DV.FrenteLoja.Core.Util
{
    internal sealed class FormatNumbersAsTextConverter : JsonConverter
    {
        public override bool CanRead => false;
        public override bool CanWrite => true;
        public override bool CanConvert(Type type) => type == typeof(int);

        public override void WriteJson(
            JsonWriter writer, object value, JsonSerializer serializer)
        {
            int number = (int)value;
            writer.WriteValue(number.ToString(CultureInfo.InvariantCulture));
        }

        public override object ReadJson(
            JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}