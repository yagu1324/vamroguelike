using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq; // 리스트 섞기(Shuffle) 기능을 위해 필수
using System.Windows.Forms;

namespace vamroguelike
{
    public partial class Select_ability : Form
    {
        // 메인 게임으로 보낼 결과값 (프로퍼티)
        public int select_num { get; set; }   // 선택한 능력치 번호
        public double add_value { get; set; } // 선택한 능력치 수치

        // ★ 핵심: 3개의 패널 정보를 저장할 배열 (클래스 대신 사용)
        // 0번 칸, 1번 칸, 2번 칸의 데이터를 각각 저장합니다.
        private int[] save_ids = new int[3];       // 능력치 번호 저장
        private double[] save_values = new double[3]; // 수치 저장
        private string[] save_ranks = new string[3];  // 등급 저장 (색칠용)

        Random rand = new Random();

        public Select_ability()
        {
            InitializeComponent();
        }

        // [1] 폼이 켜질 때 실행 (Load)
        private void Select_ability_Load(object sender, EventArgs e)
        {
            // 1. 중복 방지를 위해 0~6 카드를 섞습니다.
            List<int> deck = new List<int>() { 0, 1, 2, 3, 4, 5, 6 }; //0.공격력 1.최대체력 2.공격속도 3.방어력 4.무기크기 5.이동속도 6.경험치획득량
            var shuffledDeck = deck.OrderBy(x => Guid.NewGuid()).ToList();

            // 2. 섞인 카드를 각 패널에 배정합니다. (마지막 숫자는 칸 번호 0, 1, 2)
            SetAbility(select_1, select_image_1, select_write_1, shuffledDeck[0], 0);
            SetAbility(select_2, select_image_2, select_write_2, shuffledDeck[1], 1);
            SetAbility(select_3, select_iamge_3, select_write_3, shuffledDeck[2], 2);
        }

        // [2] 패널 세팅 함수
        // abilityId: 능력치 번호 (섞인 카드에서 나온 것)
        // slotIndex: 몇 번째 칸인지 (0, 1, 2) -> 배열 저장 위치
        private void SetAbility(Panel p, PictureBox pic, Label lbl, int abilityId, int slotIndex)
        {

            lbl.Font = new Font("맑은 고딕", 13F, FontStyle.Bold);
            // 1. 등급(Rank) 뽑기
            int rankNum = rand.Next(0, 101);
            string rank_text = "";

            if (rankNum < 50) rank_text = "Common";
            else if (rankNum < 80) rank_text = "Rare";
            else if (rankNum < 95) rank_text = "Epic";
            else if (rankNum < 99) rank_text = "Legendary";
            else rank_text = "Mythic";

            // 2. 능력치 수치(Value) 계산
            string title = "";
            double value = 0;
            string desc = ""; // 설명 텍스트를 위한 변수

            //픽쳐박스 이미지
            pic.SizeMode = PictureBoxSizeMode.Zoom;// 이미지 비율이 깨지지 않게 Zoom으로 설정 (안전장치)

            if (abilityId == 0) // 공격력
            {
                title = "공격력";
                if (rank_text == "Common") value = GetRandomValue(0.1, 1.4);
                else if (rank_text == "Rare") value = GetRandomValue(1.5, 2.9);
                else if (rank_text == "Epic") value = GetRandomValue(3.0, 4.9);
                else if (rank_text == "Legendary") value = GetRandomValue(5.0, 10.0);
                else value = GetRandomValue(10.0, 20.0);
                desc = "증가합니다.";
                pic.Image=Image.FromFile("image/sword.png"); //공격력
            }
            else if (abilityId == 1) // 체력
            {
                title = "최대체력";
                if (rank_text == "Common") value = GetRandomValue(1, 3);
                else if (rank_text == "Rare") value = GetRandomValue(3, 7);
                else if (rank_text == "Epic") value = GetRandomValue(7, 15);
                else if (rank_text == "Legendary") value = GetRandomValue(15, 30);
                else value = GetRandomValue(30, 50);
                desc = "늘어납니다.";
                pic.Image = Image.FromFile("image/heart.png"); //체력
            }
            else if (abilityId == 2) // 공속 (초당 공격 횟수 방식 추천)
            {
                title = "공격속도";
                if (rank_text == "Common") value = GetRandomValue(0.1, 0.2);
                else if (rank_text == "Rare") value = GetRandomValue(0.2, 0.4);
                else if (rank_text == "Epic") value = GetRandomValue(0.4, 0.7);
                else if (rank_text == "Legendary") value = GetRandomValue(0.7, 1.0);
                else value = GetRandomValue(1.0, 2.0);
                desc = "빨라집니다.";
                pic.Image = Image.FromFile("image/sword_speed.png"); //공격속도
            }
            else if (abilityId == 3) // 방어력
            {
                title = "방어력"; // (원래 코드 오타 수정: 공격속도 -> 방어력)
                if (rank_text == "Common") value = GetRandomValue(1, 2);
                else if (rank_text == "Rare") value = GetRandomValue(3, 5);
                else if (rank_text == "Epic") value = GetRandomValue(5, 7);
                else if (rank_text == "Legendary") value = GetRandomValue(7, 10);
                else value = GetRandomValue(10, 15);
                desc = "증가합니다.";
                pic.Image = Image.FromFile("image/shield.png"); //방어력
            }
            else if (abilityId == 4) // 무기크기
            {
                title = "무기크기";
                if (rank_text == "Common") value = GetRandomValue(0.1, 0.15);
                else if (rank_text == "Rare") value = GetRandomValue(0.15, 0.2);
                else if (rank_text == "Epic") value = GetRandomValue(0.2, 0.4);
                else if (rank_text == "Legendary") value = GetRandomValue(0.5, 0.7);
                else value = GetRandomValue(0.7, 1.0);
                desc = "커집니다.";
                pic.Image = Image.FromFile("image/potato.png"); //무기크기
            }
            else if (abilityId == 5) // 이속
            {
                title = "이동속도";
                if (rank_text == "Common") value = GetRandomValue(1, 5);
                else if (rank_text == "Rare") value = GetRandomValue(5, 10);
                else if (rank_text == "Epic") value = GetRandomValue(10, 15);
                else if (rank_text == "Legendary") value = GetRandomValue(15, 40);
                else value = GetRandomValue(40, 80);
                desc = "빨라집니다.";
                pic.Image = Image.FromFile("image/shoes.png"); //이동속도
            }
            else if (abilityId == 6) // 경험치
            {
                title = "경험치 획득량";
                if (rank_text == "Common") value = GetRandomValue(1, 5);
                else if (rank_text == "Rare") value = GetRandomValue(5, 10);
                else if (rank_text == "Epic") value = GetRandomValue(10, 15);
                else if (rank_text == "Legendary") value = GetRandomValue(15, 40);
                else value = GetRandomValue(40, 80);
                desc = "증가합니다.";
                pic.Image = Image.FromFile("image/green_gem.png"); //경험치
            }

            // 정해진 값들을 배열에 넣음 (저장용) 
            save_ids[slotIndex] = abilityId;   // 예: 0 (공격력)
            save_values[slotIndex] = value;    // 예: 5.5
            save_ranks[slotIndex] = rank_text; // 예: "Legendary"

            // 3. 디자인 적용
            // 패널 안쪽으로 여백을 줘서 테두리가 보일 공간을 만듭니다.
            p.Padding = new Padding(5);

            if (abilityId == 6 || abilityId==4)
            {
                lbl.Text = $"[{rank_text}] {title}\n{title}이(가) {value}%만큼 {desc}";
            }
            else
            {
                lbl.Text = $"[{rank_text}] {title}\n{title}이(가) {value}만큼 {desc}";
            }

            


            // 4. 번호표(Tag)에는 '칸 번호(0,1,2)'를 붙입니다.
            p.Tag = slotIndex;
            pic.Tag = slotIndex;
            lbl.Tag = slotIndex;

            // 5. 마우스 손가락 모양 설정
            p.Cursor = Cursors.Hand;
            pic.Cursor = Cursors.Hand;
            lbl.Cursor = Cursors.Hand;

            // 6. 클릭 이벤트 연결 (하나의 함수로 통일)
            p.Click += Common_Click;
            pic.Click += Common_Click;
            lbl.Click += Common_Click;

            // 7. 테두리 그리기 연결
            p.Paint -= Draw_Panel_Border; // 기존 연결 삭제 (중복 방지)
            p.Paint += Draw_Panel_Border; // 연결

            // 픽쳐박스에도 테두리 연결 (원하시면)
            pic.Paint -= Draw_Panel_Border;
            pic.Paint += Draw_Panel_Border;

            p.Invalidate(); // 다시 그려라!
        }

