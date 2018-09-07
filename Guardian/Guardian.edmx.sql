
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/15/2018 13:54:45
-- Generated from EDMX file: C:\Users\Dark-Heart\OneDrive\Documents\Visual Studio 2017\Projects\Guardian\Guardian\Guardian.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Guardian];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CRIMINELDELIT]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DELIT] DROP CONSTRAINT [FK_CRIMINELDELIT];
GO
IF OBJECT_ID(N'[dbo].[FK_PLAIGNANTPLAINTE]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PLAINTE] DROP CONSTRAINT [FK_PLAIGNANTPLAINTE];
GO
IF OBJECT_ID(N'[dbo].[FK_PLAINTEDOCUMENTS]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DOCUMENTS] DROP CONSTRAINT [FK_PLAINTEDOCUMENTS];
GO
IF OBJECT_ID(N'[dbo].[FK_CRIMINELAdresse]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AdresseSet] DROP CONSTRAINT [FK_CRIMINELAdresse];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[CRIMINEL]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CRIMINEL];
GO
IF OBJECT_ID(N'[dbo].[DELIT]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DELIT];
GO
IF OBJECT_ID(N'[dbo].[DOCUMENTS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DOCUMENTS];
GO
IF OBJECT_ID(N'[dbo].[LOG]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LOG];
GO
IF OBJECT_ID(N'[dbo].[PLAIGNANT]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PLAIGNANT];
GO
IF OBJECT_ID(N'[dbo].[PLAINTE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PLAINTE];
GO
IF OBJECT_ID(N'[dbo].[USER]', 'U') IS NOT NULL
    DROP TABLE [dbo].[USER];
GO
IF OBJECT_ID(N'[dbo].[AdresseSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdresseSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'CRIMINEL'
CREATE TABLE [dbo].[CRIMINEL] (
    [Id_crim] int IDENTITY(1,1) NOT NULL,
    [matricul_crim] varchar(5)  NULL,
    [nom_crim] varchar(15)  NOT NULL,
    [prenom_crim] varchar(15)  NOT NULL,
    [datenaiss_crim] datetime  NOT NULL,
    [typ_piece] varchar(25)  NULL,
    [num_piece] varchar(14)  NULL,
    [crim_nationa] varchar(20)  NOT NULL,
    [crim_profes] varchar(30)  NULL,
    [date_enreg] datetime  NOT NULL,
    [photo_crim] varbinary(max)  NULL,
    [deffere] bit  NULL,
    [crim_taill] decimal(3,0)  NULL,
    [crim_poid] decimal(3,0)  NULL,
    [contatct_crim] nvarchar(max)  NULL
);
GO

-- Creating table 'DELIT'
CREATE TABLE [dbo].[DELIT] (
    [Id_delit] int IDENTITY(1,1) NOT NULL,
    [nom_delit] varchar(30)  NOT NULL,
    [CRIMINELId_crim] int  NOT NULL,
    [lieu_delit] nvarchar(max)  NULL
);
GO

-- Creating table 'DOCUMENTS'
CREATE TABLE [dbo].[DOCUMENTS] (
    [Id_doc] int IDENTITY(1,1) NOT NULL,
    [doc] varbinary(max)  NOT NULL,
    [PLAINTEId_aff] int  NOT NULL,
    [docname] varchar(50)  NOT NULL
);
GO

-- Creating table 'LOG'
CREATE TABLE [dbo].[LOG] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [identifiant_log] varchar(20)  NOT NULL,
    [nom_log] varchar(20)  NOT NULL,
    [grade] varchar(30)  NULL,
    [date_log] datetime  NOT NULL,
    [user_photo] varbinary(max)  NULL
);
GO

-- Creating table 'PLAIGNANT'
CREATE TABLE [dbo].[PLAIGNANT] (
    [Id_plaignant] int IDENTITY(1,1) NOT NULL,
    [nom_plai] varchar(max)  NOT NULL,
    [prenom_plai] varchar(max)  NOT NULL,
    [contact_plai] varchar(11)  NOT NULL,
    [adress_plai] varchar(max)  NOT NULL,
    [professi_plai] varchar(max)  NULL
);
GO

-- Creating table 'PLAINTE'
CREATE TABLE [dbo].[PLAINTE] (
    [Id_aff] int IDENTITY(1,1) NOT NULL,
    [descrip_aff] varchar(max)  NOT NULL,
    [cause_aff] varchar(max)  NULL,
    [professconvo] varchar(max)  NULL,
    [nom_convoc] varchar(max)  NULL,
    [PLAIGNANTId_plaignant] int  NOT NULL,
    [code_aff] varchar(max)  NOT NULL,
    [date_aff] datetime  NOT NULL,
    [classe] bit  NULL
);
GO

-- Creating table 'USER'
CREATE TABLE [dbo].[USER] (
    [id_user] int IDENTITY(1,1) NOT NULL,
    [nom_user] varchar(10)  NOT NULL,
    [prenom_user] varchar(15)  NULL,
    [dob_user] datetime  NOT NULL,
    [mail_user] varchar(25)  NOT NULL,
    [contact] varchar(11)  NOT NULL,
    [grade_user] varchar(15)  NULL,
    [identifiant_user] varchar(15)  NOT NULL,
    [psswrd_user] varchar(15)  NOT NULL,
    [photo_user] varbinary(max)  NULL,
    [actif] bit  NOT NULL,
    [profil] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AdresseSet'
CREATE TABLE [dbo].[AdresseSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [pays] nvarchar(max)  NOT NULL,
    [ville] nvarchar(max)  NOT NULL,
    [quartier] nvarchar(max)  NOT NULL,
    [latitude] float  NOT NULL,
    [longitude] float  NOT NULL,
    [altitude] float  NULL,
    [precision] nvarchar(max)  NULL,
    [CRIMINELId_crim] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id_crim] in table 'CRIMINEL'
ALTER TABLE [dbo].[CRIMINEL]
ADD CONSTRAINT [PK_CRIMINEL]
    PRIMARY KEY CLUSTERED ([Id_crim] ASC);
GO

-- Creating primary key on [Id_delit] in table 'DELIT'
ALTER TABLE [dbo].[DELIT]
ADD CONSTRAINT [PK_DELIT]
    PRIMARY KEY CLUSTERED ([Id_delit] ASC);
GO

-- Creating primary key on [Id_doc] in table 'DOCUMENTS'
ALTER TABLE [dbo].[DOCUMENTS]
ADD CONSTRAINT [PK_DOCUMENTS]
    PRIMARY KEY CLUSTERED ([Id_doc] ASC);
GO

-- Creating primary key on [Id] in table 'LOG'
ALTER TABLE [dbo].[LOG]
ADD CONSTRAINT [PK_LOG]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id_plaignant] in table 'PLAIGNANT'
ALTER TABLE [dbo].[PLAIGNANT]
ADD CONSTRAINT [PK_PLAIGNANT]
    PRIMARY KEY CLUSTERED ([Id_plaignant] ASC);
GO

-- Creating primary key on [Id_aff] in table 'PLAINTE'
ALTER TABLE [dbo].[PLAINTE]
ADD CONSTRAINT [PK_PLAINTE]
    PRIMARY KEY CLUSTERED ([Id_aff] ASC);
GO

-- Creating primary key on [id_user] in table 'USER'
ALTER TABLE [dbo].[USER]
ADD CONSTRAINT [PK_USER]
    PRIMARY KEY CLUSTERED ([id_user] ASC);
GO

-- Creating primary key on [Id] in table 'AdresseSet'
ALTER TABLE [dbo].[AdresseSet]
ADD CONSTRAINT [PK_AdresseSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CRIMINELId_crim] in table 'DELIT'
ALTER TABLE [dbo].[DELIT]
ADD CONSTRAINT [FK_CRIMINELDELIT]
    FOREIGN KEY ([CRIMINELId_crim])
    REFERENCES [dbo].[CRIMINEL]
        ([Id_crim])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CRIMINELDELIT'
CREATE INDEX [IX_FK_CRIMINELDELIT]
ON [dbo].[DELIT]
    ([CRIMINELId_crim]);
GO

-- Creating foreign key on [PLAIGNANTId_plaignant] in table 'PLAINTE'
ALTER TABLE [dbo].[PLAINTE]
ADD CONSTRAINT [FK_PLAIGNANTPLAINTE]
    FOREIGN KEY ([PLAIGNANTId_plaignant])
    REFERENCES [dbo].[PLAIGNANT]
        ([Id_plaignant])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PLAIGNANTPLAINTE'
CREATE INDEX [IX_FK_PLAIGNANTPLAINTE]
ON [dbo].[PLAINTE]
    ([PLAIGNANTId_plaignant]);
GO

-- Creating foreign key on [PLAINTEId_aff] in table 'DOCUMENTS'
ALTER TABLE [dbo].[DOCUMENTS]
ADD CONSTRAINT [FK_PLAINTEDOCUMENTS]
    FOREIGN KEY ([PLAINTEId_aff])
    REFERENCES [dbo].[PLAINTE]
        ([Id_aff])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PLAINTEDOCUMENTS'
CREATE INDEX [IX_FK_PLAINTEDOCUMENTS]
ON [dbo].[DOCUMENTS]
    ([PLAINTEId_aff]);
GO

-- Creating foreign key on [CRIMINELId_crim] in table 'AdresseSet'
ALTER TABLE [dbo].[AdresseSet]
ADD CONSTRAINT [FK_CRIMINELAdresse]
    FOREIGN KEY ([CRIMINELId_crim])
    REFERENCES [dbo].[CRIMINEL]
        ([Id_crim])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CRIMINELAdresse'
CREATE INDEX [IX_FK_CRIMINELAdresse]
ON [dbo].[AdresseSet]
    ([CRIMINELId_crim]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------