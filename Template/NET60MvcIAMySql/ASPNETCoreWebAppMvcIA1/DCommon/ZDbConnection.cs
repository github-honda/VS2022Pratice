using System.Data.Common;
using System.Data;

namespace NET60MvcIAMySql.DCommon
{
    /// <summary>
    /// 資料庫抽象物件, 使用一致性的方法, 操作不同的資料庫來源.
    /// 內部會管理 Connection, Command...等物件, 自動 Dispose.
    /// 建議: 一個 ConnectString 應對應使用一個 ZDbClientBase
    /// </summary>
    public abstract class ZDbConnection : IDisposable
    {
#nullable enable
        public ZDbConnection()
        {
        }
        public ZDbConnection(string PConnectionString)
        {
            _ConnectionString = PConnectionString;
        }

        public string? _ConnectionString = null;

        /// <summary>
        /// 通用的 Connection.
        /// 若需使用資料庫特殊功能, 則只需轉型為該資料庫的型別(例如: SQLiteConnection)後使用. 
        /// 若開啟後, 本 Class 存活期間一律不關閉 Connection, 保持開啟狀態. 除了效率以外, 因為有些資料庫 例如 SQLite DB memory db 必須保持資料庫連線, 才能使用.
        /// </summary>
        public DbConnection? _Connection = null;

        /// <summary>
        /// 執行 Command 等待完成時間(秒數, 預設為30秒)
        /// CommandTimeout 不是 ConnectionTimeout.
        /// ConnectionTimeout 是唯讀不可設定, 預設為15秒.
        /// The time in seconds to wait for the command to execute. The default is 30 seconds.  
        /// </summary>
        public int _CommandTimeout { get; set; } = 30;

        /// <summary>
        /// 取得 DbConnectionStringBuilder 物件.
        /// DbConnectionStringBuilder 提供屬性可查詢 ConnectionString 的欄位值.
        /// </summary>
        /// <returns></returns>
        public DbConnectionStringBuilder GetBuilder()
        {
            DbConnectionStringBuilder Builder = new DbConnectionStringBuilder
            {
                ConnectionString = _ConnectionString
            };
            return Builder;
        }

        /// <summary>
        /// 取得 ConnectionString 中的欄位值.
        /// 例如: server, database...
        /// </summary>
        /// <param name="PFieldName"></param>
        /// <returns></returns>
        public string? GetConnectionStringField(string PFieldName)
        {
            return GetBuilder()[PFieldName].ToString();
        }

        /// <summary>
        /// 取得 ConnectionString 中的 server 欄位值.
        /// </summary>
        /// <returns></returns>
        public string? GetConnectionStringServer()
        {
            return GetConnectionStringField("server");
        }

        /// <summary>
        /// 取得 ConnectionString 中的 database 欄位值.
        /// </summary>
        /// <returns></returns>
        public string? GetConnectionStringDatabase()
        {
            return GetConnectionStringField("database");
        }

        /// <summary>
        /// 取得 ConnectionString 中, "Server=[主機], Database=[資料庫]"的字串.
        /// </summary>
        /// <returns></returns>
        public string GetConnectionStringServerAndDatabase()
        {
            return $"Server={GetConnectionStringServer()}, Database={GetConnectionStringDatabase()}";
        }


        /// <summary>
        /// 建立 DbConnection 物件.
        /// 由各家 Database provider 提供, 再轉型為共用的 DbConnection.
        /// </summary>
        /// <returns></returns>
        abstract public DbConnection CreateDbConnection();

        /// <summary>
        /// 建立 DbDataAdapter 物件.
        /// 由各家 Database provider 提供, 再轉型為共用的 DbDataAdapter.
        /// </summary>
        /// <returns></returns>
        abstract public DbDataAdapter CreateDbDataAdapter();

