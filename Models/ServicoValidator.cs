using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace SisAdv.Models
{
    class ServicoValidator : AbstractValidator<Servico>
    {
        public ServicoValidator()
        {
            RuleFor(x => x.Cliente).NotEmpty().WithMessage("O campo `Nome` é Obrigatório. Favor Preencher");
            RuleFor(x => x.Advogado).NotEmpty().WithMessage("O campo `Advogado` é Obrigatório. Favor Preencher");
            RuleFor(x => x.Tipo).NotEmpty().WithMessage("Algum `Tipo de Serviço` é Obrigatório. Favor Preencher");
            RuleFor(x => x.Data).NotEmpty().WithMessage("O campo `Data` é Obrigatório. Favor Preencher");


            //CÓDIGO DE CPF TAMBÉM
            //RuleFor(x => x.).NotEqual("___.___.___-__").WithMessage("O campo `CPF` é obrigatório. Favor Preencher");
            //RuleFor(x => x.CPF).Must(CPFValidator).WithMessage("CPF inválido");
        }

        /* CASO ALGUÉM PRECISE USAR DEIXAREI TODO O CÓDIGO DE CPF
        public bool CPFValidator(string cpf)
        {
            return true;
        }
        */
    }
}
