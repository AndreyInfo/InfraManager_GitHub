INSERT INTO [dbo].[RoleOperation] (OperationID, RoleID)
SELECT t.ID, 'e13e5209-83c4-4046-904f-7e8f1f941c33'
FROM [dbo].[Operation] t
LEFT JOIN [dbo].[RoleOperation] x ON x.OperationID = t.ID AND x.RoleID = 'e13e5209-83c4-4046-904f-7e8f1f941c33'
WHERE (t.ID = 799) AND (x.OperationID IS NULL)
GO