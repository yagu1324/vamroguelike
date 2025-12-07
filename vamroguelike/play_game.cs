using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
namespace vamroguelike
{
    public partial class play_game: Form
    {
        vam_soft vsf;
        Timer game_timer; // 게임 타이머 (반복주기용)
        
        
        float viewx, viewy;//화면 뷰포인트 위치

        System.Drawing.Image mapimage; // 맵 이미지

        //위로보기
        System.Drawing.Image player_front_Image;            // 캐릭터 정면(기본) 이미지
        System.Drawing.Image player_front_move_right_Image; // 캐릭터 정면, 오른발(움직일 때) 이미지
        System.Drawing.Image player_front_move_left_Image;  // 캐릭터 정면, 왼발(움직일 때) 이미지

        // 후면 (뒤) 보기
        System.Drawing.Image player_back_Image;             // 캐릭터 후면(기본) 이미지
        System.Drawing.Image player_back_move_right_Image;  // 캐릭터 후면, 오른발(움직일 때) 이미지
        System.Drawing.Image player_back_move_left_Image;   // 캐릭터 후면, 왼발(움직일 때) 이미지

        // 왼쪽 보기
        System.Drawing.Image player_left_Image;             // 캐릭터 왼쪽(기본) 이미지
        System.Drawing.Image player_left_move_right_Image;  // 캐릭터 왼쪽, 오른발(움직일 때) 이미지
        System.Drawing.Image player_left_move_left_Image;   // 캐릭터 왼쪽, 왼발(움직일 때) 이미지

        // 오른쪽 보기
        System.Drawing.Image player_right_Image;            // 캐릭터 오른쪽(기본) 이미지
        System.Drawing.Image player_right_move_right_Image; // 캐릭터 오른쪽, 오른발(움직일 때) 이미지
        System.Drawing.Image player_right_move_left_Image;  // 캐릭터 오른쪽, 왼발(움직일 때) 이미지


        //몬스터 그림
        //왼쪽
        System.Drawing.Image zombie_left_left;
        System.Drawing.Image zombie_left_right;
        //밑쪽
        System.Drawing.Image zombie_bottom_left;
        System.Drawing.Image zombie_bottom_right;
        //오른쪽
        System.Drawing.Image zombie_right_left;
        System.Drawing.Image zombie_right_right;
        //위쪽
        System.Drawing.Image zombie_top_left;
        System.Drawing.Image zombie_top_right;

        //공격 slash
        System.Drawing.Image[] attack_left_slash=new System.Drawing.Image[9];
        System.Drawing.Image[] attack_right_slash = new System.Drawing.Image[9];
        System.Drawing.Image[] attack_top_slash = new System.Drawing.Image[9];
        System.Drawing.Image[] attack_bottom_slash = new System.Drawing.Image[9];

        //드랍 아이템
        //경험치
        System.Drawing.Image green_gem;
        System.Drawing.Image blue_gem;
        System.Drawing.Image purple_gem;
        System.Drawing.Image magnet;
        System.Drawing.Image heal;
        System.Drawing.Image bomb;


        List<int> atk_delete_num;//Attack_list를 삭제하기위해 index 값을 저장할 list
        double anime_tick; //애니메이션 스프라이트를 자연스럽게 하기위한 tick 필드

