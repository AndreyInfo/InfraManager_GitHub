ALTER TABLE LifeCycleStateOperationTransition 
	DROP CONSTRAINT FK_LifeCycleStateOperationTransition_LifeCycleStateOperation

ALTER TABLE LifeCycleStateOperationTransition 
  ADD CONSTRAINT FK_LifeCycleStateOperationTransition_LifeCycleStateOperation 
  FOREIGN KEY ([LifeCycleStateOperationID]) 
  REFERENCES LifeCycleStateOperation(id) 
  ON DELETE CASCADE;


ALTER TABLE LifeCycleStateOperationTransition 
	DROP CONSTRAINT FK_LifeCycleStateOperationTransition_LifeCycleState_Finish

ALTER TABLE LifeCycleStateOperationTransition 
  ADD CONSTRAINT FK_LifeCycleStateOperationTransition_LifeCycleState_Finish 
  FOREIGN KEY ([FinishLifeCycleStateID]) 
  REFERENCES LifeCycleState(id) 
  ON DELETE CASCADE;