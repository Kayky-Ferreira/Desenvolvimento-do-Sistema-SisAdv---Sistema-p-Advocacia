#Grupo: Equipe Rocket - Sistema SISADV

#Alunos: Hellen Moronari, Kayky Ferreira, Lucas Henrique, Mateus Enrique, Vinicius Albuquerque, João Rafal, Bruno Bedelegue

#Requisitos Funcionais:

# Cadastrar Cliente, Cadastrar Advogado, Cadastrar Usuário, Cadastrar evento, Cadastrar Lucro/Despesa, Cadastrar Serviço, Acessar Diário da Justiça, 

#Banco de Dados:
 
#drop database db_sistema_sisadv;
create database db_sistema_sisadv;
use db_sistema_sisadv;

create table endereco(
id_endereco int primary key auto_increment,
rua_end varchar (70),
bairro_end varchar (50),
cidade_end varchar (50),
numero_residencia_end varchar (5)
);

ALTER TABLE endereco ADD COLUMN estado varchar (40);
ALTER TABLE endereco MODIFY COLUMN numero_residencia_end int;
desc endereco;

create table cliente(
id_cliente int primary key auto_increment,
nome_cli varchar (100) not null,
cpf_cli varchar (11) not null,
rg_cli varchar (10) not null,
telefone_cli varchar (20) not null,
profissao_cli varchar (50),
descricao_cli varchar (300),
fk_endereco int,
foreign key (fk_endereco) references endereco(id_endereco)
);

ALTER TABLE cliente ADD COLUMN email_cli VARCHAR(120);
ALTER TABLE cliente MODIFY COLUMN cpf_cli varchar (20);
DESC cliente;

create table evento(
id_evento int primary key auto_increment,
titulo_even varchar (50),
data_even date,
horario_even varchar (10),
descricao_even varchar (200),
importancia_even varchar (10),
notificacao_even boolean
);

create table advogado(
id_advogado int primary key auto_increment,
nome_adv varchar (100),
cpf_adv varchar (20),
rg_adv varchar (10),
data_nasc_adv date,
e_mail_adv varchar (150),
telefone_adv varchar (20),
descricao_adv varchar (200)
);

create table usuario(
id_user int primary key auto_increment,
senha_user varchar (40),
nome_user varchar (100),
fk_advogado int,
foreign key (fk_advogado) references advogado (id_advogado)
);

create table servico(
id_servico int primary key auto_increment,
cliente_serv varchar (100),
advogado_serv varchar (100),
valor_serv double,
data_serv date,
tipo_serv varchar (50),
descricao_serv varchar (200),
fk_advogado int,
fk_cliente int,
foreign key (fk_cliente) references cliente (id_cliente),
foreign key (fk_advogado) references advogado (id_advogado)
);

create table login(
id_login int primary key auto_increment,
data_log date,
horario_log varchar (5),
fk_user int,
foreign key (fk_user) references usuario (id_user)
);

create table processo(
id_proc int primary key auto_increment, 
valor_proc double,
descricao_proc varchar (150),
data_inicio_proc date,
status_proc varchar (100),
resultado varchar (100),
cliente_proc varchar (100),
fk_servico int,
fk_cliente int,
fk_advogado int,
foreign key (fk_cliente) references cliente (id_cliente),
foreign key (fk_advogado) references advogado (id_advogado),
foreign key (fk_servico) references servico (id_servico)
);

create table diarioJustica(
id_busca_diar int primary key auto_increment,
numero_edicao_diar int,
ano_edicao_diar int
);

create table caixa(
id_cx int primary key auto_increment,
mes_cx varchar (100),
saldo_inicial_cx double,
total_lucro_cx double,
total_despesa_cx double,
saldo_final_cx double
);

insert into caixa values (null,'Agosto/2021', 1000, 2000, 1000, 2000);

create table lucro(
id_lucro int primary key auto_increment,
valor_luc double,
data_luc date,
origem_luc varchar (100),
descricao_luc varchar (200),
forma_pagamento varchar (30),
mensal_luc bool,
fk_caixa int,
fk_servico int,
foreign key (fk_caixa) references caixa (id_cx),
foreign key (fk_servico) references servico (id_servico)
);

create table despesa(
id_despesa int primary key auto_increment,
data_desp date,
valor_desp double,
origem_desp varchar (100),
descricao_desp varchar (200),
mensal_desp bool
);

