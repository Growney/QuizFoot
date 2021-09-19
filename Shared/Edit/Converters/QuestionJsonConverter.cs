using QuizFoot.Shared.Edit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Edit.Converters
{
    public class QuestionJsonConverter : JsonConverter<Question>
    {
        private const string c_id = "Id";
        private const string c_text = "Text";
        private const string c_type = "Type";
        private const string c_data = "Data";
        public override Question Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Question question = null;

            byte[] dataBytes = null;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                string propertyName = reader.GetString();

                switch (propertyName)
                {
                    case c_data:
                    case c_type:
                    case c_text:
                    case c_id:
                        if (question == null)
                        {
                            question = new Question();
                        }
                        reader.Read();
                        break;
                    default:
                        break;
                }

                switch (propertyName)
                {
                    case c_id:
                        {
                            if (Guid.TryParse(reader.GetString(), out var questionId))
                            {
                                question.Id = questionId;
                            };
                        }
                        break;
                    case c_text:
                        {
                            question.Text = reader.GetString();
                        }
                        break;
                    case c_type:
                        {
                            if (int.TryParse(reader.GetString(), out var typeInt))
                            {
                                question.Type = (QuestionType)typeInt;
                            }
                        }
                        break;
                    case c_data:
                        {
                            var dataText = reader.GetString();
                            dataBytes = Convert.FromBase64String(dataText);
                        }
                        break;
                    default:
                        break;
                }

            }

            if (question != null && dataBytes != null)
            {
                var jsonString = Encoding.UTF8.GetString(dataBytes);
                switch (question.Type)
                {
                    case QuestionType.MultipleChoice:
                        {
                            question.QuestionObject = JsonSerializer.Deserialize<MultipleChoiceQuestion>(jsonString, options);
                        }
                        break;
                    case QuestionType.String:
                        {
                            question.QuestionObject = JsonSerializer.Deserialize<StringQuestion>(jsonString, options);
                        }
                        break;
                    default:
                        break;
                }
            }

            return question;
        }

        public override void Write(Utf8JsonWriter writer, Question value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString(c_id, value.Id);
            writer.WriteString(c_text, value.Text);
            writer.WriteString(c_type, ((int)value.Type).ToString());


            var bytes = JsonSerializer.SerializeToUtf8Bytes(value.QuestionObject, options);
            writer.WriteBase64String(c_data, bytes);
            writer.WriteEndObject();
        }
    }

}