        /// <summary>
        /// 開啟資料庫.
        /// 若未建立 DbConnection, 則新建.
        /// 若開啟後, 本 Class 存活期間一律不關閉 Connection, 保持開啟狀態. 除了效率以外, 因為有些資料庫 例如 SQLite DB memory db 必須保持資料庫連線, 才能使用.
        /// </summary>
        public void Open()
        {
            if (_Connection == null)
                _Connection = CreateDbConnection();

            if (string.IsNullOrEmpty(_Connection.ConnectionString))
            {
                //_Connection.ZOpen(_ConnectionString);
                _Connection.ConnectionString = _ConnectionString;
                _Connection.Open();
            }
            else
                throw new InvalidOperationException($"Different connection string. _ConnectionString={_ConnectionString}, _Connection.ConnectionString={_Connection.ConnectionString}");

            // 開啟時變更 _ConnectionString, 會拋出 Exception
            //else if (_Connection.ConnectionString != _ConnectionString)
            //    _Connection.ZOpen(_ConnectionString);

            if (_Connection.State == ConnectionState.Closed)
                _Connection.Open();
        }
        public void Close()
        {
            if (_Connection == null)
                return;

            if (_Connection.State == ConnectionState.Open)
                _Connection.Close();
        }
        public bool IsClosed()
        {
            if (_Connection == null)
                return true;

            if (_Connection.State == ConnectionState.Closed)
                return true;

            return false;
        }

        /// <summary>
        /// 測試連接資料庫. 
        /// 若成功連接資料庫, 則回傳 true, 否則回傳 false, 不會拋出錯誤.
        /// 錯誤訊息可經由參數 PException 取得.
        /// </summary>
        /// <param name="PException"></param>
        /// <returns></returns>
        public bool TryOpen(out Exception? PException)
        {
            PException = null;
            try
            {
                using (DbConnection cn = CreateDbConnection())
                {
                    cn.ConnectionString = _ConnectionString;
                    cn.Open();
                }
                return true;
            }
            catch (Exception ex1)
            {
                PException = ex1;
                return false;
            }
        }

        #region 盡量維持模組可獨立運作, 不要依賴太多共用函數!  
        // 20230427, 從 ZData 中搬移到 ZDbConnection

        ////// 20230427, 從 ZData 中搬移到 ZDbConnection.
        ///// <summary>
        ///// 測試連接資料庫. 
        ///// 若連接成功, 則回傳 ture, 否則回傳 false 及 exception.
        ///// </summary>
        ///// <returns></returns>
        //bool MyTryOpen(DbConnection cn1, string sConnectionString, out Exception? OutException)
        //{
        //    try
        //    {
        //        using (cn1)
        //        {
        //            cn1.ConnectionString = sConnectionString;
        //            cn1.Open();
        //            OutException = null;
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        OutException = ex;
        //        return false;
        //    }
        //}

        ////// 20230427, 從 ZData 中搬移到 ZDbConnection.
        //void MyOpen(DbConnection cn1, string? sConnectionString)
        //{
        //    cn1.ConnectionString = sConnectionString;
        //    cn1.Open();
        //}


        ///// <summary>
        ///// 呼叫 PObject.ToString() 取得字串. 
        ///// 若 PValue 為 null 或 DBNull, 則轉換為 PDefault.
        ///// </summary>
        ///// <param name="PValue"></param>
        ///// <param name="PDefault"></param>
        ///// <returns></returns>
        //string? MyToString(object? PValue, string? PDefault = null)
        //{
        //    // NotNullWhen 也可解決 Warning CS8602
        //    if (PValue == null)  //Warning CS8602  Dereference of a possibly null reference.
        //        return null;

        //    if (MyIsNullOrDBNull(PValue))
        //        return PDefault;

        //    return PValue.ToString();
        //}


        //#if NETSTANDARD2_0
        //        ///// <summary>
        //        ///// 是否為 null 或 DBNull ?
        //        ///// </summary>
        //        ///// <param name="oInput"></param>
        //        ///// <returns></returns>
        //        //bool MyIsNullOrDBNull(object? oInput) // CodeHelper NotNullWhen 若函數回傳值為 false, 則不需要檢查參數是否為 null ?.
        //        //{
        //        //    if (oInput == null)
        //        //        return true;
        //        //    return MyIsDBNull(oInput);
        //        //}

