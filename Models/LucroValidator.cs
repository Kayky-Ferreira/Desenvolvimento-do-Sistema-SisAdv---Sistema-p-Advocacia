using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace SisAdv.Models
{
    class LucroValidator : AbstractValidator<Lucro>
    {
        public LucroValidator()
        {
            RuleFor(x => x.Servico).NotEmpty().WithMessage("Um `Servico` é Obrigatório. Favor Preencher");
            RuleFor(x => x.Caixa).NotEmpty().WithMessage("Um `Caixa` é Obrigatório. Favor Preencher");
            RuleFor(x => x.Origem).NotEmpty().WithMessage("O campo `Origem` é Obrigatório. Favor Preencher");
            RuleFor(x => x.Data).NotEmpty().WithMessage("O campo `Data` é Obrigatório. Favor Preencher");
            RuleFor(x => x.FormaRecebimento).NotEmpty().WithMessage("Alguma `Forma de Pagamento` deve ser preenchida. Favor Preencher");
        }
    }
}
