using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WSCLP.Classes;

namespace WSCLP
{
    // OBSERVAÇÃO: Você pode usar o comando "Renomear" no menu "Refatorar" para alterar o nome da classe "CLPService" no arquivo de código, svc e configuração ao mesmo tempo.
    // OBSERVAÇÃO: Para iniciar o cliente de teste do WCF para testar esse serviço, selecione CLPService.svc ou CLPService.svc.cs no Gerenciador de Soluções e inicie a depuração.
    public class CLPService : ICLPService
    {
        public bool CLP(string id)
        {
            CLPCash clp = new CLPCash();
            ConsultaRegistro(clp, id);

            return true;

        }

        public static void MontagemCommand(CLPCash clp, string id)
        {

            StringBuilder cmdAcao = new StringBuilder();

            cmdAcao.Append("C:\\CybroComServer.exe");

            cmdAcao.Append(" /b=#IPV4");
            cmdAcao.Replace("#IPV4", clp.Des_ipv4);

            cmdAcao.Append(" /readall=#COFRE");
            cmdAcao.Replace("#COFRE", clp.des_cofre);

            cmdAcao.Append(" /p=#SENHA");
            cmdAcao.Replace("#SENHA", clp.des_senha);

            cmdAcao.Append(" #COFRE.");
            cmdAcao.Replace("#COFRE", clp.des_cofre);


            cmdAcao.Append("#COMANDO");
            cmdAcao.Replace("#COMANDO", clp.des_comandoacao);


            if (clp.cod_comandoacao != 3)
            {
                cmdAcao.Append("=#ACAO");
                if (clp.cod_comandoacao == 1)
                    cmdAcao.Replace("#ACAO", "1");
                else if (clp.cod_comandoacao == 0)
                    cmdAcao.Replace("#ACAO", "0");
            }

            ExecutarCMD(cmdAcao.ToString(), clp, id);
        }

        public static void ExecutarCMD(string comando, CLPCash clp, string id)
        {
            using (Process processo = new Process())
            {
                LogUser logUser = new LogUser();

                processo.StartInfo.FileName = Environment.GetEnvironmentVariable("comspec");

                processo.StartInfo.Arguments = string.Format("/c {0}", comando);  //passagem de argumento para cmd

                processo.StartInfo.RedirectStandardOutput = true;
                processo.StartInfo.UseShellExecute = false;
                processo.StartInfo.CreateNoWindow = true;

                processo.Start();
                processo.WaitForExit();

                logUser.Des_logacao = processo.StandardOutput.ReadToEnd();

                InserirLog(logUser, clp, id);

            }
        }

        public static void InserirLog(LogUser logUser, CLPCash clp, string id)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO " +
                            "CAS_LogBox (" +
                                    "Cod_func, " +
                                    "cod_regional," +
                                    "cod_filial, " +
                                    "Des_LogAcao, " +
                                    "cod_comandoAcao," +
                                    "Dt_Inclusao, " +
                                    "Dt_Alteracao, " +
                                    "Flg_Situacao)" +
                                    "\r\nVALUES ");
                sql.Append("(" +

                                clp.cod_func + ", " +
                                clp.cod_regional + ", " +
                                clp.cod_filial + ", " + " '" +
                                logUser.Des_logacao + " '" + ", " +
                                clp.cod_comandoacao + ", convert(datetime,'" +
                                clp.Dt_Inclusao + "',104) , convert(datetime,'" +
                                clp.Dt_Alteracao + "',104) ," +
                                clp.Flg_Situacao);
                sql.Append(")");

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand(sql.ToString(), conn);

                cmd.ExecuteNonQuery();

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Erro ao inserir log de registro na tabela!!", e);
            }

            AlteraFlag(id);
        }

        public static void ConsultaRegistro(CLPCash clp, string id)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                StringBuilder select = new StringBuilder();

                select.Append("SELECT " +
                                " Des_ipv4," +
                                " des_cofre," +
                                " des_senha," +
                                " cod_func," +
                                " cod_regional, " +
                                " cod_filial," +
                                " cod_comandoacao," +
                                " des_comandoacao," +
                                " Dt_Inclusao," +
                                " Dt_Alteracao," +
                                " Flg_Situacao" +
                        " FROM CAS_ClpCash" +
                        " WHERE id_clpCash = @id_clpCash");

                select.Replace("@id_clpCash", id.ToString());

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand(select.ToString(), conn);

                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    clp.Des_ipv4 = dr.IsDBNull(0) ? null : dr.GetString(0);
                    clp.des_cofre = dr.IsDBNull(1) ? null : dr.GetString(1);
                    clp.des_senha = dr.IsDBNull(2) ? null : dr.GetString(2);
                    clp.cod_func = dr.IsDBNull(3) ? 0 : dr.GetInt32(3);
                    clp.cod_regional = dr.IsDBNull(4) ? 0 : dr.GetInt32(4);
                    clp.cod_filial = dr.IsDBNull(5) ? 0 : dr.GetInt32(5);
                    clp.cod_comandoacao = dr.IsDBNull(6) ? 0 : dr.GetInt32(6);
                    clp.des_comandoacao = dr.IsDBNull(7) ? null : dr.GetString(7);
                    clp.Dt_Inclusao = dr.IsDBNull(8) ? DateTime.Now : dr.GetDateTime(8);
                    clp.Dt_Alteracao = dr.IsDBNull(9) ? DateTime.Now : dr.GetDateTime(9);
                    clp.Flg_Situacao = dr.IsDBNull(10) ? 1 : Convert.ToInt32(dr.GetBoolean(10));
                }

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Erro ao consultar registro enviado pelo CASH no banco!!", e);
            }

            MontagemCommand(clp, id);
        }

        public static void AlteraFlag(string id)
        {
            try
            {

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                StringBuilder update = new StringBuilder();

                update.Append("update CAS_ClpCash set Flg_Situacao = 0 where id_clpCash = @id_clpCash");
                update.Replace("@id_clpCash", id.ToString());

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand(update.ToString(), conn);

                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Erro ao alterar flag para deletar registro!!", e);
            }

            DeletaRegistro(id);
        }

        public static void DeletaRegistro(string id)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                StringBuilder delete = new StringBuilder();

                delete.Append("delete from CAS_ClpCash where id_clpCash = @id_clpCash");
                delete.Replace("@id_clpCash", id.ToString());

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand(delete.ToString(), conn);

                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

            }
            catch (Exception e)
            {
                throw new ArgumentException("Erro ao deletar registro!!", e);
            }
        }

    }
}
