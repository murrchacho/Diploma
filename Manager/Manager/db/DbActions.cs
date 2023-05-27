using Manager.connection;

namespace Manager.db
{
    internal class DbActions
    {
        public DbActions(SocketConnection client, ConditionsContext db)
        {
            this.client = client;
            this.db = db;
        }
        private SocketConnection client { get; set; }
        private ConditionsContext db { get; set; }
        async public void Action(string request)
        {
            if (request == "getConditions")
            {
                List<Condition> conditions = db.Conditions.ToList();
                string stringConditions = Condition.parseToString(conditions);
                client.SendData(stringConditions);
            }

            if (request == "newConditions")
            {
                Rules rule = new Rules { DependsOn = "", Description = "", Action = "message", Message = "123", Condtion = "" };
                Rules[] r = new Rules[] { rule };
                Condition condition = new Condition { Contour = "1", Var = "temp", Rules = r };
                await db.Conditions.AddAsync(condition);
                await db.SaveChangesAsync();

                var m = db.Conditions.ToList();
                foreach (Condition c in m)
                {
                    Console.WriteLine($"{c.Id}.{c.Rules}");
                }
            }

        }
    }
}
