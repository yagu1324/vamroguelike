using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vamroguelike
{
    public abstract class Mob //추상클래스
    {
        //아바타의 x,y위치
        public double x { get; set; } 
        public double y { get; set; }


        public double hp { get; set; } // 현재 체력
        public double hp_max { get; set; }//최대 체력
        public double speed { get; set; }//움직일 속도
        public double damage { get; set; }//공격력

        public double shield { get; set; } = 0;//방어력

    }
}
