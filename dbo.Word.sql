CREATE TABLE [dbo].[Word] (
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [WordQuestion] NVARCHAR (MAX) NOT NULL,
    [WordAnswer]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Word] PRIMARY KEY CLUSTERED ([Id] ASC)
);

