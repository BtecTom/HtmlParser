using System.Text;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using SchemaManager.Properties;

namespace SchemaManager
{
    public static class Schema
    {
        public static void PublishSchema()
        {
            /* todo: in the real implementation this might want to publish to an external resource that clients can use.
            For now this just writes to an internal resource */
            var path = @"C:\Users\tomjb\Documents\Xapien\HtmlParser\SchemaManager\Properties\LastestSchema.Json";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.Create(path).Close();

            File.WriteAllText(path, Generate().ToString());
        }

        public static string GetSchemaVersion()
        {
            var htmlParserConfig = Resources.SchemaConfig;

            string jsonString = Encoding.UTF8.GetString(htmlParserConfig);
            var settings = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

            return settings?["SchemaVersion"].ToString() ?? string.Empty;
        }

        public static JSchema GetLatestSchema()
        {
            /* todo: in the real implementation this might want to pull from an external resource in the same
             way that clients would use this. For now this just reads from an internal resource */
            var htmlParserConfig = Resources.LastestSchema;

            var jsonString = Encoding.UTF8.GetString(htmlParserConfig);
            return JSchema.Parse(jsonString);
        }

        public static bool Validate(JSchema schema, string json)
        {
            var parsedJson = JToken.Parse(json);
            return parsedJson.IsValid(schema);
        }

        public static string ReportToJson(Report report)
        {
            return JsonConvert.SerializeObject(report);
        }

        private static JSchema Generate()
        {
            var generator = new JSchemaGenerator();
            var schema = generator.Generate(typeof(Report));
            return schema;
        }
    }
}
