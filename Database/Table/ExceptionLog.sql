CREATE TABLE [dbo].[ExceptionLog]
(  
	[id] [int] IDENTITY(1, 1) NOT NULL,  
	[ErrorLine] [int] NULL,  
	[ErrorMessage] [nvarchar](5000) NULL,  
	[ErrorNumber] [int] NULL,  
	[ErrorProcedure] [nvarchar](128) NULL,  
	[ErrorSeverity] [int] NULL,  
	[ErrorState] [int] NULL,  
	[DateErrorRaised] [datetime] NULL  
)  