ALTER TABLE despesa ADD forma_pagamento VARCHAR (30);

create table pagamento(
id_pagamento int primary key auto_increment,
tipo_pagamento varchar (50),
data_pagamento date,
valor_pagamento double,
fk_despesa int,
fk_caixa int,
foreign key (fk_despesa) references despesa (id_despesa),
foreign key (fk_caixa) references caixa (id_cx)
);

#Procedimentos e Gatilhos

/*Para cada procedimento criado você deverá implementar no mínimo DUAS regras de negócio como por exemplo "verificar se já existe registro com o 
mesmo nome" ou "verificar se uma FK realmente existe" ou outro exemplo utilizado durante as atividades práticas da disciplina de BD II. Você também 
poderá criar um gatilho no lugar de uma regra de negócio, caso seja o caso.*/

#Tabela endereço--------------------------------------------------------------------------------------------------------------------------------------

delimiter $$
create procedure inserirEndereco(rua varchar(100), bairro varchar(50), cidade varchar(70), numero int, estado varchar (30))
begin
insert into endereco values (null, rua, bairro, cidade, numero, estado);
select 'O endereço foi cadastrado com sucesso' as Confirmacao;
end;
$$ delimiter ;
#drop procedure inserirendereco;
/*call inserirEndereco ('K5', 'Nova Brasília', 'Ji-Paraná', 2055, 1);
call inserirEndereco ('K5', 'Nova Brasília', 'Ji-Paraná', '2054', 1);
call inserirEndereco ('Avenida Monte Castelo', '2 de Abril', 'Ji-Paraná', '2050');
call inserirEndereco ('Rua dos Clientes', '2 de Abril', 'Ji-Paraná', '1360');
select * from endereco;*/
desc endereco;

#Tabela cliente--------------------------------------------------------------------------------------------------------------------------------------

delimiter $$
create procedure inserirCliente(nome varchar(100), email varchar (120), cpf varchar (20), rg varchar (10), telefone varchar(20), profissao varchar (50), descricao varchar(200), endereco int)
begin
declare verificacaocpf varchar (11);
declare verificacaorg varchar (10);
set verificacaocpf = (select cpf_cli from cliente where cpf = cpf_cli);
set verificacaorg = (select rg_cli from cliente where rg = rg_cli);
if (nome <> '') then
	if (verificacaocpf is null) then
		if (verificacaorg is null) then
			insert into cliente values (null, nome, cpf, rg, telefone, profissao, descricao, endereco, email);
			select 'Cliente inserido com sucesso' as Confirmacao;	
		else
			select 'Esse rg já foi cadastrado, portanto, esse cliente já foi cadastrado!' as Alerta;
		end if;
	else
		select 'Esse cpf já foi cadastrado!' as Alerta;
	end if;
else
	select 'É necessário inserir o nome do cliente, refaça a ação!' as Alerta;
end if;
end;
$$ delimiter ;
#drop procedure inserirCliente;
/*call inserirCliente ('Douglas Costa', 'emaildouglas@gmail.com', '808423', '803928', '69912345671', 'vendedor', 'Busca processar sua empresa por danos morais', 3);
call inserirCliente ('Maycon Douglas', '012340125', '46540125', '69912345671', 'vendedor', 'Busca processar sua empresa por danos morais', 3);
call inserirCliente ('Raça Nega', '01246445525', '49840125', '6991254541', 'cantor', 'Busca processar sua agencia', 3);
call inserirCliente ('Turma do Pagode', '01245259999', '4425', '69943541', 'cantor', 'Busca p', 3);
call inserirCliente ('Exalta Samba', '0124644', '4353455', '69544541', 'cantor', 'Busca processaa', 3);*/
select * from cliente;
select * from endereco;
desc cliente;



#Tabela evento--------------------------------------------------------------------------------------------------------------------------------------

delimiter $$
create procedure inserirEvento(titulo varchar (50), data date, horario varchar (10), descricao varchar (150), importancia varchar(10), notificacao boolean)
begin
declare verificartitulodata int;
set verificartitulodata = (select id_evento from evento where (titulo = titulo_even) and (data_even = data));
if(verificartitulodata is null) then
	insert into evento values(null, titulo, data, horario, descricao, importancia, notificacao);
	select 'Evento cadastrado com sucesso.' as Confirmacao;
else
	select 'O evento não pode ser inserido novamente pois já foi cadastrado' as Alerta;
