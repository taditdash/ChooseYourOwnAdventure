using System.Collections.Generic;

namespace ChooseYourOwnAdventure.Models
{
    public class Decision
    {
        public Decision()
        {
            ChildDecisions = new List<Decision>();
        }

        public int DecisionId { get; set; }
        public string Description { get; set; }
        public int? ParentDecisionId { get; set; }
        public int Depth { get; set; }

        public Decision ParentDecision { get; set; }
        public List<Decision> ChildDecisions { get; set; }

        public void Add(Decision child)
        {
            child.ParentDecision = this;
            this.ChildDecisions.Add(child);
        }

        public Decision Get(int id)
        {
            Decision result;

            if (this.DecisionId == id) return this;

            foreach (Decision child in this.ChildDecisions)
            {
                if (child.DecisionId == id)
                {
                    return child;
                }
                result = child.Get(id);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    }
}
