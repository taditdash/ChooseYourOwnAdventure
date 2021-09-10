using ChooseYourOwnAdventure.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ChooseYourOwnAdventure.Services
{
    public class ChooseYourOwnAdventureService
    {
        private readonly IConfiguration _configuration;

        public ChooseYourOwnAdventureService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Decision GetDecisions()
        {
            Decision root = new Decision();

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = "GetDecisions",
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();

                using SqlDataReader sqlreader = cmd.ExecuteReader();
                while (sqlreader.Read())
                {
                    int decisionId = sqlreader.GetInt32("DecisionId");

                    var decision = new Decision
                    {
                        DecisionId = decisionId,
                        Description = sqlreader.GetString("Description"),
                    };
                    if (!sqlreader.IsDBNull("ParentDecisionId"))
                    {
                        decision.ParentDecisionId = sqlreader.GetInt32("ParentDecisionId");
                    }

                    decision.Depth = sqlreader.GetInt32("Depth");

                    var parent = decision.ParentDecisionId == null 
                        ? root
                        : root.Get(decision.ParentDecisionId.Value);

                    if (decision.ParentDecisionId == null)
                        root = decision;
                    else
                        parent.Add(decision);
                }
            }

            return root;
        }
    }
}