        public play_game(bool Continue)
        {
            InitializeComponent();

            // ★★★ 더블 버퍼링 활성화 ★★★ //그리는 과정을 없애고 결과만 보여줌
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();



            mapimage = System.Drawing.Image.FromFile(@"image/grass.png"); // 맵이미지 초기화
            //아바타 이미지 초기화@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            player_front_Image = System.Drawing.Image.FromFile(@"image/human_front.png");// 아바타 이미지 초기화
            player_back_Image = System.Drawing.Image.FromFile(@"image/human_back.png");// 아바타 이미지 초기화
            player_left_Image = System.Drawing.Image.FromFile(@"image/human_left.png");// 아바타 이미지 초기화
            player_right_Image = System.Drawing.Image.FromFile(@"image/human_right.png");// 아바타 이미지 초기화
             // 정면 이동 애니메이션 로드                                                                            
            player_front_move_right_Image = System.Drawing.Image.FromFile(@"image/human_front_move_right.png");
            player_front_move_left_Image = System.Drawing.Image.FromFile(@"image/human_front_move_left.png");

            //  후면 이동 애니메이션 로드
            player_back_move_right_Image = System.Drawing.Image.FromFile(@"image/human_back_move_right.png");
            player_back_move_left_Image = System.Drawing.Image.FromFile(@"image/human_back_move_left.png");

            //  왼쪽 이동 애니메이션 로드
            player_left_move_right_Image = System.Drawing.Image.FromFile(@"image/human_left_move_right.png");
            player_left_move_left_Image = System.Drawing.Image.FromFile(@"image/human_left_move_left.png");

            // 오른쪽 이동 애니메이션 로드
            player_right_move_right_Image = System.Drawing.Image.FromFile(@"image/human_right_move_right.png");
            player_right_move_left_Image = System.Drawing.Image.FromFile(@"image/human_right_move_left.png");

            //좀비 이미지 초기화@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            // 좀비 정면 (아래) 애니메이션 로드
            zombie_bottom_left = System.Drawing.Image.FromFile(@"image/zombie_bottom_left.png");
            zombie_bottom_right = System.Drawing.Image.FromFile(@"image/zombie_bottom_right.png");

            // 좀비 후면 (위) 애니메이션 로드
            zombie_top_left = System.Drawing.Image.FromFile(@"image/zombie_top_left.png");
            zombie_top_right = System.Drawing.Image.FromFile(@"image/zombie_top_right.png");

            // 좀비 왼쪽 애니메이션 로드
            zombie_left_left = System.Drawing.Image.FromFile(@"image/zombie_left_left.png");
            zombie_left_right = System.Drawing.Image.FromFile(@"image/zombie_left_right.png");

            // 좀비 오른쪽 애니메이션 로드
            zombie_right_left = System.Drawing.Image.FromFile(@"image/zombie_right_left.png");
            zombie_right_right = System.Drawing.Image.FromFile(@"image/zombie_right_right.png");

            //slash 이미지 초기화
            for(int i = 0; i < 9; i++)
            {
                string filePath = $@"image/attack_bottom_{i+1}.png";
                System.Drawing.Image frame = System.Drawing.Image.FromFile(filePath);
                attack_bottom_slash[i] = frame;
            }
            for (int i = 0; i < 9; i++)
            {
                string filePath = $@"image/attack_left_{i + 1}.png";
                System.Drawing.Image frame = System.Drawing.Image.FromFile(filePath);
                attack_left_slash[i] = frame;
            }
            for (int i = 0; i < 9; i++)
            {
                string filePath = $@"image/attack_right_{i + 1}.png";
                System.Drawing.Image frame = System.Drawing.Image.FromFile(filePath);
                attack_right_slash[i] = frame;
            }
            for (int i = 0; i < 9; i++)
            {
                string filePath = $@"image/attack_top_{i + 1}.png";
                System.Drawing.Image frame = System.Drawing.Image.FromFile(filePath);
                attack_top_slash[i] = frame;
            }

            //드랍 아이템들
            //경험치
            green_gem = System.Drawing.Image.FromFile(@"image/green_gem.png");
            blue_gem = System.Drawing.Image.FromFile(@"image/blue_gem.png");
            purple_gem = System.Drawing.Image.FromFile(@"image/purple_gem.png");
            //드랍 아이템
            magnet = System.Drawing.Image.FromFile(@"image/magnet.png");
            heal = System.Drawing.Image.FromFile(@"image/heal.png");
            bomb = System.Drawing.Image.FromFile(@"image/bomb.png");

            vsf = new vam_soft(); // 게임 내부 소프트
            anime_tick = 1.0/vsf.fps;// anime_tick 애니메이션 스프라이트를 위해 필욯마
            this.ClientSize = new Size(vsf.formsize_x,vsf.formsize_y); //폼 크기 @@@@@
            viewx = (float)vsf.my.x;viewy=(float)vsf.my.y; // 뷰포인트값 아바타 위치로 초기화
            atk_delete_num = new List<int>(); // atk 부분 삭제하기 위해서





            if (Continue == true) // 계속하기면
            {
                vsf.LoadGame(); // 게임 불러오기
                                // 불러온 좌표에 맞춰서 화면 시점(View)도 바로 이동시켜줌
                viewx = vsf.viewx;
                viewy = vsf.viewy;
            }


            // 이 속성이 False이면, 폼 위에 TextBox 같은 컨트롤이 있을 때 폼은 키 입력을 받지 못합니다.
            this.KeyPreview = true;

            // KeyPress 이벤트 핸들러를 연결합니다.
            this.KeyDown += new KeyEventHandler(this.play_game_KeyDown);
            this.KeyUp += new KeyEventHandler(this.play_game_KeyUp);
            




            game_loop();// 실제 게임 루프
        }
        private void play_game_KeyDown(object sender, KeyEventArgs e) // 키를 누를 때
        {
            // 입력된 키에 따라 로직 클래스의 메서드를 호출합니다.
            switch (e.KeyCode)
            {
                // ➡️ 방향키 (Arrows)
                case Keys.Up:
                    vsf.key[4] = true;
                    
                    break;
                case Keys.Down:
                    //키를 누르면 다른 키들은 비활성화( 상하좌우만 공격 가능 하게 하기 위해서)
                    vsf.key[6] = true;
                    
                    break;
                case Keys.Left:
                    vsf.key[5] = true;
                   
                    break;
                case Keys.Right:
                    vsf.key[7] = true;
                   
                    break;

                // 🅰️ WASD 키
                case Keys.W:
                    vsf.key[0] = true;
                    vsf.my.see = 'w'; // 보고있는 방향을 w로
                    
                    if (vsf.player_move_anime_w == 0)//처음 눌렀을 때 1로 바뀌고
                    {
                        vsf.player_move_anime_w = 1;
                    }
                    
                    break;
                case Keys.S:
                    vsf.key[2] = true;
                    vsf.my.see = 's';// 보고있는 방향을 s로
                    if (vsf.player_move_anime_s == 0)//처음 눌렀을 때 1로 바뀌고
                    {
                        vsf.player_move_anime_s = 1;
                    }
                    
                    break;
                case Keys.A:
                    vsf.key[1] = true;
                    vsf.my.see = 'a';// 보고있는 방향을 a로
                    if (vsf.player_move_anime_a == 0)//처음 눌렀을 때 1로 바뀌고
                    {
                        vsf.player_move_anime_a = 1;
                    }
                    
                    break;
                case Keys.D:
                    vsf.key[3] = true;
                    vsf.my.see = 'd';// 보고있는 방향을 d로
                    if (vsf.player_move_anime_d == 0)//처음 눌렀을 때 1로 바뀌고
                    {
                        vsf.player_move_anime_d = 1;
                    }
                   
                    break;
                case Keys.P:
                    vsf.key[8] = true;
                    break;

                default:
                    // 다른 키 입력은 무시
                    return;
            }

            // 키 입력을 처리했으므로, 다른 컨트롤에 전달되는 것을 막습니다.
            e.Handled = true;
        }
        
