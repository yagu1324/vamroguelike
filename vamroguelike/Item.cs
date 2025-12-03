using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vamroguelike
{
    public class Item
    {
        //소환될 x,y좌표
        public double x { get; set; } 
        public double y { get; set; }
        public double size { get; set; } = 10; //아이템 크기
        public int type { get; set; }//아이템 타입/ 0=회복, 1=자석, 2=폭탄, 3=경험치

        public float speed { get; set; } = 400;
    }
}