        //        ///// <summary>
        //        ///// 物件是否為 DBNull ?
        //        ///// </summary>
        //        ///// <param name="oField"></param>
        //        ///// <returns></returns>
        //        //bool MyIsDBNull(object? oField) // CodeHelper NotNullWhen 若函數回傳值為 false, 則不需要檢查參數是否為 null ?.
        //        //{
        //        //    // 判斷 DBNull 的標準寫法
        //        //    // Represents a nonexistent value. 
        //        //    // 這樣寫也OK!
        //        //    //if (v1 is DBNull)
        //        //    //    return DefaultValue;
        //        //    //if (DBNull.Value.Equals(oField))
        //        //    //    return true;
        //        //    //else
        //        //    //    return false;
        //        //    //if (oField == System.DBNull.Value)
        //        //    //    return true;
        //        //    //else
        //        //    //    return false;
        //        //    if (oField == null)
        //        //        return false;
        //        //    return (oField == DBNull.Value);
        //        //}
        //#endif
        //#if NET6_0_OR_GREATER
        //        /// <summary>
        //        /// 是否為 null 或 DBNull ?
        //        /// </summary>
        //        /// <param name="oInput"></param>
        //        /// <returns></returns>
        //        bool MyIsNullOrDBNull([NotNullWhen(false)] object? oInput) // CodeHelper NotNullWhen 若函數回傳值為 false, 則不需要檢查參數是否為 null ?.
        //        {
        //            if (oInput == null)
        //                return true;
        //            return MyIsDBNull(oInput);
        //        }

        //        /// <summary>
        //        /// 物件是否為 DBNull ?
        //        /// </summary>
        //        /// <param name="oField"></param>
        //        /// <returns></returns>
        //        bool MyIsDBNull([NotNullWhen(false)] object? oField) // CodeHelper NotNullWhen 若函數回傳值為 false, 則不需要檢查參數是否為 null ?.
        //        {
        //            // 判斷 DBNull 的標準寫法
        //            // Represents a nonexistent value. 
        //            // 這樣寫也OK!
        //            //if (v1 is DBNull)
        //            //    return DefaultValue;
        //            //if (DBNull.Value.Equals(oField))
        //            //    return true;
        //            //else
        //            //    return false;
        //            //if (oField == System.DBNull.Value)
        //            //    return true;
        //            //else
        //            //    return false;
        //            if (oField == null)
        //                return false;
        //            return (oField == DBNull.Value);
        //        }
        //#endif

        //// 20230427, 從 ZData 中搬移到 ZDbConnection, 改為 Private MyQueryDataSet()
        //// 20230430, 搬到  QueryDataSet 中
        //DataSet MyQueryDataSet(DbCommand cmd1, DbDataAdapter adapter1)
        //{
        //    adapter1.SelectCommand = cmd1;
        //    DataSet ds1 = new DataSet();
        //    adapter1.Fill(ds1);
        //    return ds1;
        //}

        ////// 20230427, 從 ZData 中搬移到 ZDbConnection.
        ///// <summary>
        ///// 設定 Command.Parameters.
        ///// Parameter 為("@" 字元開始的字串).
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="PParameters"></param>
        //void MySetParameters(DbCommand command, Dictionary<string, object>? PParameters)
        //{
        //    // Command 建立後, 才可以使用 Parameters 屬性, 不是很方便.
        //    // 透過本函數可一次建立 Command.Parameters.
        //    // 建立 Command.Parameters 後, 如需改變 Parameter.Value, 可由 Command.Parameters[Index or Name]變更.

        //    // Parameters 必須建立在 Command 的 Instance
        //    // Sample1:
        //    //int iResult = 0;
        //    //using (ZMySqlClient db1 = CDB._Me.CreateDBClient())
        //    //{
        //    //    string sCmd = "insert into tarea (FID, FBuilding, FFloor, FProhibit, FCreateTime) values (@FID, @FBuilding, @FFloor, @FProhibit, CURRENT_TIMESTAMP)";
        //    //    Dictionary<string, object> para1 = ZDictionary.ZCreateDictionaryStringObject();
        //    //    para1.Add("@FID", _ID);
        //    //    para1.Add("@FBuilding", _sBuilding);
        //    //    para1.Add("@FFloor", _iFloor);
        //    //    para1.Add("@FProhibit", _Prohibit ? 1 : 0);
        //    //    iResult = db1.ExecuteNonQuery(sCmd, para1);
        //    //    return iResult;
        //    //}

