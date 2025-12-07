using System;
using System.Drawing; // 이미지 처리를 위해 필요
using System.Windows.Forms;
using System.IO; // 파일 존재 여부 확인을 위해 필요 (선택사항)

namespace vamroguelike
{
    public partial class End : Form
    {
        // 멤버 변수 (필요하다면 나중에 쓸 수 있도록 저장)
        int kill_count = 0;
        int score = 0;

        //  1초 딜레이를 위한 타이머와 플래그 변수
        Timer delayTimer = new Timer();
        bool canClose = false; // 처음엔 닫기 불가능 상태

        // 이제 이 폼을 띄울 때 반드시 킬 수와 점수를 넘겨주어야 합니다.
        public End(int receivedKill, int receivedScore)
        {
            InitializeComponent();

            // 외부에서 받은 값 저장
            this.kill_count = receivedKill;
            this.score = receivedScore;

            //  라벨에 값 적용 (label2: 킬카운트, label3: 점수)
            // 문자열 보간($)을 사용하여 보기 좋게 넣었습니다.
            label2.Text = $"킬 수 : {kill_count}";
            label3.Text = $"점수 : {score}";

            //  배경 이미지 설정
            // 프로그램 실행 파일(bin/Debug/...)과 같은 폴더에 "background.png"가 있어야 합니다.
           
                
            this.BackgroundImage = Image.FromFile(@"image/grass.png");
            this.BackgroundImageLayout = ImageLayout.Stretch; // 이미지를 폼 크기에 맞게 늘림




            //  1초 딜레이 타이머 설정
            delayTimer.Interval = 1000; // 1000ms = 1초
            delayTimer.Tick += DelayTimer_Tick; // 시간이 되면 실행할 함수 연결
            delayTimer.Start(); // 타이머 시작

            // 키 이벤트 설정
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(End_KeyDown);
        }
        // 1초가 지나면 실행되는 함수
        private void DelayTimer_Tick(object sender, EventArgs e)
        {
            canClose = true;   // 이제 닫을 수 있다고 허락해줌
            delayTimer.Stop(); // 타이머는 할 일 다 했으니 멈춤
        }

        private void End_KeyDown(object sender, KeyEventArgs e) //키 입력이 있으면 종료
        {
            if (canClose == true)
            {
                this.Close();
            }
        }
    }
}