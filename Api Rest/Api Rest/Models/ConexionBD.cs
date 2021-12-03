using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Api_Rest.Models
{
    public class ConexionBD
    {
        public static SqlConnection SqlCon;
        public static SqlCommand SqlCmd;
        public static SqlDataAdapter SqlDa;
        public static DataSet Ds;
        public static SqlTransaction Transaccion;


        #region "Métodos y funciones de conexión y acceso a datos"

        public ConexionBD()
        {
            SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["CadenaConexion"].ConnectionString);
            SqlCon.Open();

        }

        public ConexionBD(string CadenaConexion1)
        {
            SqlCon = new SqlConnection(CadenaConexion1);
            SqlCon.Open();
        }

        public void Execute(string Query)
        {
            SqlCmd = new SqlCommand
            {
                Connection = SqlCon,
                CommandText = Query,
                Transaction = Transaccion,
                CommandType = CommandType.Text
            };
            SqlCmd.ExecuteNonQuery();
        }


        public object Ejecutar(string Query)
        {
            SqlCmd = new SqlCommand();
            SqlDa = new SqlDataAdapter();
            DataSet Ds = new DataSet();

            SqlCmd.Connection = SqlCon;
            SqlCmd.CommandText = Query;
            SqlCmd.Transaction = Transaccion;
            SqlCmd.CommandTimeout = 50000;
            SqlCmd.CommandType = CommandType.Text;

            SqlDa.SelectCommand = SqlCmd;
            SqlDa.Fill(Ds);
            if (Ds.Tables.Count == 1)
            {
                if (Ds.Tables[0].Columns.Count == 1)
                {
                    try
                    {
                        return Ds.Tables[0].Rows[0][0].ToString();
                    }
                     catch
                    {
                        return "";
                    }
                    
                    
                } 
                else
                {
                    return Ds.Tables[0];
                }
                
            }
            else
            {
                return Ds;
            }
        }

        public object Ejecutar(object[] Parametros, string Procedimiento)
        {

            int P;
            SqlCmd = new SqlCommand();
            SqlDa = new SqlDataAdapter();
            DataSet Ds = new DataSet();

            SqlCmd.Connection = SqlCon;
            SqlCmd.CommandText = Procedimiento;
            SqlCmd.Transaction = Transaccion;
            SqlCmd.CommandTimeout = 0;
            SqlCmd.CommandType = CommandType.StoredProcedure;

            for (P = 0; P < Parametros.Length; P += 2)
            {
                SqlCmd.Parameters.Add(new SqlParameter("@" + Parametros[P], Parametros[P + 1]));
            }
            SqlDa.SelectCommand = SqlCmd;
            SqlDa.Fill(Ds);
            if (Ds.Tables.Count == 1)
            {
                return Ds.Tables[0];
            }
            else
            {
                return Ds;
            }
        }
        public void AbrirTransaccion()
        {
            Transaccion = SqlCon.BeginTransaction();
        }

        public void CerrarTransaccion()
        {
            Transaccion.Commit();
            Transaccion = null;
        }

        public void DevolerTransaccion()
        {
            Transaccion.Rollback();
        }

        public void Close()
        {
            if (SqlCon != null)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

        }
        public static bool VerificarConexion()
        {
            if (SqlCon.State == ConnectionState.Open)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

    }
}