using System;
using DV.FrenteLoja.Core.Contratos.Enums;
using Newtonsoft.Json;

namespace DV.FrenteLoja.Core.Util
{
	public class FormatEnumAsStringDescription: JsonConverter
	{
		public override bool CanRead => false;
		public override bool CanWrite => true;
		public override bool CanConvert(Type type) => type == typeof(TipoFormaPagamento);

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var number = (TipoFormaPagamento)value;
			writer.WriteValue(number.GetDescription());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotSupportedException();
		}

	}
}