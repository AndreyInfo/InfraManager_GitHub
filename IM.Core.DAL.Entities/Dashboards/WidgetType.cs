namespace InfraManager.DAL.Dashboards;

/// <summary>
/// Тип элемента dashboardItem
/// </summary>
public enum WidgetType : byte
{
	/// <summary>
	/// Значение
	/// </summary>
	Text = 0,

	/// <summary>
	/// Количество и процент
	/// </summary>
	ProgressBar = 1,

	/// <summary>
	/// Диаграмма распределения (одиночная функция)
	/// </summary>
	Diagram = 2,

	/// <summary>
	/// График (одиночная функция)
	/// </summary>
	Graph = 3,

	/// <summary>
	/// График (множество функций)
	/// </summary>
	MultiGraph = 4,

	/// <summary>
	/// Диаграмма распределения (множество функций)
	/// </summary>
	MultiDiagram = 5
}