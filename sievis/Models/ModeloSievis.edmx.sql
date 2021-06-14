
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/13/2020 23:32:34
-- Generated from EDMX file: C:\GTI\sidicot\sisip_2\sievis\Models\ModeloSievis.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [sisip];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AplicacionInterruptor_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Equipo] DROP CONSTRAINT [FK_AplicacionInterruptor_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_Gerencia_AppUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AppUsers] DROP CONSTRAINT [FK_Gerencia_AppUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_Role_AppUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AppUsers] DROP CONSTRAINT [FK_Role_AppUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_Subestacion_AppUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AppUsers] DROP CONSTRAINT [FK_Subestacion_AppUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_Usuario_AppUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AppUsers] DROP CONSTRAINT [FK_Usuario_AppUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_Zona_AppUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AppUsers] DROP CONSTRAINT [FK_Zona_AppUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_Prueba_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Archivo] DROP CONSTRAINT [FK_Prueba_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_Catalogo_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DetalleCatalogo] DROP CONSTRAINT [FK_Catalogo_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_Catalogo_FKv1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ParametroCondicion] DROP CONSTRAINT [FK_Catalogo_FKv1];
GO
IF OBJECT_ID(N'[dbo].[FK_Inspeccion_visual_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CondicionAislador] DROP CONSTRAINT [FK_Inspeccion_visual_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_Prueba_FKv1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CondicionGabineteCentralizador] DROP CONSTRAINT [FK_Prueba_FKv1];
GO
IF OBJECT_ID(N'[dbo].[FK_Inspeccion_visual_FKv8]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CondicionGabineteControl] DROP CONSTRAINT [FK_Inspeccion_visual_FKv8];
GO
IF OBJECT_ID(N'[dbo].[FK_Inspeccion_visual_FKv9]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CondicionOtrosComponentes] DROP CONSTRAINT [FK_Inspeccion_visual_FKv9];
GO
IF OBJECT_ID(N'[dbo].[FK_Inspeccion_visual_FKv1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CondicionVarillaje] DROP CONSTRAINT [FK_Inspeccion_visual_FKv1];
GO
IF OBJECT_ID(N'[dbo].[FK_Inspeccion_visual_FKv2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Densimetro] DROP CONSTRAINT [FK_Inspeccion_visual_FKv2];
GO
IF OBJECT_ID(N'[dbo].[FK_Equipo_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Prueba] DROP CONSTRAINT [FK_Equipo_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_Mecanismo_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Equipo] DROP CONSTRAINT [FK_Mecanismo_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_Modelo_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Equipo] DROP CONSTRAINT [FK_Modelo_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_Subestacion_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Equipo] DROP CONSTRAINT [FK_Subestacion_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_PruebaEspecialDetalle_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Eventos] DROP CONSTRAINT [FK_PruebaEspecialDetalle_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_Gerencia_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Zona] DROP CONSTRAINT [FK_Gerencia_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_Inspeccion_visual_FKv3]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MecanismoHidraulico] DROP CONSTRAINT [FK_Inspeccion_visual_FKv3];
GO
IF OBJECT_ID(N'[dbo].[FK_Inspeccion_visual_FKv4]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MecanismoNeumatico] DROP CONSTRAINT [FK_Inspeccion_visual_FKv4];
GO
IF OBJECT_ID(N'[dbo].[FK_Inspeccion_visual_FKv5]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MecanismoResortes] DROP CONSTRAINT [FK_Inspeccion_visual_FKv5];
GO
IF OBJECT_ID(N'[dbo].[FK_Inspeccion_visual_FKv6]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Presostato] DROP CONSTRAINT [FK_Inspeccion_visual_FKv6];
GO
IF OBJECT_ID(N'[dbo].[FK_Prueba_FKv2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Inspeccion_visual] DROP CONSTRAINT [FK_Prueba_FKv2];
GO
IF OBJECT_ID(N'[dbo].[FK_Marca_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Modelo] DROP CONSTRAINT [FK_Marca_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_Modelo_ParametrosFabricantePE_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ParametrosFabricantePE] DROP CONSTRAINT [FK_Modelo_ParametrosFabricantePE_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_Modelo_ParametrosFabricantePR_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ParametrosFabricantePR] DROP CONSTRAINT [FK_Modelo_ParametrosFabricantePR_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_ParametroCondicion_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Variables] DROP CONSTRAINT [FK_ParametroCondicion_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_Prueba_FKv3]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PruebaEspecial] DROP CONSTRAINT [FK_Prueba_FKv3];
GO
IF OBJECT_ID(N'[dbo].[FK_Prueba_PbaRutina_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PruebaRutina] DROP CONSTRAINT [FK_Prueba_PbaRutina_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_PruebaEspecial_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PruebaEspecialDetalle] DROP CONSTRAINT [FK_PruebaEspecial_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_PruebaRutina_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PruebaRutinaDetalle] DROP CONSTRAINT [FK_PruebaRutina_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_Variables_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Rangos] DROP CONSTRAINT [FK_Variables_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_Zona_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subestacion] DROP CONSTRAINT [FK_Zona_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetRoles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUsers];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[C__MigrationHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[C__MigrationHistory];
GO
IF OBJECT_ID(N'[dbo].[AplicacionInterruptor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AplicacionInterruptor];
GO
IF OBJECT_ID(N'[dbo].[AppParameters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AppParameters];
GO
IF OBJECT_ID(N'[dbo].[AppUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AppUsers];
GO
IF OBJECT_ID(N'[dbo].[Archivo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Archivo];
GO
IF OBJECT_ID(N'[dbo].[AspNetRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserClaims];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserLogins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserLogins];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[Catalogo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Catalogo];
GO
IF OBJECT_ID(N'[dbo].[CondicionAislador]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CondicionAislador];
GO
IF OBJECT_ID(N'[dbo].[CondicionGabineteCentralizador]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CondicionGabineteCentralizador];
GO
IF OBJECT_ID(N'[dbo].[CondicionGabineteControl]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CondicionGabineteControl];
GO
IF OBJECT_ID(N'[dbo].[CondicionOtrosComponentes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CondicionOtrosComponentes];
GO
IF OBJECT_ID(N'[dbo].[CondicionVarillaje]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CondicionVarillaje];
GO
IF OBJECT_ID(N'[dbo].[Densimetro]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Densimetro];
GO
IF OBJECT_ID(N'[dbo].[DetalleCatalogo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DetalleCatalogo];
GO
IF OBJECT_ID(N'[dbo].[Equipo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Equipo];
GO
IF OBJECT_ID(N'[dbo].[Eventos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Eventos];
GO
IF OBJECT_ID(N'[dbo].[Gerencia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Gerencia];
GO
IF OBJECT_ID(N'[dbo].[indiceSaludPrueba]', 'U') IS NOT NULL
    DROP TABLE [dbo].[indiceSaludPrueba];
GO
IF OBJECT_ID(N'[dbo].[Inspeccion_visual]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Inspeccion_visual];
GO
IF OBJECT_ID(N'[dbo].[Marca]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Marca];
GO
IF OBJECT_ID(N'[dbo].[Mecanismo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Mecanismo];
GO
IF OBJECT_ID(N'[dbo].[MecanismoHidraulico]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MecanismoHidraulico];
GO
IF OBJECT_ID(N'[dbo].[MecanismoNeumatico]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MecanismoNeumatico];
GO
IF OBJECT_ID(N'[dbo].[MecanismoResortes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MecanismoResortes];
GO
IF OBJECT_ID(N'[dbo].[Modelo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Modelo];
GO
IF OBJECT_ID(N'[dbo].[ParametroCondicion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ParametroCondicion];
GO
IF OBJECT_ID(N'[dbo].[ParametrosFabricantePE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ParametrosFabricantePE];
GO
IF OBJECT_ID(N'[dbo].[ParametrosFabricantePR]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ParametrosFabricantePR];
GO
IF OBJECT_ID(N'[dbo].[Presostato]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Presostato];
GO
IF OBJECT_ID(N'[dbo].[Prueba]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Prueba];
GO
IF OBJECT_ID(N'[dbo].[PruebaEspecial]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PruebaEspecial];
GO
IF OBJECT_ID(N'[dbo].[PruebaEspecialDetalle]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PruebaEspecialDetalle];
GO
IF OBJECT_ID(N'[dbo].[PruebaRutina]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PruebaRutina];
GO
IF OBJECT_ID(N'[dbo].[PruebaRutinaDetalle]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PruebaRutinaDetalle];
GO
IF OBJECT_ID(N'[dbo].[Rangos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Rangos];
GO
IF OBJECT_ID(N'[dbo].[Subestacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subestacion];
GO
IF OBJECT_ID(N'[dbo].[Variables]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Variables];
GO
IF OBJECT_ID(N'[dbo].[Zona]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Zona];
GO
IF OBJECT_ID(N'[dbo].[ParametrosFabricantePE_OLD]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ParametrosFabricantePE_OLD];
GO
IF OBJECT_ID(N'[dbo].[vCatalogoPuntuaciones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vCatalogoPuntuaciones];
GO
IF OBJECT_ID(N'[dbo].[vCatParamVarRango]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vCatParamVarRango];
GO
IF OBJECT_ID(N'[dbo].[vDatosEquipo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vDatosEquipo];
GO
IF OBJECT_ID(N'[dbo].[vDatosPrueba]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vDatosPrueba];
GO
IF OBJECT_ID(N'[dbo].[vdPrConfInterruptor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vdPrConfInterruptor];
GO
IF OBJECT_ID(N'[dbo].[vdPrMecOperacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vdPrMecOperacion];
GO
IF OBJECT_ID(N'[dbo].[vdPrSecVerifProtecciones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vdPrSecVerifProtecciones];
GO
IF OBJECT_ID(N'[dbo].[vdPrUnidadRuptora]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vdPrUnidadRuptora];
GO
IF OBJECT_ID(N'[dbo].[vIVcondAislador]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vIVcondAislador];
GO
IF OBJECT_ID(N'[dbo].[vIVcondDensimetro]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vIVcondDensimetro];
GO
IF OBJECT_ID(N'[dbo].[vIVcondGabCentralizador]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vIVcondGabCentralizador];
GO
IF OBJECT_ID(N'[dbo].[vIVcondGabControl]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vIVcondGabControl];
GO
IF OBJECT_ID(N'[dbo].[vIVcondGabControlMonitoreo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vIVcondGabControlMonitoreo];
GO
IF OBJECT_ID(N'[dbo].[vIVcondOtrosComp]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vIVcondOtrosComp];
GO
IF OBJECT_ID(N'[dbo].[vIVcondPresostato]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vIVcondPresostato];
GO
IF OBJECT_ID(N'[dbo].[vIVcondVarillaje]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vIVcondVarillaje];
GO
IF OBJECT_ID(N'[dbo].[vIVCPCms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vIVCPCms];
GO
IF OBJECT_ID(N'[dbo].[vIVIndiceSalud]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vIVIndiceSalud];
GO
IF OBJECT_ID(N'[dbo].[vIVmecHidraulico]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vIVmecHidraulico];
GO
IF OBJECT_ID(N'[dbo].[vIVmecNeumatico]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vIVmecNeumatico];
GO
IF OBJECT_ID(N'[dbo].[vIVmecResortes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vIVmecResortes];
GO
IF OBJECT_ID(N'[dbo].[vParametrosVariablesPeso]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vParametrosVariablesPeso];
GO
IF OBJECT_ID(N'[dbo].[vPeCondAislamientoUR]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vPeCondAislamientoUR];
GO
IF OBJECT_ID(N'[dbo].[vPeMecanismoOperacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vPeMecanismoOperacion];
GO
IF OBJECT_ID(N'[dbo].[vPruebasEquipoIndice]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vPruebasEquipoIndice];
GO
IF OBJECT_ID(N'[dbo].[vPuntuacionCondAislador]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vPuntuacionCondAislador];
GO
IF OBJECT_ID(N'[dbo].[vPuntuacionDensimetro]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vPuntuacionDensimetro];
GO
IF OBJECT_ID(N'[dbo].[vPuntuacionGabCentralizador]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vPuntuacionGabCentralizador];
GO
IF OBJECT_ID(N'[dbo].[vPuntuacionGabControl]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vPuntuacionGabControl];
GO
IF OBJECT_ID(N'[dbo].[vPuntuacionGabCtrlMonitoreo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vPuntuacionGabCtrlMonitoreo];
GO
IF OBJECT_ID(N'[dbo].[vPuntuacionMHidraulico]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vPuntuacionMHidraulico];
GO
IF OBJECT_ID(N'[dbo].[vPuntuacionMNeumatico]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vPuntuacionMNeumatico];
GO
IF OBJECT_ID(N'[dbo].[vPuntuacionMResortes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vPuntuacionMResortes];
GO
IF OBJECT_ID(N'[dbo].[vPuntuacionOtrosComp]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vPuntuacionOtrosComp];
GO
IF OBJECT_ID(N'[dbo].[vPuntuacionPresostato]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vPuntuacionPresostato];
GO
IF OBJECT_ID(N'[dbo].[vPuntuacionVarillaje]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vPuntuacionVarillaje];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserRoles];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'C__MigrationHistory'
CREATE TABLE [dbo].[C__MigrationHistory] (
    [MigrationId] nvarchar(150)  NOT NULL,
    [ContextKey] nvarchar(300)  NOT NULL,
    [Model] varbinary(max)  NOT NULL,
    [ProductVersion] nvarchar(32)  NOT NULL
);
GO

-- Creating table 'AplicacionInterruptor'
CREATE TABLE [dbo].[AplicacionInterruptor] (
    [id] int IDENTITY(1,1) NOT NULL,
    [descripcion] varchar(50)  NULL
);
GO

-- Creating table 'AppParameters'
CREATE TABLE [dbo].[AppParameters] (
    [Parametro] nvarchar(100)  NOT NULL,
    [Grupo] nvarchar(100)  NOT NULL,
    [ValueText] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AppUsers'
CREATE TABLE [dbo].[AppUsers] (
    [AppUserId] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(128)  NOT NULL,
    [RoleId] nvarchar(128)  NOT NULL,
    [Estatus] varchar(1)  NOT NULL,
    [NombreCompleto] varchar(300)  NOT NULL,
    [Gerencia_id] int  NULL,
    [Zona_id] int  NULL,
    [Subestacion_id] int  NULL
);
GO

-- Creating table 'Archivo'
CREATE TABLE [dbo].[Archivo] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Prueba_id] int  NOT NULL,
    [fecha] datetime  NOT NULL,
    [nombre_archivo] varchar(250)  NOT NULL,
    [extension] varchar(20)  NULL,
    [archivo_soporte] varbinary(max)  NULL,
    [nombre_prueba] varchar(100)  NULL
);
GO

-- Creating table 'AspNetRoles'
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'AspNetUserClaims'
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(128)  NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetUserLogins'
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] nvarchar(128)  NOT NULL,
    [ProviderKey] nvarchar(128)  NOT NULL,
    [UserId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(128)  NOT NULL,
    [Email] nvarchar(256)  NULL,
    [EmailConfirmed] bit  NOT NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberConfirmed] bit  NOT NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [LockoutEndDateUtc] datetime  NULL,
    [LockoutEnabled] bit  NOT NULL,
    [AccessFailedCount] int  NOT NULL,
    [UserName] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'Catalogo'
CREATE TABLE [dbo].[Catalogo] (
    [id] int IDENTITY(1,1) NOT NULL,
    [nombre] varchar(64)  NOT NULL
);
GO

-- Creating table 'CondicionAislador'
CREATE TABLE [dbo].[CondicionAislador] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Inspeccion_visual_id] int  NOT NULL,
    [corrosion] bit  NULL,
    [observaciones_corrosion] varchar(250)  NULL,
    [roturas] bit  NULL,
    [observaciones_roturas] varchar(250)  NULL,
    [grietas] bit  NULL,
    [observaciones_grietas] varchar(250)  NULL,
    [flameo] bit  NULL,
    [observaciones_flameo] varchar(250)  NULL,
    [danio_cementado] bit  NULL,
    [observaciones_cementado] varchar(250)  NULL,
    [danio_piezasfijacion] bit  NULL,
    [observaciones_piezasfijacion] varchar(250)  NULL
);
GO

-- Creating table 'CondicionGabineteCentralizador'
CREATE TABLE [dbo].[CondicionGabineteCentralizador] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Prueba_id] int  NOT NULL,
    [tablillas] varchar(2)  NULL,
    [observaciones_tablillas] varchar(250)  NULL,
    [contactores] varchar(2)  NULL,
    [observaciones_contactores] varchar(250)  NULL,
    [relevadores] varchar(2)  NULL,
    [observaciones_relevadores] varchar(250)  NULL,
    [elementos_ctrl] varchar(2)  NULL,
    [obs_elementos_ctrl] varchar(250)  NULL,
    [humedad] bit  NULL,
    [observaciones_humedad] varchar(250)  NULL,
    [roturas_internas] bit  NULL,
    [observaciones_roturasint] varchar(250)  NULL,
    [roturas_externas] bit  NULL,
    [observaciones_roturasext] varchar(250)  NULL,
    [grietas_sello] bit  NULL,
    [observaciones_grietassello] varchar(250)  NULL,
    [insectos] bit  NULL,
    [observaciones_insectos] varchar(250)  NULL,
    [desp_pintura_int] bit  NULL,
    [observaciones_pinturaint] varchar(250)  NULL,
    [desp_pintura_ext] bit  NULL,
    [observaciones_pinturaext] varchar(250)  NULL,
    [corrosion_interna] bit  NULL,
    [observaciones_corrosionint] varchar(250)  NULL,
    [corrosion_externa] bit  NULL,
    [observaciones_corrosionext] varchar(250)  NULL,
    [funciona_resis_calefactora] varchar(2)  NULL,
    [obs_fun_resis_calef] varchar(250)  NULL,
    [observaciones] varchar(512)  NULL
);
GO

-- Creating table 'CondicionGabineteControl'
CREATE TABLE [dbo].[CondicionGabineteControl] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Inspeccion_visual_id] int  NOT NULL,
    [humedad] bit  NULL,
    [observaciones_humedad] varchar(250)  NULL,
    [roturas] bit  NULL,
    [observaciones_roturas] varchar(250)  NULL,
    [grietas_sello] bit  NULL,
    [observaciones_grietas] varchar(250)  NULL,
    [insectos] bit  NULL,
    [observaciones_insectos] varchar(250)  NULL,
    [desprendimiento_pintura] bit  NULL,
    [observaciones_pintura] varchar(250)  NULL,
    [corrosion_interna] bit  NULL,
    [observaciones_corrosioninterna] varchar(250)  NULL,
    [corrosion_externa] bit  NULL,
    [observaciones_corrosionexterna] varchar(250)  NULL,
    [resistencia_calefactora] varchar(2)  NULL,
    [observaciones_rcalecfactora] varchar(250)  NULL,
    [corrosion_conex_tierra] bit  NULL,
    [observaciones_corrconx_tierra] varchar(250)  NULL,
    [disparo1] bit  NULL,
    [observaciones_disparo1] varchar(250)  NULL,
    [disparo2] bit  NULL,
    [observaciones_disparo2] varchar(250)  NULL,
    [cierre] bit  NULL,
    [observaciones_cierre] varchar(250)  NULL
);
GO

-- Creating table 'CondicionOtrosComponentes'
CREATE TABLE [dbo].[CondicionOtrosComponentes] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Inspeccion_visual_id] int  NOT NULL,
    [ruido_audible] bit  NULL,
    [observaciones_ruidoaudible] varchar(250)  NULL,
    [vibracion_audible] bit  NULL,
    [observaciones_vibracionaudible] varchar(250)  NULL,
    [corrosion_tubing] bit  NULL,
    [observaciones_ctubing] varchar(250)  NULL,
    [corrosion_ctierra] bit  NULL,
    [observaciones_ctierra] varchar(250)  NULL
);
GO

-- Creating table 'CondicionVarillaje'
CREATE TABLE [dbo].[CondicionVarillaje] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Inspeccion_visual_id] int  NOT NULL,
    [alineacion] bit  NULL,
    [observaciones_alineacion] varchar(250)  NULL,
    [pzas_fracturadas_rotas] bit  NULL,
    [observaciones_pzafracrot] varchar(250)  NULL,
    [corrosion] bit  NULL,
    [observaciones_corrosion] varchar(250)  NULL,
    [condision_pernos] varchar(2)  NULL,
    [observaciones_pernos] varchar(250)  NULL,
    [lubricacion] bit  NULL,
    [observaciones_lubricacion] varchar(250)  NULL
);
GO

-- Creating table 'Densimetro'
CREATE TABLE [dbo].[Densimetro] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Inspeccion_visual_id] int  NOT NULL,
    [carcasa] varchar(2)  NULL,
    [observaciones_carcasa] varchar(250)  NULL,
    [caratula] varchar(2)  NULL,
    [observaciones_caratula] varchar(250)  NULL,
    [condicion_aguja] varchar(2)  NULL,
    [observaciones_cond_aguja] varchar(250)  NULL,
    [nivel_glicerina] varchar(2)  NULL,
    [observaciones_nglicerina] varchar(250)  NULL,
    [escala] varchar(2)  NULL,
    [observaciones_escala] varchar(250)  NULL,
    [presionSF6_va] decimal(6,2)  NULL,
    [presionSF6_vn] decimal(6,2)  NULL,
    [observaciones_presionSF6] varchar(250)  NULL
);
GO

-- Creating table 'DetalleCatalogo'
CREATE TABLE [dbo].[DetalleCatalogo] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Catalogo_id] int  NOT NULL,
    [descripcion] varchar(128)  NOT NULL,
    [abreviatura] varchar(5)  NOT NULL,
    [valorEntero] int  NULL,
    [valorFlotante] decimal(10,2)  NULL
);
GO

-- Creating table 'Equipo'
CREATE TABLE [dbo].[Equipo] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Gerencia_id] int  NOT NULL,
    [Zona_id] int  NOT NULL,
    [Subestacion_id] int  NOT NULL,
    [Mecanismo_id] int  NOT NULL,
    [AplicacionInterruptor_id] int  NOT NULL,
    [Marca_id] int  NOT NULL,
    [Modelo_id] int  NOT NULL,
    [bahia] varchar(5)  NULL,
    [ns] varchar(20)  NULL,
    [anio_fabricacion] int  NULL,
    [tension_nominal] decimal(10,2)  NULL,
    [corriente_nominal] decimal(10,2)  NULL,
    [corriente_cc] decimal(10,2)  NULL,
    [bil] decimal(10,3)  NULL,
    [disponibilidad_refaccion_st] varchar(1)  NULL,
    [presionSF6] decimal(10,3)  NULL,
    [tipo_unidades_presion] varchar(1)  NULL,
    [presion_alarma] decimal(10,2)  NULL,
    [tipo_unidades_presion_alarma] varchar(1)  NULL,
    [altitud_operacion] int  NULL,
    [altitud_instalacion] int  NULL,
    [dis_estructural] varchar(2)  NULL,
    [conf_camaras] varchar(1)  NULL,
    [res_estatica_contactos] decimal(10,2)  NULL,
    [interruptor_contiene] varchar(1)  NULL,
    [interruptor_resistencia] decimal(10,2)  NULL,
    [interruptor_capacitor] decimal(10,2)  NULL,
    [fecha_puestaservicio] datetime  NULL,
    [fultimo_mantenimiento] datetime  NULL,
    [nivel_contaminacion] varchar(1)  NULL,
    [tipo_disparo] varchar(1)  NULL,
    [comando_cierre] varchar(1)  NULL,
    [distancia_fuga] int  NULL,
    [clase_interruptor] varchar(2)  NULL,
    [Id_EquipoSAP] varchar(50)  NULL,
    [UbicaTecnica] varchar(50)  NULL,
    [voltajeNominalBobina] int  NULL,
    [voltajeNominalBobinaCierre] int  NULL,
    [instrumento_medicionSF6] varchar(1)  NULL,
    [existe_gabinete_centralizador] bit  NULL,
    [existe_gabinetectrl_xfase] bit  NULL
);
GO

-- Creating table 'Eventos'
CREATE TABLE [dbo].[Eventos] (
    [id] int IDENTITY(1,1) NOT NULL,
    [PruebaEspecialDetalle_id] int  NOT NULL,
    [valor] real  NULL,
    [oscilograma] varbinary(max)  NULL
);
GO

-- Creating table 'Gerencia'
CREATE TABLE [dbo].[Gerencia] (
    [id] int IDENTITY(1,1) NOT NULL,
    [nombre] varchar(50)  NOT NULL
);
GO

-- Creating table 'indiceSaludPrueba'
CREATE TABLE [dbo].[indiceSaludPrueba] (
    [id] int IDENTITY(1,1) NOT NULL,
    [pruebaId] int  NULL,
    [indiceSalud] decimal(10,2)  NULL
);
GO

-- Creating table 'Inspeccion_visual'
CREATE TABLE [dbo].[Inspeccion_visual] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Prueba_id] int  NOT NULL,
    [fase] varchar(1)  NOT NULL,
    [num_operaciones] int  NULL
);
GO

-- Creating table 'Marca'
CREATE TABLE [dbo].[Marca] (
    [id] int IDENTITY(1,1) NOT NULL,
    [nombre] varchar(50)  NOT NULL
);
GO

-- Creating table 'Mecanismo'
CREATE TABLE [dbo].[Mecanismo] (
    [id] int IDENTITY(1,1) NOT NULL,
    [descripcion] varchar(50)  NOT NULL
);
GO

-- Creating table 'MecanismoHidraulico'
CREATE TABLE [dbo].[MecanismoHidraulico] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Inspeccion_visual_id] int  NOT NULL,
    [fuga_aceite] varchar(2)  NULL,
    [observaciones_fugaa] varchar(250)  NULL,
    [acumulador] varchar(2)  NULL,
    [observaciones_acumulador] varchar(250)  NULL,
    [presion_aceite] varchar(2)  NULL,
    [observaciones_presiona] varchar(250)  NULL,
    [unidad_control] varchar(2)  NULL,
    [observaciones_uctrl] varchar(250)  NULL,
    [valvulas] varchar(2)  NULL,
    [observaciones_valvulas] varchar(250)  NULL,
    [burbujas_reservorio] varchar(2)  NULL,
    [observaciones_burbujasr] varchar(250)  NULL,
    [compresor] varchar(2)  NULL,
    [observaciones_compresor] varchar(250)  NULL,
    [nivel_glicerina] varchar(2)  NULL,
    [observaciones_nglicerina] varchar(250)  NULL
);
GO

-- Creating table 'MecanismoNeumatico'
CREATE TABLE [dbo].[MecanismoNeumatico] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Inspeccion_visual_id] int  NOT NULL,
    [presion_aire] varchar(2)  NULL,
    [observaciones_presionaire] varchar(250)  NULL,
    [fuga_aire] varchar(2)  NULL,
    [observaciones_fugaaire] varchar(250)  NULL,
    [valvulas] varchar(2)  NULL,
    [observaciones_valvulas] varchar(250)  NULL,
    [corrosion_mecanismo] varchar(2)  NULL,
    [observaciones_corrmecan] varchar(250)  NULL
);
GO

-- Creating table 'MecanismoResortes'
CREATE TABLE [dbo].[MecanismoResortes] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Inspeccion_visual_id] int  NOT NULL,
    [alineacion_resortes] varchar(2)  NULL,
    [observaciones_alin_resortes] varchar(250)  NULL,
    [amortiguadores] varchar(2)  NULL,
    [observaciones_amortiguadores] varchar(250)  NULL,
    [corros_desp_pintura] bit  NULL,
    [observaciones_cordesp_pintura] varchar(250)  NULL,
    [motor] varchar(2)  NULL,
    [observaciones_motor] varchar(250)  NULL
);
GO

-- Creating table 'Modelo'
CREATE TABLE [dbo].[Modelo] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Marca_id] int  NOT NULL,
    [nombre] varchar(50)  NOT NULL
);
GO

-- Creating table 'ParametroCondicion'
CREATE TABLE [dbo].[ParametroCondicion] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Catalogo_Id] int  NOT NULL,
    [parametro] varchar(250)  NOT NULL,
    [peso] decimal(5,2)  NOT NULL
);
GO

-- Creating table 'ParametrosFabricantePE'
CREATE TABLE [dbo].[ParametrosFabricantePE] (
    [id] int IDENTITY(1,1) NOT NULL,
    [MarcaId] int  NOT NULL,
    [ModeloId] int  NOT NULL,
    [toTAperturaD1LimInf] decimal(6,2)  NULL,
    [toTAperturaD1LimSup] decimal(6,2)  NULL,
    [toTAperturaD2LimInf] decimal(6,2)  NULL,
    [toTAperturaD2LimSup] decimal(6,2)  NULL,
    [toTCierreLimInf] decimal(6,2)  NULL,
    [toTCierreLimSup] decimal(6,2)  NULL,
    [toTCierreApeCALimInf] decimal(6,2)  NULL,
    [toTCierreApeCALimSup] decimal(6,2)  NULL,
    [toTEntResPreLimInf] decimal(6,2)  NULL,
    [toTEntResPreLimSup] decimal(6,2)  NULL,
    [pdCarrTotalLimInf] decimal(6,2)  NULL,
    [pdCarrTotalLimSup] decimal(6,2)  NULL,
    [pdAnguloGiro] decimal(6,2)  NULL,
    [pdFactoConversion] decimal(6,2)  NULL,
    [daVelocidadLimInf] decimal(6,2)  NULL,
    [daVelocidadLimSup] decimal(6,2)  NULL,
    [daSobreviaje] decimal(6,2)  NULL,
    [daRebote] decimal(6,2)  NULL,
    [dcVelocidadLimInf] decimal(6,2)  NULL,
    [dcVelocidadLimSup] decimal(6,2)  NULL,
    [dcSobreviaje] decimal(6,2)  NULL,
    [dcRebote] decimal(6,2)  NULL,
    [dcPenetracionLimInf] decimal(6,2)  NULL,
    [dcPenetracionLimSup] decimal(6,2)  NULL,
    [boTensionNominal] decimal(6,2)  NULL,
    [boAperturaIPico] decimal(6,2)  NULL,
    [boCierreIPico] decimal(6,2)  NULL,
    [mtrTensionNominal] decimal(6,2)  NULL,
    [mtrCorrienteArranque] decimal(6,2)  NULL,
    [mtrTCargaResorteLimInf] decimal(6,2)  NULL,
    [mtrTCargaResorteLimSup] decimal(6,2)  NULL
);
GO

-- Creating table 'ParametrosFabricantePR'
CREATE TABLE [dbo].[ParametrosFabricantePR] (
    [id] int IDENTITY(1,1) NOT NULL,
    [numeroCamarasPolo] int  NULL,
    [tiempoAperturaD1] int  NULL,
    [criterioTAD1] varchar(5)  NULL,
    [tiempoAperturaD2] int  NULL,
    [criterioTAD2] varchar(5)  NULL,
    [tiempoCierre] int  NULL,
    [ModeloId] int  NOT NULL,
    [Marca_id] int  NOT NULL,
    [ParametrosFabricantePR_ID] decimal(28,0)  NOT NULL
);
GO

-- Creating table 'Presostato'
CREATE TABLE [dbo].[Presostato] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Inspeccion_visual_id] int  NOT NULL,
    [carcasa] varchar(2)  NULL,
    [observaciones_carcasa] varchar(250)  NULL,
    [caratula] varchar(2)  NULL,
    [observaciones_caratula] varchar(250)  NULL,
    [condicion_aguja] varchar(2)  NULL,
    [observaciones_cond_aguja] varchar(250)  NULL,
    [nivel_glicerina] varchar(2)  NULL,
    [observaciones_nglicerina] varchar(250)  NULL,
    [escala] varchar(2)  NULL,
    [observaciones_escala] varchar(250)  NULL,
    [presionSF6_va] decimal(6,2)  NULL,
    [presionSF6_vn] decimal(6,2)  NULL,
    [observaciones_presionSF6] varchar(250)  NULL,
    [temperatura_va] decimal(6,2)  NULL,
    [temperatura_vn] decimal(6,2)  NULL,
    [observaciones_temperatura] varchar(250)  NULL
);
GO

-- Creating table 'Prueba'
CREATE TABLE [dbo].[Prueba] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Equipo_id] int  NOT NULL,
    [fecha_prueba] datetime  NOT NULL,
    [fecha_inspeccion] datetime  NULL,
    [instrumento_medicionSF6] varchar(1)  NOT NULL,
    [existe_gabinete_centralizador] bit  NULL,
    [existe_gabinetectrl_xfase] bit  NULL,
    [evalBasica] bit  NOT NULL,
    [evalExtendida] bit  NOT NULL,
    [indiceSaludBasica] decimal(5,2)  NULL,
    [indiceSaludExtendida] decimal(5,2)  NULL,
    [indiceConfiabilidad] decimal(5,2)  NULL
);
GO

-- Creating table 'PruebaEspecial'
CREATE TABLE [dbo].[PruebaEspecial] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Prueba_id] int  NOT NULL,
    [temperatura_ambiente] decimal(10,2)  NULL,
    [func_bobina_aper70d1] varchar(2)  NULL,
    [func_bobina_aper70d2] varchar(2)  NULL,
    [func_bobina_cierre85] varchar(2)  NULL,
    [arranque_minbobd1] decimal(10,2)  NULL,
    [arranque_minbobd2] decimal(10,2)  NULL,
    [arranque_minbob_cierre] decimal(10,2)  NULL,
    [sec_nominal_operacion] varchar(2)  NULL,
    [numero_motores] smallint  NULL
);
GO

-- Creating table 'PruebaEspecialDetalle'
CREATE TABLE [dbo].[PruebaEspecialDetalle] (
    [id] int IDENTITY(1,1) NOT NULL,
    [PruebaEspecial_id] int  NOT NULL,
    [fase] varchar(1)  NOT NULL,
    [long_contac_arqueo_c1] decimal(6,2)  NULL,
    [long_contac_arqueo_c2] decimal(6,2)  NULL,
    [temMaxTermSuperior_c1] decimal(10,2)  NULL,
    [temMaxTermSuperior_c2] decimal(10,2)  NULL,
    [temMaxTermInferior_c1] decimal(10,2)  NULL,
    [temMaxTermInferior_c2] decimal(10,2)  NULL,
    [ubicacion_tempmax] varchar(4)  NULL,
    [ipd1] decimal(8,2)  NULL,
    [t1d1] decimal(8,2)  NULL,
    [t2d1] decimal(8,2)  NULL,
    [ttd1] decimal(8,2)  NULL,
    [ipd2] decimal(8,2)  NULL,
    [t1d2] decimal(8,2)  NULL,
    [t2d2] decimal(8,2)  NULL,
    [ttd2] decimal(8,2)  NULL,
    [ip_cierre] decimal(8,2)  NULL,
    [t1_cierre] decimal(8,2)  NULL,
    [t2_cierre] decimal(8,2)  NULL,
    [tt_cierre] decimal(8,2)  NULL,
    [pda_velocidad_ap1] decimal(6,2)  NULL,
    [pda_carreratot_ap1] decimal(6,2)  NULL,
    [pda_sobrerecorrido_ap1] decimal(6,2)  NULL,
    [pda_rebote_ap1] decimal(6,2)  NULL,
    [pda_velocidad_ap2] decimal(6,2)  NULL,
    [pda_carreratot_ap2] decimal(6,2)  NULL,
    [pda_sobrerecorrido_ap2] decimal(6,2)  NULL,
    [pda_rebote_ap2] decimal(6,2)  NULL,
    [pardescierre_velocidad] decimal(6,2)  NULL,
    [pardescierre_carreratot] decimal(6,2)  NULL,
    [pardescierre_sobrerecorrido] decimal(6,2)  NULL,
    [pardescierre_rebote] decimal(6,2)  NULL,
    [pardescierre_penetracion] decimal(6,2)  NULL,
    [pardescierre_penetracion_c2] decimal(6,2)  NULL,
    [subprodsf6_so2] decimal(6,2)  NULL,
    [subprodsf6_hf] decimal(6,2)  NULL,
    [subprodsf6_cf4] decimal(6,2)  NULL,
    [tapertura_c1] decimal(6,2)  NULL,
    [tapertura_c2] decimal(6,2)  NULL,
    [tapertura_cierre_c1] decimal(6,2)  NULL,
    [tapertura_cierre_c2] decimal(6,2)  NULL,
    [tcierre_apertura1_c1] decimal(6,2)  NULL,
    [tcierre_apertura1_c2] decimal(6,2)  NULL,
    [tcierre_apertura2_c1] decimal(6,2)  NULL,
    [tcierre_apertura2_c2] decimal(6,2)  NULL,
    [imotor] decimal(10,2)  NULL,
    [tcarga_resorte] decimal(10,2)  NULL
);
GO

-- Creating table 'PruebaRutina'
CREATE TABLE [dbo].[PruebaRutina] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Prueba_id] int  NOT NULL,
    [seccuencia_displibre] varchar(2)  NULL,
    [pba_antibombeo] varchar(2)  NULL,
    [discrepancia_polos] varchar(2)  NULL,
    [ttotal_discrepancia] decimal(10,5)  NULL,
    [fuga] bit  NULL,
    [frecuencia_llenado_SF6] int  NULL,
    [muestreo_gas_xfase] bit  NULL
);
GO

-- Creating table 'PruebaRutinaDetalle'
CREATE TABLE [dbo].[PruebaRutinaDetalle] (
    [id] int IDENTITY(1,1) NOT NULL,
    [PruebaRutina_id] int  NOT NULL,
    [fase] varchar(1)  NULL,
    [unidad_medicion_humedad] varchar(1)  NULL,
    [humedad] decimal(6,2)  NULL,
    [purezasf6] decimal(6,2)  NULL,
    [aire] decimal(6,2)  NULL,
    [unidad_medicion_presion] varchar(1)  NULL,
    [presion] decimal(6,2)  NULL,
    [tapertura_d1c1] decimal(6,2)  NULL,
    [tapertura_d1c2] decimal(6,2)  NULL,
    [tapertura_d2c1] decimal(6,2)  NULL,
    [tapertura_d2c2] decimal(6,2)  NULL,
    [tcierre_c1] decimal(6,2)  NULL,
    [tcierre_c2] decimal(6,2)  NULL,
    [tent_resprein_c1] decimal(6,2)  NULL,
    [tent_resprein_c2] decimal(6,2)  NULL,
    [tcierreapertura_c1] decimal(6,2)  NULL,
    [tcierreapertura_c2] decimal(6,2)  NULL,
    [res_estat_contactos_d1c1] decimal(6,2)  NULL,
    [res_estat_contactos_d1c2] decimal(6,2)  NULL,
    [resitencia_ohmica_rpi_c1] decimal(8,2)  NULL,
    [capacitancia_condesadores_c1] decimal(8,2)  NULL,
    [resitencia_ohmica_rpi_c2] decimal(8,2)  NULL,
    [capacitancia_condesadores_c2] decimal(8,2)  NULL
);
GO

-- Creating table 'Rangos'
CREATE TABLE [dbo].[Rangos] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Variable_id] int  NOT NULL,
    [nomRango] varchar(64)  NULL,
    [valorMenor] decimal(8,2)  NULL,
    [valorMayor] decimal(8,2)  NULL,
    [observacion] varchar(256)  NULL,
    [recomendacion] varchar(2048)  NULL
);
GO

-- Creating table 'Subestacion'
CREATE TABLE [dbo].[Subestacion] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Gerencia_id] int  NOT NULL,
    [Zona_id] int  NOT NULL,
    [nombre] varchar(50)  NOT NULL
);
GO

-- Creating table 'Variables'
CREATE TABLE [dbo].[Variables] (
    [id] int IDENTITY(1,1) NOT NULL,
    [ParametroCondicion_id] int  NOT NULL,
    [nomVariable] varchar(250)  NULL,
    [peso] decimal(5,2)  NULL
);
GO

-- Creating table 'Zona'
CREATE TABLE [dbo].[Zona] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Gerencia_id] int  NOT NULL,
    [nombre] varchar(250)  NOT NULL
);
GO

-- Creating table 'ParametrosFabricantePE_OLD'
CREATE TABLE [dbo].[ParametrosFabricantePE_OLD] (
    [id] int  NOT NULL,
    [Marca_id] int  NOT NULL,
    [Modelo_id] int  NOT NULL,
    [tiempoCargaResorte] decimal(6,2)  NULL,
    [anguloGiro] decimal(6,2)  NULL,
    [factorConversion] decimal(6,2)  NULL,
    [carreraTotal] decimal(6,2)  NULL,
    [velocidadInferiorA] decimal(6,2)  NULL,
    [velocidadSuperiorA] decimal(6,2)  NULL,
    [carreraTotalIncremento] decimal(6,2)  NULL,
    [sobreviajeA] decimal(6,2)  NULL,
    [criterioSobreviaje] varchar(5)  NULL,
    [reboteA] decimal(6,2)  NULL,
    [velocidadInferiorC] decimal(6,2)  NULL,
    [velocidadSuperiorC] decimal(6,2)  NULL,
    [sobreviajeC] decimal(6,2)  NULL,
    [reboteC] decimal(6,2)  NULL,
    [penetracionC] decimal(6,2)  NULL,
    [bobinaAperturaCorrientePico] decimal(6,2)  NULL,
    [bobinaCierreCorrientePico] decimal(6,2)  NULL,
    [ParametrosFabricantePE_ID] decimal(28,0) IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'vCatalogoPuntuaciones'
CREATE TABLE [dbo].[vCatalogoPuntuaciones] (
    [catId] int  NOT NULL,
    [detId] int  NOT NULL,
    [nombre] varchar(64)  NOT NULL,
    [descripcion] varchar(128)  NOT NULL,
    [abreviatura] varchar(5)  NOT NULL,
    [valorEntero] int  NULL,
    [valorFlotante] decimal(10,2)  NULL
);
GO

-- Creating table 'vCatParamVarRango'
CREATE TABLE [dbo].[vCatParamVarRango] (
    [catalogoId] int  NOT NULL,
    [parametroId] int  NOT NULL,
    [variableId] int  NOT NULL,
    [rangoId] int  NOT NULL,
    [catalogo] varchar(64)  NOT NULL,
    [parametro] varchar(250)  NOT NULL,
    [parPeso] decimal(5,2)  NOT NULL,
    [variable] varchar(250)  NULL,
    [varPeso] decimal(5,2)  NULL,
    [rango] varchar(64)  NULL,
    [valorMenor] decimal(8,2)  NULL,
    [valorMayor] decimal(8,2)  NULL,
    [observacion] varchar(256)  NULL,
    [recomendacion] varchar(2048)  NULL
);
GO

-- Creating table 'vDatosEquipo'
CREATE TABLE [dbo].[vDatosEquipo] (
    [id] int  NOT NULL,
    [gerencia] varchar(50)  NOT NULL,
    [zona] varchar(250)  NOT NULL,
    [subestacion] varchar(50)  NOT NULL,
    [marca] varchar(50)  NOT NULL,
    [modelo] varchar(50)  NOT NULL,
    [mecanismo] varchar(50)  NOT NULL,
    [aplicacion] varchar(50)  NULL,
    [afabricacion] int  NULL,
    [fecha_puestaservicio] datetime  NULL,
    [bahia] varchar(5)  NULL,
    [ns] varchar(20)  NULL,
    [tension_nominal] decimal(10,2)  NULL,
    [corriente_nominal] decimal(10,2)  NULL,
    [disestructural] varchar(13)  NULL,
    [configuracioncamaras] varchar(7)  NULL,
    [Zona_id] int  NOT NULL,
    [Gerencia_id] int  NOT NULL,
    [Id_EquipoSAP] varchar(50)  NULL
);
GO

-- Creating table 'vDatosPrueba'
CREATE TABLE [dbo].[vDatosPrueba] (
    [equipoId] int  NOT NULL,
    [bahia] varchar(5)  NOT NULL,
    [ns] varchar(20)  NOT NULL,
    [tension_nominal] decimal(10,2)  NULL,
    [dis_estructural] varchar(2)  NULL,
    [instrumento_medicionSF6] varchar(1)  NOT NULL,
    [existe_gabinete_centralizador] bit  NULL,
    [existe_gabinetectrl_xfase] bit  NULL,
    [evalBasica] bit  NOT NULL,
    [evalExtendida] bit  NOT NULL,
    [disEstructural] varchar(13)  NULL,
    [confCamaras] varchar(7)  NULL,
    [pruebaid] int  NOT NULL,
    [fecha_inspeccion] datetime  NULL,
    [insMedicionSF6] varchar(10)  NULL,
    [GabCentralizador] varchar(26)  NULL,
    [GabCtrlFase] varchar(32)  NULL,
    [evaluacionBasica] varchar(17)  NULL,
    [evaluacionExtendida] varchar(20)  NULL,
    [ivId] int  NOT NULL,
    [fase] varchar(1)  NOT NULL,
    [numOperaciones] int  NULL
);
GO

-- Creating table 'vdPrConfInterruptor'
CREATE TABLE [dbo].[vdPrConfInterruptor] (
    [equipoId] int  NOT NULL,
    [pruebaId] int  NOT NULL,
    [fecha_puestaservicio] datetime  NULL,
    [aPuestaServicio] int  NULL,
    [fultimo_mantenimiento] datetime  NULL,
    [aUltimoManto] int  NULL,
    [numOperaciones] int  NULL,
    [disponibilidad_refaccion_st] varchar(1)  NULL,
    [nivel_contaminacion] varchar(1)  NULL
);
GO

-- Creating table 'vdPrMecOperacion'
CREATE TABLE [dbo].[vdPrMecOperacion] (
    [id] int  NOT NULL,
    [prueba_id] int  NOT NULL,
    [fase] varchar(1)  NULL,
    [tapertura_d1c1] decimal(6,2)  NULL,
    [tapertura_d1c2] decimal(6,2)  NULL,
    [tapertura_d2c1] decimal(6,2)  NULL,
    [tapertura_d2c2] decimal(6,2)  NULL,
    [tcierre_c1] decimal(6,2)  NULL,
    [tcierre_c2] decimal(6,2)  NULL,
    [tent_resprein_c1] decimal(6,2)  NULL,
    [tent_resprein_c2] decimal(6,2)  NULL
);
GO

-- Creating table 'vdPrSecVerifProtecciones'
CREATE TABLE [dbo].[vdPrSecVerifProtecciones] (
    [Prueba_id] int  NOT NULL,
    [fase] varchar(1)  NULL,
    [seccuencia_displibre] varchar(2)  NULL,
    [tcierreapertura_c1] decimal(6,2)  NULL,
    [tcierreapertura_c2] decimal(6,2)  NULL,
    [pba_antibombeo] varchar(2)  NULL,
    [discrepancia_polos] varchar(2)  NULL,
    [ttotal_discrepancia] decimal(10,5)  NULL
);
GO

-- Creating table 'vdPrUnidadRuptora'
CREATE TABLE [dbo].[vdPrUnidadRuptora] (
    [pruebaId] int  NOT NULL,
    [fase] varchar(1)  NULL,
    [res_estat_contactos_d1c1] decimal(6,2)  NULL,
    [res_estat_contactos_d1c2] decimal(6,2)  NULL,
    [resitencia_ohmica_rpi_c1] decimal(8,2)  NULL,
    [resitencia_ohmica_rpi_c2] decimal(8,2)  NULL,
    [capacitancia_condesadores_c1] decimal(8,2)  NULL,
    [capacitancia_condesadores_c2] decimal(8,2)  NULL,
    [humedad] decimal(6,2)  NULL,
    [unidad_medicion_humedad] varchar(1)  NULL,
    [purezasf6] decimal(6,2)  NULL,
    [aire] decimal(6,2)  NULL,
    [frecuencia_llenado_sf6] int  NULL,
    [presionSF6] decimal(10,3)  NOT NULL,
    [unidadPresionSF6] varchar(1)  NULL,
    [presion_alarma] decimal(10,2)  NULL,
    [presionsf6_fase] decimal(6,2)  NULL,
    [unidadPresionSF6Fase] varchar(1)  NULL
);
GO

-- Creating table 'vIVcondAislador'
CREATE TABLE [dbo].[vIVcondAislador] (
    [PruebaId] int  NOT NULL,
    [puntuacion] int  NULL
);
GO

-- Creating table 'vIVcondDensimetro'
CREATE TABLE [dbo].[vIVcondDensimetro] (
    [PruebaId] int  NOT NULL,
    [puntuacion] int  NULL
);
GO

-- Creating table 'vIVcondGabCentralizador'
CREATE TABLE [dbo].[vIVcondGabCentralizador] (
    [PruebaId] int  NOT NULL,
    [puntuacion] int  NULL
);
GO

-- Creating table 'vIVcondGabControl'
CREATE TABLE [dbo].[vIVcondGabControl] (
    [PruebaId] int  NOT NULL,
    [puntuacion] int  NULL
);
GO

-- Creating table 'vIVcondGabControlMonitoreo'
CREATE TABLE [dbo].[vIVcondGabControlMonitoreo] (
    [PruebaId] int  NOT NULL,
    [puntuacion] int  NULL
);
GO

-- Creating table 'vIVcondOtrosComp'
CREATE TABLE [dbo].[vIVcondOtrosComp] (
    [PruebaId] int  NOT NULL,
    [puntuacion] int  NULL
);
GO

-- Creating table 'vIVcondPresostato'
CREATE TABLE [dbo].[vIVcondPresostato] (
    [PruebaId] int  NOT NULL,
    [puntuacion] int  NULL
);
GO

-- Creating table 'vIVcondVarillaje'
CREATE TABLE [dbo].[vIVcondVarillaje] (
    [PruebaId] int  NOT NULL,
    [puntuacion] int  NULL
);
GO

-- Creating table 'vIVCPCms'
CREATE TABLE [dbo].[vIVCPCms] (
    [pruebaId] int  NOT NULL,
    [catId] int  NOT NULL,
    [paramId] int  NOT NULL,
    [varId] int  NOT NULL,
    [catalogo] varchar(64)  NOT NULL,
    [parametro] varchar(250)  NOT NULL,
    [nomVariable] varchar(250)  NULL,
    [varPeso] decimal(5,2)  NULL,
    [CPCm] decimal(37,21)  NULL
);
GO

-- Creating table 'vIVIndiceSalud'
CREATE TABLE [dbo].[vIVIndiceSalud] (
    [pruebaId] int  NOT NULL,
    [ISx] float  NULL
);
GO

-- Creating table 'vIVmecHidraulico'
CREATE TABLE [dbo].[vIVmecHidraulico] (
    [PruebaId] int  NOT NULL,
    [puntuacion] int  NULL
);
GO

-- Creating table 'vIVmecNeumatico'
CREATE TABLE [dbo].[vIVmecNeumatico] (
    [PruebaId] int  NOT NULL,
    [puntuacion] int  NULL
);
GO

-- Creating table 'vIVmecResortes'
CREATE TABLE [dbo].[vIVmecResortes] (
    [PruebaId] int  NOT NULL,
    [puntuacion] int  NULL
);
GO

-- Creating table 'vParametrosVariablesPeso'
CREATE TABLE [dbo].[vParametrosVariablesPeso] (
    [catId] int  NOT NULL,
    [paramId] int  NOT NULL,
    [varId] int  NOT NULL,
    [catalogo] varchar(64)  NOT NULL,
    [parametro] varchar(250)  NOT NULL,
    [paramPeso] decimal(5,2)  NOT NULL,
    [nomVariable] varchar(250)  NULL,
    [varPeso] decimal(5,2)  NULL
);
GO

-- Creating table 'vPeCondAislamientoUR'
CREATE TABLE [dbo].[vPeCondAislamientoUR] (
    [Prueba_id] int  NOT NULL,
    [Id] int  NOT NULL,
    [fase] varchar(1)  NOT NULL,
    [long_contac_arqueo_c1] decimal(6,2)  NULL,
    [long_contac_arqueo_c2] decimal(6,2)  NULL,
    [temMaxTermInferior_c1] decimal(10,2)  NULL,
    [temMaxTermInferior_c2] decimal(10,2)  NULL,
    [temMaxTermSuperior_c1] decimal(10,2)  NULL,
    [temMaxTermSuperior_c2] decimal(10,2)  NULL,
    [ubicacion_tempmax] varchar(4)  NULL,
    [subprodsf6_so2] decimal(6,2)  NULL,
    [subprodsf6_hf] decimal(6,2)  NULL,
    [subprodsf6_cf4] decimal(6,2)  NULL
);
GO

-- Creating table 'vPeMecanismoOperacion'
CREATE TABLE [dbo].[vPeMecanismoOperacion] (
    [Prueba_id] int  NOT NULL,
    [pruebaEspecialId] int  NOT NULL,
    [peDetalleId] int  NOT NULL,
    [fase] varchar(1)  NOT NULL,
    [func_bobina_aper70d1] varchar(2)  NULL,
    [func_bobina_aper70d2] varchar(2)  NULL,
    [func_bobina_cierre85] varchar(2)  NULL,
    [arranque_minbobd1] decimal(10,2)  NULL,
    [arranque_minbobd2] decimal(10,2)  NULL,
    [arranque_minbob_cierre] decimal(10,2)  NULL,
    [ipd1] decimal(8,2)  NULL,
    [t1d1] decimal(8,2)  NULL,
    [t2d1] decimal(8,2)  NULL,
    [ttd1] decimal(8,2)  NULL,
    [ipd2] decimal(8,2)  NULL,
    [t1d2] decimal(8,2)  NULL,
    [t2d2] decimal(8,2)  NULL,
    [ttd2] decimal(8,2)  NULL,
    [ip_cierre] decimal(8,2)  NULL,
    [t1_cierre] decimal(8,2)  NULL,
    [t2_cierre] decimal(8,2)  NULL,
    [tt_cierre] decimal(8,2)  NULL,
    [pda_velocidad_ap1] decimal(6,2)  NULL,
    [pda_carreratot_ap1] decimal(6,2)  NULL,
    [pda_sobrerecorrido_ap1] decimal(6,2)  NULL,
    [pda_rebote_ap1] decimal(6,2)  NULL,
    [pda_velocidad_ap2] decimal(6,2)  NULL,
    [pda_carreratot_ap2] decimal(6,2)  NULL,
    [pda_sobrerecorrido_ap2] decimal(6,2)  NULL,
    [pda_rebote_ap2] decimal(6,2)  NULL,
    [pardescierre_velocidad] decimal(6,2)  NULL,
    [pardescierre_carreratot] decimal(6,2)  NULL,
    [pardescierre_sobrerecorrido] decimal(6,2)  NULL,
    [pardescierre_rebote] decimal(6,2)  NULL,
    [pardescierre_penetracion] decimal(6,2)  NULL,
    [pardescierre_penetracion_c2] decimal(6,2)  NULL,
    [sec_nominal_operacion] varchar(2)  NULL,
    [tapertura_c1] decimal(6,2)  NULL,
    [tapertura_c2] decimal(6,2)  NULL,
    [tapertura_cierre_c1] decimal(6,2)  NULL,
    [tapertura_cierre_c2] decimal(6,2)  NULL,
    [tcierre_apertura1_c1] decimal(6,2)  NULL,
    [tcierre_apertura1_c2] decimal(6,2)  NULL,
    [tcierre_apertura2_c1] decimal(6,2)  NULL,
    [tcierre_apertura2_c2] decimal(6,2)  NULL,
    [imotor] decimal(10,2)  NULL,
    [tcarga_resorte] decimal(10,2)  NULL,
    [numero_motores] smallint  NULL
);
GO

-- Creating table 'vPruebasEquipoIndice'
CREATE TABLE [dbo].[vPruebasEquipoIndice] (
    [equipoId] int  NOT NULL,
    [pruebaid] int  NOT NULL,
    [gerencia] varchar(50)  NOT NULL,
    [zona] varchar(250)  NOT NULL,
    [subestacion] varchar(50)  NOT NULL,
    [bahia] varchar(5)  NULL,
    [ns] varchar(20)  NULL,
    [tension_nominal] decimal(10,2)  NULL,
    [dis_estructural] varchar(2)  NULL,
    [instrumento_medicionSF6] varchar(1)  NOT NULL,
    [existe_gabinete_centralizador] bit  NULL,
    [existe_gabinetectrl_xfase] bit  NULL,
    [disEstructural] varchar(13)  NULL,
    [confCamaras] varchar(7)  NULL,
    [fecha_inspeccion] datetime  NULL,
    [insMedicionSF6] varchar(10)  NULL,
    [GabCentralizador] varchar(26)  NULL,
    [GabCtrlFase] varchar(32)  NULL,
    [evaluacionBasica] varchar(17)  NULL,
    [evaluacionExtendida] varchar(20)  NULL,
    [evalBasica] bit  NOT NULL,
    [indiceSaludBasica] decimal(5,2)  NULL,
    [evalExtendida] bit  NOT NULL,
    [indiceSaludExtendida] decimal(5,2)  NULL,
    [indiceConfiabilidad] decimal(5,2)  NULL,
    [Gerencia_id] int  NOT NULL,
    [Zona_id] int  NOT NULL,
    [Subestacion_id] int  NOT NULL,
    [fecha_prueba] datetime  NOT NULL
);
GO

-- Creating table 'vPuntuacionCondAislador'
CREATE TABLE [dbo].[vPuntuacionCondAislador] (
    [pruebaId] int  NOT NULL,
    [puntuacion] varchar(1)  NULL
);
GO

-- Creating table 'vPuntuacionDensimetro'
CREATE TABLE [dbo].[vPuntuacionDensimetro] (
    [pruebaid] int  NOT NULL,
    [puntuacion] varchar(1)  NULL
);
GO

-- Creating table 'vPuntuacionGabCentralizador'
CREATE TABLE [dbo].[vPuntuacionGabCentralizador] (
    [PruebaId] int  NOT NULL,
    [puntuacion] varchar(1)  NULL
);
GO

-- Creating table 'vPuntuacionGabControl'
CREATE TABLE [dbo].[vPuntuacionGabControl] (
    [PruebaId] int  NOT NULL,
    [puntuacion] varchar(1)  NULL
);
GO

-- Creating table 'vPuntuacionGabCtrlMonitoreo'
CREATE TABLE [dbo].[vPuntuacionGabCtrlMonitoreo] (
    [PruebaId] int  NOT NULL,
    [puntuacion] varchar(1)  NOT NULL
);
GO

-- Creating table 'vPuntuacionMHidraulico'
CREATE TABLE [dbo].[vPuntuacionMHidraulico] (
    [pruebaid] int  NOT NULL,
    [puntuacion] varchar(1)  NULL
);
GO

-- Creating table 'vPuntuacionMNeumatico'
CREATE TABLE [dbo].[vPuntuacionMNeumatico] (
    [pruebaid] int  NOT NULL,
    [puntuacion] varchar(1)  NULL
);
GO

-- Creating table 'vPuntuacionMResortes'
CREATE TABLE [dbo].[vPuntuacionMResortes] (
    [pruebaid] int  NOT NULL,
    [puntuacion] varchar(1)  NULL
);
GO

-- Creating table 'vPuntuacionOtrosComp'
CREATE TABLE [dbo].[vPuntuacionOtrosComp] (
    [pruebaid] int  NOT NULL,
    [puntuacion] varchar(1)  NULL
);
GO

-- Creating table 'vPuntuacionPresostato'
CREATE TABLE [dbo].[vPuntuacionPresostato] (
    [pruebaid] int  NOT NULL,
    [puntuacion] varchar(1)  NULL
);
GO

-- Creating table 'vPuntuacionVarillaje'
CREATE TABLE [dbo].[vPuntuacionVarillaje] (
    [pruebaid] int  NOT NULL,
    [puntuacion] varchar(1)  NULL
);
GO

-- Creating table 'AspNetUserRoles'
CREATE TABLE [dbo].[AspNetUserRoles] (
    [AspNetRoles_Id] nvarchar(128)  NOT NULL,
    [AspNetUsers_Id] nvarchar(128)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [MigrationId], [ContextKey] in table 'C__MigrationHistory'
ALTER TABLE [dbo].[C__MigrationHistory]
ADD CONSTRAINT [PK_C__MigrationHistory]
    PRIMARY KEY CLUSTERED ([MigrationId], [ContextKey] ASC);
GO

-- Creating primary key on [id] in table 'AplicacionInterruptor'
ALTER TABLE [dbo].[AplicacionInterruptor]
ADD CONSTRAINT [PK_AplicacionInterruptor]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Parametro], [Grupo] in table 'AppParameters'
ALTER TABLE [dbo].[AppParameters]
ADD CONSTRAINT [PK_AppParameters]
    PRIMARY KEY CLUSTERED ([Parametro], [Grupo] ASC);
GO

-- Creating primary key on [UserId], [RoleId] in table 'AppUsers'
ALTER TABLE [dbo].[AppUsers]
ADD CONSTRAINT [PK_AppUsers]
    PRIMARY KEY CLUSTERED ([UserId], [RoleId] ASC);
GO

-- Creating primary key on [id] in table 'Archivo'
ALTER TABLE [dbo].[Archivo]
ADD CONSTRAINT [PK_Archivo]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetRoles'
ALTER TABLE [dbo].[AspNetRoles]
ADD CONSTRAINT [PK_AspNetRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [PK_AspNetUserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [LoginProvider], [ProviderKey], [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [PK_AspNetUserLogins]
    PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey], [UserId] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'Catalogo'
ALTER TABLE [dbo].[Catalogo]
ADD CONSTRAINT [PK_Catalogo]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'CondicionAislador'
ALTER TABLE [dbo].[CondicionAislador]
ADD CONSTRAINT [PK_CondicionAislador]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'CondicionGabineteCentralizador'
ALTER TABLE [dbo].[CondicionGabineteCentralizador]
ADD CONSTRAINT [PK_CondicionGabineteCentralizador]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'CondicionGabineteControl'
ALTER TABLE [dbo].[CondicionGabineteControl]
ADD CONSTRAINT [PK_CondicionGabineteControl]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'CondicionOtrosComponentes'
ALTER TABLE [dbo].[CondicionOtrosComponentes]
ADD CONSTRAINT [PK_CondicionOtrosComponentes]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'CondicionVarillaje'
ALTER TABLE [dbo].[CondicionVarillaje]
ADD CONSTRAINT [PK_CondicionVarillaje]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Densimetro'
ALTER TABLE [dbo].[Densimetro]
ADD CONSTRAINT [PK_Densimetro]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'DetalleCatalogo'
ALTER TABLE [dbo].[DetalleCatalogo]
ADD CONSTRAINT [PK_DetalleCatalogo]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Equipo'
ALTER TABLE [dbo].[Equipo]
ADD CONSTRAINT [PK_Equipo]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Eventos'
ALTER TABLE [dbo].[Eventos]
ADD CONSTRAINT [PK_Eventos]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Gerencia'
ALTER TABLE [dbo].[Gerencia]
ADD CONSTRAINT [PK_Gerencia]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'indiceSaludPrueba'
ALTER TABLE [dbo].[indiceSaludPrueba]
ADD CONSTRAINT [PK_indiceSaludPrueba]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Inspeccion_visual'
ALTER TABLE [dbo].[Inspeccion_visual]
ADD CONSTRAINT [PK_Inspeccion_visual]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Marca'
ALTER TABLE [dbo].[Marca]
ADD CONSTRAINT [PK_Marca]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Mecanismo'
ALTER TABLE [dbo].[Mecanismo]
ADD CONSTRAINT [PK_Mecanismo]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'MecanismoHidraulico'
ALTER TABLE [dbo].[MecanismoHidraulico]
ADD CONSTRAINT [PK_MecanismoHidraulico]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'MecanismoNeumatico'
ALTER TABLE [dbo].[MecanismoNeumatico]
ADD CONSTRAINT [PK_MecanismoNeumatico]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'MecanismoResortes'
ALTER TABLE [dbo].[MecanismoResortes]
ADD CONSTRAINT [PK_MecanismoResortes]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id], [Marca_id] in table 'Modelo'
ALTER TABLE [dbo].[Modelo]
ADD CONSTRAINT [PK_Modelo]
    PRIMARY KEY CLUSTERED ([id], [Marca_id] ASC);
GO

-- Creating primary key on [id] in table 'ParametroCondicion'
ALTER TABLE [dbo].[ParametroCondicion]
ADD CONSTRAINT [PK_ParametroCondicion]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'ParametrosFabricantePE'
ALTER TABLE [dbo].[ParametrosFabricantePE]
ADD CONSTRAINT [PK_ParametrosFabricantePE]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [ParametrosFabricantePR_ID] in table 'ParametrosFabricantePR'
ALTER TABLE [dbo].[ParametrosFabricantePR]
ADD CONSTRAINT [PK_ParametrosFabricantePR]
    PRIMARY KEY CLUSTERED ([ParametrosFabricantePR_ID] ASC);
GO

-- Creating primary key on [id] in table 'Presostato'
ALTER TABLE [dbo].[Presostato]
ADD CONSTRAINT [PK_Presostato]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Prueba'
ALTER TABLE [dbo].[Prueba]
ADD CONSTRAINT [PK_Prueba]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'PruebaEspecial'
ALTER TABLE [dbo].[PruebaEspecial]
ADD CONSTRAINT [PK_PruebaEspecial]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'PruebaEspecialDetalle'
ALTER TABLE [dbo].[PruebaEspecialDetalle]
ADD CONSTRAINT [PK_PruebaEspecialDetalle]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'PruebaRutina'
ALTER TABLE [dbo].[PruebaRutina]
ADD CONSTRAINT [PK_PruebaRutina]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'PruebaRutinaDetalle'
ALTER TABLE [dbo].[PruebaRutinaDetalle]
ADD CONSTRAINT [PK_PruebaRutinaDetalle]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Rangos'
ALTER TABLE [dbo].[Rangos]
ADD CONSTRAINT [PK_Rangos]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id], [Gerencia_id], [Zona_id] in table 'Subestacion'
ALTER TABLE [dbo].[Subestacion]
ADD CONSTRAINT [PK_Subestacion]
    PRIMARY KEY CLUSTERED ([id], [Gerencia_id], [Zona_id] ASC);
GO

-- Creating primary key on [id] in table 'Variables'
ALTER TABLE [dbo].[Variables]
ADD CONSTRAINT [PK_Variables]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id], [Gerencia_id] in table 'Zona'
ALTER TABLE [dbo].[Zona]
ADD CONSTRAINT [PK_Zona]
    PRIMARY KEY CLUSTERED ([id], [Gerencia_id] ASC);
GO

-- Creating primary key on [id], [Marca_id], [Modelo_id], [ParametrosFabricantePE_ID] in table 'ParametrosFabricantePE_OLD'
ALTER TABLE [dbo].[ParametrosFabricantePE_OLD]
ADD CONSTRAINT [PK_ParametrosFabricantePE_OLD]
    PRIMARY KEY CLUSTERED ([id], [Marca_id], [Modelo_id], [ParametrosFabricantePE_ID] ASC);
GO

-- Creating primary key on [catId], [detId], [nombre], [descripcion], [abreviatura] in table 'vCatalogoPuntuaciones'
ALTER TABLE [dbo].[vCatalogoPuntuaciones]
ADD CONSTRAINT [PK_vCatalogoPuntuaciones]
    PRIMARY KEY CLUSTERED ([catId], [detId], [nombre], [descripcion], [abreviatura] ASC);
GO

-- Creating primary key on [catalogoId], [parametroId], [variableId], [rangoId], [catalogo], [parametro], [parPeso] in table 'vCatParamVarRango'
ALTER TABLE [dbo].[vCatParamVarRango]
ADD CONSTRAINT [PK_vCatParamVarRango]
    PRIMARY KEY CLUSTERED ([catalogoId], [parametroId], [variableId], [rangoId], [catalogo], [parametro], [parPeso] ASC);
GO

-- Creating primary key on [id], [gerencia], [zona], [subestacion], [marca], [modelo], [mecanismo], [Zona_id], [Gerencia_id] in table 'vDatosEquipo'
ALTER TABLE [dbo].[vDatosEquipo]
ADD CONSTRAINT [PK_vDatosEquipo]
    PRIMARY KEY CLUSTERED ([id], [gerencia], [zona], [subestacion], [marca], [modelo], [mecanismo], [Zona_id], [Gerencia_id] ASC);
GO

-- Creating primary key on [equipoId], [bahia], [ns], [instrumento_medicionSF6], [evalBasica], [evalExtendida], [pruebaid], [ivId], [fase] in table 'vDatosPrueba'
ALTER TABLE [dbo].[vDatosPrueba]
ADD CONSTRAINT [PK_vDatosPrueba]
    PRIMARY KEY CLUSTERED ([equipoId], [bahia], [ns], [instrumento_medicionSF6], [evalBasica], [evalExtendida], [pruebaid], [ivId], [fase] ASC);
GO

-- Creating primary key on [equipoId], [pruebaId] in table 'vdPrConfInterruptor'
ALTER TABLE [dbo].[vdPrConfInterruptor]
ADD CONSTRAINT [PK_vdPrConfInterruptor]
    PRIMARY KEY CLUSTERED ([equipoId], [pruebaId] ASC);
GO

-- Creating primary key on [id], [prueba_id] in table 'vdPrMecOperacion'
ALTER TABLE [dbo].[vdPrMecOperacion]
ADD CONSTRAINT [PK_vdPrMecOperacion]
    PRIMARY KEY CLUSTERED ([id], [prueba_id] ASC);
GO

-- Creating primary key on [Prueba_id] in table 'vdPrSecVerifProtecciones'
ALTER TABLE [dbo].[vdPrSecVerifProtecciones]
ADD CONSTRAINT [PK_vdPrSecVerifProtecciones]
    PRIMARY KEY CLUSTERED ([Prueba_id] ASC);
GO

-- Creating primary key on [pruebaId], [presionSF6] in table 'vdPrUnidadRuptora'
ALTER TABLE [dbo].[vdPrUnidadRuptora]
ADD CONSTRAINT [PK_vdPrUnidadRuptora]
    PRIMARY KEY CLUSTERED ([pruebaId], [presionSF6] ASC);
GO

-- Creating primary key on [PruebaId] in table 'vIVcondAislador'
ALTER TABLE [dbo].[vIVcondAislador]
ADD CONSTRAINT [PK_vIVcondAislador]
    PRIMARY KEY CLUSTERED ([PruebaId] ASC);
GO

-- Creating primary key on [PruebaId] in table 'vIVcondDensimetro'
ALTER TABLE [dbo].[vIVcondDensimetro]
ADD CONSTRAINT [PK_vIVcondDensimetro]
    PRIMARY KEY CLUSTERED ([PruebaId] ASC);
GO

-- Creating primary key on [PruebaId] in table 'vIVcondGabCentralizador'
ALTER TABLE [dbo].[vIVcondGabCentralizador]
ADD CONSTRAINT [PK_vIVcondGabCentralizador]
    PRIMARY KEY CLUSTERED ([PruebaId] ASC);
GO

-- Creating primary key on [PruebaId] in table 'vIVcondGabControl'
ALTER TABLE [dbo].[vIVcondGabControl]
ADD CONSTRAINT [PK_vIVcondGabControl]
    PRIMARY KEY CLUSTERED ([PruebaId] ASC);
GO

-- Creating primary key on [PruebaId] in table 'vIVcondGabControlMonitoreo'
ALTER TABLE [dbo].[vIVcondGabControlMonitoreo]
ADD CONSTRAINT [PK_vIVcondGabControlMonitoreo]
    PRIMARY KEY CLUSTERED ([PruebaId] ASC);
GO

-- Creating primary key on [PruebaId] in table 'vIVcondOtrosComp'
ALTER TABLE [dbo].[vIVcondOtrosComp]
ADD CONSTRAINT [PK_vIVcondOtrosComp]
    PRIMARY KEY CLUSTERED ([PruebaId] ASC);
GO

-- Creating primary key on [PruebaId] in table 'vIVcondPresostato'
ALTER TABLE [dbo].[vIVcondPresostato]
ADD CONSTRAINT [PK_vIVcondPresostato]
    PRIMARY KEY CLUSTERED ([PruebaId] ASC);
GO

-- Creating primary key on [PruebaId] in table 'vIVcondVarillaje'
ALTER TABLE [dbo].[vIVcondVarillaje]
ADD CONSTRAINT [PK_vIVcondVarillaje]
    PRIMARY KEY CLUSTERED ([PruebaId] ASC);
GO

-- Creating primary key on [pruebaId], [catId], [paramId], [varId], [catalogo], [parametro] in table 'vIVCPCms'
ALTER TABLE [dbo].[vIVCPCms]
ADD CONSTRAINT [PK_vIVCPCms]
    PRIMARY KEY CLUSTERED ([pruebaId], [catId], [paramId], [varId], [catalogo], [parametro] ASC);
GO

-- Creating primary key on [pruebaId] in table 'vIVIndiceSalud'
ALTER TABLE [dbo].[vIVIndiceSalud]
ADD CONSTRAINT [PK_vIVIndiceSalud]
    PRIMARY KEY CLUSTERED ([pruebaId] ASC);
GO

-- Creating primary key on [PruebaId] in table 'vIVmecHidraulico'
ALTER TABLE [dbo].[vIVmecHidraulico]
ADD CONSTRAINT [PK_vIVmecHidraulico]
    PRIMARY KEY CLUSTERED ([PruebaId] ASC);
GO

-- Creating primary key on [PruebaId] in table 'vIVmecNeumatico'
ALTER TABLE [dbo].[vIVmecNeumatico]
ADD CONSTRAINT [PK_vIVmecNeumatico]
    PRIMARY KEY CLUSTERED ([PruebaId] ASC);
GO

-- Creating primary key on [PruebaId] in table 'vIVmecResortes'
ALTER TABLE [dbo].[vIVmecResortes]
ADD CONSTRAINT [PK_vIVmecResortes]
    PRIMARY KEY CLUSTERED ([PruebaId] ASC);
GO

-- Creating primary key on [catId], [paramId], [varId], [catalogo], [parametro], [paramPeso] in table 'vParametrosVariablesPeso'
ALTER TABLE [dbo].[vParametrosVariablesPeso]
ADD CONSTRAINT [PK_vParametrosVariablesPeso]
    PRIMARY KEY CLUSTERED ([catId], [paramId], [varId], [catalogo], [parametro], [paramPeso] ASC);
GO

-- Creating primary key on [Prueba_id], [Id], [fase] in table 'vPeCondAislamientoUR'
ALTER TABLE [dbo].[vPeCondAislamientoUR]
ADD CONSTRAINT [PK_vPeCondAislamientoUR]
    PRIMARY KEY CLUSTERED ([Prueba_id], [Id], [fase] ASC);
GO

-- Creating primary key on [Prueba_id], [pruebaEspecialId], [peDetalleId], [fase] in table 'vPeMecanismoOperacion'
ALTER TABLE [dbo].[vPeMecanismoOperacion]
ADD CONSTRAINT [PK_vPeMecanismoOperacion]
    PRIMARY KEY CLUSTERED ([Prueba_id], [pruebaEspecialId], [peDetalleId], [fase] ASC);
GO

-- Creating primary key on [equipoId], [pruebaid], [gerencia], [zona], [subestacion], [instrumento_medicionSF6], [evalBasica], [evalExtendida], [Gerencia_id], [Zona_id], [Subestacion_id], [fecha_prueba] in table 'vPruebasEquipoIndice'
ALTER TABLE [dbo].[vPruebasEquipoIndice]
ADD CONSTRAINT [PK_vPruebasEquipoIndice]
    PRIMARY KEY CLUSTERED ([equipoId], [pruebaid], [gerencia], [zona], [subestacion], [instrumento_medicionSF6], [evalBasica], [evalExtendida], [Gerencia_id], [Zona_id], [Subestacion_id], [fecha_prueba] ASC);
GO

-- Creating primary key on [pruebaId] in table 'vPuntuacionCondAislador'
ALTER TABLE [dbo].[vPuntuacionCondAislador]
ADD CONSTRAINT [PK_vPuntuacionCondAislador]
    PRIMARY KEY CLUSTERED ([pruebaId] ASC);
GO

-- Creating primary key on [pruebaid] in table 'vPuntuacionDensimetro'
ALTER TABLE [dbo].[vPuntuacionDensimetro]
ADD CONSTRAINT [PK_vPuntuacionDensimetro]
    PRIMARY KEY CLUSTERED ([pruebaid] ASC);
GO

-- Creating primary key on [PruebaId] in table 'vPuntuacionGabCentralizador'
ALTER TABLE [dbo].[vPuntuacionGabCentralizador]
ADD CONSTRAINT [PK_vPuntuacionGabCentralizador]
    PRIMARY KEY CLUSTERED ([PruebaId] ASC);
GO

-- Creating primary key on [PruebaId] in table 'vPuntuacionGabControl'
ALTER TABLE [dbo].[vPuntuacionGabControl]
ADD CONSTRAINT [PK_vPuntuacionGabControl]
    PRIMARY KEY CLUSTERED ([PruebaId] ASC);
GO

-- Creating primary key on [PruebaId], [puntuacion] in table 'vPuntuacionGabCtrlMonitoreo'
ALTER TABLE [dbo].[vPuntuacionGabCtrlMonitoreo]
ADD CONSTRAINT [PK_vPuntuacionGabCtrlMonitoreo]
    PRIMARY KEY CLUSTERED ([PruebaId], [puntuacion] ASC);
GO

-- Creating primary key on [pruebaid] in table 'vPuntuacionMHidraulico'
ALTER TABLE [dbo].[vPuntuacionMHidraulico]
ADD CONSTRAINT [PK_vPuntuacionMHidraulico]
    PRIMARY KEY CLUSTERED ([pruebaid] ASC);
GO

-- Creating primary key on [pruebaid] in table 'vPuntuacionMNeumatico'
ALTER TABLE [dbo].[vPuntuacionMNeumatico]
ADD CONSTRAINT [PK_vPuntuacionMNeumatico]
    PRIMARY KEY CLUSTERED ([pruebaid] ASC);
GO

-- Creating primary key on [pruebaid] in table 'vPuntuacionMResortes'
ALTER TABLE [dbo].[vPuntuacionMResortes]
ADD CONSTRAINT [PK_vPuntuacionMResortes]
    PRIMARY KEY CLUSTERED ([pruebaid] ASC);
GO

-- Creating primary key on [pruebaid] in table 'vPuntuacionOtrosComp'
ALTER TABLE [dbo].[vPuntuacionOtrosComp]
ADD CONSTRAINT [PK_vPuntuacionOtrosComp]
    PRIMARY KEY CLUSTERED ([pruebaid] ASC);
GO

-- Creating primary key on [pruebaid] in table 'vPuntuacionPresostato'
ALTER TABLE [dbo].[vPuntuacionPresostato]
ADD CONSTRAINT [PK_vPuntuacionPresostato]
    PRIMARY KEY CLUSTERED ([pruebaid] ASC);
GO

-- Creating primary key on [pruebaid] in table 'vPuntuacionVarillaje'
ALTER TABLE [dbo].[vPuntuacionVarillaje]
ADD CONSTRAINT [PK_vPuntuacionVarillaje]
    PRIMARY KEY CLUSTERED ([pruebaid] ASC);
GO

-- Creating primary key on [AspNetRoles_Id], [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [PK_AspNetUserRoles]
    PRIMARY KEY CLUSTERED ([AspNetRoles_Id], [AspNetUsers_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AplicacionInterruptor_id] in table 'Equipo'
ALTER TABLE [dbo].[Equipo]
ADD CONSTRAINT [FK_AplicacionInterruptor_FK]
    FOREIGN KEY ([AplicacionInterruptor_id])
    REFERENCES [dbo].[AplicacionInterruptor]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AplicacionInterruptor_FK'
CREATE INDEX [IX_FK_AplicacionInterruptor_FK]
ON [dbo].[Equipo]
    ([AplicacionInterruptor_id]);
GO

-- Creating foreign key on [Gerencia_id] in table 'AppUsers'
ALTER TABLE [dbo].[AppUsers]
ADD CONSTRAINT [FK_Gerencia_AppUsers]
    FOREIGN KEY ([Gerencia_id])
    REFERENCES [dbo].[Gerencia]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Gerencia_AppUsers'
CREATE INDEX [IX_FK_Gerencia_AppUsers]
ON [dbo].[AppUsers]
    ([Gerencia_id]);
GO

-- Creating foreign key on [RoleId] in table 'AppUsers'
ALTER TABLE [dbo].[AppUsers]
ADD CONSTRAINT [FK_Role_AppUsers]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Role_AppUsers'
CREATE INDEX [IX_FK_Role_AppUsers]
ON [dbo].[AppUsers]
    ([RoleId]);
GO

-- Creating foreign key on [Subestacion_id], [Zona_id], [Gerencia_id] in table 'AppUsers'
ALTER TABLE [dbo].[AppUsers]
ADD CONSTRAINT [FK_Subestacion_AppUsers]
    FOREIGN KEY ([Subestacion_id], [Zona_id], [Gerencia_id])
    REFERENCES [dbo].[Subestacion]
        ([id], [Zona_id], [Gerencia_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Subestacion_AppUsers'
CREATE INDEX [IX_FK_Subestacion_AppUsers]
ON [dbo].[AppUsers]
    ([Subestacion_id], [Zona_id], [Gerencia_id]);
GO

-- Creating foreign key on [UserId] in table 'AppUsers'
ALTER TABLE [dbo].[AppUsers]
ADD CONSTRAINT [FK_Usuario_AppUsers]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Zona_id], [Gerencia_id] in table 'AppUsers'
ALTER TABLE [dbo].[AppUsers]
ADD CONSTRAINT [FK_Zona_AppUsers]
    FOREIGN KEY ([Zona_id], [Gerencia_id])
    REFERENCES [dbo].[Zona]
        ([id], [Gerencia_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Zona_AppUsers'
CREATE INDEX [IX_FK_Zona_AppUsers]
ON [dbo].[AppUsers]
    ([Zona_id], [Gerencia_id]);
GO

-- Creating foreign key on [Prueba_id] in table 'Archivo'
ALTER TABLE [dbo].[Archivo]
ADD CONSTRAINT [FK_Prueba_FK]
    FOREIGN KEY ([Prueba_id])
    REFERENCES [dbo].[Prueba]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Prueba_FK'
CREATE INDEX [IX_FK_Prueba_FK]
ON [dbo].[Archivo]
    ([Prueba_id]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserClaims]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserLogins]
    ([UserId]);
GO

-- Creating foreign key on [Catalogo_id] in table 'DetalleCatalogo'
ALTER TABLE [dbo].[DetalleCatalogo]
ADD CONSTRAINT [FK_Catalogo_FK]
    FOREIGN KEY ([Catalogo_id])
    REFERENCES [dbo].[Catalogo]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Catalogo_FK'
CREATE INDEX [IX_FK_Catalogo_FK]
ON [dbo].[DetalleCatalogo]
    ([Catalogo_id]);
GO

-- Creating foreign key on [Catalogo_Id] in table 'ParametroCondicion'
ALTER TABLE [dbo].[ParametroCondicion]
ADD CONSTRAINT [FK_Catalogo_FKv1]
    FOREIGN KEY ([Catalogo_Id])
    REFERENCES [dbo].[Catalogo]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Catalogo_FKv1'
CREATE INDEX [IX_FK_Catalogo_FKv1]
ON [dbo].[ParametroCondicion]
    ([Catalogo_Id]);
GO

-- Creating foreign key on [Inspeccion_visual_id] in table 'CondicionAislador'
ALTER TABLE [dbo].[CondicionAislador]
ADD CONSTRAINT [FK_Inspeccion_visual_FK]
    FOREIGN KEY ([Inspeccion_visual_id])
    REFERENCES [dbo].[Inspeccion_visual]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Inspeccion_visual_FK'
CREATE INDEX [IX_FK_Inspeccion_visual_FK]
ON [dbo].[CondicionAislador]
    ([Inspeccion_visual_id]);
GO

-- Creating foreign key on [Prueba_id] in table 'CondicionGabineteCentralizador'
ALTER TABLE [dbo].[CondicionGabineteCentralizador]
ADD CONSTRAINT [FK_Prueba_FKv1]
    FOREIGN KEY ([Prueba_id])
    REFERENCES [dbo].[Prueba]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Prueba_FKv1'
CREATE INDEX [IX_FK_Prueba_FKv1]
ON [dbo].[CondicionGabineteCentralizador]
    ([Prueba_id]);
GO

-- Creating foreign key on [Inspeccion_visual_id] in table 'CondicionGabineteControl'
ALTER TABLE [dbo].[CondicionGabineteControl]
ADD CONSTRAINT [FK_Inspeccion_visual_FKv8]
    FOREIGN KEY ([Inspeccion_visual_id])
    REFERENCES [dbo].[Inspeccion_visual]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Inspeccion_visual_FKv8'
CREATE INDEX [IX_FK_Inspeccion_visual_FKv8]
ON [dbo].[CondicionGabineteControl]
    ([Inspeccion_visual_id]);
GO

-- Creating foreign key on [Inspeccion_visual_id] in table 'CondicionOtrosComponentes'
ALTER TABLE [dbo].[CondicionOtrosComponentes]
ADD CONSTRAINT [FK_Inspeccion_visual_FKv9]
    FOREIGN KEY ([Inspeccion_visual_id])
    REFERENCES [dbo].[Inspeccion_visual]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Inspeccion_visual_FKv9'
CREATE INDEX [IX_FK_Inspeccion_visual_FKv9]
ON [dbo].[CondicionOtrosComponentes]
    ([Inspeccion_visual_id]);
GO

-- Creating foreign key on [Inspeccion_visual_id] in table 'CondicionVarillaje'
ALTER TABLE [dbo].[CondicionVarillaje]
ADD CONSTRAINT [FK_Inspeccion_visual_FKv1]
    FOREIGN KEY ([Inspeccion_visual_id])
    REFERENCES [dbo].[Inspeccion_visual]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Inspeccion_visual_FKv1'
CREATE INDEX [IX_FK_Inspeccion_visual_FKv1]
ON [dbo].[CondicionVarillaje]
    ([Inspeccion_visual_id]);
GO

-- Creating foreign key on [Inspeccion_visual_id] in table 'Densimetro'
ALTER TABLE [dbo].[Densimetro]
ADD CONSTRAINT [FK_Inspeccion_visual_FKv2]
    FOREIGN KEY ([Inspeccion_visual_id])
    REFERENCES [dbo].[Inspeccion_visual]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Inspeccion_visual_FKv2'
CREATE INDEX [IX_FK_Inspeccion_visual_FKv2]
ON [dbo].[Densimetro]
    ([Inspeccion_visual_id]);
GO

-- Creating foreign key on [Equipo_id] in table 'Prueba'
ALTER TABLE [dbo].[Prueba]
ADD CONSTRAINT [FK_Equipo_FK]
    FOREIGN KEY ([Equipo_id])
    REFERENCES [dbo].[Equipo]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Equipo_FK'
CREATE INDEX [IX_FK_Equipo_FK]
ON [dbo].[Prueba]
    ([Equipo_id]);
GO

-- Creating foreign key on [Mecanismo_id] in table 'Equipo'
ALTER TABLE [dbo].[Equipo]
ADD CONSTRAINT [FK_Mecanismo_FK]
    FOREIGN KEY ([Mecanismo_id])
    REFERENCES [dbo].[Mecanismo]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Mecanismo_FK'
CREATE INDEX [IX_FK_Mecanismo_FK]
ON [dbo].[Equipo]
    ([Mecanismo_id]);
GO

-- Creating foreign key on [Modelo_id], [Marca_id] in table 'Equipo'
ALTER TABLE [dbo].[Equipo]
ADD CONSTRAINT [FK_Modelo_FK]
    FOREIGN KEY ([Modelo_id], [Marca_id])
    REFERENCES [dbo].[Modelo]
        ([id], [Marca_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Modelo_FK'
CREATE INDEX [IX_FK_Modelo_FK]
ON [dbo].[Equipo]
    ([Modelo_id], [Marca_id]);
GO

-- Creating foreign key on [Subestacion_id], [Gerencia_id], [Zona_id] in table 'Equipo'
ALTER TABLE [dbo].[Equipo]
ADD CONSTRAINT [FK_Subestacion_FK]
    FOREIGN KEY ([Subestacion_id], [Gerencia_id], [Zona_id])
    REFERENCES [dbo].[Subestacion]
        ([id], [Gerencia_id], [Zona_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Subestacion_FK'
CREATE INDEX [IX_FK_Subestacion_FK]
ON [dbo].[Equipo]
    ([Subestacion_id], [Gerencia_id], [Zona_id]);
GO

-- Creating foreign key on [PruebaEspecialDetalle_id] in table 'Eventos'
ALTER TABLE [dbo].[Eventos]
ADD CONSTRAINT [FK_PruebaEspecialDetalle_FK]
    FOREIGN KEY ([PruebaEspecialDetalle_id])
    REFERENCES [dbo].[PruebaEspecialDetalle]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PruebaEspecialDetalle_FK'
CREATE INDEX [IX_FK_PruebaEspecialDetalle_FK]
ON [dbo].[Eventos]
    ([PruebaEspecialDetalle_id]);
GO

-- Creating foreign key on [Gerencia_id] in table 'Zona'
ALTER TABLE [dbo].[Zona]
ADD CONSTRAINT [FK_Gerencia_FK]
    FOREIGN KEY ([Gerencia_id])
    REFERENCES [dbo].[Gerencia]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Gerencia_FK'
CREATE INDEX [IX_FK_Gerencia_FK]
ON [dbo].[Zona]
    ([Gerencia_id]);
GO

-- Creating foreign key on [Inspeccion_visual_id] in table 'MecanismoHidraulico'
ALTER TABLE [dbo].[MecanismoHidraulico]
ADD CONSTRAINT [FK_Inspeccion_visual_FKv3]
    FOREIGN KEY ([Inspeccion_visual_id])
    REFERENCES [dbo].[Inspeccion_visual]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Inspeccion_visual_FKv3'
CREATE INDEX [IX_FK_Inspeccion_visual_FKv3]
ON [dbo].[MecanismoHidraulico]
    ([Inspeccion_visual_id]);
GO

-- Creating foreign key on [Inspeccion_visual_id] in table 'MecanismoNeumatico'
ALTER TABLE [dbo].[MecanismoNeumatico]
ADD CONSTRAINT [FK_Inspeccion_visual_FKv4]
    FOREIGN KEY ([Inspeccion_visual_id])
    REFERENCES [dbo].[Inspeccion_visual]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Inspeccion_visual_FKv4'
CREATE INDEX [IX_FK_Inspeccion_visual_FKv4]
ON [dbo].[MecanismoNeumatico]
    ([Inspeccion_visual_id]);
GO

-- Creating foreign key on [Inspeccion_visual_id] in table 'MecanismoResortes'
ALTER TABLE [dbo].[MecanismoResortes]
ADD CONSTRAINT [FK_Inspeccion_visual_FKv5]
    FOREIGN KEY ([Inspeccion_visual_id])
    REFERENCES [dbo].[Inspeccion_visual]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Inspeccion_visual_FKv5'
CREATE INDEX [IX_FK_Inspeccion_visual_FKv5]
ON [dbo].[MecanismoResortes]
    ([Inspeccion_visual_id]);
GO

-- Creating foreign key on [Inspeccion_visual_id] in table 'Presostato'
ALTER TABLE [dbo].[Presostato]
ADD CONSTRAINT [FK_Inspeccion_visual_FKv6]
    FOREIGN KEY ([Inspeccion_visual_id])
    REFERENCES [dbo].[Inspeccion_visual]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Inspeccion_visual_FKv6'
CREATE INDEX [IX_FK_Inspeccion_visual_FKv6]
ON [dbo].[Presostato]
    ([Inspeccion_visual_id]);
GO

-- Creating foreign key on [Prueba_id] in table 'Inspeccion_visual'
ALTER TABLE [dbo].[Inspeccion_visual]
ADD CONSTRAINT [FK_Prueba_FKv2]
    FOREIGN KEY ([Prueba_id])
    REFERENCES [dbo].[Prueba]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Prueba_FKv2'
CREATE INDEX [IX_FK_Prueba_FKv2]
ON [dbo].[Inspeccion_visual]
    ([Prueba_id]);
GO

-- Creating foreign key on [Marca_id] in table 'Modelo'
ALTER TABLE [dbo].[Modelo]
ADD CONSTRAINT [FK_Marca_FK]
    FOREIGN KEY ([Marca_id])
    REFERENCES [dbo].[Marca]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Marca_FK'
CREATE INDEX [IX_FK_Marca_FK]
ON [dbo].[Modelo]
    ([Marca_id]);
GO

-- Creating foreign key on [ModeloId], [MarcaId] in table 'ParametrosFabricantePE'
ALTER TABLE [dbo].[ParametrosFabricantePE]
ADD CONSTRAINT [FK_Modelo_ParametrosFabricantePE_FK]
    FOREIGN KEY ([ModeloId], [MarcaId])
    REFERENCES [dbo].[Modelo]
        ([id], [Marca_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Modelo_ParametrosFabricantePE_FK'
CREATE INDEX [IX_FK_Modelo_ParametrosFabricantePE_FK]
ON [dbo].[ParametrosFabricantePE]
    ([ModeloId], [MarcaId]);
GO

-- Creating foreign key on [ModeloId], [Marca_id] in table 'ParametrosFabricantePR'
ALTER TABLE [dbo].[ParametrosFabricantePR]
ADD CONSTRAINT [FK_Modelo_ParametrosFabricantePR_FK]
    FOREIGN KEY ([ModeloId], [Marca_id])
    REFERENCES [dbo].[Modelo]
        ([id], [Marca_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Modelo_ParametrosFabricantePR_FK'
CREATE INDEX [IX_FK_Modelo_ParametrosFabricantePR_FK]
ON [dbo].[ParametrosFabricantePR]
    ([ModeloId], [Marca_id]);
GO

-- Creating foreign key on [ParametroCondicion_id] in table 'Variables'
ALTER TABLE [dbo].[Variables]
ADD CONSTRAINT [FK_ParametroCondicion_FK]
    FOREIGN KEY ([ParametroCondicion_id])
    REFERENCES [dbo].[ParametroCondicion]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ParametroCondicion_FK'
CREATE INDEX [IX_FK_ParametroCondicion_FK]
ON [dbo].[Variables]
    ([ParametroCondicion_id]);
GO

-- Creating foreign key on [Prueba_id] in table 'PruebaEspecial'
ALTER TABLE [dbo].[PruebaEspecial]
ADD CONSTRAINT [FK_Prueba_FKv3]
    FOREIGN KEY ([Prueba_id])
    REFERENCES [dbo].[Prueba]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Prueba_FKv3'
CREATE INDEX [IX_FK_Prueba_FKv3]
ON [dbo].[PruebaEspecial]
    ([Prueba_id]);
GO

-- Creating foreign key on [Prueba_id] in table 'PruebaRutina'
ALTER TABLE [dbo].[PruebaRutina]
ADD CONSTRAINT [FK_Prueba_PbaRutina_FK]
    FOREIGN KEY ([Prueba_id])
    REFERENCES [dbo].[Prueba]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Prueba_PbaRutina_FK'
CREATE INDEX [IX_FK_Prueba_PbaRutina_FK]
ON [dbo].[PruebaRutina]
    ([Prueba_id]);
GO

-- Creating foreign key on [PruebaEspecial_id] in table 'PruebaEspecialDetalle'
ALTER TABLE [dbo].[PruebaEspecialDetalle]
ADD CONSTRAINT [FK_PruebaEspecial_FK]
    FOREIGN KEY ([PruebaEspecial_id])
    REFERENCES [dbo].[PruebaEspecial]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PruebaEspecial_FK'
CREATE INDEX [IX_FK_PruebaEspecial_FK]
ON [dbo].[PruebaEspecialDetalle]
    ([PruebaEspecial_id]);
GO

-- Creating foreign key on [PruebaRutina_id] in table 'PruebaRutinaDetalle'
ALTER TABLE [dbo].[PruebaRutinaDetalle]
ADD CONSTRAINT [FK_PruebaRutina_FK]
    FOREIGN KEY ([PruebaRutina_id])
    REFERENCES [dbo].[PruebaRutina]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PruebaRutina_FK'
CREATE INDEX [IX_FK_PruebaRutina_FK]
ON [dbo].[PruebaRutinaDetalle]
    ([PruebaRutina_id]);
GO

-- Creating foreign key on [Variable_id] in table 'Rangos'
ALTER TABLE [dbo].[Rangos]
ADD CONSTRAINT [FK_Variables_FK]
    FOREIGN KEY ([Variable_id])
    REFERENCES [dbo].[Variables]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Variables_FK'
CREATE INDEX [IX_FK_Variables_FK]
ON [dbo].[Rangos]
    ([Variable_id]);
GO

-- Creating foreign key on [Zona_id], [Gerencia_id] in table 'Subestacion'
ALTER TABLE [dbo].[Subestacion]
ADD CONSTRAINT [FK_Zona_FK]
    FOREIGN KEY ([Zona_id], [Gerencia_id])
    REFERENCES [dbo].[Zona]
        ([id], [Gerencia_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Zona_FK'
CREATE INDEX [IX_FK_Zona_FK]
ON [dbo].[Subestacion]
    ([Zona_id], [Gerencia_id]);
GO

-- Creating foreign key on [AspNetRoles_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles]
    FOREIGN KEY ([AspNetRoles_Id])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers]
    FOREIGN KEY ([AspNetUsers_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserRoles_AspNetUsers'
CREATE INDEX [IX_FK_AspNetUserRoles_AspNetUsers]
ON [dbo].[AspNetUserRoles]
    ([AspNetUsers_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------