뷰의 datacontext는 view의 코드비하인드에서 처리하지 않는다.

ServiceCollectionExtensions의 AddCommonService에서 windowservice를 싱글톤으로 등록, subwindow의 뷰모델과 뷰를 등록한다.

windowservice 클래스의 register 메서드에서 뷰모델의 타입을 dictionary로 등록한다.

이후 view를 show해야할경우 3.에서 등록된 viewmodel의 타입을 바탕으로 내부 서비스 필드를 통해 getrequiredservice로 획득한뒤 datacontext에 리턴한다.