        //    // Sample2:
        //    //Dictionary<string, object> parameters
        //    //using (ZMySqlClient db1 = CDB._Me.CreateDBClient())
        //    //{
        //    //    string sCmd = "select t2.* from ttrace t1, ttracelog t2 where t1.FNo = t2.FNo and t2.FTimeReceive>@FTimeReceive order by t2.FID";
        //    //    Dictionary<string, object> para1 = ZDictionary.ZCreateDictionaryStringObject();
        //    //    para1.Add("@FTimeReceive", _TimeReadDB);
        //    //    DataTable t1 = db1.QueryDataTable(sCmd, para1);
        //    //    foreach (DataRow row1 in t1.Rows)
        //    //    {
        //    //        iResult++;
        //    //        string sID = row1["FID"].ZToString_Object();
        //    //        lock (_Lock)
        //    //        {
        //    //            CItemTag item1 = GetItem(sID);
        //    //            if (item1 == null)
        //    //            {
        //    //                CLog.Me.Error($"讀取到未註冊的TrackerID ={sID}.");
        //    //                continue;
        //    //            }
        //    //            item1.DBFetchTraceLog(row1);
        //    //            if (item1.ReceiveTime > dLast)
        //    //                dLast = item1.ReceiveTime;
        //    //        }
        //    //    }
        //    //}

        //    // Sample3: Parameter 用 ? 的, 不是用 首字為 "@" 字元
        //    // Transaction sample:
        //    //// 只用一個 SQLiteTransaction
        //    //using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
        //    //{
        //    //    using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
        //    //    {
        //    //        SQLiteParameter myparam = new SQLiteParameter();
        //    //        int n;

        //    //        mycommand.CommandText = "INSERT INTO [MyTable] ([MyId]) VALUES(?)";
        //    //        mycommand.Parameters.Add(myparam);

        //    //        // 迴圈中單純新增作業, 不要包含 SQLiteCommand, SQLiteParameter.
        //    //        for (n = 0; n < 100000; n++)
        //    //        {
        //    //            myparam.Value = n + 1;
        //    //            mycommand.ExecuteNonQuery();
        //    //        }
        //    //    }
        //    //    // 只需 Commit 一個 SQLiteTransaction
        //    //    mytransaction.Commit();
        //    //}


        //    if (PParameters == null)
        //        return;
        //    foreach (var param in PParameters)
        //    {
        //        var parameter = command.CreateParameter();
        //        parameter.ParameterName = param.Key;
        //        // 20230427, 從 ZData 中搬移到 ZDbConnection.
        //        //parameter.Value = param.Value ?? ZObject.ZDBNullObject(); // CodeHelper.
        //        parameter.Value = param.Value ?? DBNull.Value; // CodeHelper.
        //        command.Parameters.Add(parameter);
        //    }
        //}



        //// 20230427, 從 ZData 中搬移到 ZDbConnection.

        ///// <summary>
        ///// 建立 DbCommand.
        ///// 使用後應 Dispose.
        ///// 若只變更 Command.Parameters[Name].Value,  則執行重複的 SQL Command 時, 可加速執行.
        ///// </summary>
        ///// <param name="PConnection"></param>
        ///// <param name="PCommandText"></param>
        ///// <param name="PParameters"></param>
        ///// <param name="PTransaction"></param>
        ///// <returns></returns>
        //DbCommand MyCreateCommand(DbConnection PConnection, string PCommandText, Dictionary<string, object>? PParameters = null, DbTransaction? PTransaction = null)
        //{
        //    // sample:
        //    //using (DbTransaction mytransaction = CDB.MMemoryDB.BeginTransaction())
        //    //{
        //    //    Dictionary<string, object> Para1 = ZDictionary.ZCreateDictionaryStringObject();
        //    //    Para1.Add("@V1", "1");
        //    //    Para1.Add("@V2", "2");
        //    //    using (DbCommand cmd1 = CDB.MMemoryDB.CreateCommand("INSERT into TKeyValue VALUES (@V1, @V2);", Para1, mytransaction))
        //    //    {
        //    //        for (int i = 0; i < 10; i++)
        //    //        {
        //    //            cmd1.ZSetParameterValue("@V1", "A" + i.ToString());
        //    //            cmd1.ZSetParameterValue("@V2", "B" + i.ToString());
        //    //            cmd1.ZExecuteNonQuery();
        //    //        }
        //    //    }
        //    //    // 只需 Commit 一個 SQLiteTransaction
        //    //    mytransaction.Commit();
        //    //}

        //    DbCommand command = PConnection.CreateCommand();
        //    command.CommandText = PCommandText;
        //    command.CommandTimeout = _CommandTimeout;
        //    command.Transaction = PTransaction;
        //    // 20230427, 從 ZData 中搬移到 ZDbConnection.
        //    // ZSetParameters(command, parameters);
        //    MySetParameters(command, PParameters);
        //    return command;
        //}