        private void play_game_KeyUp(object sender, KeyEventArgs e) // 키를 떌때
        {
            // 입력된 키에 따라 로직 클래스의 배열 값을 false로 설정합니다.
            switch (e.KeyCode)
            {
                // ➡️ 방향키 (Arrows)
                case Keys.Up:
                    vsf.key[4] = false;
                    break;
                case Keys.Down:
                    vsf.key[6] = false;
                    break;
                case Keys.Left:
                    vsf.key[5] = false;
                    break;
                case Keys.Right:
                    vsf.key[7] = false;
                    break;

                // 🅰️ WASD 키
                case Keys.W:
                    vsf.key[0] = false;
                    vsf.player_move_anime_w = 0;
                    // W를 뗐을 때, 다른 키가 눌려있는지 확인
                    if (vsf.key[3]) vsf.my.see = 'd';      // D가 눌려있다면 d를 본다
                    else if (vsf.key[1]) vsf.my.see = 'a'; // A가 눌려있다면 a를 본다
                    else if (vsf.key[2]) vsf.my.see = 's'; // S가 눌려있다면 s를 본다
                                                       
                    break;

                case Keys.S:
                    vsf.key[2] = false;
                    vsf.player_move_anime_s = 0;
                    // S를 뗐을 때, 다른 키가 눌려있는지 확인
                    if (vsf.key[3]) vsf.my.see = 'd';
                    else if (vsf.key[1]) vsf.my.see = 'a';
                    else if (vsf.key[0]) vsf.my.see = 'w';
                    break;

                case Keys.A:
                    vsf.key[1] = false;
                    vsf.player_move_anime_a = 0;
                    // A를 뗐을 때, 다른 키가 눌려있는지 확인
                    if (vsf.key[0]) vsf.my.see = 'w';
                    else if (vsf.key[2]) vsf.my.see = 's';
                    else if (vsf.key[3]) vsf.my.see = 'd';
                    break;

                case Keys.D:
                    vsf.key[3] = false;
                    vsf.player_move_anime_d = 0;
                    // D를 뗐을 때, 다른 키가 눌려있는지 확인
                    if (vsf.key[0]) vsf.my.see = 'w';
                    else if (vsf.key[2]) vsf.my.see = 's';
                    else if (vsf.key[1]) vsf.my.see = 'a';
                    break;

                case Keys.P:
                    vsf.key[8] = false;
                    break;

                default:
                    return;
            }

            // 키 입력을 처리했으므로, 다른 컨트롤에 전달되는 것을 막습니다.
            e.Handled = true;
        }


        void game_loop() // 게임을 실제로 반복시킬 중요한 루프
        {
            game_timer = new Timer();
            game_timer.Interval = 1000 / vsf.fps; // 1000 = 1초, fps = 10 번으로 나눈다
            game_timer.Tick += game_timer_Tick; //Timer 객체의 'Tick' 이벤트 필드에 연결합니다.
            game_timer.Start(); //시작

        }
        void view_point_check() // 뷰 포인트를 옮길 메소드 
        {
            // 아바타 위치를 중점으로 좌우 750의 길이가 0~맵크기 사이일 경우 실행
            if (vsf.my.x - vsf.formsize_x/2 >= 0&&vsf.my.x+ vsf.formsize_x/2<=vsf.mapsize_x)
            {
                viewx = vsf.formsize_x / 2f - (float)vsf.my.x; // 왜 이렇게 되는지는 솔직히 저도 잘 몰르겠습니다
                //뷰포인트가 이렇게 본다고합니다
            }
            // 아바타 위치를 중점으로 상하 450의 길이가 0~맵크기 사이일 경우 실행
            if (vsf.my.y - vsf.formsize_y / 2 >= 0 && vsf.my.y + vsf.formsize_y / 2 <= vsf.mapsize_y)
            {
                viewy = vsf.formsize_y / 2f - (float)vsf.my.y;
            }
        }

        private void play_game_Load(object sender, EventArgs e)
        {

        }


