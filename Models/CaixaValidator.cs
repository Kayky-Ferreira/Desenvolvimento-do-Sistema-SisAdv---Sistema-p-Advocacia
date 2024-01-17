using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using System.Threading.Tasks;

namespace SisAdv.Models
{
    class CaixaValidator : AbstractValidator<Caixa>
    {
        public CaixaValidator()
        {
            RuleFor(x => x.Mes).NotEmpty().WithMessage("O campo `Mês` é Obrigatório. Favor Preencher");
        }
    }
}