        #endregion



        #region Execute
        public int ExecuteNonQuery(string PCmd, Dictionary<string, object>? PParameters = null, DbTransaction? PTransaction = null)
        {
            // Connection 不要 close/dispose connection, 保留給外部決定, 或最後由本 class IDisposable 處理.
            Open();
            using (DbCommand cmd = CreateCommand(PCmd, PParameters, PTransaction))
                //// 20230427, 讓 ZDbConnection 可獨立運作, 從 ZData 搬移到 ZDbConnection 中.
                //return cmd.ZExecuteNonQuery();
                return cmd.ExecuteNonQuery();
        }


        public object? ExecuteScalar(string PCmd, Dictionary<string, object>? PParameters = null, DbTransaction? PTransaction = null)
        {
            // Connection 不要 close/dispose connection, 保留給外部決定, 或最後由本 class IDisposable 處理.
            Open();
            using (DbCommand cmd = CreateCommand(PCmd, PParameters, PTransaction))
                // 20230427, 從 ZData 中搬移到 ZDbConnection.
                //return cmd.ZExecuteScalar();
                return cmd.ExecuteScalar();
        }
        public DbDataReader ExecuteReader(string PCmd, Dictionary<string, object>? PParameters = null, DbTransaction? PTransaction = null)
        {
            // Connection 不要 close/dispose connection, 保留給外部決定, 或最後由本 class IDisposable 處理.
            Open();
            //// 20230427, 從 ZData 中搬移到 ZDbConnection.
            //return CreateCommand(PCmd, PParameters, PTransaction).ZExecuteReader();
            return CreateCommand(PCmd, PParameters, PTransaction).ExecuteReader(CommandBehavior.Default);
        }



        #endregion

        #region 常用 CRUD
        // todo 增加常用組合. 

        /// <summary>
        /// 常用 update {sTable} set {sValueField}=@oValue where {sKeyField}= @oKeyValue.
        /// </summary>
        /// <param name="PTableName"></param>
        /// <param name="PKeyFieldName"></param>
        /// <param name="PKeyFieldValue"></param>
        /// <returns></returns>
        public int ExecuteUpdate(string PTableName, string PKeyFieldName, object PKeyFieldValue, string PValueFieldName, object PValue, DbTransaction? PTransaction = null)
        {
            string sCmd = $"update {PTableName} set {PValueFieldName}=@oValue where {PKeyFieldName}= @oKeyValue";
            Dictionary<string, object> para1 = new Dictionary<string, object>()
            {
                { "@oKeyValue", PKeyFieldValue },
                { "@oValue", PValue }
            };
            return ExecuteNonQuery(sCmd, para1, PTransaction);
        }

        /// <summary>
        /// 常用 delete from {sTable} where {sField}= @oValue
        /// </summary>
        /// <param name="PTableName"></param>
        /// <param name="PFieldName"></param>
        /// <param name="PValue"></param>
        /// <returns></returns>
        public int ExecuteDelete(string PTableName, string PFieldName, object PValue, DbTransaction? PTransaction = null)
        {
            string sCmd = $"delete from {PTableName} where {PFieldName}= @oValue";
            Dictionary<string, object> para1 = new Dictionary<string, object>
            {
                { "@oValue", PValue }
            };
            return ExecuteNonQuery(sCmd, para1, PTransaction);
        }
        #endregion


