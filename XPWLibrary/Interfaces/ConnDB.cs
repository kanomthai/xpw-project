using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace XPWLibrary.Interfaces
{
    public class ConnDB
    {
        public DataSet GetFill(string sql)
        {
            DataSet dr = new DataSet();
            try
            {
                OracleConnection conn = new OracleConnection(StaticFunctionData.ConnString);
                switch (conn.State.ToString() != "Open")
                {
                    case true:
                        conn.Open();
                        break;
                }
                OracleDataAdapter da = new OracleDataAdapter(sql, conn);
                da.Fill(dr);
                //conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR ====>");
                Console.WriteLine(sql);
                Console.WriteLine("END ====>");
                //throw ex;
            }
            return dr;
        }

        public int Excute(string sql)
        {
            int i = 0;
            try
            {
                OracleConnection conn = new OracleConnection(StaticFunctionData.ConnString);
                switch (conn.State.ToString() != "Open")
                {
                    case true:
                        conn.Open();
                        break;
                }
                OracleCommand cmd = new OracleCommand(sql, conn);
                i = cmd.ExecuteNonQuery();
                //conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR ====>");
                Console.WriteLine(sql);
                Console.WriteLine("END ====>");
                throw ex;
            }
            return i;
        }

        public bool ExcuteSQL(string sql)
        {
            bool i = true;
            try
            {
                OracleConnection conn = new OracleConnection(StaticFunctionData.ConnString);
                switch (conn.State.ToString() != "Open")
                {
                    case true:
                        conn.Open();
                        break;
                }
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.ExecuteNonQuery();
                //conn.Close();
            }
            catch (Exception ex)
            {
                i = false;
                Console.WriteLine("ERROR ====>");
                Console.WriteLine(sql);
                Console.WriteLine(ex.Message);
                Console.WriteLine("END ====>");
                throw ex;
            }
            return i;
        }

        public void CloseConnectDB()
        {
            OracleConnection conn = new OracleConnection(StaticFunctionData.ConnString);
            conn.Close();
        }
    }
}