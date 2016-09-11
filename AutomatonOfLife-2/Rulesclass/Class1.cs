using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rulesclass
{
    public enum State
    {
        idle=0,
        alive=1,
        dead=2

    };

   public class Rule
    {

        public int neighborcount
        {
            get; set;
        }
        public int neighborstate
        {
            get; set;
        }
        public int finalstate { get; set; }
        public string moreless;
        public int type { get; set; }
        public Rule(int neighborcount, int neighborstate, int finalstate, string moreless, int type)
        {
            this.neighborcount = neighborcount;
            this.neighborstate = neighborstate;
            this.finalstate = finalstate;
            this.moreless = moreless;
            this.type = type;
        }
    }

    public class Cell
    {
        public State state
        { get; set; }
        public int positionx { get; set; }
        public int positiony { get; set; }
        public string color { get; set; }
    }
}