        /// <summary>
        /// 建立 DbCommand.
        /// DbCommand 使用後應 Dispose.
        /// 本 Class 所有使用 DbCommand 的呼叫, 均由此控制.
        /// 如需效能, 可先取得 DbCommand 後, 再於重複的 SQL 指令中, 只變更 Parameter.value 傳遞參數給資料庫執行.
        /// </summary>
        /// <param name="PCommandText"></param>
        /// <param name="PParameters"></param>
        /// <param name="PTransaction"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public DbCommand CreateCommand(string PCommandText, Dictionary<string, object>? PParameters = null, DbTransaction? PTransaction = null)
        {
            //if (_Connection == null)
            //    throw new NullReferenceException($"{nameof(_Connection)} is null.");
            // 20230427, 從 ZData 中搬移到 ZDbConnection.
            //return _Connection.ZCreateCommand(PCmd, PParameters, _CommandTimeout, PTransaction);


            // sample:
            //using (DbTransaction mytransaction = CDB.MMemoryDB.BeginTransaction())
            //{
            //    Dictionary<string, object> Para1 = ZDictionary.ZCreateDictionaryStringObject();
            //    Para1.Add("@V1", "1");
            //    Para1.Add("@V2", "2");
            //    using (DbCommand cmd1 = CDB.MMemoryDB.CreateCommand("INSERT into TKeyValue VALUES (@V1, @V2);", Para1, mytransaction))
            //    {
            //        for (int i = 0; i < 10; i++)
            //        {
            //            cmd1.ZSetParameterValue("@V1", "A" + i.ToString());
            //            cmd1.ZSetParameterValue("@V2", "B" + i.ToString());
            //            cmd1.ZExecuteNonQuery();
            //        }
            //    }
            //    // 只需 Commit 一個 SQLiteTransaction
            //    mytransaction.Commit();
            //}


            // 取消 MyCreateCommand() 搬到下面
            //return MyCreateCommand(_Connection, PCmd, PParameters, _CommandTimeout, PTransaction);
            //DbCommand command = PConnection.CreateCommand();
            if (_Connection == null)
                throw new NullReferenceException($"{nameof(_Connection)} is null.");
            DbCommand command = _Connection.CreateCommand();
            command.CommandText = PCommandText;
            command.CommandTimeout = _CommandTimeout;
            command.Transaction = PTransaction;
            // 20230427, 從 ZData 中搬移到 ZDbConnection.
            // ZSetParameters(command, parameters);

            // 20230430, 將 MySetParameters 搬來這裡
            //MySetParameters(command, PParameters);
            // Command 建立後, 才可以使用 Parameters 屬性, 不是很方便.
            // 透過本函數可一次建立 Command.Parameters.
            // 建立 Command.Parameters 後, 如需改變 Parameter.Value, 可由 Command.Parameters[Index or Name]變更.

            // Parameters 必須建立在 Command 的 Instance
            // Sample1:
            //int iResult = 0;
            //using (ZMySqlClient db1 = CDB._Me.CreateDBClient())
            //{
            //    string sCmd = "insert into tarea (FID, FBuilding, FFloor, FProhibit, FCreateTime) values (@FID, @FBuilding, @FFloor, @FProhibit, CURRENT_TIMESTAMP)";
            //    Dictionary<string, object> para1 = ZDictionary.ZCreateDictionaryStringObject();
            //    para1.Add("@FID", _ID);
            //    para1.Add("@FBuilding", _sBuilding);
            //    para1.Add("@FFloor", _iFloor);
            //    para1.Add("@FProhibit", _Prohibit ? 1 : 0);
            //    iResult = db1.ExecuteNonQuery(sCmd, para1);
            //    return iResult;
            //}

            // Sample2:
            //Dictionary<string, object> parameters
            //using (ZMySqlClient db1 = CDB._Me.CreateDBClient())
            //{
            //    string sCmd = "select t2.* from ttrace t1, ttracelog t2 where t1.FNo = t2.FNo and t2.FTimeReceive>@FTimeReceive order by t2.FID";
            //    Dictionary<string, object> para1 = ZDictionary.ZCreateDictionaryStringObject();
            //    para1.Add("@FTimeReceive", _TimeReadDB);
            //    DataTable t1 = db1.QueryDataTable(sCmd, para1);
            //    foreach (DataRow row1 in t1.Rows)
            //    {
            //        iResult++;
            //        string sID = row1["FID"].ZToString_Object();
            //        lock (_Lock)
            //        {
            //            CItemTag item1 = GetItem(sID);
            //            if (item1 == null)
            //            {
            //                CLog.Me.Error($"讀取到未註冊的TrackerID ={sID}.");
            //                continue;
            //            }
            //            item1.DBFetchTraceLog(row1);
            //            if (item1.ReceiveTime > dLast)
            //                dLast = item1.ReceiveTime;
            //        }
            //    }
            //}

            // Sample3: Parameter 用 ? 的, 不是用 首字為 "@" 字元
            // Transaction sample:
            //// 只用一個 SQLiteTransaction
            //using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
            //{
            //    using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
            //    {
            //        SQLiteParameter myparam = new SQLiteParameter();
            //        int n;

            //        mycommand.CommandText = "INSERT INTO [MyTable] ([MyId]) VALUES(?)";
            //        mycommand.Parameters.Add(myparam);

            //        // 迴圈中單純新增作業, 不要包含 SQLiteCommand, SQLiteParameter.
            //        for (n = 0; n < 100000; n++)
            //        {
            //            myparam.Value = n + 1;
            //            mycommand.ExecuteNonQuery();
            //        }
            //    }
            //    // 只需 Commit 一個 SQLiteTransaction
            //    mytransaction.Commit();
            //}
            if (PParameters != null)
            {
                foreach (var param in PParameters)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = param.Key;
                    // 20230427, 從 ZData 中搬移到 ZDbConnection.
                    //parameter.Value = param.Value ?? ZObject.ZDBNullObject(); // CodeHelper.
                    parameter.Value = param.Value ?? DBNull.Value; // CodeHelper.
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }

        #region Query...
        /// <summary>
        /// 查詢 DataSet.
        /// </summary>
        /// <param name="PCommandText"></param>
        /// <param name="PParameters"></param>
        /// <param name="PTransaction"></param>
        /// <returns></returns>
        public DataSet QueryDataSet(string PCommandText, Dictionary<string, object>? PParameters = null, DbTransaction? PTransaction = null)
        {
            // Connection 不要 close/dispose connection, 保留給外部決定, 或最後由本 class IDisposable 處理.
            Open();
            using (DbCommand cmd = CreateCommand(PCommandText, PParameters, PTransaction))
            // 20230427, 從 ZData 中搬移到 ZDbConnection, 改為 Private MyQueryDataSet()
            //return cmd.ZQueryDataSet(CreateDbDataAdapter());
            //return MyQueryDataSet(cmd, CreateDbDataAdapter());
            {
                DbDataAdapter adapter1 = CreateDbDataAdapter();
                adapter1.SelectCommand = cmd;
                DataSet ds1 = new DataSet();
                adapter1.Fill(ds1);
                return ds1;
            }
        }

        /// <summary>
        /// 執行 Command.ExecuteScalar(), 並確認結果是否有值 ?
        /// 若查詢結果為 null 或 DBNull.Value, 則無值=false, 否則為有值=true.
        /// </summary>
        /// <param name="PCmd"></param>
        /// <param name="PParameters"></param>
        /// <param name="PTransaction"></param>
        /// <returns></returns>
        public bool QueryExist(string PCmd, Dictionary<string, object>? PParameters = null, DbTransaction? PTransaction = null)
        {
            // Connection 不要 close/dispose connection, 保留給外部決定, 或最後由本 class IDisposable 處理.
            Open();
            using (DbCommand cmd = CreateCommand(PCmd, PParameters, PTransaction))
                return !cmd.ExecuteScalar().ZIsNullOrDBNull();
        }

        /// <summary>
        /// 查詢 DataRow.
        /// 回傳查詢結果 DataSet 中, 第1個 Datatable 中的第1筆 DataRow.
        /// </summary>
        /// <param name="PCmd"></param>
        /// <param name="PParameters"></param>
        /// <param name="PTransaction"></param>
        /// <returns></returns>
        public DataRow? QueryRow(string PCmd, Dictionary<string, object>? PParameters = null, DbTransaction? PTransaction = null)
        {
            // 不要 close/dispose connection, 保留給外部決定, 或最後由本 class IDisposable 處理.
            DataTable? t1 = QueryTable(PCmd, PParameters, PTransaction);
            if (t1 == null)
                return null;
            if (t1.Rows.Count < 1)
                return null;
            return t1.Rows[0];
        }

        /// <summary>
        /// 查詢符合(select * from sTable where sField=oValue)的第1筆DataRow.
        /// 常用於 Primary key 查詢.
        /// </summary>
        /// <param name="PTableName"></param>
        /// <param name="PFieldName"></param>
        /// <param name="PValue"></param>
        /// <param name="PTransaction"></param>
        /// <returns></returns>
        public DataRow? QueryRow(string PTableName, string PFieldName, object PValue, DbTransaction? PTransaction = null)
        {
            // 不要 close/dispose connection, 保留給外部決定, 或最後由本 class IDisposable 處理.
            string sCmd = $"select * from {PTableName} where {PFieldName} = @FPKey";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@FPKey", PValue }
            };
            DataRow? row1 = QueryRow(sCmd, parameters, PTransaction);
            return row1;
        }