        void game_timer_Tick(object sender, EventArgs e) { //실제로 틱 마다 실행하는 코드
            if (vsf.game_stop == true) { return; }
            if (vsf.game_end == true) // 게임이 끝났을 경우
            {
                game_timer.Stop(); // 1. 타이머 멈춤
                this.Hide();       // 2. 게임 화면 숨김

                using (End endForm = new End(vsf.kill_count,vsf.score)) // 3. 엔딩 화면 띄우기
                {
                    endForm.ShowDialog();
                }

                this.Close(); // 4. 엔딩 화면 닫히면 play_game 종료 (start 화면으로 복귀)
                return;
            }
            vsf.play_form_soft(); // 게임 내부의 좌표들을 처리함
            view_point_check(); // 뷰포인트 확인 후 옮김
            this.Invalidate();// 다시 그리기
        }

        private void play_game_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (vsf.game_end != true) // 게임이 로직대로 끝난게 아닌 강제로 종료되었을 때
            {
                vsf.viewx = viewx; 
                vsf.viewy = viewy;
                vsf.SaveGame();
                System.Windows.Forms.Application.Exit();//게임 창을 닫으면 모든 폼을 종료한다
            }
            
        }

        protected override void OnPaint(PaintEventArgs e) //그림그리기
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // 배경색 또는 맵 그리기 (선택)
            //g.Clear(Color.Black);//이거 안하면 안됨 그래서 그냥 넣었음



            g.TranslateTransform(viewx, viewy);// 뷰포인트 옮기기


            g.DrawImage(mapimage, 0, 0, vsf.mapsize_x, vsf.mapsize_y);//맵의 크기는 0~mapsize(5000) 까지 그림



            
            //적 몬스터 그리기
            for (int i = 0; i < vsf.monsters.Count; i++)// 몬스터 수 만큼 반복
            {
                if (vsf.monsters[i].type == 0) //좀비
                {
                    if (vsf.monsters[i].see == 'w') //위쪽
                    {
                        if ((int)vsf.monsters[i].move_smooth_count % 2 == 0)//왼발
                        {
                            g.DrawImage(zombie_top_left, (float)vsf.monsters[i].x, (float)vsf.monsters[i].y, vsf.monsters[i].size_x, vsf.monsters[i].size_y);
                        }
                        else//오른발
                        {
                            g.DrawImage(zombie_top_right, (float)vsf.monsters[i].x, (float)vsf.monsters[i].y, vsf.monsters[i].size_x, vsf.monsters[i].size_y);
                        }
                    }
                    else if (vsf.monsters[i].see == 'a')//왼쪽
                    {
                        if ((int)vsf.monsters[i].move_smooth_count % 2 == 0)//왼발
                        {
                            g.DrawImage(zombie_left_left, (float)vsf.monsters[i].x, (float)vsf.monsters[i].y, vsf.monsters[i].size_x, vsf.monsters[i].size_y);
                        }
                        else//오른발
                        {
                            g.DrawImage(zombie_left_right, (float)vsf.monsters[i].x, (float)vsf.monsters[i].y, vsf.monsters[i].size_x, vsf.monsters[i].size_y);
                        }
                    }
                    else if (vsf.monsters[i].see == 's')//밑쪽
                    {
                        if ((int)vsf.monsters[i].move_smooth_count % 2 == 0)//왼발
                        {
                            g.DrawImage(zombie_bottom_left, (float)vsf.monsters[i].x, (float)vsf.monsters[i].y, vsf.monsters[i].size_x, vsf.monsters[i].size_y);
                        }
                        else//오른발
                        {
                            g.DrawImage(zombie_bottom_right, (float)vsf.monsters[i].x, (float)vsf.monsters[i].y, vsf.monsters[i].size_x, vsf.monsters[i].size_y);
                        }
                    }
                    else//오른쪽
                    {
                        if ((int)vsf.monsters[i].move_smooth_count % 2 == 0)//왼발
                        {
                            g.DrawImage(zombie_right_left, (float)vsf.monsters[i].x, (float)vsf.monsters[i].y, vsf.monsters[i].size_x, vsf.monsters[i].size_y);
                        }
                        else//오른발
                        {
                            g.DrawImage(zombie_right_right, (float)vsf.monsters[i].x, (float)vsf.monsters[i].y, vsf.monsters[i].size_x, vsf.monsters[i].size_y);
                        }
                    }
                }
                else if (vsf.monsters[i].type == 1) //다른 몬스터들 추가 예정
                {

                }
            }

            //무기 이펙트 그리기
            float correction;//이미지가 좀 한쪽으로 커서 보정값
            float draw_plus_image_size; //size 보다 그림이 이상하게보여서 그림을 키울 생각입니다

