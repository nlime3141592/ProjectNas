namespace NAS
{
    public class LoginData
    {
        // NOTE: 사용자 고유 번호
        public int uuid { get; set; } = -1;

        // NOTE: 사용자 이름
        public string name { get; set; } = "";

        // NOTE: 사용자가 속한 부서의 고유 번호
        public int department { get; set; } = 0;

        // NOTE: 사용자의 권한 레벨
        public int level { get; set; } = 0;
    }
}
