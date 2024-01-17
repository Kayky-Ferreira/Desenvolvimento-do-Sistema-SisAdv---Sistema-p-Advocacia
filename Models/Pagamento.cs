using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisAdv.Models
{
    class Pagamento
    {
        /*
         * id_pagamento
            tipo_pagamento
            data_pagamento
            valor_pagamento
            fk_despesa
            fk_caixa
         * 
         * */

        public int Id { get; set; }
        public DateTime? DataPagamento { get; set; }
        public String TipoPagamento { get; set; }
        public String Origem { get; set; }
        public Despesa Despesa { get; set; }
        public Caixa Caixa { get; set; }
        public double Valor { get; set; }
    }
}
