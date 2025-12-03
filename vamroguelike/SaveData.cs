using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// json 데이터는 값을 직렬화 하여저장한다
// 저장하고 싶은 데이터들을 굳이 따로 txt 모양으로 저장하지 않아도 사용한 상태 그대로 저장했다가 다시 빼낼 수 있다
namespace vamroguelike
{
    // 저장하고 싶은 데이터들을 모두 담는 클래스
    public class SaveData
    {
        public User PlayerData { get; set; }           // 플레이어 정보 (my)
        public List<Monster> MonsterList { get; set; } // 몬스터 리스트 (monsters)
        public List<Item> ItemList { get; set; }       // 아이템 리스트 (item)
        public List<Item> EatenItems { get; set; }     // 먹은 아이템 리스트 (eat)
        public double GameTime { get; set; }           // 진행된 게임 시간 (timer)

        // viewx, viewy: 화면 뷰포트의 위치 저장
        public float viewx { get; set; }
        public float viewy { get; set; }
    }
}
