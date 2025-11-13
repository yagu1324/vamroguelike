using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vamroguelike
{
    class vam_soft
    {
        public bool[] key = new bool[8]; // 키보드 입력 0=w 1=a  2=s 3=d   4=위 5=왼 6=아래 7=오른
        int f = 2; // 1=메인화면, 2=플레이 화면
        play_game play;
        public User my;
        public int fps = 100; // 1초를 몇번 나눌것인가?
        bool game_end = false;
        public int mapsize_x { get; set; } = 3000;
        public int mapsize_y { get; set; } = 3000;



        //캐릭터가 움직일 때 생기는 애니메이션을 위해 아주 중요한 필드
        public double player_move_anime_w { get; set; } = 0;
        public double player_move_anime_a { get; set; } = 0;
        public double player_move_anime_s { get; set; } = 0;
        public double player_move_anime_d { get; set; } = 0;

        public vam_soft() {
            my = new User();
            play_form_soft();
            my.x = mapsize_x/2;my.y = mapsize_y/2; // 아바타의 위치는 항상 맵의 정중앙에서 시작한다
        }

        public void key_check()
        {
            if (key[0]) //w를 눌렀을 때
            {
                if((my.y - (my.speed / fps)) > 0) // 맵 크기 보다 작아야함
                {
                    my.y -= my.speed/fps; //움직임
                    
                }
                if (player_move_anime_w != 0)
                {
                    player_move_anime_w += my.speed / (fps*5.0); //애니메이션 카운터
                }
                

            }
            if (key[1])//a를 눌렀을 때
            {
                if ((my.x - (my.speed / fps)) > 0) // 맵 크기 보다 작아야함
                {
                    my.x -= my.speed / fps;
                }
                if (player_move_anime_a != 0)
                {
                    player_move_anime_a += my.speed / (fps * 5.0); //애니메이션 카운터
                }
            }
            if (key[2])//s를 눌렀을 때
            {
                if ((my.y + (my.speed / fps)) <= mapsize_y) // 맵 크기 보다 작아야함
                {
                    my.y += my.speed / fps; //움직임
                }
                if (player_move_anime_s != 0)
                {
                    player_move_anime_s += my.speed / (fps * 5.0); //애니메이션 카운터
                }
            }
            if (key[3])//d를 눌렀을 때
            {
                if ((my.x + (my.speed / fps)) <= mapsize_x) // 맵 크기 보다 작아야함
                {
                    my.x += my.speed / fps;
                }
                if (player_move_anime_d != 0)
                {
                    player_move_anime_d += my.speed / (fps * 5.0); //애니메이션 카운터
                }

            }
        }
        
        public void play_form_soft()
        {
           key_check();
        }
        
    }
}
