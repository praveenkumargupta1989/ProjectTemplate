USE [<Database Name>]  --INSERT DATABASE HERE
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
GO
alter Procedure [dbo].[iud<ProcedureName>]  --INSERT PROC NAME HERE
<Parameters>
AS
/* **********************************************************************
Author			:  
Creation Date	: 
Description		: 
*************************************************************************
Sample
======
exec --INSERT PROC NAME AND PARAMETERS HERE
GRANT EXECUTE on [PROC NAME HERE] TO public

Change History
DATE		CHANGED BY		CHANGE CODE		DESCRIPTION

*/
BEGIN
	SET NOCOUNT ON
	/*WRITE LOGGING CODE IF ANY BEFORE TRANSACTION*/
	DECLARE @transtate BIT
	IF @@TRANCOUNT = 0
	BEGIN
		SET @transtate = 1
		BEGIN TRANSACTION transtate
	END

	BEGIN TRY
		/*INSERT CODE HERE : ALL BUSINESS DATA CODE*/
		
		IF @transtate = 1 AND XACT_STATE() = 1
			COMMIT TRANSACTION transtate
			
		/*WRITE LOGGING CODE IF ANY AFTER TRANSACTION*/
	END TRY
	
	SET NOCOUNT OFF
	BEGIN CATCH
		/*DO NOT WRITE ANY BUSINESS/LOGGING CODE AFTR BEGIN CATCH*/
		DECLARE @Error_Message VARCHAR(5000), @Error_Severity INT, @Error_State INT, @Error_Line INT,
				@Error_Number INT, @Error_Procedure VARCHAR(300)
		
		SELECT 	@Error_Message = ERROR_MESSAGE(), @Error_Line = ERROR_LINE(), @Error_Severity = ERROR_SEVERITY(), @Error_State = ERROR_STATE()
				@Error_Number = Error_Number(), @Error_Procedure = Error_Procedure()

		IF @transtate = 1 AND XACT_STATE() <> 0
			ROLLBACK TRANSACTION
			
		EXEC iudLogErrorInfo @Error_Line, @Error_Message, @Error_Number, @Error_Procedure, @Error_Severity, @Error_State
		/*DO NOT WRITE ANY BUSINESS/LOGGING CODE BEFORE THIS STATEMENT*/
		
		/*WRITE ANY BUSINESS/LOGGING CODE FROM HERE*/
		
		RAISERROR (@Error_Message, @Error_Severity, @Error_State)
	END CATCH
END