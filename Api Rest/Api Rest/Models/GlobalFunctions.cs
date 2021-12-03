using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Api_Rest.Models
{
    public class GlobalFunctions
    {
        public Boolean YaLeyoReglas = false;
        public DataTable DtReglas;
        // GET: api/GlobalFunctions
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public decimal ValD(object ValorTexto)
        {
            decimal Va;
            try
            {
                try
                {
                    decimal.TryParse(ValorTexto.ToString(), System.Globalization.NumberStyles.Any, null, out Va);
                }
                catch (Exception ex)
                {
                    Va = (decimal)Convert.ToDecimal(ValorTexto.ToString());
                }
                return Va;
            }
            catch (Exception ex)
            {
                Va = 0;
                return Va;
            }
        }

        public string ReglaDeNegocio(int Numero_Empresa, int ReglaNumero, string LLave = "", string ValorDefault = "")
        {
            try
            {

                string rs;
                ConexionBD conn = new ConexionBD();
                conn.AbrirTransaccion();
                rs = (string) conn.Ejecutar("SELECT respuesta FROM dbo.reglas_emp WHERE id_emp = " + Numero_Empresa.ToString() + " AND id_reglas = " + ReglaNumero.ToString() + " AND llave = '" + LLave.ToString() + "'");
                conn.CerrarTransaccion();
                if (rs =="")
                {
                    return ValorDefault;
                }
                else
                {
                    return rs.ToString();
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public DataTable Filtrar_DataTable(DataTable dt, string filter, string sort = "")
        {
            DataRow[] rows;
            DataTable dtNew;
            dtNew = dt.Clone();
            rows = dt.Select(filter, sort);
            foreach (DataRow dr in rows)
                dtNew.ImportRow(dr);
            return dtNew;
        }


        public DataTable nameTable(DataTable dt, string nombre)
        {
            DataTable mydt = new DataTable();
            mydt = dt.Copy();
            mydt.TableName = nombre;
            return mydt;
        }

        public string DataSetToJSONWithJSONNet(DataSet Ds)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(Ds, Formatting.Indented);
            return JSONString;
        }

        public object Gdf(DataTable Dt, string NombreCampo)
        {
            return Dt.Rows[0][NombreCampo];
        }


        public bool Fin(DataTable Dt)
        {
            if (Dt is null)
            {
                return true;
            }
            else if (Dt.Rows.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}