        //// 20230427, 從 ZData 中搬移到 ZDbConnection.
        /// <summary>
        /// 執行 Command.ExecuteScalar(), 並將結果轉為 string.
        /// </summary>
        /// <param name="cmd1"></param>
        /// <returns></returns>
        public string? QueryString(DbCommand cmd1)
        {
            // 20230427, 從 ZData 中搬移到 ZDbConnection.
            //return ZExecuteScalar(cmd1).ZToString(null);
            return cmd1.ExecuteScalar().ZToString();
        }


        /// <summary>
        /// 執行 Command.ExecuteScalar(), 並將結果轉為 string.
        /// </summary>
        /// <param name="PCmd"></param>
        /// <param name="PParameters"></param>
        /// <param name="PTransaction"></param>
        /// <returns></returns>
        public string? QueryString(string PCmd, Dictionary<string, object>? PParameters = null, DbTransaction? PTransaction = null)
        {
            // Connection 不要 close/dispose connection, 保留給外部決定, 或最後由本 class IDisposable 處理.
            Open();
            using (DbCommand cmd = CreateCommand(PCmd, PParameters, PTransaction))
                //// 20230427, 從 ZData 中搬移到 ZDbConnection.
                //return cmd.ZQueryString();
                return QueryString(cmd);
        }


