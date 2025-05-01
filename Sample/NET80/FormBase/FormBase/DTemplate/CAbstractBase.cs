/* 
CAbstract.cs


#### abstract 抽象物件備忘
1. 在方法或屬性宣告裡使用 abstract 修飾詞，表示該此方法或屬性沒有包含實作, 必須由繼承的子類別實作內容.
2. 抽象方法預設就是vitual方法, 可由子類別以override方式覆寫.
3. 抽象物件必須被繼承才能使用, 因此沒有 constructor. 
 Constructors on abstract types can be called only by derived types. Because public constructors create instances of a type, and you cannot create instances of an abstract type, an abstract type that has a public constructor is incorrectly designed.


*/

namespace FormBase.DTemplate
{
    /// <summary>
    /// ZLib 習慣將 abstract class 字尾命名為 Base.
    /// </summary>
    public abstract class CAbstractBase : IDisposable
    {
        public CAbstractBase()
        { }

        #region property
        /// <summary>
        /// Constructor Launch time.
        /// </summary>
        public readonly DateTime _CreateTime = DateTime.Now;

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
