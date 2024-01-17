using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace SisAdv.Models
{
    class PagamentoValidator : AbstractValidator<Pagamento>
    {
        public PagamentoValidator()
        {
            RuleFor(x => x.DataPagamento).NotEmpty().WithMessage("A `Data` é Obrigatório. Favor Preencher");
            RuleFor(x => x.Despesa).NotEmpty().WithMessage("Uma `Despesa` deve ser adicionada. Favor Preencher");
            RuleFor(x => x.Caixa).NotEmpty().WithMessage("Um 'Caixa' deve ser adicionado. Favor Preencher");
            RuleFor(x => x.TipoPagamento).NotEmpty().WithMessage("O campo `Tipo de Pagamento` é Obrigatório e importante. Favor Preencher");
        }
    }
}