            for (int i = vsf.Attack.Count - 1; i >= 0; i--)
            {
                correction = (float)vsf.Attack[i].size / 10;//이미지가 좀 한쪽으로 커서 보정값
                //이 배율에 따라 그림이 더 커집니다
                draw_plus_image_size = (float)vsf.Attack[i].size * 0.4f;//size 보다 그림이 이상하게보여서 그림을 키울 생각입니다
                if (vsf.Attack[i].see == 'w') // 위쪽
                {
                    // 1. 시간 흐르게 하기
                    vsf.Attack[i].anime_dur_count += anime_tick;

                    // 2. 한 프레임당 걸려야 하는 시간 계산
                    double timePerFrame = vsf.Attack[i].anime_dur / (double)attack_top_slash.Length;

                    // 3. 시간이 되었으면 다음 프레임으로
                    if (vsf.Attack[i].anime_dur_count >= timePerFrame)
                    {
                        vsf.Attack[i].sprite_count++; // 1장씩 부드럽게 넘어감

                        // 0으로 초기화하지 않고 뺍니다 
                        vsf.Attack[i].anime_dur_count -= timePerFrame;
                    }

                    // 4. 배열 범위를 넘었는지 확인 (삭제 처리)
                    if (vsf.Attack[i].sprite_count >= attack_top_slash.Length)
                    {
                        atk_delete_num.Add(i); // 삭제 리스트에 추가
                    }
                    else
                    {
                        // 5. 그리기 (삭제될 상황이 아닐 때만 그림)
                        // 좌표는 원래 주신대로 (x, y) 그대로 사용
                        g.DrawImage(attack_top_slash[vsf.Attack[i].sprite_count],
                            (float)vsf.Attack[i].x + correction - draw_plus_image_size / 2,
                            (float)vsf.Attack[i].y - draw_plus_image_size / 2,
                            (float)vsf.Attack[i].size + draw_plus_image_size,
                            (float)vsf.Attack[i].size + draw_plus_image_size);
                    }
                }
                else if (vsf.Attack[i].see == 'a')//왼
                {
                    // 1. 시간 흐르게 하기
                    vsf.Attack[i].anime_dur_count += anime_tick;

                    // 2. 한 프레임당 걸려야 하는 시간 계산
                    double timePerFrame = vsf.Attack[i].anime_dur / (double)attack_top_slash.Length;

                    // 3. 시간이 되었으면 다음 프레임으로
                    if (vsf.Attack[i].anime_dur_count >= timePerFrame)
                    {
                        vsf.Attack[i].sprite_count++; // 1장씩 부드럽게 넘어감 (기존 += 2 삭제)

                        // ★ 중요: 0으로 초기화하지 않고 뺍니다 (시간 오차 누적 방지 -> 끊김 해결)
                        vsf.Attack[i].anime_dur_count -= timePerFrame;
                    }

                    // 4. 배열 범위를 넘었는지 확인 (삭제 처리)
                    if (vsf.Attack[i].sprite_count >= attack_top_slash.Length)
                    {
                        atk_delete_num.Add(i); // 삭제 리스트에 추가
                    }
                    else
                    {
                        // 5. 그리기 (삭제될 상황이 아닐 때만 그림)
                        // 좌표는 원래 주신대로 (x, y) 그대로 사용
                        g.DrawImage(attack_left_slash[vsf.Attack[i].sprite_count],
                            (float)vsf.Attack[i].x - draw_plus_image_size / 2,
                            (float)vsf.Attack[i].y - correction - draw_plus_image_size / 2,
                            (float)vsf.Attack[i].size + draw_plus_image_size,
                            (float)vsf.Attack[i].size + draw_plus_image_size);
                    }
                }
                else if (vsf.Attack[i].see == 's')//밑
                {
                    // 1. 시간 흐르게 하기
                    vsf.Attack[i].anime_dur_count += anime_tick;

                    // 2. 한 프레임당 걸려야 하는 시간 계산
                    double timePerFrame = vsf.Attack[i].anime_dur / (double)attack_top_slash.Length;//애니메이션 최대시간/애니메이션 스프라이트 갯수 = 한스프라이트당 지속되야할 시간

                    // 3. 시간이 되었으면 다음 프레임으로
                    if (vsf.Attack[i].anime_dur_count >= timePerFrame)
                    {
                        vsf.Attack[i].sprite_count++; // 1장씩 부드럽게 넘어감 (기존 += 2 삭제)

                        // ★ 중요: 0으로 초기화하지 않고 뺍니다 (시간 오차 누적 방지 -> 끊김 해결)
                        vsf.Attack[i].anime_dur_count -= timePerFrame;
                    }

                    // 4. 배열 범위를 넘었는지 확인 (삭제 처리)
                    if (vsf.Attack[i].sprite_count >= attack_top_slash.Length)
                    {
                        atk_delete_num.Add(i); // 삭제 리스트에 추가
                    }
                    else
                    {
                        // 5. 그리기 (삭제될 상황이 아닐 때만 그림)
                        // 좌표는 원래 주신대로 (x, y) 그대로 사용
                        g.DrawImage(attack_bottom_slash[vsf.Attack[i].sprite_count],
                            (float)vsf.Attack[i].x - correction - draw_plus_image_size / 2,
                            (float)vsf.Attack[i].y - draw_plus_image_size / 2,
                            (float)vsf.Attack[i].size + draw_plus_image_size,
                            (float)vsf.Attack[i].size + draw_plus_image_size);
                    }
                }
                else if (vsf.Attack[i].see == 'd')//우
                {
                    // 1. 시간 흐르게 하기
                    vsf.Attack[i].anime_dur_count += anime_tick;

                    // 2. 한 프레임당 걸려야 하는 시간 계산
                    double timePerFrame = vsf.Attack[i].anime_dur / (double)attack_top_slash.Length;

                    // 3. 시간이 되었으면 다음 프레임으로
                    if (vsf.Attack[i].anime_dur_count >= timePerFrame)
                    {
                        vsf.Attack[i].sprite_count++; // 1장씩 부드럽게 넘어감 (기존 += 2 삭제)

                        // ★ 중요: 0으로 초기화하지 않고 뺍니다 (시간 오차 누적 방지 -> 끊김 해결)
                        vsf.Attack[i].anime_dur_count -= timePerFrame;
                    }

                    // 4. 배열 범위를 넘었는지 확인 (삭제 처리)
                    if (vsf.Attack[i].sprite_count >= attack_top_slash.Length)
                    {
                        atk_delete_num.Add(i); // 삭제 리스트에 추가
                    }
                    else
                    {
                        // 5. 그리기 (삭제될 상황이 아닐 때만 그림)
                        // 좌표는 원래 주신대로 (x, y) 그대로 사용
                        g.DrawImage(attack_right_slash[vsf.Attack[i].sprite_count],
                            (float)vsf.Attack[i].x - draw_plus_image_size / 2,
                            (float)vsf.Attack[i].y + correction - draw_plus_image_size / 2,
                            (float)vsf.Attack[i].size + draw_plus_image_size,
                            (float)vsf.Attack[i].size + draw_plus_image_size);
                    }
                }
            }
            for (int i = 0; i < atk_delete_num.Count; i++) //삭제
            {
                vsf.Attack.RemoveAt(atk_delete_num[i]);
            }
            atk_delete_num.Clear();//쓰레기통 비우기













