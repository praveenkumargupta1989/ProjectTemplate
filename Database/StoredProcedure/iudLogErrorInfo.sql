CREATE PROCEDURE [dbo].[iudLogErrorInfo]
@Error_Line INT,
@Error_Message VARCHAR(5000), 
@Error_Number INT, 
@Error_Procedure VARCHAR(300),
@Error_Severity INT, 
@Error_State INT
AS
BEGIN
	INSERT INTO ExceptionLog(ErrorLine, ErrorMessage, ErrorNumber, ErrorProcedure, ErrorSeverity, ErrorState, DateErrorRaised)  
	SELECT  @Error_Line,  @Error_Message,  Error_Number() as ErrorNumber,  Error_Procedure() as 'Proc',  
			Error_Severity() as ErrorSeverity,  Error_State() as ErrorState,  GETDATE () as DateErrorRaised
end  