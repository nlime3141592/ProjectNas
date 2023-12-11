namespace NAS
{
    // NOTE: 서비스 객체의 추상 클래스입니다.
    public abstract class NasService
    {
        // NOTE: 모든 서비스 객체는 이 함수를 구현하여 서비스 로직을 만들어야 합니다.
        public abstract NasServiceResult Execute();
    }
}
