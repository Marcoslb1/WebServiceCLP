using ClpCash.DTO;
using ClpCash.Infraestructure;
using System;
using Dapper;
using System.Text;
using System.Collections.Generic;

namespace ClpCash.Repository
{
    public class BaseRepository
    {
        public static string ConsultaSenhaFilial(CLPCash clp)
        {
            using (var conn = SingleConnection.GetConnection())
            {
                try
                {
                    StringBuilder select = new StringBuilder();

                    select.Append("SELECT " +
                                    " DES_SENHACLP" +
                            " FROM CAS_SENHACLP" +
                            " WHERE cod_regional = @cod_regional and cod_filial = @cod_filial");

                    select.Replace("@cod_regional", clp.Cod_regional.ToString());
                    select.Replace("@cod_filial", clp.Cod_filial.ToString());

                    var senha = conn.Query<CLPCash>(select.ToString());

                    foreach (CLPCash senh in senha) return senh.Des_senhaclp;

                    return null;
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Erro ao consultar senha da filial no banco!!", e);
                }
            }
        }
    }
}