end if;
end;
$$ delimiter ;

/*call inserirEvento ('Audiência com cliente Raça Negra', '2021-08-31', '14:00', 'Audiência, levar documentos', 'Alta', true);
call inserirEvento ('Audiência com cliente Raça N', '2021-08-31', '14:00 PM', 'Audiência, levar documentos', 'baixa', true);*/
desc evento;
select * from evento;



#Tabela advogado--------------------------------------------------------------------------------------------------------------------------------------

delimiter $$
create procedure inserirAdvogado(nome varchar (100), cpf varchar (20), rg varchar (20), data_nascimento date, email varchar (100), telefone varchar (20), descricao varchar (100))
begin
declare verificacaocpf varchar (11);
declare verificacaorg varchar (10);
set verificacaocpf = (select cpf_adv from advogado where cpf = cpf_adv);
set verificacaorg = (select rg_adv from advogado where rg = rg_adv);
if (nome <> '') then
	if (verificacaocpf is null) then
		if (verificacaorg is null) then
			insert into advogado values (null, nome, cpf, rg, data_nascimento, email, telefone, descricao);
			select 'Advogado inserido com sucesso' as Confirmacao;	
		else
			select 'Esse rg já foi cadastrado, portanto, esse advogado já foi cadastrado!' as Alerta;
		end if;
	else
		select 'Esse cpf já foi cadastrado!' as Alerta;
	end if;
else
	select 'É necessário inserir o nome do advogado, refaça a ação!' as Alerta;
end if;
end;
$$ delimiter ;

/*call inserirAdvogado ('Sr.Delais', '04564564654', '0214545', '1990-05-04', 'srdelais@gmail.com', '694564231554', 'Advogado desde 2005');
call inserirAdvogado ('Sr.Jackson', '04565645646', '5456465', '1995-09-14', 'Jackson@gmail.com', '694564231554', 'Advogado desde 2010');
call inserirAdvogado ('Teste Sem Usuario', '04565645634', '5456455', '1345-09-14', '894@gmail.com', '694587954231554', 'Advogado desde 2015');
call inserirAdvogado ('Teste DOIS', '0456545634', '55456465', '1345-09-14', '222222@gmail.com', '694587955451554', 'Advogado desde 2015');*/
select * from advogado;
DESC ADVOGADO;
#delete from advogado where id_advogado > 0;


#Tabela usuário--------------------------------------------------------------------------------------------------------------------------------------
desc usuario;
delimiter $$
create procedure inserirUsuario(senha varchar (40), nome varchar(100), advogado int)
begin
declare verificacaologin varchar (100);
declare verificacaoadvogado int;

set verificacaologin = (select nome_user from usuario where (nome_user = nome));
set verificacaoadvogado = (select fk_advogado from usuario where (fk_advogado = advogado));

if (verificacaologin is null) then
	if (verificacaoadvogado is null) then
		insert into usuario values (null, senha, nome, advogado);
		select 'Usuário cadastrado com sucesso!' as Confirmacao;
	else 
		select 'Esse advogado já foi cadastrado! Altere e tente novamente.' as Alerta;
    end if;
else
	select 'Esse nome de usuário já está sendo utilizado por um outro usuário! Altere e tente novamente.' as Alerta;
end if;
end;
$$ delimiter ;
#drop procedure inserirusuario;
/*call inserirUsuario ('testesenha', 'Vinicius Teste', 4);
call inserirUsuario ('123', 'srde', 1);
#call inserirUsuario ('435345', 'Lucas Teste', 3);
#call inserirUsuario ('435345', 'Professor JACKSON', 2);
#CALL inserirUsuario ('12345', 'Lucas teste', 2);*/
select * from advogado;
select * from usuario;
desc servico;

#Tabela servico--------------------------------------------------------------------------------------------------------------------------------------

delimiter $$
create procedure inserirServico(valor double, data date, tipo varchar (100), advogado int, cliente int, descricao varchar (200))
begin
declare testeadvogado int;
declare testecliente int;
declare verificarjaexistente int;
declare buscarnomecliente varchar (100);
declare buscarnomeadvogado varchar (100);

set buscarnomecliente = (select nome_cli from cliente where cliente = id_cliente);
set buscarnomeadvogado = (select nome_adv from advogado where advogado = id_advogado);

