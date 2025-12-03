using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vamroguelike
{
    public class Weapons
    {
        //생성될 좌표
        public double x { get; set; }
        public double y { get; set; }
        public double damage { get; set; } = 1;//무기 공격력
        public int type { get; set; } // 무기 타입 0:slash
        public double size { get; set; } = 100; //무기 크기
        public int sprite_count { get; set; } = 0; // 이미지 스프라이트 카운트(0~8)
        public char see { get; set; }//무기를 쓰는 방향
        public double anime_dur { get; set; } = 0.25; //애니메이션 유지시간
        public double anime_dur_count { get; set; } = 0; //애니메이션 유지시간 카운트
        public object copy() // 값 복사하기
        {
            return this.MemberwiseClone();
        }
    }
}
