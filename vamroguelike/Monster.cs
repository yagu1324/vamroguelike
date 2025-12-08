using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace vamroguelike
{

    public class Monster : Mob
    {
        public int type { get; set; } // 몬스터 타입, 0= 좀비
        public int size_x { get; set; } = 30; //크기는 50
        public int size_y { get; set; } = 50; //크기는 50

        public char see { get; set; } //바라보는곳  w a s d

        public double move_smooth_count { get; set; } = 0; //크기는 0 커질 떄 마다 발이 바뀜
        public int exp_type { get; set; } = 0; // 경험치 타입

        public static Monster operator -(Monster monster, User user)//연산자 중복, 몬스터가 공격을 맞았을 경우
        {
            double total_attack = (user.weapons.damage + user.damage); //계산공식, 유저공격력 + 무기 공격력

            double real_damage = Math.Max(total_attack * (1/(1+monster.shield*0.01)), 0); //리그오브레전드의 계산식

            // 체력 감소
            monster.hp -= real_damage; 

            return monster;//반환
        }
        public Monster(int i)
        {
            type = i; // 몬스터 타입 정하기
            speed = 30; // 기본 속도 30으로 초기화
            hp_max = 5; // 최대 체력 5
            hp = hp_max; // 체력회복
            damage = 0.5;// 초반 공격력 0.5
        }
    }
}
