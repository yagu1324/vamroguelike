using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace vamroguelike
{
    class vam_soft
    {
        public double timer { get; set; } // 게임타이머
        public bool game_stop { get; set; } //게임멈출 함수
        public bool[] key = new bool[9]; // 키보드 입력 0=w 1=a  2=s 3=d   4=위 5=왼 6=아래 7=오른 8=p (pause) 능력치 보는거
        int f = 2; // 1=메인화면, 2=플레이 화면
        play_game play;
        public User my;
        public int fps = 100; // 1초를 몇번 나눌것인가?
        bool game_end = false;
        Random rand = new Random();

        double invisible_time = 0.1; //무적시간
        double invisible_count = 0; //무적 카운트

        //맵 사이즈
        public int mapsize_x { get; set; } = 3000;
        public int mapsize_y { get; set; } = 3000;

        // 폼
        public int formsize_x { get; set; } = 1500;
        public int formsize_y { get; set; } = 900;

        public List<Monster> monsters { get; set; } // 소환된 몬스터를 담을 곳 // 소환된 몬스터를 담을 곳
        public List<Weapons> Attack { get; set; }//공격시 여기에 생김
        public List<Item> item { get; set; }//드랍 아이템
        public List<Item> eat { get; set; }//드랍 아이템 먹은 리스트


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
            Attack = new List<Weapons>();
            item = new List<Item>();
            eat = new List<Item>();
            timer = 0;

            my.x = mapsize_x/2;my.y = mapsize_y/2; // 아바타의 위치는 항상 맵의 정중앙에서 시작한다



            play_form_soft();//실제 게임 소프트
            
        }

        public void key_check()
        {
            my.damage_delay_count += my.damage_delay / fps;//초당 이만큼 때린다는뜻
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
            if (1 <= my.damage_delay_count) // 공속제한
            {
                // [1] 위 (Up) 공격
                if (key[4])
                {
                    if (my.weapons.type == 0) // 무기의 타입이 slash일 경우
                    {
                        Weapons atk_weapon = (Weapons)my.weapons.copy(); // 새 객체 생성 및 복사

                        // X: 아바타 중심-무기사이즈, Y: 아바타 위쪽 끝
                        atk_weapon.x = my.x + my.size / 2 - atk_weapon.size / 2;
                        atk_weapon.y = my.y - atk_weapon.size;
                        atk_weapon.see = 'w';

                        Attack.Add(atk_weapon);
                        my.damage_delay_count = 0; // 공속 카운트 초기화
                    }
                }
                // [2] 왼쪽 (Left) 공격
                else if (key[5])
                {
                    if (my.weapons.type == 0)
                    {
                        Weapons atk_weapon = (Weapons)my.weapons.copy(); //아바타 무기 정보 복사

                        // X: 아바타 왼쪽 끝, Y: 아바타 중심-무기사이즈
                        atk_weapon.x = my.x - atk_weapon.size;
                        atk_weapon.y = my.y + my.size / 2 - atk_weapon.size / 2;
                        atk_weapon.see = 'a';

                        Attack.Add(atk_weapon);
                        my.damage_delay_count = 0; // 공속 카운트 초기화
                    }
                }
                // [3] 아래 (Down) 공격
                else if (key[6])
                {
                    if (my.weapons.type == 0)
                    {
                        Weapons atk_weapon = (Weapons)my.weapons.copy();

                        // X: 아바타 중심-무기사이즈, Y: 아바타 아래쪽 끝
                        atk_weapon.x = my.x + my.size / 2 - atk_weapon.size / 2;
                        atk_weapon.y = my.y + my.size;
                        atk_weapon.see = 's';

                        Attack.Add(atk_weapon);
                        my.damage_delay_count = 0; // 공속 카운트 초기화
                    }
                }
                // [4] 오른쪽 (Right) 공격
                else if (key[7])
                {
                    if (my.weapons.type == 0)
                    {
                        Weapons atk_weapon = (Weapons)my.weapons.copy();

                        // X: 아바타 오른쪽 끝, Y: 아바타 중심=무기사이즈
                        atk_weapon.x = my.x + my.size;
                        atk_weapon.y = my.y + my.size / 2 - atk_weapon.size / 2;
                        atk_weapon.see = 'd';

                        Attack.Add(atk_weapon);
                        my.damage_delay_count = 0; // 공속 카운트 초기화
                    }
                }
                else if (key[8]) // 일시 정지 키를 눌렀을 떄
                {
                    //일시정지 키를 눌렀을 때
                    game_stop = true; //토글 형식으로 일시정지 설정
                    key[8] = false; //키 초기화

                    see_ability sa = new see_ability(); //능력치 창 열기
                    //능력치 창에 유저 능력치 전달
                    sa.attack_damage = my.damage;
                    sa.attack_speed = my.damage_delay;
                    sa.move_speed = my.speed;
                    sa.max_hp = my.hp_max;
                    sa.exp_plus = my.exp_plus;
                    sa.weapon_size = my.weapons.size;
                    sa.eat_size = my.eat_size;
                    sa.weapon_damage = my.weapons.damage;
                    sa.shield = my.shield;




                    sa.ShowDialog();
                    game_stop = false; //능력치 창 닫히면 다시 게임 재개

                }
            }


        }

        void spawn_monster(Monster m)// 몬스터를 자동스폰하는것, 화면 밖에서 나와야한다
        {
            int spawn_min_dis = 100; // 스폰되는 최대 거리 범위
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
            for (int i = 0; i < monsters.Count; i++) // 몬스터 수만큼 반복
            {
                // 1. 거리 차이 구하기 (밑변 dx, 높이 dy)
                double dx = my.x - monsters[i].x;
                double dy = my.y - monsters[i].y;

                // 2. 대각선 거리 구하기 (피타고라스 정의: 빗변)
                double dist = Math.Sqrt(dx * dx + dy * dy);

                // 거리가 0이면 움직이지 않음 (0으로 나누기 방지)
                if (dist == 0) continue;

                // 3. 이동할 거리 계산 (프레임당 이동 거리)
                double moveStep = monsters[i].speed / fps;

                // 4. 좌표 이동
                // (dx / dist)는 코사인(Cos) 값과 같고, (dy / dist)는 사인(Sin) 값과 같습니다.
                // 즉, 비율대로 x, y를 쪼개서 더해줍니다.
                monsters[i].x += (dx / dist) * moveStep;
                monsters[i].y += (dy / dist) * moveStep;

                // 5. 바라보는 방향(Sprite) 결정
                // x축 이동량이 더 크면 좌우, y축 이동량이 더 크면 상하 이미지를 씀
                if (Math.Abs(dx) > Math.Abs(dy))
                {
                    if (dx > 0) monsters[i].see = 'd'; // 오른쪽
                    else monsters[i].see = 'a';        // 왼쪽
                }
                else
                {
                    if (dy > 0) monsters[i].see = 's'; // 아래
                    else monsters[i].see = 'w';        // 위
                }

                // 6. 애니메이션 카운트 (기존 유지)
                monsters[i].move_smooth_count += monsters[i].speed / (fps * move_smooth);
            }
        }
        
        void drop_item(Monster m) //몬스터 드랍아이템, 경험치도 같이 떨군다
        {
            
            //경험치
            Item exp = new Item();//경험치 객체 생성
            exp.x = m.x+m.size_x/2/2; exp.y=m.y+m.size_y/2;// 좌표값 지정
            
            exp.type = 3;//경험치 생성
            item.Add(exp); // 리스트 값에 저장

            //드랍아이템
            Item i = new Item(); // item 객체 생성
            i.x = m.x + m.size_x / 2 / 2; i.y= m.y + m.size_y / 2;// item 좌표 지정
            if (rand.Next(500) == 0) //   1/500확률
            {
                i.size = 15;//특별한 아이템은 크기가 15
                i.type=rand.Next(3); // 0~2까지 랜덤하게뜸
                item.Add(i);// 아이템 저장
            }
            
        }
        
        void avatar_crash_check() //몬스터와 유저가 부딪히면 체력을 깐다
        {
            if(invisible_count < invisible_time) //무적시간이면 패스
            {
                invisible_count += 1.0 / fps; //무적 카운트 증가
                return;
            }
            else
            {
                for (int i = 0; i < monsters.Count; i++)
                {

                    //몬스터와 아바타
                    RectangleF monsterRect = new RectangleF((float)monsters[i].x, (float)monsters[i].y, monsters[i].size_x, monsters[i].size_y);
                    RectangleF avatarRect = new RectangleF((float)my.x, (float)my.y, (float)my.size, (float)my.size);
                    if (avatarRect.IntersectsWith(monsterRect))//닿았을 경우
                    {
                        double real_damage = Math.Max(monsters[i].damage - my.shield, 0); //방어력에 따른 데미지 감소
                        my.hp -= real_damage; //데미지 깎임
                    }
                }
                invisible_count = 0; //무적 카운트 초기화
            }
                
        }

        void monster_crash_check()// 몬스터 크래쉬 체크
        {
            for (int j = 0; j < Attack.Count; j++)//때린 무기 갯수만큼 반복
            {
                for (int i = monsters.Count-1; i >=0; i--) //몬스터 숫자만큼 반복
                {
                    //몹과 몬스터를 정사각형으로 지정
                    RectangleF attackRect = new RectangleF((float)Attack[j].x, (float)Attack[j].y, (float)Attack[j].size, (float)Attack[j].size);
                    RectangleF monsterRect = new RectangleF((float)monsters[i].x, (float)monsters[i].y, monsters[i].size_x, monsters[i].size_y);

                    if (monsterRect.IntersectsWith(attackRect))//몬스터와, 공격의 사각형이 서로 닿았을 경우
                    {
                        monsters[i].hp -= Attack[j].damage + my.damage;//데미지 계산은 무기공격력 + 유저 공격력이다
                    }
                    if (monsters[i].hp <= 0)//체력이 0 이하로 떨어졌을 경우 삭제한다
                    {
                        drop_item(monsters[i]);//몬스터 드랍아이템
                        monsters.RemoveAt(i); //삭제
                    }
                }
            }
        }
        void item_eat() // 아이템 먹기 : 아이템을 먹어야할 상태의 x,y를 조절, + 아이템이 먹어졌는지 확인하는 crash도 함꼐 하겠습니다
        {
            //crash 체크 (여기에 들어가면 이제 먹어지는 아이템이 되는겁니다)
            for(int i = 0; i < item.Count; i++)//item 리스트 만큼
            {
                //이제 my.x my.y에서 eat_size만큼 넓힌다음에 닿았는지 확인
                RectangleF avatarRect = new RectangleF((float)my.x-(float)my.eat_size, (float)my.y - (float)my.eat_size, my.size+ (float)my.eat_size*2, my.size+ (float)my.eat_size*2);
                RectangleF itemRect = new RectangleF((float)item[i].x, (float)item[i].y, (float)item[i].size, (float)item[i].size);
                if(avatarRect.IntersectsWith(itemRect))//아바타와 아이템이 닿았을 경우
                {
                    eat.Add(item[i]);//먹은 리스트에 추가
                    item.RemoveAt(i);//원래 아이템 리스트에서 삭제
                }
            }

            //먹어지는거 움직이는 함수
            for(int i=eat.Count-1; i>=0; i--)
            {
                //거리 구하기
                double dx = my.x+my.size/2 - eat[i].x;
                double dy = my.y+my.size/2 - eat[i].y;
                // [현재 거리 구하기] (대각선방향) 
                double dist = Math.Sqrt(dx * dx + dy * dy);

                // 거리가 매우 가까워지면 삭제 
                if (dist <= eat[i].size) //아이템과 부딪힐 정도
                {
                    //먹은 아이템 효과 발동
                    if (eat[i].type == 0)//회복
                    {
                        //더 작은값으로 가지는데 만약 최대체력보다 많아지면 최대체력으로 고정
                        my.hp = Math.Min(my.hp + 10, my.hp_max); //체력 10회복
                    }
                    else if (eat[i].type == 1)//자석 모든 경험치 아이템들 싹쓸이
                    {
                        for(int j=item.Count - 1; j >= 0; j--)
                        {
                            if (item[j].type>= 3) //경험치 아이템일 경우(타입이 3이상이면 경험치 아이템)
                            {
                                item[j].speed = 500; //속도 증가
                                eat.Add(item[j]);//먹은 리스트에 추가
                                item.RemoveAt(j);//원래 아이템 리스트에서 삭제
                            }
                        }
                    }
                    else if (eat[i].type == 2)//폭탄
                    {
                        //주변 몬스터들 100데미지
                        for (int j = monsters.Count - 1; j >= 0; j--)
                        {
                            double mdx = (my.x + my.size / 2) - (monsters[j].x + monsters[j].size_x / 2);
                            double mdy = (my.y + my.size / 2) - (monsters[j].y + monsters[j].size_y / 2);
                            double mdist = Math.Sqrt(mdx * mdx + mdy * mdy);
                            if (mdist <= Math.Max(formsize_x,formsize_y)) //폼사이즈 크기중 가장 큰값
                            {
                                monsters[j].hp -= 100; //50 데미지
                                if (monsters[j].hp <= 0)//체력이 0 이하로 떨어졌을 경우 삭제한다
                                {
                                    drop_item(monsters[j]);//몬스터 드랍아이템
                                    monsters.RemoveAt(j); //삭제
                                }
                            }
                        }
                    }
                    else if(eat[i].type == 3)//경험치
                    {
                        my.exp += 1*(1+my.exp_plus); //경험치 1획득  (exp_plus의 배율에 따라 경험치를 더 휙득한다)
                    }
                    else if(eat[i].type == 4)//경험치 블루 
                    {
                        my.exp += 5 * (1 + my.exp_plus); //경험치 5획득
                    }
                    else if(item[i].type == 5)//경험치 퍼플
                    {
                        my.exp += 20 * (1 + my.exp_plus); //경험치 20획득
                    }
                    // 여기에 실제 아이템 효과 적용 코드 추가
                    eat.RemoveAt(i);
                    continue; // 다음 반복으로 넘어감
                }


                // 거리 차이(dx) / 전체 거리(dist) = 1만큼의 방향
                //즉 dx/dist = 한번 움직일 떄의 비율을 뜻한다 eat[i].speed/fps = 1틱당 움직이는 진짜 스피드
                eat[i].x += (dx / dist) * eat[i].speed/fps;
                eat[i].y += (dy / dist) * eat[i].speed/fps;
            }
        }

        void level_up()//레벨업 함수
        {
            //경험치가 최대 경험치를 넘었을 때 레벨업
            if (my.exp >= my.exp_max)
            {
                game_stop=true; // 게임을 멈춤!
                my.exp -= my.exp_max; //남은 경험치는 다음 레벨로 넘긴다
                my.Lv += 1; //레벨업
                my.exp_max = (int)(my.exp_max * 1.2); //다음 레벨 최대 경험치 증가

                //랜덤 능력치 고름!
                Select_ability abilityForm = new Select_ability();
                //0.공격력 1.최대체력 2.공격속도 3.방어력 4.무기크기 5.이동속도 6.경험치획득량
                int select_ability=-1; //선택한 능력 번호
                double value=0; // 적용할 능력 값

                if (abilityForm.ShowDialog() == DialogResult.OK) //제대로 클릭했으면 ok / 만약 그냥 창을 끄면 그대로 넘어감(이거 안하면 오류남)
                {
                    // 폼에서 선택한 값 꺼내오기
                    select_ability = abilityForm.select_num;
                    value = abilityForm.add_value;
                }
                abilityForm.Dispose(); // 윈폼 할당 제거

                if (select_ability == 0)
                {//공격력
                    my.damage += value;
                }
                else if (select_ability == 1) //최대체력
                {
                    my.hp += value;
                    my.hp_max += value;
                }
                else if(select_ability == 2)//공격속도
                {
                    my.damage_delay += value;
                }
                else if( select_ability == 3)//방어력
                {
                    my.shield += value;
                }
                else if (select_ability == 4)//무기 크기
                {
                    my.weapons.size *= (1 + value);// 무기 크기 배율로 증가
                }
                else if (select_ability == 5)//이동속도
                {
                    my.speed+= value;
                }
                else if (select_ability == 6)//경험치 획득량
                {
                    my.exp_plus += value/100;
                }
                else if (select_ability == 7)//먹는 범위 증가
                {
                    my.eat_size *= (1 + value);
                }

                //이걸해야 윈폼 다른 창이 켜졌을 때 키나 눌려져있는 현상이 사라진다
                for (int i = 0; i < 9; i++)
                {
                    key[i]=false; //키 초기화
                }

                game_stop = false; //게임 멈춤을 끔
            }
        }
        
        void Dif_level() // 난이도 조정
        {
            Monster monster = new Monster(0); // 기본 좀비타입으로
            int dif_level_field = (int)((timer / 60) + 1);
            monster.hp_max = dif_level_field * dif_level_field;
            monster.hp = dif_level_field * dif_level_field;
            monster.damage= dif_level_field * dif_level_field;
            monster.shield= dif_level_field * dif_level_field;
            spawn_monster(monster);
        }
        
        public void play_form_soft()//실제 실행될 게임 메소드
        {
           
           monster_move();
           Dif_level();
           monster_crash_check();
           avatar_crash_check();
           item_eat();
           level_up();
           key_check();
           timer += 1.5 / fps;// 게임 시간 저장 1.0으로 하니까 느림
        }
        
    }
}
