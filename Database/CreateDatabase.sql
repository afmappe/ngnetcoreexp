USE master;
GO 

DROP DATABASE IF EXISTS BD_Empresa;
GO

CREATE DATABASE BD_Empresa;
GO

USE BD_Empresa;
GO

CREATE TABLE Tb_Empleado
(
	Codi_Empleado				Int PRIMARY KEY IDENTITY,
	Nombres_Empleado			Varchar(100),
	Apellidos_Empleado			Varchar(100),
	Direccion_Empleado			Varchar(200),
	Telefono_Empleado			Varchar(200),
	Email_Empleado			    Varchar(200),
	FechaNacimiento_Empleado	DateTime,
	Sueldo_Empleado			    Real,
	Activo						Bit
);
GO
-------------Listar Todos los Empleados----------------
CREATE Procedure usp_Empleado_ListarTodos
AS
	SELECT 
		Codi_Empleado,
		Nombres_Empleado,
		Apellidos_Empleado,
		Direccion_Empleado,
		Telefono_Empleado,
		Email_Empleado,
		FechaNacimiento_Empleado,
		Sueldo_Empleado,
		Activo						
	FROM Tb_Empleado
GO

-------------Filtrar los Empleados----------------
CREATE Procedure usp_Empleado_Filtrar
@Nombres_Empleado			Varchar(100),
@Apellidos_Empleado			Varchar(100)
AS
	SELECT 
		Codi_Empleado			,
		Nombres_Empleado		,
		Apellidos_Empleado		,
		Direccion_Empleado		,
		Telefono_Empleado		,
		Email_Empleado			,
		FechaNacimiento_Empleado	,
		Sueldo_Empleado			,
		Activo						
	FROM Tb_Empleado
	WHERE Nombres_Empleado LIKE '%'+ @Nombres_Empleado +'%'
	AND Apellidos_Empleado LIKE '%'+ @Apellidos_Empleado +'%'
GO
------------Registrar el Empleado------------------
CREATE Procedure usp_Empleado_Registrar
@Nombres_Empleado		Varchar(100),
@Apellidos_Empleado		Varchar(100),
@Direccion_Empleado		Varchar(200),
@Telefono_Empleado		Varchar(200),
@Email_Empleado			Varchar(200),
@FechaNacimiento_Empleado	DateTime,
@Sueldo_Empleado		Real,
@Activo				Bit
AS
 
	INSERT INTO Tb_Empleado
	(
		Nombres_Empleado		, 
		Apellidos_Empleado		, 
		Direccion_Empleado		,
		Telefono_Empleado		, 
		Email_Empleado			, 
		FechaNacimiento_Empleado	,
		Sueldo_Empleado			, 
		Activo						
	)
	VALUES
	(
		@Nombres_Empleado		, 
		@Apellidos_Empleado		, 
		@Direccion_Empleado		,
		@Telefono_Empleado		, 
		@Email_Empleado			, 
		@FechaNacimiento_Empleado	,
		@Sueldo_Empleado		, 
		@Activo							
	)
GO

------------Modificar el Empleado------------------
CREATE Procedure usp_Empleado_Modificar
@Codi_Empleado			Int,
@Nombres_Empleado		Varchar(100),
@Apellidos_Empleado		Varchar(100),
@Direccion_Empleado		Varchar(200),
@Telefono_Empleado		Varchar(200),
@Email_Empleado			Varchar(200),
@FechaNacimiento_Empleado	DateTime,
@Sueldo_Empleado		Real,
@Activo				Bit
AS
 
	UPDATE Tb_Empleado
	SET
		Nombres_Empleado			= @Nombres_Empleado		, 
		Apellidos_Empleado			= @Apellidos_Empleado		, 
		Direccion_Empleado			= @Direccion_Empleado		,
		Telefono_Empleado			= @Telefono_Empleado		, 
		Email_Empleado			        = @Email_Empleado		, 
		FechaNacimiento_Empleado		= @FechaNacimiento_Empleado	,
		Sueldo_Empleado			        = @Sueldo_Empleado		, 
		Activo					= @Activo
	WHERE Codi_Empleado = @Codi_Empleado

GO
------------Eliminar el Empleado------------------
CREATE Procedure usp_Empleado_Eliminar
@Codi_Empleado				Int
AS
 
	DELETE FROM Tb_Empleado
	WHERE Codi_Empleado = @Codi_Empleado
GO

USE BD_Empresa;
GO

EXEC usp_Empleado_Registrar 
	@Nombres_Empleado = 'Andres Felipe',
	@Apellidos_Empleado ='Mape Medina',
	@Direccion_Empleado = 'KR 72bis 152b-25',
	@Telefono_Empleado = '317-379-8480',
	@Email_Empleado = 'afmappe@outloock.com',
	@FechaNacimiento_Empleado  = '19910128',
	@Sueldo_Empleado = '100',
	@Activo	= 1
	;
GO