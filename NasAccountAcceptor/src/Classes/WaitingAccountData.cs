namespace NAS
{
    // NOTE: 가입 승인 대기자의 계정 정보를 포함합니다.
    public class WaitingAccountData
    {
        public int uuid { get; set; } // NOTE: 사용자 고유 번호
        public string name { get; set; } // NOTE: 사용자 이름
        public string id { get; set; } // NOTE: 사용자 아이디
        public string regdate { get; set; } // NOTE: 사용자의 가입 날짜
    }
}