            //아이템들 그리기
            for (int i = 0; i < vsf.item.Count; i++)
            {

                if (vsf.item[i].type == 3) //초록 경험치
                {
                    g.DrawImage(green_gem, (float)vsf.item[i].x, (float)vsf.item[i].y, (float)vsf.item[i].size, (float)vsf.item[i].size);
                }
                else if (vsf.item[i].type == 4) //파랑 경험치
                {
                    g.DrawImage(blue_gem, (float)vsf.item[i].x, (float)vsf.item[i].y, (float)vsf.item[i].size, (float)vsf.item[i].size);
                }
                else if (vsf.item[i].type == 5) //보라 경험치
                {
                    g.DrawImage(purple_gem, (float)vsf.item[i].x, (float)vsf.item[i].y, (float)vsf.item[i].size, (float)vsf.item[i].size);
                }
                else if (vsf.item[i].type == 1) //자석
                {
                    g.DrawImage(magnet, (float)vsf.item[i].x, (float)vsf.item[i].y, (float)vsf.item[i].size, (float)vsf.item[i].size);
                }
                else if (vsf.item[i].type == 0) //회복약
                {
                    g.DrawImage(heal, (float)vsf.item[i].x, (float)vsf.item[i].y, (float)vsf.item[i].size, (float)vsf.item[i].size);
                }
                else if (vsf.item[i].type == 2) //폭탄
                {
                    g.DrawImage(bomb, (float)vsf.item[i].x, (float)vsf.item[i].y, (float)vsf.item[i].size, (float)vsf.item[i].size);
                }
            }
            // 먹은 아이템
            for (int i = 0; i < vsf.eat.Count; i++)
            {
                if (vsf.eat[i].type == 3) //초록 경험치
                {
                    g.DrawImage(green_gem, (float)vsf.eat[i].x, (float)vsf.eat[i].y, (float)vsf.eat[i].size, (float)vsf.eat[i].size);
                }
                else if (vsf.eat[i].type == 4) //파랑 경험치
                {
                    g.DrawImage(blue_gem, (float)vsf.eat[i].x, (float)vsf.eat[i].y, (float)vsf.eat[i].size, (float)vsf.eat[i].size);
                }
                else if (vsf.eat[i].type == 5) //보라 경험치
                {
                    g.DrawImage(purple_gem, (float)vsf.eat[i].x, (float)vsf.eat[i].y, (float)vsf.eat[i].size, (float)vsf.eat[i].size);
                }
                else if (vsf.eat[i].type == 1) //자석
                {
                    g.DrawImage(magnet, (float)vsf.eat[i].x, (float)vsf.eat[i].y, (float)vsf.eat[i].size, (float)vsf.eat[i].size);
                }
                else if (vsf.eat[i].type == 0) //회복약
                {
                    g.DrawImage(heal, (float)vsf.eat[i].x, (float)vsf.eat[i].y, (float)vsf.eat[i].size, (float)vsf.eat[i].size);
                }
                else if (vsf.eat[i].type == 2) //폭탄
                {
                    g.DrawImage(bomb, (float)vsf.eat[i].x, (float)vsf.eat[i].y, (float)vsf.eat[i].size, (float)vsf.eat[i].size);
                }
            }









