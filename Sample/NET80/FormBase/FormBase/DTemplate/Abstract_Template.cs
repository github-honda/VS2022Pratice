/* 
Abstract_Template.cs


#### abstract 抽象物件備忘
1. 在方法或屬性宣告裡使用 abstract 修飾詞，表示該此方法或屬性沒有包含實作, 必須由繼承的子類別實作內容.
2. 抽象方法預設就是vitual方法, 可由子類別以override方式覆寫.
3. 抽象物件必須被繼承才能使用, 因此沒有 constructor. 
 Constructors on abstract types can be called only by derived types. Because public constructors create instances of a type, and you cannot create instances of an abstract type, an abstract type that has a public constructor is incorrectly designed.


*/


namespace FormBase.DTemplate
{
    public abstract class Abstract_Template
    {
        public Abstract_Template() 
        { }

        #region property
        /// <summary>
        /// Constructor Launch time.
        /// </summary>
        public readonly DateTime _CreateTime;

        /// <summary>
        /// (專案程式啟動執行)時間序號, 專案中最小的時間序號.
        /// (17位數的 Utc 時間到毫秒) + (2位數的亂數) 共19位數字, 可存放為 long 型別.
        /// 輸出格式可分為 1. 有序格式(有序到毫秒), 2. 無序格式(有序到年月, 其餘無序), 兩者僅格式不同, 內容相同, 也可以互轉.
        /// </summary>
        public string _String1 { get; private set; } = string.Empty;

        #endregion

        #region abstract 子類別必須實作的介面.
        public abstract void Build();
        #endregion

        #region virtual 子類別可覆寫的介面.
        public virtual void Flush()
        {
        }
        #endregion

        #region Dispose pattern
        private bool _Disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    //  dispose managed state (managed objects)
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                _Disposed = true;
            }
        }

        // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ZStore()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion


    }
}