set testeadvogado = (select id_advogado from advogado where id_advogado = advogado);
set testecliente = (select id_cliente from cliente where id_cliente = cliente);

if (testeadvogado is not null) then
	if (testecliente is not null) then
		insert into servico values (null, buscarnomecliente, buscarnomeadvogado, valor, data, tipo, descricao, advogado, cliente);
		select 'Serviço cadastrado no sistema.' as Confirmacao;
	else
		select 'Falta a escolha de um cliente para este serviço, ou o valor inserido não é valido!' as Alerta;
	end if;
else
	select 'O Advogado precisa ser inserido no serviço, ou o valor inserido não é valido!' as Alerta;
end if;
end;
$$ delimiter ;
#drop procedure inserirServico;
/*call inserirServico (1000, '2021-08-31', 'Civil', 1, 2, 'Buscando se defender');
call inserirServico (5000, '2021-08-20', 'Administrativo', 2, 2, 'Buscando se defender');
call inserirServico (3410, '2021-05-20', 'Eleitoral', 2, 1, 'Buscando se defender');*/
select * from servico;
select * from usuario;
select * from cliente;
select * from advogado;
desc servico;


#Tabela login--------------------------------------------------------------------------------------------------------------------------------------

delimiter $$
create procedure inserirLogin(data date, horario varchar (10), usuario int)
begin
declare validarfk int;
set validarfk = (select id_user from usuario where id_user = usuario);
if (validarfk is not null) then
	if (data is not null) then
		if (horario is not null) then
			insert into login values (null, data, horario, usuario);
            select 'As informações de login foram registradas na base de dados!' as Confirmacao;
        else
			select 'O horário não foi inserido, tente novamente!' as Alerta;
		end if;
    else
		select 'A data não foi inserida, tente novamente!' as Alerta;
	end if;
else
	select 'Não foi inserido o usuário, tente novmente!' as Alerta;
end if;
end;
$$ delimiter ;

/*call inserirLogin('2021-05-03', '14:00', 1);
call inserirLogin('2021-05-03', '1500', 2);*/
select * from login;



#Tabela processo--------------------------------------------------------------------------------------------------------------------------------------

delimiter $$ 
create procedure registrarProcesso(descricao varchar (150), datadeinicio date, status varchar (100), resultado varchar (100), servico int)
begin
declare nomecliente varchar (100);
declare idCliente varchar (100);
declare idAdvogado varchar (100);
declare valor double;
declare verificarServico int;
set nomecliente = (select cliente_serv from servico where id_servico = servico);
set idCliente = (select fk_cliente from servico where id_servico = servico);
set idAdvogado = (select fk_advogado from servico where id_servico = servico);
set valor = (select valor_serv from servico where id_servico = servico);
if (descricao <> "") then
	if (servico <> 0) then
		insert into processo values(null, valor, descricao, datadeinicio, status, resultado, nomecliente, servico, idCliente, idAdvogado);
		select 'Processo registrado no sistema!' as Confirmacao;
	else
		select 'O Serviço não pode ser nulo, tente novamente.' as Alerta;
	end if;
else
	select 'Insira uma descrição para o processo!' as Alerta;
end if;
end;
$$ delimiter ;

/*call registrarProcesso('Processo devido a danos morais', '2021-03-05','em andamento', 'sem resultado', 1);
call registrarProcesso('Processo devido a má administração', '2021-06-30','em andamento', 'sem resultado', 2);*/
select * from processo;
select * from servico;
select * from cliente;
select * from advogado;
desc processo;

#Tabela Diário--------------------------------------------------------------------------------------------------------------------------------------

delimiter $$
create procedure registrarDiario(numeroedicao int, anoedicao int)
begin
if (anoedicao >= 1970 and anoedicao is not null) then
	if (numeroedicao is not null and numeroedicao > 0) then
		insert into diariojustica values (null, numeroedicao, anoedicao);
        select 'Acesso registrado com sucesso' as Confirmacao;
	else
		select 'O número da edição não foi preenchido, tente novamente.' as Alerta;
	end if;
else
	select 'O ano inserido precisa ser após ou igual ao ano de 1970, tente novamente.' as Alerta;
end if;
end;
$$ delimiter ;

/*call registrarDiario (1, 2000);
call registrarDiario (3, 1970);*/
select * from diariojustica;




#Tabela caixa--------------------------------------------------------------------------------------------------------------------------------------

