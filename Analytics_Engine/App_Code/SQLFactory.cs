using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

//namespace GeNaNBANew.DbConn
//{
public class SQLFactory
{


    public SqlConnection getConnection()
    {
        //string connString = "Data Source=SUBIRRANJAN\\SQLEXPRESS1;Initial Catalog=GeNaNBANew;Integrated Security=True";
        string connString = "Data Source=dell;Initial Catalog=GeNaNBANew;Integrated Security=True";

        
        SqlConnection conn = new SqlConnection(connString);
        conn.Open();
        return conn;

    }


}
//}