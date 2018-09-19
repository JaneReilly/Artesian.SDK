//using Newtonsoft.Json;
//using System.ComponentModel.DataAnnotations;
//using System;
//using Newtonsoft.Json.Linq;

//namespace Artesian.SDK.Dto
//{
//    public class OperationConverter : JsonConverter
//    {
//        public override bool CanConvert(Type objectType)
//        {
//            return objectType == typeof(OperationParams);
//        }

//        public override bool CanWrite
//        {
//            get
//            {
//                return false;
//            }
//        }

//        class FakeParams
//        {
//            [Required]
//            public OperationType Type { get; set; }

//            [Required]
//            public JObject Params { get; set; }
//        }

//        public override object ReadJson(JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
//        {
//            if (reader.TokenType == JsonToken.Null)
//                return null;

//            var fake = serializer.Deserialize<FakeParams>(reader);
//            if (fake == null)
//                return null;
            
//            var target = existingValue as OperationParams;
//            if (target == null)
//                target = new OperationParams();

//            target.Type = fake.Type;

//            switch (target.Type)
//            {
//                case OperationType.EnableTag:
//                    target.Params = fake.Params.ToObject<OperationEnableDisableTag>(serializer);
//                    break;
//                case OperationType.DisableTag:
//                    target.Params = fake.Params.ToObject<OperationEnableDisableTag>(serializer);
//                    break;
//                case OperationType.UpdateAggregationRule:
//                    target.Params = fake.Params.ToObject<OperationUpdateAggregationRule>(serializer);
//                    break;
//                case OperationType.UpdateTimeTransformID:
//                    target.Params = fake.Params.ToObject<OperationUpdateTimeTransform>(serializer);
//                    break;
//                case OperationType.UpdateOriginalTimeZone:
//                    target.Params = fake.Params.ToObject<OperationUpdateOriginalTimeZone>(serializer);
//                    break;
//                case OperationType.UpdateProviderDescription:
//                    target.Params = fake.Params.ToObject<OperationUpdateProviderDescription>(serializer);
//                    break;
//                default:
//                    throw new InvalidOperationException();
//            }

//            return target;
//        }

//        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
