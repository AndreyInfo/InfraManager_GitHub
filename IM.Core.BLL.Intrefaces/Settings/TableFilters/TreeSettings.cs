namespace InfraManager.BLL.Settings.TableFilters
{
    public class TreeSettings
    {
        public ObjectClass[] AvailableClassID { get; init; } //это то, что может показывать дерево (а значит и возвращать)
        public ObjectClass[] FinishClassID { get; init; } //просто узлы, в которых по дефолту ничего нет, сразу без плюсиков
        public ObjectClass[] TargetClassID { get; init; } //это то, что ищет искалка
        public OperationID[] OperationsID { get; init; } //ограничиваем выдачу дерева этими операциями
        public NavigatorTypes TreeType { get; init; }
        public bool UseAccessIsGranted { get; init; }
    }
}
