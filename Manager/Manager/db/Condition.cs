using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Manager.db
{
    internal class Condition
    {
        public int Id { get; set; }
        public string Contour { get; set; }
        public string Var { get; set; }
        [Column(TypeName = "jsonb")]
        public Rules[] Rules { get; set; }
        public static string parseToString(List<Condition> conditions)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(conditions, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }
    }
    public class Rules
    {
        public string DependsOn { get; set; }
        public string Condtion { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }

    }
}