            //캐릭터 움직이는거 그리기
            if (vsf.my.see == 'w')
            {
                // W (후면) 방향
                if ((int)vsf.player_move_anime_w == 0)
                {
                    g.DrawImage(player_back_Image, (float)vsf.my.x, (float)vsf.my.y, vsf.my.size, vsf.my.size);
                }
                // 왼발 (vsf.player_move_anime_w가 짝수일 때)
                else if ((int)vsf.player_move_anime_w % 2 == 0)
                {
                    g.DrawImage(player_back_move_left_Image, (float)vsf.my.x, (float)vsf.my.y, vsf.my.size, vsf.my.size);
                }
                // 오른발 (vsf.player_move_anime_w가 홀수일 때)
                else
                {
                    g.DrawImage(player_back_move_right_Image, (float)vsf.my.x, (float)vsf.my.y, vsf.my.size, vsf.my.size);
                }
            }
            else if (vsf.my.see == 'a')
            {
                // A (왼쪽) 방향
                // 애니메이션 카운터 변수는 'vsf.player_move_anime_a'라고 가정
                if ((int)vsf.player_move_anime_a == 0)
                {
                    g.DrawImage(player_left_Image, (float)vsf.my.x, (float)vsf.my.y, vsf.my.size, vsf.my.size);
                }
                // 왼발
                else if ((int)vsf.player_move_anime_a % 2 == 0)
                {
                    g.DrawImage(player_left_move_left_Image, (float)vsf.my.x, (float)vsf.my.y, vsf.my.size, vsf.my.size);
                }
                // 오른발
                else
                {
                    g.DrawImage(player_left_move_right_Image, (float)vsf.my.x, (float)vsf.my.y, vsf.my.size, vsf.my.size);
                }
            }
            else if (vsf.my.see == 'd')
            {
                // D (오른쪽) 방향
                // 애니메이션 카운터 변수는 'vsf.player_move_anime_d'라고 가정
                if ((int)vsf.player_move_anime_d == 0)
                {
                    g.DrawImage(player_right_Image, (float)vsf.my.x, (float)vsf.my.y, vsf.my.size, vsf.my.size);
                }
                // 왼발
                else if ((int)vsf.player_move_anime_d % 2 == 0)
                {
                    g.DrawImage(player_right_move_left_Image, (float)vsf.my.x, (float)vsf.my.y, vsf.my.size, vsf.my.size);
                }
                // 오른발
                else
                {
                    g.DrawImage(player_right_move_right_Image, (float)vsf.my.x, (float)vsf.my.y, vsf.my.size, vsf.my.size);
                }
            }
            else // vsf.my.see == 's' (정면 또는 기본)
            {
                // S (정면) 방향
                // 애니메이션 카운터 변수는 'vsf.player_move_anime_s'라고 가정
                if ((int)vsf.player_move_anime_s == 0)
                {
                    g.DrawImage(player_front_Image, (float)vsf.my.x, (float)vsf.my.y, vsf.my.size, vsf.my.size);
                }
                // 왼발
                else if ((int)vsf.player_move_anime_s % 2 == 0)
                {
                    g.DrawImage(player_front_move_left_Image, (float)vsf.my.x, (float)vsf.my.y, vsf.my.size, vsf.my.size);
                }
                // 오른발
                else
                {
                    g.DrawImage(player_front_move_right_Image, (float)vsf.my.x, (float)vsf.my.y, vsf.my.size, vsf.my.size);
                }
            }

            //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 체력바 그리기 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            // 1. 체력 비율 계산 (0.0 ~ 1.0)(0% ~ 100%) 라는뜻
            // 0으로 나누는 오류 방지를 위해 hp_max가 0일 경우 처리 (안전장치)
            float hpRatio = 0;
            hpRatio = (float)vsf.my.hp / (float)vsf.my.hp_max;
            
            // 체력이 0보다 작아지면 바가 뚫고 나가는 것 방지
            if (hpRatio < 0) hpRatio = 0;

            // 2. 체력바 위치 및 크기 설정
            float hpbarWidth = vsf.my.size;       // 너비: 캐릭터 크기(size)와 동일하게
            float hpbarHeight = 6;                // 높이: 6픽셀 (원하는 대로 조절 가능)
            float hpbarX = (float)vsf.my.x-vsf.my.size/20;       // X좌표: 캐릭터와 동일
            float hpbarY = (float)vsf.my.y - hpbarHeight;  // Y좌표: 캐릭터 머리(y)보다 10픽셀 위로

            // 3. 그리기 도구 생성 (빨강: 깎인 체력/배경, 초록: 남은 체력)
            // using을 쓰는 이유 그게 좀더 안정적이라고 합니다
            using (SolidBrush backBrush = new SolidBrush(Color.Red))
            using (SolidBrush healthBrush = new SolidBrush(Color.LimeGreen))
            using (Pen borderPen = new Pen(Color.Black, 1)) // 테두리용 펜
            {
                // (1) 배경(빨간색) 그리기 - 전체 체력바 크기
                g.FillRectangle(backBrush, hpbarX, hpbarY, hpbarWidth, hpbarHeight);

                // (2) 현재 체력(초록색) 그리기 - 비율(hpRatio)만큼 너비 조절
                g.FillRectangle(healthBrush, hpbarX, hpbarY, hpbarWidth*hpRatio, hpbarHeight);

                // (3) 테두리 그리기 (검은색) - 깔끔하게 보이도록 외곽선 추가
                g.DrawRectangle(borderPen, hpbarX, hpbarY, hpbarWidth, hpbarHeight);
            }


            




