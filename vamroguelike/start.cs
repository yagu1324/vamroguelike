using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace vamroguelike
{
    public partial class start : Form
    {
        // 배경 스크롤을 위한 변수들
        Image backgroundImage; // 배경 이미지
        Timer backgroundTimer; // 움직임을 제어할 타이머
        int scrollY = 0;       // 현재 Y 위치
        int scrollSpeed = 2;   // 내려오는 속도 (클수록 빠름)

        public start()
        {
            InitializeComponent();

            //  화면 깜빡임 제거 (더블 버퍼링)
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();

            //  배경 이미지 로드 (경로를 본인 이미지에 맞게 수정하세요)
            // 예시로 기존에 있던 grass.png를 사용하거나, 더 큰 배경 이미지를 넣으세요.
            try
            {
                backgroundImage = Image.FromFile(@"image/grass.png");
            }
            catch
            {
                // 이미지가 없을 경우 오류 방지를 위해 빈 비트맵 생성 (혹은 메시지박스)
                backgroundImage = new Bitmap(1, 1);
            }

            //  타이머 설정
            backgroundTimer = new Timer();
            backgroundTimer.Interval = 20; // 약 50 FPS (1000ms / 20ms)
            backgroundTimer.Tick += BackgroundTimer_Tick;
            backgroundTimer.Start();
        }

        // 타이머가 울릴 때마다 실행 (좌표 이동)
        private void BackgroundTimer_Tick(object sender, EventArgs e)
        {
            scrollY += scrollSpeed; // 아래로 이동

            // 이미지가 한 바퀴 다 돌았으면 다시 0으로 초기화 (무한 루프)
            if (scrollY >= this.ClientSize.Height)
            {
                scrollY = 0;
            }

            // 화면을 다시 그리라고 요청 -> OnPaint 실행됨
            this.Invalidate();
        }

        // 실제 그림을 그리는 곳 (폼의 그리기 이벤트 오버라이드)
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);


            //  현재 내려오고 있는 이미지 그리기
            e.Graphics.DrawImage(backgroundImage, new Rectangle(0, scrollY, this.ClientSize.Width, this.ClientSize.Height));

            //  그 위쪽 빈 공간을 채울 두 번째 이미지 그리기
            e.Graphics.DrawImage(backgroundImage, new Rectangle(0, scrollY - this.ClientSize.Height, this.ClientSize.Width, this.ClientSize.Height));
            
        }


       

        // [새로하기 버튼]
        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists("savegame.json"))
            {
                File.Delete("savegame.json");
            }

            

            play_game game = new play_game(false);
            this.Hide();
            game.ShowDialog();
            //게임이 닫히면 끄는게 아닌 다시 보여줌
            this.Show();
            
        }

        // [이어하기 버튼]
        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists("savegame.json"))
            {
                

                play_game game = new play_game(true);
                this.Hide();
                game.ShowDialog();
                
                this.Show();
              
            }
            else
            {
                MessageBox.Show("저장된 게임이 없습니다!");
            }
        }

        private void start_Load(object sender, EventArgs e) // 창이 켜지자 마자 생성
        {
            if (File.Exists("savegame.json"))
            {
                DialogResult result = MessageBox.Show(
                    "저장된 데이터가 있습니다. 이어하시겠습니까?",
                    "알림",
                    MessageBoxButtons.YesNo
                );

                if (result == DialogResult.Yes)
                {
                    button2_Click(sender, e);
                }
            }
        }
    }
}