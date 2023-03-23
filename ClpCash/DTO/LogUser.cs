using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClpCash.DTO
{
    public class LogUser
    {
        public int Cod_func { get; set; }
        public int Cod_regional { get; set; }
        public int Cod_filial { get; set; }
        public string Des_logacao { get; set; }
        public int Cod_comandoAcao { get; set; }
        public DateTime Dt_inclusao { get; set; }
        public DateTime Dt_alteracao { get; set; }
        public bool Flg_situacao { get; set; }
    }
}