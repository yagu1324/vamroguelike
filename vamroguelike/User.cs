using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vamroguelike
{
    class User : Mob
    {
        public Weapons w { get; set; } // 무기

        public double damage_delay { get; set; }//공속

        public int Lv { get; set; } = 1; //레벨

        public double exp { get; set; } = 0; // 현재 경험치
        public double exp_max { get; set; } = 100;
        public int size { get; set; } = 50;

        public User()
        {
            speed = 50; // 기본 속도 10으로 초기화
            hp_max = 10; // 최대 체력 10
            hp = hp_max; // 체력 
            damage = 1;// 초반 공격력 1
        }
    }
}