        /// <summary>
        /// 查詢 DataTable.
        /// 例如: QueryDataTable("select * from Table1 order by Field1") 可取得 Table1 資料.
        /// 若需要查詢條件, 則應從 parameters 傳入, 以免 SQL injection.
        /// </summary>
        /// <param name="PCmd"></param>
        /// <param name="PParameters"></param>
        /// <param name="Transaction1"></param>
        /// <returns></returns>
        public DataTable? QueryTable(string PCmd, Dictionary<string, object>? PParameters = null, DbTransaction? Transaction1 = null)
        {
            // 不要 close/dispose connection, 保留給外部決定, 或最後由本 class IDisposable 處理.
            DataSet ds1 = QueryDataSet(PCmd, PParameters, Transaction1);
            if (ds1.Tables.Count < 1)
                return null;
            return ds1.Tables[0];
        }


        /// <summary>
        /// 常用 select * from {sTable} where {sField} = @oValue.
        /// </summary>
        /// <param name="PTableName"></param>
        /// <param name="PFieldName"></param>
        /// <param name="PValue"></param>
        /// <param name="PTransaction"></param>
        /// <returns></returns>
        public DataTable QueryTable(string PTableName, string PFieldName, object PValue, DbTransaction? PTransaction = null)
        {
            string sCmd = $"select * from {PTableName} where {PFieldName} = @oValue";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@oValue", PValue }
            };
            // 不要 close/dispose connection, 保留給外部決定, 或最後由本 class IDisposable 處理.
            return QueryDataSet(sCmd, parameters, PTransaction).Tables[0];
        }

        #endregion

        #region Dispose Part
        bool _Disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    if (_Connection != null)
                    {
                        Close();
                        _Connection.Dispose();
                    }
                }
                _Connection = null;
                _Disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        public DbConnection? Get_Connection()
        {
            return _Connection;
        }
        #endregion
        #region Transaction

        // Transaction sample:
        //// 只用一個 SQLiteTransaction
        //using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
        //{
        //    using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
        //    {
        //        SQLiteParameter myparam = new SQLiteParameter();
        //        int n;

        //        mycommand.CommandText = "INSERT INTO [MyTable] ([MyId]) VALUES(?)";
        //        mycommand.Parameters.Add(myparam);

        //        // 迴圈中單純新增作業, 不要包含 SQLiteCommand, SQLiteParameter.
        //        for (n = 0; n < 100000; n++)
        //        {
        //            myparam.Value = n + 1;
        //            mycommand.ExecuteNonQuery();
        //        }
        //    }
        //    // 只需 Commit 一個 SQLiteTransaction
        //    mytransaction.Commit();
        //}

        public DbTransaction? BeginTransaction()
        {
            if (_Connection == null)
                return null;
            return _Connection.BeginTransaction(); // CodeHelper
        }
        public void Commit(DbTransaction PTransaction)
        {
            PTransaction.Commit(); // CodeHelper
        }
        public static void Rollback(DbTransaction PTransaction)
        {
            PTransaction.Rollback(); // CodeHelper
        }
        #endregion

    }
}
