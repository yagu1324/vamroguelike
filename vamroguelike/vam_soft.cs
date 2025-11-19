using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

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
        Random rand = new Random();

        //맵 사이즈
        public int mapsize_x { get; set; } = 3000;
        public int mapsize_y { get; set; } = 3000;

        // 폼
        public int formsize_x { get; set; } = 1500;
        public int formsize_y { get; set; } = 900;

        public List<Monster> monsters { get; private set; } // 소환된 몬스터를 담을 곳 // 소환된 몬스터를 담을 곳



        //캐릭터가 움직일 때 생기는 애니메이션을 위해 아주 중요한 필드
        public double player_move_anime_w { get; set; } = 0;
        public double player_move_anime_a { get; set; } = 0;
        public double player_move_anime_s { get; set; } = 0;
        public double player_move_anime_d { get; set; } = 0;

        double move_smooth = 7.0;// 움직임을 자연스럽게 보이기 위한 값 / 값이 높을 수록 느리게 애니메이션이 바뀐다
        public vam_soft() {
            //초기화
            my = new User();
            monsters = new List<Monster>(); 

            my.x = mapsize_x/2;my.y = mapsize_y/2; // 아바타의 위치는 항상 맵의 정중앙에서 시작한다


            play_form_soft();//실제 게임 소프트
            
        }

        public void key_check()
        {
            
            //이동 속도는 speed/fps = 1초당 speed만큼 움직인다는 뜻
            if (key[0]) //w를 눌렀을 때
            {
                if((my.y - (my.speed / fps)) > 0) // 움직인곳이 맵 크기 보다 작아야함
                {
                    my.y -= my.speed/fps; //움직임 
                    
                }
                if (player_move_anime_w != 0)
                {
                    player_move_anime_w += my.speed / (fps* move_smooth); //애니메이션 카운터
                }
                

            }
            if (key[1])//a를 눌렀을 때
            {
                if ((my.x - (my.speed / fps)) > 0) //움직인곳이 맵 크기 보다 작아야함
                {
                    my.x -= my.speed / fps;//이동 속도는 speed/fps = 1초당 speed만큼 움직인다는 뜻
                }
                if (player_move_anime_a != 0)
                {
                    player_move_anime_a += my.speed / (fps * move_smooth); //애니메이션 카운터
                }
            }
            if (key[2])//s를 눌렀을 때
            {
                if ((my.y + (my.speed / fps)) <= mapsize_y) // 움직인곳이 맵 크기 보다 작아야함
                {
                    my.y += my.speed / fps; //움직임
                }

                if (player_move_anime_s != 0)
                {
                    player_move_anime_s += my.speed / (fps * move_smooth); //애니메이션 카운터
                }
            }
            if (key[3])//d를 눌렀을 때
            {
                if ((my.x + (my.speed / fps)) <= mapsize_x) // 움직인곳이 맵 크기 보다 작아야함
                {
                    my.x += my.speed / fps;
                }
                if (player_move_anime_d != 0)
                {
                    player_move_anime_d += my.speed / (fps * move_smooth); //애니메이션 카운터
                }

            }
        }

        void spawn_monster()// 몬스터를 자동스폰하는것, 화면 밖에서 나와야한다
        {
            
            int spawn_min_dis = 100; // 스폰되는 최대 거리 범위
            Monster m = new Monster(0); // 일단 기본은 좀비로 설정
            int see=rand.Next(4); //스폰된 방향 설정
            if (see == 0)//w
            {
                m.x=rand.Next(-formsize_x/2+(int)my.x- spawn_min_dis, (int)my.x+formsize_x/2+ spawn_min_dis);//x는 자유
                m.y = rand.Next(formsize_y / 2 + (int)my.y, (int)my.y + formsize_y / 2 + spawn_min_dis);//y는 화면밖에서의 자유
            }
            else if(see == 1)//a
            {
                m.x = rand.Next(-spawn_min_dis - formsize_x / 2 + (int)my.x, -formsize_x / 2 + (int)my.x); //화면 왼쪽 100 밖
                m.y= rand.Next(-formsize_y / 2 + (int)my.y- spawn_min_dis, (int)my.y + formsize_y / 2+ spawn_min_dis);//y는 화면밖에서의 자유
            }
            else if(see == 2)//s
            {
                m.x = rand.Next(-formsize_x / 2 + (int)my.x - spawn_min_dis, (int)my.x + formsize_x / 2 + spawn_min_dis);//x는 자유
                m.y = rand.Next(-spawn_min_dis - formsize_y / 2 + (int)my.y, -formsize_y / 2 + (int)my.y);//y는 화면 아래쪽 100 밖에서
            }
            else//d
            {
                m.x = rand.Next(formsize_x / 2 + (int)my.x, formsize_x / 2 + spawn_min_dis + (int)my.x); //화면 왼쪽 100 밖
                m.y= rand.Next(-formsize_y / 2 + (int)my.y-spawn_min_dis, formsize_y / 2 + spawn_min_dis + (int)my.y);//y는 화면 위쪽 100 밖에서
            }

            monsters.Add(m);
        }
        
        void monster_move() // 몬스터 움직임
        {
            double r = 360 / 16.0;
            for (int i = 0; i < monsters.Count; i++) //몬스터 수만큼 반복
            {
                double dx=my.x - monsters[i].x,dy=my.y-monsters[i].y; // 거리 차이 구하기
                double angle=Math.Atan2(dy,dx)*(180.0/Math.PI);//내 캐릭터와 이 좀비캐릭터의 최소 거리 각도를 알려준다

                
                if (angle < 0)//음수이면 360을 더한다 -> 이러면 0~360의 값이 된다 / 단!!!! 0은 위가 아니라 오른쪽값이다
                {
                    angle += 360;
                }



                if (angle >= 348.75 || angle < 11.25) 
                {
                    // 0. 완전 오른쪽 (Right) 3시

                    monsters[i].see = 'd';//  보는 방향을 바꾸고
                    monsters[i].x += monsters[i].speed / fps; // 1초당 움직일 수 있는 속도에 맞추어 속도를 늘린다
                }
                else if (angle < 33.75)
                {
                    // 1. 오른쪽에서 살짝 아래 (Right - Down) //3.5
                    monsters[i].see = 'd';//  보는 방향 아래

                    //총 움직일 수 있는 거리를 3칸이라 가정, monster speed를 3으로 나누고, x,y 를 조금씩 나눠서 증가
                    monsters[i].x += monsters[i].speed*(2.0/3.0) / fps;//2
                    monsters[i].y += monsters[i].speed * (1.0 / 3.0) / fps;//1

                }

                else if (angle < 56.25)// 오른쪽 밑 대각
                {
                    // 2. 오른쪽 아래 대각선 정중앙 (South-East Diagonal)
                    monsters[i].see = 'd';//  보는 방향 아래
                    monsters[i].x += monsters[i].speed * (2.0 / 3.0) / fps; //2.0
                    monsters[i].y += monsters[i].speed * (2.0 / 3.0) / fps; //2.0
                }

                else if (angle < 78.75)
                {
                    // 3. 아래쪽에서 살짝 오른쪽 (Down - Right)
                    monsters[i].see = 's';//보는 방향 밑
                    monsters[i].x += monsters[i].speed * (1.0 / 3.0) / fps; //1
                    monsters[i].y += monsters[i].speed * (2.0 / 3.0) / fps; //2

                }
                else if (angle < 101.25)
                {
                    // 4. 완전 아래쪽 (Down)
                    monsters[i].see = 's';//보는 방향 밑
                    monsters[i].y += monsters[i].speed * (3.0 / 3.0) / fps; //3
                }
                else if (angle < 123.75)
                {
                    // 5. 아래쪽에서 살짝 왼쪽 (Down - Left)
                    monsters[i].see = 's';//보는 방향 밑
                    monsters[i].x -= monsters[i].speed * (1.0 / 3.0) / fps; //1
                    monsters[i].y += monsters[i].speed * (2.0 / 3.0) / fps; //2
                }

                else if (angle < 146.25)
                {
                    // 6. 왼쪽 아래 대각선 정중앙 (South-West Diagonal)
                    monsters[i].see = 'a';//보는 방향 왼
                    monsters[i].x -= monsters[i].speed * (2.0 / 3.0) / fps; //2.0
                    monsters[i].y += monsters[i].speed * (2.0 / 3.0) / fps; //2.0
                }

                else if (angle < 168.75)
                {
                    // 7. 왼쪽에서 살짝 아래 (Left - Down)
                    monsters[i].see = 'a';//보는 방향 왼
                    monsters[i].x -= monsters[i].speed * (2.0 / 3.0) / fps; //1
                    monsters[i].y += monsters[i].speed * (1.0 / 3.0) / fps; //2

                }
                else if (angle < 191.25)
                {
                    // 8. 완전 왼쪽 (Left)
                    monsters[i].see = 'a';//보는 방향 왼
                    monsters[i].x -= monsters[i].speed * (3.0 / 3.0) / fps; //1
                }
                else if (angle < 213.75)
                {
                    // 9. 왼쪽에서 살짝 위 (Left - Up)
                    monsters[i].see = 'a';//보는 방향 왼
                    monsters[i].x -= monsters[i].speed * (2.0 / 3.0) / fps; //1
                    monsters[i].y -= monsters[i].speed * (1.0 / 3.0) / fps; //2
                }

                else if (angle < 236.25)
                {
                    // 10. 왼쪽 위 대각선 정중앙 (North-West Diagonal)
                    monsters[i].see = 'a';//보는 방향 왼
                    monsters[i].x -= monsters[i].speed * (2.0 / 3.0) / fps; //2.0
                    monsters[i].y -= monsters[i].speed * (2.0 / 3.0) / fps; //2.0
                }

                else if (angle < 258.75)
                {
                    // 11. 위쪽에서 살짝 왼쪽 (Up - Left)
                    monsters[i].see = 'w';//보는 방향 위
                    monsters[i].x -= monsters[i].speed * (1.0 / 3.0) / fps; //-1
                    monsters[i].y -= monsters[i].speed * (2.0 / 3.0) / fps; //-2
                }
                else if (angle < 281.25)
                {
                    // 12. 완전 위쪽 (Up)
                    monsters[i].see = 'w';//보는 방향 위
                    monsters[i].y -= monsters[i].speed * (3.0 / 3.0) / fps; //-3
                }
                else if (angle < 303.75)
                {
                    // 13. 위쪽에서 살짝 오른쪽 (Up - Right)
                    monsters[i].see = 'w';//보는 방향 위
                    monsters[i].x += monsters[i].speed * (1.0 / 3.0) / fps; //+1
                    monsters[i].y -= monsters[i].speed * (2.0 / 3.0) / fps; //-2
                }
                else if (angle < 326.25)
                {
                    // 14. 오른쪽 위 대각선 정중앙 (North-East Diagonal)
                    monsters[i].see = 'd';//보는 방향 오른
                    monsters[i].x += monsters[i].speed * (2.0 / 3.0) / fps; //2.0
                    monsters[i].y -= monsters[i].speed * (2.0 / 3.0) / fps; //-2.0
                }
                else
                {
                    // 15. 오른쪽에서 살짝 위 (Right - Up)
                    monsters[i].see = 'd';//보는 방향 오른
                    monsters[i].x += monsters[i].speed * (2.0 / 3.0) / fps; //+2
                    monsters[i].y -= monsters[i].speed * (1.0 / 3.0) / fps; //-1
                }

                monsters[i].move_smooth_count += monsters[i].speed / (fps * move_smooth);// 움직임 발을 바꾸기 위한 것

            }


        }
        
        public void play_form_soft()
        {
           spawn_monster();
            monster_move();
           key_check();
        }
        
    }
}
