using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vamroguelike
{
    public partial class see_ability : Form
    {
        // 폼 크기
        public int form_x { get; set; } = 700;
        public int form_y { get; set; } = 700;

        // 능력치 변수들
        public double attack_damage { get; set; }   // 공격력
        public double attack_speed { get; set; }    // 공속
        public double move_speed { get; set; }      // 이동속도
        public double max_hp { get; set; }          // 최대체력
        public double exp_plus { get; set; }        // 경험치 획득량
        public double weapon_size { get; set; }     // 무기 크기
        public double eat_size { get; set; }        // 아이템 먹는 크기
        public double weapon_damage { get; set; }   // 무기 공격력
        public double shield { get; set; }          // 방어력

        // 메인 게임 데이터를 받아올 변수
        User myData;

        // ★ 생성자 수정: User 데이터를 받도록 변경
        public see_ability()
        {
            InitializeComponent();
        }

        private void see_ability_Load(object sender, EventArgs e)
        {
            this.ClientSize = new Size(form_x, form_y); // 폼 크기 설정

            label1.Text = $"유저 공격력: {attack_damage:F1}";
            label2.Text = $"공격 속도: {attack_speed:F1}"; 
            label3.Text = $"이동 속도: {move_speed:F0}";
            label4.Text = $"최대 체력: {max_hp}";
            label5.Text = $"경험치 보너스: +{exp_plus * 100:F0}%";
            label6.Text = $"무기 크기: {weapon_size:F0}";
            label7.Text = $"아이템 획득 범위: {eat_size}";
            label8.Text = $"무기 공격력: {weapon_damage:F1}";
            label9.Text = $"방어력: {shield}";

            // =========================================================
            // 3. 이미지 넣기 (pictureBox 1~9)
            // =========================================================

            // 모든 픽쳐박스 비율 유지 설정 (반복문 사용 가능하지만 직관적으로 나열)
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox7.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox8.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox9.SizeMode = PictureBoxSizeMode.Zoom;

            // 1. 유저 공격력 (검)
            pictureBox1.Image = Image.FromFile(@"image/sword.png");

            // 2. 공속 (빠른 검)
            pictureBox2.Image = Image.FromFile(@"image/sword_speed.png");

            // 3. 이동 속도 (신발)
            pictureBox3.Image = Image.FromFile(@"image/shoes.png");

            // 4. 최대 체력 (하트)
            pictureBox4.Image = Image.FromFile(@"image/heart.png");

            // 5. 경험치 보너스 (보석)
            pictureBox5.Image = Image.FromFile(@"image/green_gem.png");

            // 6. 무기 크기 (감자)
            pictureBox6.Image = Image.FromFile(@"image/potato.png");

            // 7. 자석 범위 (자석)
            pictureBox7.Image = Image.FromFile(@"image/magnet.png");

            // 8. 무기 공격력 (이펙트) - 공격력과 구분을 위해 이펙트 이미지 사용
            pictureBox8.Image = Image.FromFile(@"image/axe.png");

            // 9. 방어력 (방패)
            pictureBox9.Image = Image.FromFile(@"image/shield.png");
        }
    }
}