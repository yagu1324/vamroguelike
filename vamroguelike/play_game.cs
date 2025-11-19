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


        public play_game()
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


            vsf = new vam_soft(); // 게임 내부 소프트
            this.ClientSize = new Size(vsf.formsize_x,vsf.formsize_y); //폼 크기 @@@@@

            viewx = (float)vsf.my.x;viewy=(float)vsf.my.y; // 뷰포인트값 아바타 위치로 초기화
            
            

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
            vsf.play_form_soft(); // 게임 내부의 좌표들을 처리함
            view_point_check(); // 뷰포인트 확인 후 옮김
            this.Invalidate();// 다시 그리기
        }
        protected override void OnPaint(PaintEventArgs e) //그림그리기
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // 배경색 또는 맵 그리기 (선택)
            //g.Clear(Color.Black);//이거 안하면 안됨 그래서 그냥 넣었음



            g.TranslateTransform(viewx, viewy);// 뷰포인트 옮기기


            g.DrawImage(mapimage,0, 0, vsf.mapsize_x, vsf.mapsize_y);//맵의 크기는 0~mapsize(5000) 까지 그림




            //적 몬스터 그리기
            for (int i = 0; i < vsf.monsters.Count; i++)// 몬스터 수 만큼 반복
            {
                if (vsf.monsters[i].type == 0) //좀비
                {
                    if (vsf.monsters[i].see == 'w') //위쪽
                    {
                        if ((int)vsf.monsters[i].move_smooth_count % 2 == 0)//왼발
                        {
                            Console.WriteLine("그림 그리는중");
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


            //아이템들 그리기




        }
    }
}
