using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSCLP.Classes
{
    public class CLPCash
    {
        public int Id_ClpCash { get; set; }
        public string Des_ipv4 { get; set; }
        public string des_cofre { get; set; }
        public string des_senha { get; set; }
        public int cod_func { get; set; }
        public int cod_regional { get; set; }
        public int cod_filial { get; set; }
        public int cod_comandoacao { get; set; }
        public string des_comandoacao { get; set; }
        public DateTime Dt_Inclusao { get; set; }
        public DateTime Dt_Alteracao { get; set; }
        public int Flg_Situacao { get; set; }
    }
}