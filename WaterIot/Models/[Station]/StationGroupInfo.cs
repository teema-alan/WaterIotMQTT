using System;
using System.Collections.Generic;

namespace WaterIot.Models
{
	/// <summary>
	/// 監測站群組
	/// </summary>
	public class StationGroupInfo : InfoBase
	{
		/// <summary>
		/// [必要] 監測站群組Id
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// 監測站群組名稱
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 監測站群組描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 上層群組Id,如果是根群組則為 null
		/// </summary>
		public Guid? ParentStationGroupId { get; set; }

		public IEnumerable<StationGroupInfo> ChildStationGroups { get; set; }

		/// <summary>
		/// 根群組為0,根群組下一層群組為1,以此類推
		/// </summary>
		public int TreeLevel { get; set; }

		/// <summary>
		/// 監測站群組所擁有的監測站
		/// </summary>
		public IEnumerable<StationInfo> LinkedStations { get; set; }
	}
}