delimiter $$
create procedure cadastrarCaixa(mes varchar (100))
begin
declare teste varchar (100);
declare saldoanterior double;
SET teste = (SELECT mes_cx FROM caixa WHERE (mes_cx = mes));
if (teste is null) then
	set saldoanterior = (select saldo_final_cx from caixa where id_cx = (select max(id_cx) from caixa));
    insert into caixa values (null, mes, saldoanterior, 0, 0, saldoanterior);
    select concat('Caixa de ', mes, ' cadastrado com sucesso!') as Confirmacao;
else
	select 'Esse mês já foi cadastrado em outro caixa, tente novamente.' as Alerta;
end if;
end;
$$ delimiter ;

/*call cadastrarCaixa('Setembro/2021');*/

delimiter $$
create procedure cadastrarCaixaInicial(mes varchar (100), saldoinicial double)
begin
declare teste varchar (100);
SET teste = (SELECT mes_cx FROM caixa WHERE (mes_cx = mes));
if (teste is null) then
    insert into caixa values (null, mes, saldoinicial, 0, 0, 0);
    select concat('Caixa de ', mes, ' cadastrado com sucesso!') as Confirmacao;
else
	select 'Esse mês já foi cadastrado em outro caixa, tente novamente.' as Alerta;
end if;
end;
$$ delimiter ;
desc caixa;
#call cadastrarcaixainicial('Teste Caixa', 1500);
select * from caixa; 


#Tabela Lucro--------------------------------------------------------------------------------------------------------------------------------------

delimiter $$
create trigger atualizarLucroInsert after insert on lucro
for each row
begin
update caixa set total_lucro_cx = total_lucro_cx + new.valor_luc where (id_cx = new.fk_caixa);
update caixa set saldo_final_cx = saldo_inicial_cx + total_lucro_cx - total_despesa_cx where (id_cx = new.fk_caixa);
end;
$$ delimiter ;
call cadastrarLucro ('2021-02-24', 'Raça Negra', 'Processo', 'A vista no dinheiro', false, 6, 4);
select * from servico;
select * from caixa;
select * from lucro;
update caixa set total_lucro_cx = 0 where id_cx = 6;

delimiter $$
create trigger atualizarLucroUpdate after update on lucro
for each row
begin
update caixa set total_lucro_cx = total_lucro_cx + new.valor_luc where (id_cx = new.fk_caixa);
update caixa set total_lucro_cx = total_lucro_cx - old.valor_luc where (id_cx = new.fk_caixa);
update caixa set saldo_final_cx = saldo_inicial_cx + total_lucro_cx - total_despesa_cx where (id_cx = new.fk_caixa);
end;
$$ delimiter ;

/*update lucro set valor_luc = 2000 where id_lucro= 1;
select * from lucro;
select * from caixa;*/


delimiter $$
create trigger atualizarLucroDelete after delete on lucro
for each row
begin
update caixa set total_lucro_cx = total_lucro_cx - old.valor_luc where (id_cx = old.fk_caixa);
update caixa set saldo_final_cx = saldo_inicial_cx + total_lucro_cx - total_despesa_cx where (id_cx = old.fk_caixa);

end;
$$ delimiter ;

/*delete from lucro where id_lucro = 1;
select * from lucro;
select * from caixa;*/


delimiter $$
create procedure cadastrarLucro(data date, origem varchar (100), descricao varchar (200), formadepagamento varchar (30), mensal boolean, caixa int, servico int)
begin
declare buscarvalorprocesso double;
declare verificacao int;
set buscarvalorprocesso = (select valor_serv from servico where servico = id_servico);
set verificacao = (select fk_servico from lucro where fk_servico = servico);
if (Origem <> "") then
		if (verificacao is null) then
			insert into lucro values (null, buscarvalorprocesso, data, origem, descricao, formadepagamento, mensal, caixa, servico);
			select 'Lucro cadastrado no sistema com sucesso.' as Confirmacao;
        else
			select 'Esse serviço já deu origem a um outro lucro. Não será registrado!' as Alerta;
        end if;
else
	select 'A origem precisa ser especificada, tente novamente.' as Alerta; 
end if;
end;
$$ delimiter ;

/*call cadastrarLucro ('2021-02-24', 'Maycon Douglas', 'Processo', 'A vista no dinheiro', false, 2, 2);
call cadastrarLucro ('2021-02-24', 'Raça Negra', 'Processo', 'A vista no dinheiro', false, 3, 1);*/
select * from lucro;
select * from servico;
select * from caixa;
desc lucro;


