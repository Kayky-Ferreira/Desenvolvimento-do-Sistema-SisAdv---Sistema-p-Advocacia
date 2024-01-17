using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace SisAdv.Models
{
    class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator()
        {
            RuleFor(x => x.NomeUser).NotEmpty().WithMessage("O campo `Login` é Obrigatório pois será o seu acesso ao sistema. Favor Preencher");
            RuleFor(x => x.Senha).NotEmpty().WithMessage("O campo `Senha` não foi preenchido ou as Senhas estão diferentes. Verifique e tente novamente.");
            RuleFor(x => x.Advogado).NotEmpty().WithMessage("O campo `Advogado` é Obrigatório. Favor Preencher");
        }
        public UsuarioValidator(int segundavalidacao)
        {
            if (segundavalidacao == 1)
                RuleFor(x => x.Senha).NotEmpty().WithMessage("O campo `Senha` não foi preenchido ou as Senhas estão diferentes. Verifique e tente novamente.");
        }
    }
}
