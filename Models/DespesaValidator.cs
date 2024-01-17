using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace SisAdv.Models
{
    class DespesaValidator : AbstractValidator<Despesa>
    {
        public DespesaValidator()
        {
            RuleFor(x => x.Origem).NotEmpty().WithMessage("O campo `Origem` é Obrigatório. Favor Preencher");
            RuleFor(x => x.Data).NotEmpty().WithMessage("O campo `Data` é Obrigatório. Favor Preencher");
            RuleFor(x => x.FormaPagamento).NotEmpty().WithMessage("Alguma `Forma de Pagamento` deve ser preenchida. Favor Preencher");
            RuleFor(x => x.Valor).NotEmpty().WithMessage("O campo `Valor` é Obrigatório. Favor Preencher");
        }
    }
}