#Tabela Despesa--------------------------------------------------------------------------------------------------------------------------------------

delimiter $$
create procedure inserirDespesa(data date, valor double, origem varchar (100), descricao varchar (200), mensal boolean, formapagamento varchar (30))
begin
if(valor > 0) then
	insert into despesa values (null, data, valor, origem, descricao, mensal, formapagamento);
    select 'Despesa inserida com sucesso' as Confirmacao;
else
	select 'O valor inserido precisa ser maior que 0.00R$' as Alerta;
end if;
end;
$$ delimiter ;

/*call inserirDespesa('2020-09-10', 200, 'Conta de luz', 'Conta do escritório', false, null);
call inserirDespesa('2020-09-25', 50, 'Conta de Água', 'Conta do escritório', false, null);*/
select * from despesa;



#Tabela pagamento--------------------------------------------------------------------------------------------------------------------------------------

delimiter $$
create trigger atualizarPagamentoInsert after insert on pagamento
for each row
begin
update caixa set total_despesa_cx = total_despesa_cx + new.valor_pagamento where (id_cx = new.fk_caixa);
update caixa set saldo_final_cx = saldo_inicial_cx + total_lucro_cx - total_despesa_cx where (id_cx = new.fk_caixa);
end;
$$ delimiter ;

select * from pagamento;
select * from caixa;
select * from despesa;

/*FICOU FALTANDO UPDATE PAGAMENTO
delimiter $$
create trigger atualizarPagamentoUpdate after update on pagamento
for each row
begin
update caixa set total_despesa_cx = total_despesa_cx + new.valor_pagamento where (id_cx = new.fk_caixa);
update caixa set total_despesa_cx = total_despesa_cx - old.valor_pagamento where (id_cx = new.fk_caixa);
update caixa set saldo_final_cx = saldo_inicial_cx + total_lucro_cx - total_despesa_cx where (id_cx = new.fk_caixa);
end;
$$ delimiter ;*/

/*update pagamento set valor_pagamento = 100 where id_pagamento = 3;
select * from caixa;*/


delimiter $$
create trigger atualizarDespesaInsertDelete after delete on pagamento
for each row
begin
update caixa set total_despesa_cx = total_despesa_cx - old.valor_pagamento where (id_cx = old.fk_caixa);
update caixa set saldo_final_cx = saldo_inicial_cx + total_lucro_cx - total_despesa_cx where (id_cx = old.fk_caixa);

end;
$$ delimiter ;

/*delete from pagamento where id_pagamento = 2;
select * from pagamento;
select * from caixa;*/

ALTER TABLE pagamento ADD origem_despesa VARCHAR (150);
desc pagamento;
delimiter $$
create procedure registrarPagamento(tipo varchar (50), data date, despesa int, caixa int)
begin
declare testedespesa int;
declare testecx int;
declare verificarpagamentos int;
declare valorpagamento double;
declare origem VARCHAR (150);
set origem = (select origem_desp from despesa where id_despesa = despesa);
set verificarpagamentos = (select id_pagamento from pagamento where (data = data_pagamento) and (fk_despesa = despesa) or (fk_despesa = despesa));
set testedespesa = (select id_despesa from despesa where despesa = id_despesa);
set testecx = (select id_cx from caixa where caixa = id_cx);
set valorpagamento = (select valor_desp from despesa where despesa = id_despesa);
if (testecx is not null) then
	if(testedespesa is not null) then
		if(verificarpagamentos is null) then
			insert into pagamento values (null, tipo, data, valorpagamento, despesa, caixa, origem);
			select 'Pagamento realizado com sucesso' as Confirmacao;
		else
			select 'Essa despesa já foi paga!' as Alerta;
		end if;
	else
		select 'Essa despesa informada não existe, o pagamento não será efetuado.' as Alerta; 
	end if;
else
	select 'O caixa informado não existe, o pagamento não será efetuado.' as Alerta;
end if;
end;
$$ delimiter ;

/*call registrarPagamento ('No Cartão de Crédito', '2021-07-20', 1, 2);
call registrarPagamento ('No Cartão de Crédito', '2021-07-20', 2, 2);
call registrarPagamento ('No Dinheiro', '2021-08-20', 3, 2);*/
select * from pagamento;
select * from despesa;
select * from caixa;





































