using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Parser.parser
{
    internal class Condition
    {
        public int Id { get; set; }
        public string Contour { get; set; }
        public string Var { get; set; }
        [Column(TypeName = "jsonb")]
        public Rules[] Rules { get; set; }
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
