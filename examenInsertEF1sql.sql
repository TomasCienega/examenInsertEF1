-- DESKTOP-JJ9DM3F\\SQLEXPRESS
create database examenInsertEF1
use examenInsertEF1
create table Departamento
(
	idDepartamento int identity(1,1) not null, 
	nombreDepartamento varchar(50) not null,
	constraint PK_Departamento primary key (idDepartamento)
)
insert into Departamento(nombreDepartamento) values('RH'),('VENTAS'),('COMPRAS'),('IT'),('REDES')
select * from Departamento;

create table Empleado
(
	idEmpleado int identity(1,1) not null,
	nombreEmpleado varchar(100) not null,
	idDepartamento int not null,
	constraint PK_Empleado primary key (idEmpleado),
	constraint FK_Empleado_Departamento foreign key (idDepartamento)
										references Departamento(idDepartamento)
)
EXEC sp_rename 'Empleado.nombreEmpledo', 'nombreEmpleado', 'COLUMN';
insert into Empleado(nombreEmpleado,idDepartamento)values('Ariel',1),('Kosuke',2)
select * from Empleado;


create procedure sp_ListarEmpleadosPorDep
(
	@idDepartamento int
)
as
begin 
	select e.idEmpleado,e.nombreEmpleado, d.idDepartamento,d.nombreDepartamento 
	from Empleado e 
	inner join Departamento d 
	on e.idDepartamento = d.idDepartamento
	where d.idDepartamento = @idDepartamento
end