using System;

namespace ClpCash.DTO
{
    public class CLPCash
    {
        public int Id_ClpCash { get; set; }
        public string Des_ipv4 { get; set; }
        public string Des_cofre { get; set; }
        public int Cod_func { get; set; }
        public int Cod_regional { get; set; }
        public int Cod_filial { get; set; }
        public int Cod_comandoacao { get; set; }
        public string Nom_tecnico { get; set; }
        public DateTime Dt_Inclusao { get; set; }
        public DateTime Dt_Alteracao { get; set; }
        public int Flg_Situacao { get; set; }
        public int Flg_Status { get; set; }
        public string Tp_comandoAcao { get; set; }
        public string Des_senhaclp { get; set; }

        public CLPCash(string des_senhaclp)
        {
            this.Des_senhaclp = des_senhaclp;
        }

        public CLPCash(string des_ipv4, string des_cofre, int cod_func, int cod_regional, int cod_filial, DateTime dt_inclusao, DateTime dt_alteracao, int flg_situacao, int flg_status, string tp_comandoAcao, string nom_tecnico, int id_clpcash)
        {
            Des_ipv4 = des_ipv4;
            Des_cofre = des_cofre;
            Cod_func = cod_func;
            Cod_regional = cod_regional;
            Cod_filial = cod_filial;
            Dt_Inclusao = dt_inclusao;
            Dt_Alteracao = dt_alteracao;
            Flg_Situacao = flg_situacao;
            Flg_Status = flg_status;
            Tp_comandoAcao = tp_comandoAcao;
            Id_ClpCash = id_clpcash;

        }

        public CLPCash() { }
    }
}