            //@@@@@@@@@@@@ 경험치 바 그리기 @@@@@@@@@@@@@@@@@@@@@@

            // 1. 좌표계 초기화 (필수!) // 이걸 안하면 뷰포인트가 적용된 상태로 그려집니다
            g.ResetTransform();
            using (SolidBrush bgBrush = new SolidBrush(Color.Gray))        // 배경색
            using (SolidBrush expBrush = new SolidBrush(Color.Gold))        // 경험치색
            using (Pen borderPen = new Pen(Color.Black, 2))                 // 테두리
            using (Font expFont = new Font("맑은 고딕", 10, FontStyle.Bold)) // 폰트
            using (Brush textBrush = new SolidBrush(Color.Black))           // 글씨 색
            {
                // 1. 경험치 비율 계산 (0.0 ~ 1.0)
                float expRatio = 0;
                
                expRatio = (float)vsf.my.exp / (float)vsf.my.exp_max;
                
                // 비율이 1을 넘지 않도록 제한
                if (expRatio > 1) expRatio = 1;

                // 2. 위치 및 크기 설정 (변수명 앞에 exp를 붙였습니다)
                float expBarHeight = 20;                        // 높이
                float expBarWidth = this.ClientSize.Width;      // 너비 (화면 전체)
                float expBarX = 0;                              // X좌표 (왼쪽 끝)
                float expBarY = this.ClientSize.Height - expBarHeight; // Y좌표 (화면 맨 아래)

                // 3. 그리기
                // (1) 배경 (빈 게이지)
                g.FillRectangle(bgBrush, expBarX, expBarY, expBarWidth, expBarHeight);

                // (2) 현재 경험치 (차오른 게이지)
                // 너비를 비율(expRatio)만큼 곱해서 그림
                g.FillRectangle(expBrush, expBarX, expBarY, expBarWidth * expRatio, expBarHeight);

                // (3) 테두리 그리기
                g.DrawRectangle(borderPen, expBarX, expBarY, expBarWidth, expBarHeight);

                // (4) 텍스트 표시 (예: "LV.1 ( 50 / 100 )")
                string expText = $"LV.{vsf.my.Lv} ( {(int)vsf.my.exp} / {vsf.my.exp_max} )";

                // 글자를 바 정중앙에 놓기 위한 계산
                SizeF textSize = g.MeasureString(expText, expFont);
                float textX = expBarWidth / 2 - textSize.Width / 2;
                float textY = expBarY + (expBarHeight - textSize.Height) / 2;

                g.DrawString(expText, expFont, textBrush, textX, textY);
            }





            //@@@@@@@@@@@@@@@@ 타이머 @@@@@@@@@@@@@@@@@@@@@@
            // 1. 좌표계 초기화 (필수!) // 이걸 안하면 뷰포인트가 적용된 상태로 그려집니다
            g.ResetTransform();

            // 2. 폰트 및 브러시 설정
            // 글씨체: 맑은 고딕, 크기: 20, 스타일: 굵게
            using (Font timerFont = new Font("맑은 고딕", 20, FontStyle.Bold))
            using (Brush timerBrush = new SolidBrush(Color.White)) // 글자색: 흰색
            {
                // 3. 분과 초 계산하기
                int minutes = (int)vsf.timer / 60; // 전체 초를 60으로 나눈 몫 (분)
                int seconds = (int)vsf.timer % 60; // 전체 초를 60으로 나눈 나머지 (초)

                // 텍스트 만들기 (D2는 두 자리 숫자로 맞춘다는 뜻입니다. 예: 5 -> 05)
                // 예: "Time : 02:05"
                string timeText = $"Time {minutes}:{seconds}";

                // 4. 글자 위치 계산 (화면 정중앙 상단)
                // 글자의 실제 크기(가로, 세로)를 측정합니다.
                SizeF textSize = g.MeasureString(timeText, timerFont);

                // x좌표: (화면너비 - 글자너비) / 2  -> 이렇게 해야 정확히 가운데 옵니다.
                float x = (this.ClientSize.Width - textSize.Width) / 2;

                // y좌표: 위에서 20픽셀 띄움
                float y = 20;

                // 5. 그리기
                // (선택) 그림자 효과: 검은색으로 살짝 비껴서 먼저 그리면 글씨가 더 잘 보입니다.
                using (Brush shadowBrush = new SolidBrush(Color.Black))
                {
                    g.DrawString(timeText, timerFont, shadowBrush, x + 2, y + 2); // 그림자
                }

                // 진짜 글씨 그리기
                g.DrawString(timeText, timerFont, timerBrush, x, y);
            }














        }

    }
}
