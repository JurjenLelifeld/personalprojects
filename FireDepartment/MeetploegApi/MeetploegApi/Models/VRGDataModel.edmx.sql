
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/26/2015 09:39:04
-- Generated from EDMX file: C:\Users\eigenaar\Source\Repos\MeetploegApi5\MeetploegApi\MeetploegApi\Models\VRGDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [meetploegapi_db];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_IncidentAssignment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssignmentSet] DROP CONSTRAINT [FK_IncidentAssignment];
GO
IF OBJECT_ID(N'[dbo].[FK_MeasuringTeamAssignment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssignmentSet] DROP CONSTRAINT [FK_MeasuringTeamAssignment];
GO
IF OBJECT_ID(N'[dbo].[FK_UserMeasuringTeam]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserSet] DROP CONSTRAINT [FK_UserMeasuringTeam];
GO
IF OBJECT_ID(N'[dbo].[FK_MessageMessage_to_user]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MessageToUserSet] DROP CONSTRAINT [FK_MessageMessage_to_user];
GO
IF OBJECT_ID(N'[dbo].[FK_MessageIncident]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MessageSet] DROP CONSTRAINT [FK_MessageIncident];
GO
IF OBJECT_ID(N'[dbo].[FK_MeasurementAssignment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MeasurementSet] DROP CONSTRAINT [FK_MeasurementAssignment];
GO
IF OBJECT_ID(N'[dbo].[FK_MessageToUserUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MessageToUserSet] DROP CONSTRAINT [FK_MessageToUserUser];
GO
IF OBJECT_ID(N'[dbo].[FK_MeasuringTeamUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserSet] DROP CONSTRAINT [FK_MeasuringTeamUser];
GO
IF OBJECT_ID(N'[dbo].[FK_GpsLocationMeasuringTeam]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GpsLocationSet] DROP CONSTRAINT [FK_GpsLocationMeasuringTeam];
GO
IF OBJECT_ID(N'[dbo].[FK_GpsLocationIncident]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GpsLocationSet] DROP CONSTRAINT [FK_GpsLocationIncident];
GO
IF OBJECT_ID(N'[dbo].[FK_MeasureAssignment_inherits_Assignment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssignmentSet_MeasureAssignment] DROP CONSTRAINT [FK_MeasureAssignment_inherits_Assignment];
GO
IF OBJECT_ID(N'[dbo].[FK_GasMeasurement_inherits_Measurement]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MeasurementSet_GasMeasurement] DROP CONSTRAINT [FK_GasMeasurement_inherits_Measurement];
GO
IF OBJECT_ID(N'[dbo].[FK_Earthquake_inherits_Measurement]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MeasurementSet_Earthquake] DROP CONSTRAINT [FK_Earthquake_inherits_Measurement];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[IncidentSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IncidentSet];
GO
IF OBJECT_ID(N'[dbo].[AssignmentSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AssignmentSet];
GO
IF OBJECT_ID(N'[dbo].[MeasuringTeamSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MeasuringTeamSet];
GO
IF OBJECT_ID(N'[dbo].[UserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet];
GO
IF OBJECT_ID(N'[dbo].[MessageSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MessageSet];
GO
IF OBJECT_ID(N'[dbo].[MessageToUserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MessageToUserSet];
GO
IF OBJECT_ID(N'[dbo].[MeasurementSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MeasurementSet];
GO
IF OBJECT_ID(N'[dbo].[GpsLocationSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GpsLocationSet];
GO
IF OBJECT_ID(N'[dbo].[AssignmentSet_MeasureAssignment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AssignmentSet_MeasureAssignment];
GO
IF OBJECT_ID(N'[dbo].[MeasurementSet_GasMeasurement]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MeasurementSet_GasMeasurement];
GO
IF OBJECT_ID(N'[dbo].[MeasurementSet_Earthquake]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MeasurementSet_Earthquake];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'IncidentSet'
CREATE TABLE [dbo].[IncidentSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Address] nvarchar(max)  NULL,
    [GasMoldCode] int  NOT NULL,
    [GasMoldColor] nvarchar(max)  NOT NULL,
    [WindDirection] int  NOT NULL,
    [Time] datetime  NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [Details] nvarchar(max)  NOT NULL,
    [Active] bit  NOT NULL,
    [Latitude] float  NOT NULL,
    [Longitude] float  NOT NULL
);
GO

-- Creating table 'AssignmentSet'
CREATE TABLE [dbo].[AssignmentSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IncidentId] int  NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [Time] datetime  NOT NULL,
    [MeasuringTeamId] int  NOT NULL,
    [Details] nvarchar(max)  NOT NULL,
    [Arrived] bit  NOT NULL,
    [Active] bit  NOT NULL,
    [Latitude] float  NOT NULL,
    [Longitude] float  NOT NULL
);
GO

-- Creating table 'MeasuringTeamSet'
CREATE TABLE [dbo].[MeasuringTeamSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [Departed] bit  NOT NULL,
    [Latitude] float  NOT NULL,
    [Longitude] float  NOT NULL
);
GO

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MeasuringTeamId] int  NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NULL,
    [Role] nvarchar(max)  NOT NULL,
    [PhoneId] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'MessageSet'
CREATE TABLE [dbo].[MessageSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [Time] datetime  NOT NULL,
    [SenderUserId] int  NOT NULL,
    [IncidentId] int  NOT NULL
);
GO

-- Creating table 'MessageToUserSet'
CREATE TABLE [dbo].[MessageToUserSet] (
    [MessageId] int  NOT NULL,
    [UserId] int  NOT NULL,
    [Delivered] bit  NOT NULL,
    [Read] bit  NOT NULL
);
GO

-- Creating table 'MeasurementSet'
CREATE TABLE [dbo].[MeasurementSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AssignmentId] int  NOT NULL,
    [MeasurementType] nvarchar(max)  NOT NULL,
    [Observations] nvarchar(max)  NOT NULL,
    [MeasurementTime] datetime  NOT NULL,
    [Latitude] float  NOT NULL,
    [Longitude] float  NOT NULL
);
GO

-- Creating table 'GpsLocationSet'
CREATE TABLE [dbo].[GpsLocationSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MeasuringTeamId] int  NOT NULL,
    [IncidentId] int  NOT NULL,
    [Latitude] float  NOT NULL,
    [Longitude] float  NOT NULL,
    [Time] datetime  NOT NULL
);
GO

-- Creating table 'AssignmentSet_MeasureAssignment'
CREATE TABLE [dbo].[AssignmentSet_MeasureAssignment] (
    [GastubeCode] int  NOT NULL,
    [LelMeasurement] bit  NOT NULL,
    [Automess] bit  NOT NULL,
    [AutomessProbe] bit  NOT NULL,
    [DoseMeasurer] bit  NOT NULL,
    [BreathableAir] bit  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'MeasurementSet_GasMeasurement'
CREATE TABLE [dbo].[MeasurementSet_GasMeasurement] (
    [LelResult] float  NOT NULL,
    [PumpStrokes] int  NOT NULL,
    [FirstPumpStroke] datetime  NOT NULL,
    [Concentration] float  NOT NULL,
    [CoMeasurement] float  NOT NULL,
    [GasTubeCode] int  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'MeasurementSet_Earthquake'
CREATE TABLE [dbo].[MeasurementSet_Earthquake] (
    [VictimsState] tinyint  NOT NULL,
    [BuildingsState] tinyint  NOT NULL,
    [InfrastructureState] tinyint  NOT NULL,
    [VictimDetails] nvarchar(max)  NOT NULL,
    [BuildingDetails] nvarchar(max)  NOT NULL,
    [InfrastructureDetails] nvarchar(max)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'IncidentSet'
ALTER TABLE [dbo].[IncidentSet]
ADD CONSTRAINT [PK_IncidentSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AssignmentSet'
ALTER TABLE [dbo].[AssignmentSet]
ADD CONSTRAINT [PK_AssignmentSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MeasuringTeamSet'
ALTER TABLE [dbo].[MeasuringTeamSet]
ADD CONSTRAINT [PK_MeasuringTeamSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MessageSet'
ALTER TABLE [dbo].[MessageSet]
ADD CONSTRAINT [PK_MessageSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [MessageId], [UserId] in table 'MessageToUserSet'
ALTER TABLE [dbo].[MessageToUserSet]
ADD CONSTRAINT [PK_MessageToUserSet]
    PRIMARY KEY CLUSTERED ([MessageId], [UserId] ASC);
GO

-- Creating primary key on [Id] in table 'MeasurementSet'
ALTER TABLE [dbo].[MeasurementSet]
ADD CONSTRAINT [PK_MeasurementSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GpsLocationSet'
ALTER TABLE [dbo].[GpsLocationSet]
ADD CONSTRAINT [PK_GpsLocationSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AssignmentSet_MeasureAssignment'
ALTER TABLE [dbo].[AssignmentSet_MeasureAssignment]
ADD CONSTRAINT [PK_AssignmentSet_MeasureAssignment]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MeasurementSet_GasMeasurement'
ALTER TABLE [dbo].[MeasurementSet_GasMeasurement]
ADD CONSTRAINT [PK_MeasurementSet_GasMeasurement]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MeasurementSet_Earthquake'
ALTER TABLE [dbo].[MeasurementSet_Earthquake]
ADD CONSTRAINT [PK_MeasurementSet_Earthquake]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [IncidentId] in table 'AssignmentSet'
ALTER TABLE [dbo].[AssignmentSet]
ADD CONSTRAINT [FK_IncidentAssignment]
    FOREIGN KEY ([IncidentId])
    REFERENCES [dbo].[IncidentSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IncidentAssignment'
CREATE INDEX [IX_FK_IncidentAssignment]
ON [dbo].[AssignmentSet]
    ([IncidentId]);
GO

-- Creating foreign key on [MeasuringTeamId] in table 'AssignmentSet'
ALTER TABLE [dbo].[AssignmentSet]
ADD CONSTRAINT [FK_MeasuringTeamAssignment]
    FOREIGN KEY ([MeasuringTeamId])
    REFERENCES [dbo].[MeasuringTeamSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MeasuringTeamAssignment'
CREATE INDEX [IX_FK_MeasuringTeamAssignment]
ON [dbo].[AssignmentSet]
    ([MeasuringTeamId]);
GO

-- Creating foreign key on [MeasuringTeamId] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [FK_UserMeasuringTeam]
    FOREIGN KEY ([MeasuringTeamId])
    REFERENCES [dbo].[MeasuringTeamSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserMeasuringTeam'
CREATE INDEX [IX_FK_UserMeasuringTeam]
ON [dbo].[UserSet]
    ([MeasuringTeamId]);
GO

-- Creating foreign key on [MessageId] in table 'MessageToUserSet'
ALTER TABLE [dbo].[MessageToUserSet]
ADD CONSTRAINT [FK_MessageMessage_to_user]
    FOREIGN KEY ([MessageId])
    REFERENCES [dbo].[MessageSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IncidentId] in table 'MessageSet'
ALTER TABLE [dbo].[MessageSet]
ADD CONSTRAINT [FK_MessageIncident]
    FOREIGN KEY ([IncidentId])
    REFERENCES [dbo].[IncidentSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MessageIncident'
CREATE INDEX [IX_FK_MessageIncident]
ON [dbo].[MessageSet]
    ([IncidentId]);
GO

-- Creating foreign key on [AssignmentId] in table 'MeasurementSet'
ALTER TABLE [dbo].[MeasurementSet]
ADD CONSTRAINT [FK_MeasurementAssignment]
    FOREIGN KEY ([AssignmentId])
    REFERENCES [dbo].[AssignmentSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MeasurementAssignment'
CREATE INDEX [IX_FK_MeasurementAssignment]
ON [dbo].[MeasurementSet]
    ([AssignmentId]);
GO

-- Creating foreign key on [UserId] in table 'MessageToUserSet'
ALTER TABLE [dbo].[MessageToUserSet]
ADD CONSTRAINT [FK_MessageToUserUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MessageToUserUser'
CREATE INDEX [IX_FK_MessageToUserUser]
ON [dbo].[MessageToUserSet]
    ([UserId]);
GO

-- Creating foreign key on [MeasuringTeamId] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [FK_MeasuringTeamUser]
    FOREIGN KEY ([MeasuringTeamId])
    REFERENCES [dbo].[MeasuringTeamSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MeasuringTeamUser'
CREATE INDEX [IX_FK_MeasuringTeamUser]
ON [dbo].[UserSet]
    ([MeasuringTeamId]);
GO

-- Creating foreign key on [MeasuringTeamId] in table 'GpsLocationSet'
ALTER TABLE [dbo].[GpsLocationSet]
ADD CONSTRAINT [FK_GpsLocationMeasuringTeam]
    FOREIGN KEY ([MeasuringTeamId])
    REFERENCES [dbo].[MeasuringTeamSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GpsLocationMeasuringTeam'
CREATE INDEX [IX_FK_GpsLocationMeasuringTeam]
ON [dbo].[GpsLocationSet]
    ([MeasuringTeamId]);
GO

-- Creating foreign key on [IncidentId] in table 'GpsLocationSet'
ALTER TABLE [dbo].[GpsLocationSet]
ADD CONSTRAINT [FK_GpsLocationIncident]
    FOREIGN KEY ([IncidentId])
    REFERENCES [dbo].[IncidentSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GpsLocationIncident'
CREATE INDEX [IX_FK_GpsLocationIncident]
ON [dbo].[GpsLocationSet]
    ([IncidentId]);
GO

-- Creating foreign key on [Id] in table 'AssignmentSet_MeasureAssignment'
ALTER TABLE [dbo].[AssignmentSet_MeasureAssignment]
ADD CONSTRAINT [FK_MeasureAssignment_inherits_Assignment]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[AssignmentSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'MeasurementSet_GasMeasurement'
ALTER TABLE [dbo].[MeasurementSet_GasMeasurement]
ADD CONSTRAINT [FK_GasMeasurement_inherits_Measurement]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[MeasurementSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'MeasurementSet_Earthquake'
ALTER TABLE [dbo].[MeasurementSet_Earthquake]
ADD CONSTRAINT [FK_Earthquake_inherits_Measurement]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[MeasurementSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------