        // [3] 공통 클릭 이벤트
        private void Common_Click(object sender, EventArgs e)
        {
            Control clickedControl = (Control)sender;

            if (clickedControl.Tag != null)
            {
                // 1. 내가 몇 번째 칸인지 확인 (0, 1, 2)
                int slotIndex = (int)clickedControl.Tag;

                // 2. 아까 배열에 저장해둔 진짜 값을 꺼내서 메인으로 보낼 준비 끝!
                this.select_num = save_ids[slotIndex];     // 능력치 번호
                this.add_value = save_values[slotIndex];   // 수치

                // 3. 닫기 (성공)
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        // [4] 테두리 그리기 함수 (배열에서 등급을 확인해서 색칠)
        private void Draw_Panel_Border(object sender, PaintEventArgs e)
        {
            Control c = (Control)sender; // Panel 또는 PictureBox
            if (c.Tag == null) return;

            // 1. 칸 번호 확인
            int slotIndex = (int)c.Tag;

            // 2. 배열에서 등급(Rank) 꺼내기
            string myRank = save_ranks[slotIndex];

            // 3. 색깔 결정
            Color borderColor = Color.Gray;
            switch (myRank)
            {
                case "Common": borderColor = Color.Gray; break;
                case "Rare": borderColor = Color.DodgerBlue; break;     // 파랑
                case "Epic": borderColor = Color.MediumPurple; break;   // 보라
                case "Legendary": borderColor = Color.Gold; break;      // 노랑
                case "Mythic": borderColor = Color.Red; break;          // 빨강
            }

            // 4. 그리기 (두께 5)
            using (Pen myPen = new Pen(borderColor, 5))
            {
                // 테두리가 안쪽으로 예쁘게 들어오도록 좌표 보정
                int halfPen = (int)(myPen.Width / 2);
                Rectangle rect = new Rectangle(halfPen, halfPen, c.Width - (int)myPen.Width, c.Height - (int)myPen.Width);
                e.Graphics.DrawRectangle(myPen, rect);
            }
        }

        // 랜덤 수치 계산 도우미 (소수점 1자리 반올림)
        private double GetRandomValue(double min, double max)
        {
            return Math.Round((rand.NextDouble() * (max - min) + min), 1);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
    }
}